using AngularApp.Server.Data;
using AngularApp.Server.Jobs;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

// Http-�������
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// ����������� � PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Hangfire + PostgreSQL
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// ������������ ������ (DI)
builder.Services.AddScoped<CurrencyRateJob>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:2484")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowLocalhostFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Hangfire Dashboard
app.UseHangfireDashboard();

// ����������� ������� ������ ��� � 60 �����
RecurringJob.AddOrUpdate<CurrencyRateJob>(
    "update-currency-rates-every-60-minutes",
    job => job.FetchAndSaveRates(),
    "45 * * * *"
);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
