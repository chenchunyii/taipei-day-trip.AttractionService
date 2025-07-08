using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using taipei_day_trip_dotnet.Data;
using taipei_day_trip_dotnet.Services;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application.Interfaces;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application.Services;
using taipei_day_trip_dotnet.TapipeiDayTrip.Infrastructure.Repositories;
using TapipeiDayTrip.Application.Interfaces;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// 綁定到所有網絡接口的5197端口
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5197);
});

// 添加控制器
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // 設定命名策略為 camelCase
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// setting mysql
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaipeiDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// setting swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Taipei Day Trip API",
        Version = "v1"
    });

    // 設定 Swagger 的 servers，包含反向代理的路徑
    c.AddServer(new OpenApiServer
    {
        Url = "https://chun-web-api.online/api/attraction",
        Description = "Production server (via nginx reverse proxy)"
    });

    // 本地開發環境的 server
    c.AddServer(new OpenApiServer
    {
        Url = "http://localhost:5197",
        Description = "Local development server"
    });
});

// setting cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// setting http client
builder.Services.AddHttpClient();

// add services to the container.
builder.Services.AddScoped<IAttractionService, AttractionServices>();
builder.Services.AddScoped<IAttractionRepository, AttractionRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// add automapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/attraction/swagger/v1/swagger.json", "Taipei Day Trip API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();

app.Run();