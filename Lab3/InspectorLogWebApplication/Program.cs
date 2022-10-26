using InspectorLogWebApplication.Services;
using InspectorLogWebApplication.Data;
using Microsoft.EntityFrameworkCore;
using InspectorLogWebApplication.Models;
using InspectorLogWebApplication.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
string connString = builder.Configuration.GetConnectionString("SQLConnection");
builder.Services.AddDbContext<InspectorLogContext>(options => options.UseSqlServer(connString));

builder.Services.AddMemoryCache();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<ICachedViolations, CachedViolations>();
builder.Services.AddScoped<ICachedInspections, CachedInspections>();
builder.Services.AddScoped<ICachedInspectors, CachedInspectors>();

var app = builder.Build();
var env = builder.Environment;

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseSession();

app.Map("/info", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Информация:</H1>";
        strResponse += "<BR> Сервер: " + context.Request.Host;
        strResponse += "<BR> Путь: " + context.Request.PathBase;
        strResponse += "<BR> Протокол: " + context.Request.Protocol;
        strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
        await context.Response.WriteAsync(strResponse);
    });
});

app.Map("/inspectors", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedInspectors cachedInspectors = context.RequestServices.GetService<ICachedInspectors>();
        IEnumerable<Inspector> inspectors = cachedInspectors.GetInspectors(20);
        string HtmlString = "<HTML><HEAD><TITLE>Инспекторы</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список инспекторов</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Код</TH>";
        HtmlString += "<TH>Фамилия</TH>";
        HtmlString += "<TH>Имя</TH>";
        HtmlString += "<TH>Отчество</TH>";
        HtmlString += "<TH>Подразделение</TH>";
        HtmlString += "</TR>";
        foreach (var inspector in inspectors)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + inspector.Id + "</TD>";
            HtmlString += "<TD>" + inspector.SecondName + "</TD>";
            HtmlString += "<TD>" + inspector.FirstName + "</TD>";
            HtmlString += "<TD>" + inspector.MiddleName + "</TD>";
            HtmlString += "<TD>" + inspector.Subdivision + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/violations", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedViolations cachedViolations = context.RequestServices.GetService<ICachedViolations>();
        IEnumerable<Violation> violations = cachedViolations.GetViolations(20);
        string HtmlString = "<HTML><HEAD><TITLE>Нарушения</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список нарушений</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Код</TH>";
        HtmlString += "<TH>Сумма штрафа</TH>";
        HtmlString += "<TH>Срок оплаты</TH>";
        HtmlString += "<TH>Срок исправления</TH>";
        HtmlString += "</TR>";
        foreach (var violation in violations)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + violation.Id + "</TD>";
            HtmlString += "<TD>" + violation.Fine + "</TD>";
            HtmlString += "<TD>" + violation.DueDate.ToString("dd.MM.yyyy") + "</TD>";
            HtmlString += "<TD>" + violation.EliminationDate.ToString("dd.MM.yyyy") + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/inspections", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedInspectors cachedInspectors = context.RequestServices.GetService<ICachedInspectors>();
        cachedInspectors.AddInspectors("Inspectors20", 1000);
        ICachedInspections cachedInspections = context.RequestServices.GetService<ICachedInspections>();
        IEnumerable<Inspection> inspections = cachedInspections.GetInspections(20);
        string HtmlString = "<HTML><HEAD><TITLE>Инспекции</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список инспекций</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Код</TH>";
        HtmlString += "<TH>Фамилия инспектора</TH>";
        HtmlString += "<TH>Дата</TH>";
        HtmlString += "</TR>";
        foreach (var inspection in inspections)
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + inspection.Id + "</TD>";
            HtmlString += "<TD>" + inspection.Inspector.SecondName + "</TD>";
            HtmlString += "<TD>" + inspection.Date.ToString("dd.MM.yyyy") + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/searchInspectors", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        string inspectorSecondName;
        context.Request.Cookies.TryGetValue("inspectorSecondName", out inspectorSecondName);
        ICachedInspectors cachedInspectors = context.RequestServices.GetService<ICachedInspectors>();
        IEnumerable<Inspector> inspectors = cachedInspectors.GetInspectors(1000);
        string HtmlString = "<HTML><HEAD><TITLE>Инспекторы по фамилии</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список инспекторов</H1>" +
        "<BODY><FORM action ='/searchInspectors' / >" +
        "Фамилия:<BR><INPUT type = 'text' name = 'inspectorSecondName' value = " + inspectorSecondName + ">" +
        "<BR><BR><INPUT type ='submit' value='Сохранить в cookies и вывести инспекторов с заданной фамилией'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Код</TH>";
        HtmlString += "<TH>Фамилия</TH>";
        HtmlString += "<TH>Имя</TH>";
        HtmlString += "<TH>Отчество</TH>";
        HtmlString += "<TH>Подразделение</TH>";
        HtmlString += "</TR>";
        inspectorSecondName = context.Request.Query["inspectorSecondName"];
        if (inspectorSecondName != null)
        {
            context.Response.Cookies.Append("inspectorSecondName", inspectorSecondName);
        }
        foreach (var inspector in inspectors.Where(i => i.SecondName.Trim() == inspectorSecondName))
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + inspector.Id + "</TD>";
            HtmlString += "<TD>" + inspector.SecondName + "</TD>";
            HtmlString += "<TD>" + inspector.FirstName + "</TD>";
            HtmlString += "<TD>" + inspector.MiddleName + "</TD>";
            HtmlString += "<TD>" + inspector.Subdivision + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Map("/searchViolations", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        int fine;
        if (context.Session.Keys.Contains("fine"))
        {
            fine = context.Session.Get<int>("fine");
        }
        fine = Convert.ToInt32(context.Request.Query["fine"]);
        ICachedViolations cachedViolations = context.RequestServices.GetService<ICachedViolations>();
        IEnumerable<Violation> violations = cachedViolations.GetViolations(20);
        string HtmlString = "<HTML><HEAD><TITLE>Нарушения по штрафу</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>Список нарушений по штрафу</H1>" +
        "<BODY><FORM action = '/searchViolations'/>" +
        "Штраф: <BR><INPUT type = 'text' name = 'fine' value = " + fine + ">" +
        "<BR><BR><INPUT type = 'submit' value = 'Сохранить в сессию и вывести нарушения со штрафом больше заданного'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>Код</TH>";
        HtmlString += "<TH>Сумма штрафа</TH>";
        HtmlString += "<TH>Срок оплаты</TH>";
        HtmlString += "<TH>Срок исправления</TH>";
        HtmlString += "</TR>";
        foreach (var violation in violations.Where(v => v.Fine > fine))
        {
            HtmlString += "<TR>";
            HtmlString += "<TD>" + violation.Id + "</TD>";
            HtmlString += "<TD>" + violation.Fine + "</TD>";
            HtmlString += "<TD>" + violation.DueDate.ToString("dd.MM.yyyy") + "</TD>";
            HtmlString += "<TD>" + violation.EliminationDate.ToString("dd.MM.yyyy") + "</TD>";
            HtmlString += "</TR>";
        }
        HtmlString += "</TABLE>";
        HtmlString += "<BR><A href='/'>Главная</A></BR>";
        HtmlString += "</BODY></HTML>";

        await context.Response.WriteAsync(HtmlString);
    });
});

