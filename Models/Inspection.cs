using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorLogWebApplication.Models
{
    public class Inspection
    {
        public int Id { get; set; }
        public int InspectorId { get; set; }
        public int EnterpriseId { get; set; }
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Display(Name = "Номер")]
        public string Number { get; set; }
        [Display(Name = "Инспектор")]
        public Inspector Inspector { get; set; }
        [Display(Name = "Предприятие")]
        public Enterprise Enterprise { get; set; }
        public IEnumerable<Violation> Violations { get; set; }
        public Inspection()
        {
            Violations = new List<Violation>();
        }
    }
}
