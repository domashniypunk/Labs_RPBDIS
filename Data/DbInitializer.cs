using InspectorLogWebApplication.Models;

namespace InspectorLogWebApplication.Data
{
    public static class DbInitializer
    {
        public static void Initialize(InspectorLogContext db)
        {
            db.Database.EnsureCreated();

            if(db.OwnershipTypes.Any())
            {
                return;
            }

            const int violationTypesNumber = 50;
            const int inspectorsNumber = 50;
            const int enterpriceNumber = 500;
            const int inspectionsNumber = 500;
            const int violationsNumber = 1000;

            Random random = new Random(20);
            string[] ownershipTypes =
            {
                "Частная",
                "Государственная",
                "Общественная",
                "Муниципальная",
                "Смешанная"
            };

            // Заполнение таблицы типов собственности
            for (int i = 0; i < ownershipTypes.Length; i++)
            {
                OwnershipType ownershipType = new OwnershipType()
                {
                    Name = ownershipTypes[i]
                };
                db.OwnershipTypes.Add(ownershipType);
            }
            db.SaveChanges();

            string[] violationTypes =
            {
                "Не проведение инструктажа по охране труда",
                "Нарушение режима труда и отдыха работников",
                "Необеспечение работников средствами защиты",
                "Не проведение аттестации рабочих мест",
                "Отсутствие специалиста по охране труда",
                "Невыполнение требований по расследованию несчастных случаев на производстве",
                "Другое нарушение"
            };

            for (int i = 0; i < violationTypesNumber; i++)
            {
                string violationName;
                if (i < violationTypes.Length)
                {
                    violationName = violationTypes[i];
                }
                else
                {
                    violationName = violationTypes.Last() + "_" + (i - violationTypes.Length);
                }
                ViolationType violationType = new ViolationType()
                {
                    Name = violationName,
                    Fine = random.NextDouble() * 10000 + 1,
                    CorrectionTerm = random.Next(730)
                };
                db.ViolationTypes.Add(violationType);
            }
            db.SaveChanges();

            string[] menFirstNames =
{
                "Иван",
                "Федор",
                "Петр",
                "Виктор",
                "Степан",
                "Вечеслав",
                "Артем",
                "Серафим",
                "Александр",
                "Эдуард",
                "Олег"
            };
            string[] womanFirstNames =
            {
                "Мария",
                "Анна",
                "Дарья",
                "Валерия",
                "Диана",
                "Наталья",
                "Владислава",
                "Виктория",
                "Ксения",
                "Вечеслава",
                "Ульяна",
                "Екатерина",
                "Светлана",
                "Анастасия",
                "Александра",
                "Евгения"
            };
            string[] secondNames =
            {
                "Заяц",
                "Дрозд",
                "Пикун",
                "Манько",
                "Довбань",
                "Петренко",
                "Иваненко",
                "Емельяненко",
                "Дмитриенко",
                "Зайченко",
                "Гоголь",
                "Федоренко",
                "Громыко"
            };

            for (int i = 0; i < inspectorsNumber; i++)
            {
                string employeeFirstName = "";
                string employeeMiddlename = "";
                int gender = random.Next(2);
                if(gender == 0)
                {
                    employeeFirstName = menFirstNames[random.Next(menFirstNames.Length)];
                    employeeMiddlename = menFirstNames[random.Next(menFirstNames.Length)] + "ович";
                }
                else
                {
                    employeeFirstName = womanFirstNames[random.Next(womanFirstNames.Length)];
                    employeeMiddlename = menFirstNames[random.Next(menFirstNames.Length)] + "овна";
                }
                string employeeSecondName = secondNames[random.Next(secondNames.Length)];
                Inspector inspector = new Inspector()
                {
                    FirstName = employeeFirstName,
                    LastName = employeeSecondName,
                    MiddleName = employeeMiddlename,
                    Subdivision = "Подразделение_" + random.Next(100)
                };
                db.Inspectors.Add(inspector);
            }
            db.SaveChanges();

            string[] towns = 
            { 
                "Светлогорск", "Речица", "Гомель", "Минск",
                "Солигорск", "Витебск", "Могилев", "Мозырь",
                "Барановичи", "Жлобин", "Гродно", "Кобрин",
                "Полоцк", "Туров", "Глубокае", "Пинск",
                "Василевичи", "Орша", "Несвиж", "Бобруйск",
                "Слоним", "Борисов"
            };
            string[] streets = 
            { 
                "Советская", "Кирова", "Пролетарская", "Коммунарова",
                "Речная", "Высокая", "Озерная", "Текстильная",
                "Восточная", "Кленовская", "Кутузова", "Трудовая",
                "Крестьянская", "Первомайская", "Береговая", "Горная",
                "Добрушская", "Тургенева", "Луначарского", "Урицкого",
                "Гоголя", "Маркса", "Полесская", "Ленина", "Луговая",
                "Столярная", "Брянская", "Встречная", "Тимофеенко",
                "Бакунина", "Лермонтова", "Чехова", "Чайковского", "Ломоносова"
            };
            for (int i = 0; i < enterpriceNumber;i++)
            {
                string bossFirstName = "";
                string bossMiddlename = "";
                int gender = random.Next(2);
                if (gender == 0)
                {
                    bossFirstName = menFirstNames[random.Next(menFirstNames.Length)];
                    bossMiddlename = menFirstNames[random.Next(menFirstNames.Length)] + "ович";
                }
                else
                {
                    bossFirstName = womanFirstNames[random.Next(womanFirstNames.Length)];
                    bossMiddlename = menFirstNames[random.Next(menFirstNames.Length)] + "овна";
                }
                string bossLastName = secondNames[random.Next(secondNames.Length)];
                string phoneNumber = "";
                for (int j = 0; j < 9; j++)
                {
                    phoneNumber += random.Next(10);
                }
                int ownershipTypeId = db.OwnershipTypes
                    .Select(ot => ot.Id).ToList()[random.Next(db.OwnershipTypes.Count())];
                Enterprise enterprise = new Enterprise()
                {
                    Name = "Предприятие_" + i,
                    BossFirstName = bossFirstName,
                    BossMiddleName = bossMiddlename,
                    BossLastName = bossLastName,
                    BossPhoneNumber = phoneNumber,
                    OwnershipTypeId = ownershipTypeId,
                    Adress = towns[random.Next(towns.Length)] + ", " +
                    streets[random.Next(streets.Length)] + ", " + random.Next(100)
                };
                db.Enterprises.Add(enterprise);
            }

            db.SaveChanges();

            for(int i = 0;i < inspectionsNumber; i++)
            {
                int enterpriseId = db.Enterprises
                    .Select(e => e.Id).ToList()[random.Next(db.Enterprises.Count())];
                int inspectorId = db.Inspectors
                    .Select(insp => insp.Id).ToList()[random.Next(db.Inspectors.Count())];
                Inspection inspection = new Inspection()
                {
                    EnterpriseId = enterpriseId,
                    InspectorId = inspectorId,
                    Number = i.ToString(),
                    Date = DateTime.Now - (new TimeSpan(random.Next(3650), 0, 0, 0)),
                };
                db.Inspections.Add(inspection);
            }
            db.SaveChanges();

            for(int i = 0;i < violationsNumber;i++)
            {
                int violationTypeId = db.ViolationTypes
                    .Select(vt => vt.Id).ToList()[random.Next(db.ViolationTypes.Count())];
                int inspectionId = db.Inspections
                    .Select(insp => insp.Id).ToList()[random.Next(db.Inspections.Count())];
                DateTime inspDate = db.Inspections.Where(i => i.Id == inspectionId).First().Date;
                DateTime dueDate = inspDate + (new TimeSpan(random.Next(3650), 0, 0, 0));
                DateTime eliminationDate = inspDate + (new TimeSpan(random.Next(730), 0, 0, 0));
                Violation violation = new Violation()
                {
                    DueDate = dueDate,
                    EliminationDate = eliminationDate,
                    Fine = random.NextDouble() * 10000 + 1,
                    FinePaid = random.Next(2) == 1,
                    ViolationCorrected = random.Next(2) == 1,
                    ViolationTypeId = violationTypeId,
                    InspectionId = inspectionId,
                };
                db.Violations.Add(violation);
            }
            db.SaveChanges();
        }
    }
}
