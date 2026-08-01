using market_place;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Añadir Configuración
builder.Services.Configure<GroqSettings>(
    builder.Configuration.GetSection("Groq"));

builder.Services.AddHttpClient<IGroqService, GroqService>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AgroMarket Local API",
        Version = "v1",
        Description = "API de AgroMarket Local: catálogo de productos agrícolas, perfiles de agricultores, pedidos, entregas, reseñas y recomendaciones con IA.",
        Contact = new OpenApiContact
        {
            Name = "Equipo AgroMarket Local"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Inicializar la base de datos desde el archivo schema.sql si no existe.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var dataSource = new SqliteConnectionStringBuilder(builder.Configuration.GetConnectionString("Default")).DataSource;
    if (!Path.IsPathRooted(dataSource))
        dataSource = Path.Combine(builder.Environment.ContentRootPath, dataSource);

    if (!File.Exists(dataSource))
    {
        var schemaPath = Path.Combine(builder.Environment.ContentRootPath, "..", "schema.sql");
        db.Database.ExecuteSqlRaw(File.ReadAllText(schemaPath));
    }
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AgroMarket Local API v1"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
