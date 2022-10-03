using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using snabel_up.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 

    builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" }));

  var connectionString = builder.Configuration.GetConnectionString("cs");
  builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString)
           );
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

app.UseHttpsRedirection();

app.UseCors(c=> c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
