using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ILocationRepository
    {
        bool Save(LocationModel model);
        LocationModel Get(int neighborhoodId, int landNumber, int parcelNumber);
    }
}
