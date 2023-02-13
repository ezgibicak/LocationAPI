using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class GeometryModel
    {
        public List<List<List<object>>> coordinates { get; set; }

        public string type { get; set; }
    }
}
