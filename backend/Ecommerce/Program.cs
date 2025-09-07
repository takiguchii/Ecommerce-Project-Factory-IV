using Microsoft.EntityFrameworkCore; // Adicione este using
using Ecommerce.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// --- O BLOCO DEVE VIR AQUI ---
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<EcommerceDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
// --- FIM DO BLOCO ---

// Adicione esta linha para que os controllers funcionem
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// A "construção" da App acontece aqui, depois de todos os serviços configurados
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adicione esta linha para que a API saiba usar os controllers
app.MapControllers();

// O código do WeatherForecast pode ser removido, pois não faz parte da sua API
/*
var summaries = new[] { ... };
app.MapGet("/weatherforecast", () => { ... });
*/

app.Run();

// A record do WeatherForecast também pode ser removida
// record WeatherForecast(...)