using CampaignBudgetingAPI.Data;
using CampaignBudgetingAPI.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ---------- Logging ----------
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
                 .Enrich.FromLogContext());

// ---------- Services ----------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Total-Count", "X-Page-Number", "X-Page-Size"));
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtension(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHealthChecks();

// ---------- Database ----------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Log.Warning("Connection string 'DefaultConnection' not found.");
}
else
{
    Log.Information("Using connection string: {ConnectionString}", connectionString);
    builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
}

// ---------- Build ----------
var app = builder.Build();

// ---------- Middleware ----------

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.EnableTryItOutByDefault();
        c.DisplayRequestDuration();
    });

_ = app.UseMiddleware<ExceptionMiddleware>();
// app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseHealthChecks("/health");
app.UseAuthorization();

app.MapControllers();

app.Run();
