using CampaignBudgetingAPI.Data;
using CampaignBudgetingAPI.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()      // Permite qualquer domínio (origem)
                  .AllowAnyMethod()      // Permite qualquer método HTTP (GET, POST, PUT, DELETE, etc.)
                  .AllowAnyHeader()      // Permite qualquer cabeçalho na requisição
                  .WithExposedHeaders("X-Total-Count", "X-Page-Number", "X-Page-Size"); // EXPÕE HEADERS CUSTOMIZADOS para paginação
        });
});

// Add services to the container.
_ = builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
_ = builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
_ = builder.Services.AddSwaggerExtension(builder.Configuration);
_ = builder.Services.AddAutoMapper(typeof(MappingProfile));
Console.WriteLine(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

Log.Debug("Connection String: {ConnectionString}", builder.Configuration.GetConnectionString("ConnectionStrings:DefaultConnection"));
var g = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");


_ = builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        g
       
    ));

var port = Environment.GetEnvironmentVariable("PORT");

// Se a variável PORT existir, configure o Kestrel para usá-la.
// Se você não tiver problemas, PODE IGNORAR ESTE PASSO.
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(int.Parse(port)); // Escuta em 0.0.0.0 na porta definida
    });
}
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI(c =>
    {
        c.EnableTryItOutByDefault();
        c.DisplayRequestDuration();
    });
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
