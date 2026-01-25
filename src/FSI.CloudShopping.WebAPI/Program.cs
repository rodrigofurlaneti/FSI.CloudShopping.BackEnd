using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Services;
using FSI.CloudShopping.Infrastructure; // Namespace onde está o seu AddInfrastructure
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Serviços de Infraestrutura ---
// Registra o SqlDbConnector e todos os Repositories via Stored Procedures
builder.Services.AddInfrastructure(builder.Configuration);

// --- 2. Injeção de Dependência da Aplicação ---
builder.Services.AddScoped<ICustomerAppService, CustomerAppService>();
builder.Services.AddScoped<IProductAppService, ProductAppService>();
builder.Services.AddScoped<IOrderAppService, OrderAppService>();
builder.Services.AddScoped<ICartAppService, CartAppService>();
builder.Services.AddScoped<IAddressAppService, AddressAppService>();
builder.Services.AddScoped<IContactAppService, ContactAppService>();
builder.Services.AddScoped<IPaymentAppService, PaymentAppService>();

// --- 3. Configuração do AutoMapper ---
// Esta linha agora funcionará após a instalação do pacote de extensão
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --- 4. Configurações da API ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FSI.CloudShopping API", Version = "v1" });
});

var app = builder.Build();

// --- 5. Pipeline de Middleware ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FSI.CloudShopping v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();