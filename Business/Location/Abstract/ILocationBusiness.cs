using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Location.Abstract
{
    public interface ILocationBusiness
    {
        Task<ResultModel<LocationModel>> Get();
        Task<ResultModel<LocationModel>> GetDistrict(int cityId);
        Task<ResultModel<LocationModel>> GetNeighborhood(int districtId);
        Task<ResultModel<LocationModel>> GetLocation(int neighborhoodId, int landNumber, int parcelNumber);

    }
}
