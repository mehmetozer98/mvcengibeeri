using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCEngiBeering.Models
{
    public class BBMachine
    {
        public int id {get; set; }
        public Guid uniqueid { get; set; }
        [Column("currentstate"),ForeignKey("id")]
        public int currentstateid { get; set; }
        public int currentspeed { get; set; }
        [Column("currentproduct"),ForeignKey("id")]
        public int currentproductid { get; set; }
        public int currentamount { get; set; }
        
        [ForeignKey("id")]
        public MachineState currentState { get; set; }
        [ForeignKey("id")]
        public ProductType currentProduct { get; set; }
        public List<Batch> Batches { get; set; }
        
    }
}
