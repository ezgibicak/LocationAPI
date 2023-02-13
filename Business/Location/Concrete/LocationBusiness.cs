using Business.Location.Abstract;
using Cache.Abstract;
using DataAccess.Abstract;
using Core.Helper;
using Microsoft.Extensions.Configuration;
using Model;
using Newtonsoft.Json;

namespace Business.Location.Concrete
{
    public class LocationBusiness : ILocationBusiness
    {
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _config;
        private readonly ICityRepository _cityRepo;
        private readonly IDistrictRepository _districtRepo;
        private readonly INeighborhoodRepository _neRepo;
        private readonly ILocationRepository _locationRepo;
        RequestHelper requestHelper = new RequestHelper();
        RedisHelper redisHelper = null;


        public LocationBusiness(ICacheService cacheService, IConfiguration configuration, IDistrictRepository districtRepo, ICityRepository cityRepo, INeighborhoodRepository neRepo, ILocationRepository locationRepo)
        {
            _districtRepo = districtRepo;
            _cacheService = cacheService;
            _config = configuration;
            _cityRepo = cityRepo;
            _neRepo = neRepo;
            _locationRepo = locationRepo;
            redisHelper = new RedisHelper(_cacheService);
        }

        public async Task<ResultModel<LocationModel>> Get()
        {
            IConfigurationSection url = _config.GetSection("CityUrl");
            ResultModel<LocationModel> resultModel = new ResultModel<LocationModel>();
            resultModel.Data = new List<LocationModel>();
            string key = "cityList";
            var redisResult = redisHelper.SearchRedisResult(key);
            //Rediste yok ise
            if (redisResult.Result == null)
            {
                //Tüm iller veritabanında yok ise??
                var cityEntity = _cityRepo.GetAll();
                if (cityEntity.Count > 0)
                {
                    foreach (var city in cityEntity)
                    {
                        //Veritabanında var ise
                        resultModel.Data.Add(city);
                    }
                    redisHelper.SetDataRedis(key, cityEntity);
                }
                //Veritabanında ve rediste yok ise
                else
                {
                    string responseFromServer = requestHelper.SendRequest(url.Value);
                    FeatureModel featureModel = JsonConvert.DeserializeObject<FeatureModel>(responseFromServer);
                    redisHelper.SetDataRedis(key, featureModel.features);
                    foreach (var cityItem in featureModel.features)
                    {
                        _cityRepo.Save(cityItem);
                        resultModel.Data.Add(cityItem);

                    }
                }

            }
            else
            {
                //Rediste var
                resultModel.Data.AddRange(redisResult.Result);
            }
            return resultModel;
        }
        public async Task<ResultModel<LocationModel>> GetDistrict(int cityId)
        {
            var url = _config.GetSection("DistrictBaseUrl");
            ResultModel<LocationModel> resultModel = new ResultModel<LocationModel>();
            resultModel.Data = new List<LocationModel>();
            string requestUrl = $"{url.Value}/{cityId}";
            string key = "districtId_";
            var redisKey = key + cityId;
            var redisResult = redisHelper.SearchRedisResult(redisKey);
            //Rediste yok ise
            if (redisResult.Result == null)
            {
                var districtEntity = _districtRepo.Get(cityId);
                if (districtEntity.Count != 0)
                {
                    //Veritabanında var ise sonuca ekle
                    resultModel.Data.AddRange(districtEntity);
                    //Veritabanındaki veriyi redise ekle
                    redisHelper.SetDataRedis(redisKey, districtEntity);
                }
                else
                {
                    //Veritabanında ve Rediste yok ise
                    string responseFromServer = requestHelper.SendRequest(requestUrl);
                    FeatureModel featureModel = JsonConvert.DeserializeObject<FeatureModel>(responseFromServer);
                    //Rediste yok ise requestten gelen veriyi ekle
                    redisHelper.SetDataRedis(redisKey, featureModel.features);
                    foreach (var district in featureModel.features)
                    {
                        //Veritabanında yok ise requestten gelen veriyi ekle
                        district.properties.ilId = cityId;
                        _districtRepo.Save(district);
                        resultModel.Data.Add(district);
                    }

                }
            }
            else
            {
                //Rediste var
                resultModel.Data.AddRange(redisResult.Result);
            }
            return resultModel;
        }
        public async Task<ResultModel<LocationModel>> GetNeighborhood(int districtId)
        {
            var url = _config.GetSection("NeighborhoodBaseUrl");
            ResultModel<LocationModel> resultModel = new ResultModel<LocationModel>();
            resultModel.Data = new List<LocationModel>();
            string requestUrl = $"{url.Value}/{districtId}";
            string key = "neighborhoodId_";
            string redisKey = key + districtId;
            var redisResult = redisHelper.SearchRedisResult(redisKey);
            //Rediste yok ise
            if (redisResult.Result == null)
            {
                var neighborhoodEntity = _neRepo.Get(districtId);
                if (neighborhoodEntity.Count != 0)
                {
                    //Veritabanında var ise sonuca ekle
                    resultModel.Data.AddRange(neighborhoodEntity);
                    //Veritabanındaki veriyi redise ekle
                    redisHelper.SetDataRedis(redisKey, neighborhoodEntity);

                }
                else
                {
                    //Veritabanında ve Rediste yok
                    string responseFromServer = requestHelper.SendRequest(requestUrl);
                    FeatureModel featureModel = JsonConvert.DeserializeObject<FeatureModel>(responseFromServer);
                    //Rediste yok ise requestten gelen veriyi ekle
                    redisHelper.SetDataRedis(redisKey, featureModel.features);
                    foreach (var neighborhood in featureModel.features)
                    {
                        //Veritabanında yok ise requestten gelen veriyi ekle
                        neighborhood.properties.ilceId = districtId;
                        _neRepo.Save(neighborhood);
                        resultModel.Data.Add(neighborhood);

                    }

                }
            }
            else
            {
                //Rediste var
                resultModel.Data.AddRange(redisResult.Result);
            }
            return resultModel;
        }
        public async Task<ResultModel<LocationModel>> GetLocation(int neighborhoodId, int landNumber, int parcelNumber)
        {
            var url = _config.GetSection("ParcelBaseUrl");
            LocationModel locationModel = null;
            ResultModel<LocationModel> resultModel = new ResultModel<LocationModel>();
            resultModel.Data = new List<LocationModel>();
            string key = $"{neighborhoodId}_{landNumber}_{parcelNumber}";
            string urlParams = $"{neighborhoodId}/{landNumber}/{parcelNumber}";
            string requestUrl = url.Value + urlParams;
            var redisResult = redisHelper.SearchResult(key);
            //Rediste yok
            if (redisResult.Result == null)
            {
                var locationEntity = _locationRepo.Get(neighborhoodId, landNumber, parcelNumber);
                if (locationEntity.properties.id != 0)
                {
                    //Veritabanında var ise
                    resultModel.Data.Add(locationEntity);
                    //Veritabanındaki veriyi redise ekle
                    redisHelper.SetDataRedis(key, locationEntity);
                }
                else
                {
                    string responseFromServer = requestHelper.SendRequest(requestUrl);
                    locationModel = JsonConvert.DeserializeObject<LocationModel>(responseFromServer);
                    //Veritabanında yok ise
                    _locationRepo.Save(locationModel);
                    //Rediste yok
                    redisHelper.SetDataRedis(key, locationModel);
                    resultModel.Data.Add(locationModel);
                }
            }
            else
            {
                resultModel.Data.Add(redisResult.Result);

            }
            return resultModel;
        }

    }
}
