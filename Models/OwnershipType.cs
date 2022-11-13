using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorLogWebApplication.Models
{
    public class OwnershipType
    {
        public int Id { get; set; }
        [Display(Name = "Тип собственности")]
        public string Name { get; set; }
        public IEnumerable<Enterprise> Enterprises { get; set; }
        public OwnershipType()
        {
            Enterprises = new List<Enterprise>();
        }
    }
}
