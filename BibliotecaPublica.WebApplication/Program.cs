using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Registrar DbContext (ajuste a connection string em appsettings.json)
builder.Services.AddDbContext<BibliotecaPublicaDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar serviços de domínio
builder.Services.AddScoped<LivroService>();
builder.Services.AddScoped<AutorService>();
builder.Services.AddScoped<EditoraService>();
builder.Services.AddScoped<FilialService>();
builder.Services.AddScoped<ExemplarService>();
builder.Services.AddScoped<EmprestimoService>();
builder.Services.AddScoped<AssociadoService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
