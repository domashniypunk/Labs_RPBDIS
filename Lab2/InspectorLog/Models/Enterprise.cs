using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorLog.Models
{
    public class Enterprise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnershipTypeId { get; set; }
        public string Adress { get; set; }
        public string BossFirstName { get; set; }
        public string BossSecondName { get; set; }
        public string BossMiddleName { get; set; }
        public string BossPhoneNumber { get; set; }
        public OwnershipType OwnershipType { get; set; }
    }
}
