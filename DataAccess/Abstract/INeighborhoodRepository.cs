using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface INeighborhoodRepository
    {
        bool Save(LocationModel model);
        List<LocationModel> Get(int id);
    }
}
