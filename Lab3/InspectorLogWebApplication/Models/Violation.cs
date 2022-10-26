using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorLogWebApplication.Models
{
    public class Violation
    {
        public int Id { get; set; }
        public decimal Fine { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime EliminationDate { get; set; }
        public bool FinePaid { get; set; }
        public bool ViolationCorrected { get; set; }
        public int InspectionId { get; set; }
        public int ViolationTypeId { get; set; }
        public Inspection Inspection { get; set; }
        public ViolationType ViolationType { get; set; }
    }
}
