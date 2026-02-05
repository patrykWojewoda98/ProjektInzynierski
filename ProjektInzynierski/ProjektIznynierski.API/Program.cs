using ProjektIznynierski.Infrastructure;
using ProjektIznynierski.Application;
using ProjektIznynierski.Presentation;
using DotNetEnv;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

Env.Load();
// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();
app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
app.UsePresentation();


app.Run();
