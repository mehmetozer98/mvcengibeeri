using System.Collections.Generic;

namespace MVCEngiBeering.Models
{
    public class ProductType
    {
        public int id { get; set; }
        public string name { get; set; }
        
        public List<BBMachine> Machines { get; set; }
    }
}