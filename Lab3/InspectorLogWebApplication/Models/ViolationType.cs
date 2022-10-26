using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorLogWebApplication.Models
{
    public class ViolationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Fine { get; set; }
        public int CorrectionTerm { get; set; }
    }
}
