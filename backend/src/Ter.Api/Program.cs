using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ter.Api.Data;
using Ter.Api.Extensions;
using Ter.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

// Base de données PostgreSQL (chaîne de connexion "Default", voir README)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Controllers + validation automatique des requêtes (dossier Validators/)
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>());
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Services métier : enregistrés automatiquement (dossier Services/)
builder.Services.AddBusinessServices();

// Réponses d'erreur standardisées (format ProblemDetails)
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.MapControllers();

app.Run();