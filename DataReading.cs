using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCEngiBeering.Models
{
    public class DataReading
    {
        public int id { get; set; }
        [ForeignKey("id"),Required]
        public int batchid { get; set; }
        public int type {get; set;}
        public DateTime time {get; set;}
        public float value {get; set;}
        
        public Batch batch { get; set; }
    }
}