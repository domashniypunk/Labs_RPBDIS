using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorLog.Models
{
    public class Inspection
    {
        public int Id { get; set; }
        public int InspectorId { get; set; }
        public int EnterpriseId { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public Inspector Inspector { get; set; }
        public Enterprise Enterprise { get; set; }
    }
}
