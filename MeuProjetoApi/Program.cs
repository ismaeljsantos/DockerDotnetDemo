using MeuProjetoApi.Data;
using MeuProjetoApi.Services;
using MeuProjetoApi.Services.Interfaces;
using MeuProjetoApi.Services.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var encryptionKey = builder.Configuration.GetValue<string>("Cpf:EncryptionKey");
if (string.IsNullOrEmpty(encryptionKey))
{
    throw new InvalidOperationException("CPF_ENCRYPTION_KEY não foi configurada. Verifique variáveis de ambiente/User Secrets.");
}
builder.Services.AddSingleton(new CpfSecurity(encryptionKey));
builder.Services.AddScoped<IPessoaService, PessoaService>();
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
