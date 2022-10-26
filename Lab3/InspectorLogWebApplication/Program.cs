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
        string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>����������:</H1>";
        strResponse += "<BR> ������: " + context.Request.Host;
        strResponse += "<BR> ����: " + context.Request.PathBase;
        strResponse += "<BR> ��������: " + context.Request.Protocol;
        strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";
        await context.Response.WriteAsync(strResponse);
    });
});

app.Map("/inspectors", (appBuilder) =>
{
    appBuilder.Run(async (context) =>
    {
        ICachedInspectors cachedInspectors = context.RequestServices.GetService<ICachedInspectors>();
        IEnumerable<Inspector> inspectors = cachedInspectors.GetInspectors(20);
        string HtmlString = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ �����������</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>�������</TH>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>��������</TH>";
        HtmlString += "<TH>�������������</TH>";
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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>���������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ ���������</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>����� ������</TH>";
        HtmlString += "<TH>���� ������</TH>";
        HtmlString += "<TH>���� �����������</TH>";
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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>���������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ ���������</H1>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>������� ����������</TH>";
        HtmlString += "<TH>����</TH>";
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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>���������� �� �������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ �����������</H1>" +
        "<BODY><FORM action ='/searchInspectors' / >" +
        "�������:<BR><INPUT type = 'text' name = 'inspectorSecondName' value = " + inspectorSecondName + ">" +
        "<BR><BR><INPUT type ='submit' value='��������� � cookies � ������� ����������� � �������� ��������'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>�������</TH>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>��������</TH>";
        HtmlString += "<TH>�������������</TH>";
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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
        string HtmlString = "<HTML><HEAD><TITLE>��������� �� ������</TITLE></HEAD>" +
        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
        "<BODY><H1>������ ��������� �� ������</H1>" +
        "<BODY><FORM action = '/searchViolations'/>" +
        "�����: <BR><INPUT type = 'text' name = 'fine' value = " + fine + ">" +
        "<BR><BR><INPUT type = 'submit' value = '��������� � ������ � ������� ��������� �� ������� ������ ���������'></FORM>" +
        "<TABLE BORDER=1>";
        HtmlString += "<TR>";
        HtmlString += "<TH>���</TH>";
        HtmlString += "<TH>����� ������</TH>";
        HtmlString += "<TH>���� ������</TH>";
        HtmlString += "<TH>���� �����������</TH>";
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
        HtmlString += "<BR><A href='/'>�������</A></BR>";
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
    string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
    "<BODY><H1>�������</H1>";
    HtmlString += "<H2>������ �������� � ��� �������</H2>";
    HtmlString += "<BR><A href='/'>�������</A></BR>";
    HtmlString += "<BR><A href='/inspections'>���������</A></BR>";
    HtmlString += "<BR><A href='/violations'>���������</A></BR>";
    HtmlString += "<BR><A href='/inspectors'>����������</A></BR>";
    HtmlString += "<BR><A href='/searchInspectors'>����� �� �����������</A></BR>";
    HtmlString += "<BR><A href='/searchViolations'>����� �� ����������</A></BR>";
    HtmlString += "<BR><A href='/info'>���������� � �������</A></BR>";
    HtmlString += "</BODY></HTML>";

    return context.Response.WriteAsync(HtmlString);
});

app.Run();