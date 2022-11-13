using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorLogWebApplication.Models
{
    public class Enterprise
    {
        public int Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "Адрес")]
        public string Adress { get; set; }
        [Display(Name = "Имя начальника")]
        public string BossFirstName { get; set; }
        [Display(Name = "Фамилия начальника")]
        public string BossLastName { get; set; }
        [Display(Name = "Отчество начальника")]
        public string BossMiddleName { get; set; }
        [Display(Name = "Телефон начальника")]
        public string BossPhoneNumber { get; set; }
        [Display(Name = "Тип собственности")]
        public OwnershipType OwnershipType { get; set; }
        public int OwnershipTypeId { get; set; }
        public IEnumerable<Inspection> Inspections { get; set; }
        public Enterprise()
        {
            Inspections = new List<Inspection>();
        }
    }
}
