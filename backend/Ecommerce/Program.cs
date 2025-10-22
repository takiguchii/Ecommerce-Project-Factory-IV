// --- Usings Existentes ---
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data.Seed; 
using Ecommerce.Data.Context;
using Ecommerce.Interfaces;
using Ecommerce.Repositories;
using Ecommerce.Service;
using Ecommerce.Interfaces.Repositories; 
using Ecommerce.Interfaces.Services; 

// --- USINGS NOVOS PARA AUTENTICAÇÃO ---
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Conexão DB (Existente) ---
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<EcommerceDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// --- Repositories e Services (Existentes) ---
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProviderService, ProviderService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();

builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// --- Controllers (Existente) ---
builder.Services.AddControllers();

// =======================================================
// ============= INÍCIO: BLOCO DE AUTENTICAÇÃO ===========
// =======================================================
        
// 1. Configura o ASP.NET Core Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>() 
    .AddEntityFrameworkStores<EcommerceDbContext>() 
    .AddDefaultTokenProviders(); 

// 2. Configura a Autenticação (JWT)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// 3. Configura o JWT Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // Em desenvolvimento. Mude para true em produção.
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

// 4. Adiciona a Autorização (para usar o [Authorize])
builder.Services.AddAuthorization();
        
// =======================================================
// ============== FIM: BLOCO DE AUTENTICAÇÃO =============
// =======================================================


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CORS (Existente) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // URL do seu frontend Vue
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// --- SeedData (Existente) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EcommerceDbContext>();
    context.Database.EnsureCreated(); 
    SeedData.Initialize(context);
}

// --- Swagger (Existente) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

// --- ADICIONADOS (Importante a ordem) ---
app.UseAuthentication();
app.UseAuthorization();
        
app.MapControllers();
        
app.Run();