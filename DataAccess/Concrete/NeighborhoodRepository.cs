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
    public class NeighborhoodRepository : INeighborhoodRepository
    {
        private readonly ApplicationContext _context;
        public NeighborhoodRepository(ApplicationContext context)
        {
            _context = context;
        }
        public List<LocationModel> Get(int id)
        {
            List<LocationModel> locationList = new List<LocationModel>();
            try
            {
                List<Neighborhood> model = _context.Neighborhood.Where(x => x.DistrictId == id).ToList();
                foreach (Neighborhood neighborhood in model)
                {
                    LocationModel location = new LocationModel();
                    location.properties = new PropertyModel();
                    location.properties.text = neighborhood.Name;
                    location.properties.id = neighborhood.Id;
                    location.properties.ilceId = neighborhood.DistrictId.HasValue ? neighborhood.DistrictId.Value : 0;
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
                Neighborhood neighborhood = new Neighborhood();
                try
                {
                    neighborhood.Name = model.properties.text;
                    neighborhood.Id = model.properties.id;
                    neighborhood.DistrictId = model.properties.ilceId;
                    _context.Neighborhood.Add(neighborhood);
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
