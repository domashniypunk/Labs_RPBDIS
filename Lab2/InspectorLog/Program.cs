using System.Collections;
using System.Data;
using InspectorLog.Data;
using InspectorLog.Models;

namespace InspectorLog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string menuItem = "";
            using (InspectorLogContext context = new InspectorLogContext())
            {
                while (menuItem != "11")
                {
                    Menu();
                    menuItem = Console.ReadLine();
                    Console.Clear();
                    switch (menuItem)
                    {
                        case "1":
                            Print(GetViolationTypes(context, 10), "Список типов нарушений:");
                            break;
                        case "2":
                            Print(GetViolationTypesByFine(context, 10),
                                "Список типов нарушений с возможным штрафом большим 6000:");
                            break;
                        case "3":
                            Print(GetViolationCountByType(context, 10),
                                "Общее количество нарушений по каждому из типов:");
                            break;
                        case "4":
                            Print(GetViolationsWithType(context, 10), "Типы нарушений:");
                            break;
                        case "5":
                            Print(GetNonPaidFireViolationsWithType(context, 10),
                                "Типы нарушений с неуплаченным штрафом:");
                            break;
                        case "6":
                            Console.WriteLine("Введите фамилию: ");
                            string secondName = Console.ReadLine();
                            Console.WriteLine("Введите имя: ");
                            string firstName = Console.ReadLine();
                            Console.WriteLine("Введите отчество: ");
                            string middleName = Console.ReadLine();
                            Console.WriteLine("Введите подразделение: ");
                            string subdivision = Console.ReadLine();
                            Inspector inspector = new Inspector
                            {
                                SecondName = secondName,
                                FirstName = firstName,
                                MiddleName = middleName,
                                Subdivision = subdivision
                            };
                            AddInspector(context, inspector);
                            Print(context.Inspectors.OrderByDescending(ins => ins.Id).Take(10).Select(ins =>
                            new
                            {
                                Код_инспектора = ins.Id,
                                Фамилия = ins.SecondName,
                                Имя = ins.FirstName,
                                Отчество = ins.MiddleName,
                                Подразделение = ins.Subdivision
                            }), "Последние добавленные инспектора: ");
                            break;
                        case "7":
                            Random random = new Random();
                            Console.WriteLine("Введите наименование предприятия");
                            string name = Console.ReadLine();
                            Console.WriteLine("Введите фамилию начальника: ");
                            string bossSecondName = Console.ReadLine();
                            Console.WriteLine("Введите имя начальника: ");
                            string bossFirstName = Console.ReadLine();
                            Console.WriteLine("Введите отчество начальника: ");
                            string bossMiddleName = Console.ReadLine();
                            Console.WriteLine("Введите адрес предприятия: ");
                            string adress = Console.ReadLine();
                            List<int> ids = context.OwnershipTypes.Select(ot => ot.Id).ToList();
                            int ownership = ids[random.Next(ids.Count - 1)];
                            Enterprise enterprise = new Enterprise
                            {
                                Name = name,
                                BossFirstName = bossFirstName,
                                BossSecondName = bossSecondName,
                                BossMiddleName = bossMiddleName,
                                BossPhoneNumber = random.Next(1000000, 9999999).ToString(),
                                OwnershipTypeId = ownership,
                                Adress = adress
                            };
                            AddEnterprice(context, enterprise);
                            var enterprises = context.Enterprises.OrderByDescending(ent => ent.Id).Take(10).Select(ent =>
                            new
                            {
                                Код_предприятия = ent.Id,
                                Наименование = ent.Name,
                                Адрес = ent.Adress
                            });
                            Print(enterprises, "Последние добавленные предприятия: ");
                            break;
                        case "8":
                            Print(context.Inspectors.OrderByDescending(ins => ins.Id).Take(10).Select(ins =>
                            new
                            {
                                Код_инспектора = ins.Id,
                                Фамилия = ins.SecondName,
                                Имя = ins.FirstName,
                                Отчество = ins.MiddleName,
                                Подразделение = ins.Subdivision
                            }), "Последние добавленные инспектора: ");
                            Console.WriteLine("Введите код инспектора");
                            int inspectorId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Будет удалён инспектор с кодом: " + inspectorId.ToString());
                            DelInspector(context, inspectorId);
                            Print(context.Inspectors.OrderByDescending(ins => ins.Id).Take(10).Select(ins =>
                            new
                            {
                                Код_инспектора = ins.Id,
                                Фамилия = ins.SecondName,
                                Имя = ins.FirstName,
                                Отчество = ins.MiddleName,
                                Подразделение = ins.Subdivision
                            }), "Последние добавленные инспектора: ");
                            break;
                        case "9":
                            Print(context.Enterprises.OrderByDescending(ent => ent.Id).Take(10).Select(ent =>
                            new
                            {
                                Код_предприятия = ent.Id,
                                Наименование = ent.Name,
                                Адрес = ent.Adress
                            }), "Последние добавленные предприятия: ");
                            Console.WriteLine("Введите код предприятия");
                            int enterpriseId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Будет удалёно предприятие с кодом: " + enterpriseId.ToString());
                            DelEnterprise(context, enterpriseId);
                            Print(context.Enterprises.OrderByDescending(ent => ent.Id).Take(10).Select(ent =>
                            new
                            {
                                Код_предприятия = ent.Id,
                                Наименование = ent.Name,
                                Адрес = ent.Adress
                            }), "Последние добавленные предприятия: ");
                            break;
                        case "10":
                            Print(GetViolationTypes(context, 10), "Типы штрафов до обновления");
                            UpdateViolationTypes(context);
                            Print(GetViolationTypes(context, 10), "Типы штрафов после обновления");
                            break;
                        case "11":
                            break;
                        default:
                            break;
                    }
                    Console.Clear();
                }
            }
        }
        public static void Menu()
        {
            Console.WriteLine("Выберите пункт меню...");
            Console.WriteLine("1. Вывод списка типов нарушений;");
            Console.WriteLine("2. Вывод списка типов нарушений с возможным штрафом большим 6000;");
            Console.WriteLine("3. Вывод общего количества нарушений по каждому из типов;");
            Console.WriteLine("4. Вывод типов всех нарушений;");
            Console.WriteLine("5. Вывод типов всех нарушений с невыплаченным штрафом;");
            Console.WriteLine("6. Добавить инспектора;");
            Console.WriteLine("7. Добавить предприятие;");
            Console.WriteLine("8. Удалить инспектора;");
            Console.WriteLine("9. Удалить предприятие;");
            Console.WriteLine("10. Увеличить возможный срок исправления на 7 для типов нарушений с возможным штрафом большим 6000;");
            Console.WriteLine("11. Выход.");
        }
        public static void Print(IEnumerable records, string queryText)
        {
            Console.WriteLine(queryText);
            foreach(var record in records)
            {
                Console.WriteLine(record);
            }
            Console.WriteLine("Нажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
        }
        // Запрос на выборку списка типов нарушений
        public static IEnumerable GetViolationTypes(InspectorLogContext context, int recordsNumber)
        {
            var query = context.ViolationTypes.Select(vt => 
            new 
            {
                Код_типа_нарушения = vt.Id, 
                Наименование = vt.Name, 
                Возможный_штраф = vt.Fine,
                Возможные_сроки_исправления = vt.CorrectionTerm}
            );
            return query.Take(recordsNumber).ToList();
        }
        // Запрос на выборку списка типов нарушений со возможным штрафом большим 6000
        public static IEnumerable GetViolationTypesByFine(InspectorLogContext context, int recordsNumber)
        {
            var query = context.ViolationTypes.Where(vt => vt.Fine > 6000).Select(vt =>
            new
            {
                Код_типа_нарушения = vt.Id,
                Наименование = vt.Name,
                Возможный_штраф = vt.Fine,
                Возможные_сроки_исправления = vt.CorrectionTerm
            }
            );
            return query.Take(recordsNumber).ToList();
        }
        // Запрос на выборку количества нарушение по каждому типу нарушения 
        public static IEnumerable GetViolationCountByType(InspectorLogContext context, int recordsNumber)
        {
            var query = context.Violations.GroupBy(v => v.ViolationTypeId, v => v.Id)
                .Select(vl =>
                new
                {
                    Код_типа_нарушения = vl.Key,
                    Количество_нарушений = vl.Count()
                });
            return query.Take(recordsNumber).ToList();
        }
        // Запрос на выборку нарушений с указанием типов нарушений
        public static IEnumerable GetViolationsWithType(InspectorLogContext context, int recordsNumber)
        {
            var query = context.Violations.Join(context.ViolationTypes,
                v => v.ViolationTypeId, vt => vt.Id, (v, vt) =>
                new
                {
                    Код_нарушения = v.Id,
                    Тип_нарушения = vt.Name
                });
            return query.Take(recordsNumber).ToList();
        }
        //Полчение неоплаченных штрафов с указанием типов нарушений
        public static IEnumerable GetNonPaidFireViolationsWithType(InspectorLogContext context, int recordsNumber)
        {
            var query = context.Violations.Where(vl => vl.FinePaid == true).Join(context.ViolationTypes,
                v => v.ViolationTypeId, vt => vt.Id, (v, vt) =>
                new
                {
                    Код_нарушения = v.Id,
                    Тип_нарушения = vt.Name
                });
            return query.Take(recordsNumber).ToList();
        }
        //Добавление инспектора
        public static void AddInspector(InspectorLogContext context, Inspector inspector)
        {
            context.Inspectors.Add(inspector);
            context.SaveChanges();
        }
        //Добавление предприятия
        public static void AddEnterprice(InspectorLogContext context, Enterprise enterprise)
        {
            context.Enterprises.Add(enterprise);
            context.SaveChanges();
        }
        //Удаление инспектора
        public static void DelInspector(InspectorLogContext context, int Id)
        {
            context.Violations.RemoveRange(context.Violations.Where(v => v.Inspection.InspectorId == Id));
            context.Inspections.RemoveRange(context.Inspections.Where(insp => insp.InspectorId == Id));
            context.Inspectors.RemoveRange(context.Inspectors.Where(ins => ins.Id == Id));
            context.SaveChanges();
        }
        //Удаление предприятия
        public static void DelEnterprise(InspectorLogContext context, int Id)
        {
            context.Violations.RemoveRange(context.Violations.Where(v => v.Inspection.EnterpriseId == Id));
            context.Inspections.RemoveRange(context.Inspections.Where(ins => ins.EnterpriseId == Id));
            context.Enterprises.RemoveRange(context.Enterprises.Where(ent => ent.Id == Id));
            context.SaveChanges();
        }
        public static void UpdateViolationTypes(InspectorLogContext context)
        {
            var updatingRecords = context.ViolationTypes.Where(vt => vt.Fine > 6000);
            foreach (var violationType in updatingRecords)
            {
                violationType.CorrectionTerm = violationType.CorrectionTerm + 7;
            }
            context.SaveChanges();
        }
    }
}
