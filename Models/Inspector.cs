using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorLogWebApplication.Models
{
    public class Inspector
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Subdivision { get; set; }
        public IEnumerable<Inspection> Inspections { get; set; }
        public Inspector()
        {
            Inspections = new List<Inspection>();
        }
    }
}
