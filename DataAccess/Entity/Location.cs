using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    [Table("Location")]
    public class Location
    {
        [Key]
        public int ID { get; set; }
        public int CityId { get; set; }
        public long DistrictId { get; set; }
        public long NeighborhoodId { get; set; }
        public long ParcelId { get; set; }
        //ZeminId
        public long GroundId { get; set; }
        public string ParcelNumber { get; set; }
        //Nitelik
        public string Property { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string NeighborhoodName { get; set; }
        public double Area { get; set; }
        //Ada Numarası
        public long LandNumber { get; set; }
        public string State { get; set; }
        //ZeminKmdurum
        public string GroundState { get; set; }

    }
}
