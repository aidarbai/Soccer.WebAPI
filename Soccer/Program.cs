using Soccer.BLL.Services;
using Soccer.DAL.Repositories;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using MediatR;
using Serilog;
using Soccer.BLL.Services.Interfaces;
using Soccer.DAL.Repositories.Interfaces;
using FluentValidation.AspNetCore;
using Soccer.BLL.Validators;
using FluentValidation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

string logPath = "";

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Configuration.AddJsonFile("Settings\\config.json", optional: false, reloadOnChange: false);
    logPath = "Logs\\log.txt";
}
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    builder.Configuration.AddJsonFile("Settings/config.json", optional: false, reloadOnChange: false);
    logPath = "Logs/log.txt";
}

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day));

// Add services to the container.

builder.Services.AddTransient<IImportService, ImportService>();
builder.Services.AddTransient<IHttpClientService, HttpClientService>();
builder.Services.AddTransient<ILeagueService, LeagueService>();
builder.Services.AddTransient<ITeamService, TeamService>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IHttpClientService, HttpClientService>();
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IPlayerRepository, PlayerRepository>();
builder.Services.AddTransient<ITeamRepository, TeamRepository>();
builder.Services.AddTransient<ILeagueRepository, LeagueRepository>();


builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
        .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<PlayerSearchByParametersModelValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader())
);

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
