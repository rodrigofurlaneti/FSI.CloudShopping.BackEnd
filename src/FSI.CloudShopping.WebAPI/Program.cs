using FSI.CloudShopping.Application;
using FSI.CloudShopping.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FSI.CloudShopping API", Version = "v1" });
});
builder.Services.AddCors(options => {
    options.AddPolicy("ConectaStorePolicy", policy => {
        policy.WithOrigins(
                "http://localhost:4173",
                "http://localhost:5173",
                "https://fsi-cloudshopping-frontend.onrender.com" 
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FSI.CloudShopping v1"));
app.UseCors("ConectaStorePolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();