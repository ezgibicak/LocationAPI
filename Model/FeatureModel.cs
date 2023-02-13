using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class FeatureModel
    {
        public List<LocationModel> features { get; set; }
    }
}
