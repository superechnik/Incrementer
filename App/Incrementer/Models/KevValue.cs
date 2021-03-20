using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Incrementer.Models
{
    public class KeyValue
    {
        [Key]
        public string Key { get; set; }
        public decimal Value { get; set; }
    }
}