app.Run((context) =>
{
    ICachedInspectors cachedInspectors = context.RequestServices.GetService<ICachedInspectors>();
    cachedInspectors.AddInspectors("Inspectors20", 1000);
    ICachedInspections cachedInspections = context.RequestServices.GetService<ICachedInspections>();
    cachedInspections.AddInspections("Inspections20", 1000);
    ICachedViolations cachedViolations = context.RequestServices.GetService<ICachedViolations>();
    cachedViolations.AddViolations("Violations20", 1000);
    string HtmlString = "<HTML><HEAD><TITLE>Главная</TITLE></HEAD>" +
    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
    "<BODY><H1>Главная</H1>";
    HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
    HtmlString += "<BR><A href='/'>Главная</A></BR>";
    HtmlString += "<BR><A href='/inspections'>Инспекции</A></BR>";
    HtmlString += "<BR><A href='/violations'>Нарушения</A></BR>";
    HtmlString += "<BR><A href='/inspectors'>Инспекторы</A></BR>";
    HtmlString += "<BR><A href='/searchInspectors'>Поиск по инспекторам</A></BR>";
    HtmlString += "<BR><A href='/searchViolations'>Поиск по нарушениям</A></BR>";
    HtmlString += "<BR><A href='/info'>Информация о клиенте</A></BR>";
    HtmlString += "</BODY></HTML>";

    return context.Response.WriteAsync(HtmlString);
});

app.Run();