using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.Entity;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class DistrictRepository: IDistrictRepository
    {
        private readonly ApplicationContext _context;
        public DistrictRepository(ApplicationContext context)
        {
            _context = context;
        }
        public List<LocationModel> Get(int id)
        {
            List<LocationModel> locationList = new List<LocationModel>();
            try
            {
                List<District> model = _context.District.Where(x => x.CityId == id).ToList();
                foreach (District district in model)
                {
                    LocationModel location = new LocationModel();
                    location.properties = new PropertyModel();
                    location.properties.text = district.Name;
                    location.properties.id = district.Id;
                    location.properties.ilId = district.CityId.HasValue ? district.CityId.Value : 0;
                    locationList.Add(location); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return locationList;
        }
        public bool Save(LocationModel model)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                District district = new District();
                try
                {
                    district.Name = model.properties.text;
                    district.Id = model.properties.id;
                    district.CityId = model.properties.ilId;
                    _context.District.Add(district);
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
