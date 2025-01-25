using Microsoft.EntityFrameworkCore;
using FinancialAppBackend.Servieces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinancialAppBackend.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<TransactionTypeService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Finance Management API",
        Version = "v1",
        Description = "API для керування особистими фінансами"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();