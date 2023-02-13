using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.Entity;
using Model;

namespace Core.Concrete
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationContext _context;
        public LocationRepository(ApplicationContext context)
        {
            _context = context;
        }
        public LocationModel Get(int neighborhoodId, int landNumber, int parcelNumber)
        {
            LocationModel location = new LocationModel();
            location.properties = new PropertyModel();
            try
            {
                var parcelNo = parcelNumber.ToString();
                Location model = _context.Location.Where(x => x.NeighborhoodId == neighborhoodId && x.LandNumber == landNumber && x.ParcelNumber == parcelNo).FirstOrDefault();
                if (model != null)
                {
                    location.properties.ilAd = model.CityName;
                    location.properties.ilId = model.CityId;
                    location.properties.ilceId = model.DistrictId;
                    location.properties.ilceAd = model.DistrictName;
                    location.properties.nitelik = model.Property;
                    location.properties.mahalleAd = model.NeighborhoodName;
                    location.properties.mahalleId = model.NeighborhoodId;
                    location.properties.parselId = model.ParcelId;
                    location.properties.parselNo = model.ParcelNumber;
                    location.properties.adaNo = model.LandNumber;
                    location.properties.alan = model.Area;
                    location.properties.zeminId = model.GroundId;
                    location.properties.zeminKmdurum = model.GroundState;
                    location.properties.durum = model.State;
                    location.properties.id = model.ID;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return location;
        }
        public bool Save(LocationModel model)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                Location location = new Location();
                try
                {
                    location.CityName = model.properties.ilAd;
                    location.CityId = model.properties.ilId;
                    location.DistrictId = model.properties.ilceId;
                    location.DistrictName = model.properties.ilceAd;
                    location.Property = model.properties.nitelik;
                    location.NeighborhoodName = model.properties.mahalleAd;
                    location.NeighborhoodId = model.properties.mahalleId;
                    location.ParcelId = model.properties.parselId;
                    location.ParcelNumber = model.properties.parselNo;
                    location.LandNumber = model.properties.adaNo;
                    location.Area = model.properties.alan;
                    location.GroundId = model.properties.zeminId;
                    location.State = model.properties.durum;
                    location.GroundState = model.properties.zeminKmdurum;
                    _context.Location.Add(location);
                    _context.SaveChanges();
                    dbContextTransaction.Commit();

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
                return true;
            }
        }
    }
}
