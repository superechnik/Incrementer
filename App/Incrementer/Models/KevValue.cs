using System.ComponentModel.DataAnnotations;

namespace Incrementer.Models
{
    public class KeyValue
    {
        [Key]
        public string Key { get; set; }
        public decimal Value { get; set; }
    }
}
