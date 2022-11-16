using Microsoft.EntityFrameworkCore;
using RestApiProject.Data;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(path: "C:\\Users\\appiah\\source\\repos\\Apps\\App-logs\\log-.txt",
    outputTemplate: "{Timestamp:yyy-MM-dd HH-:mm:ss} [{level:u3}] {Message:1j}{Newline}{Exception}",
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: LogEventLevel.Information
    ).CreateLogger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////add cors 
//   builder.Services.AddCors(c => {
//            c.AddPolicy("CorePolicy", builder =>
//                    builder.AllowAnyHeader()
//                            .AllowAnyMethod()
//                            .AllowAnyOrigin());
//        });

//to use in build memory db
//builder.Services.AddDbContext<DataBaseContext>(options => options.UseInMemoryDatabase("MyAPDb"));

builder.Services.AddDbContext<DataBaseContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectToDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseCors("CorePolicy");
app.MapControllers();

app.Run();
