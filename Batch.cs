using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCEngiBeering.Models
{
    public class Batch
    {
        public int id { get; set; }
        [Column("machineid"),ForeignKey("id")]
        public int machine { get; set; }
        [Column("producttype"),ForeignKey("id")]
        public int producttype { get; set; }
        public int setamount { get; set; }
        public int setspeed { get; set; }
        public int actualamount { get; set; }
        public int defectiveamount { get; set; }
        
        [ForeignKey("id")]
        public BBMachine Machine { get; set; }
        [ForeignKey("id")]
        public ProductType ProductType { get; set; }
        public ICollection<DataReading> DataReadings { get; set; }
    }
}