using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Spatial;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    [Table("Neighborhood")]
    public class Neighborhood
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public Geometry? Coordinate { get; set; }
        public long? DistrictId { get; set; }
    }
}
