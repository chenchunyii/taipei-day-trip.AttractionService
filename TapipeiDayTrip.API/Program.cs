using Microsoft.EntityFrameworkCore;
using taipei_day_trip_dotnet.Data;
using taipei_day_trip_dotnet.Services;
using taipei_day_trip_dotnet.TapipeiDayTrip.Application;
using TapipeiDayTrip.Application.Interfaces;


var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
// 綁定到所有網絡接口的5197端口
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5197); // 
});
// 添加控制器
builder.Services.AddControllers();
// setting mysql
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaipeiDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// setting swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// setting cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy
                          .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                      });
});
// add services to the  container.
builder.Services.AddScoped<IAttractionService, AttractionServices>();
builder.Services.AddScoped<IAttractionRepository, AttractionRepository>();
// add automapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
