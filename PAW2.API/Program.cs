using PAW2.Business;
using PAW2.Repositories;
using System.Text.Json.Serialization;
using PAW2.Business.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//***************************************************************************
builder.Services.AddScoped<IBusinessCatalog, BusinessCatalog>();
builder.Services.AddScoped<IRepositoryCatalog, RepositoryCatalog>();

builder.Services.AddScoped<IBusinessCategory, BusinessCategory>();
builder.Services.AddScoped<IRespositoryCategory, RepositoryCategory>();

builder.Services.AddScoped<IBusinessProduct, BusinessProduct>();
builder.Services.AddScoped<IRepositoryProduct, RepositoryProduct>();

builder.Services.AddScoped<IBusinessInventory, BusinessInventory>();
builder.Services.AddScoped<IRepositoryInventory, RepositoryInventory>();

builder.Services.AddScoped<IBusinessUser, BusinessUser>();
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();

builder.Services.AddScoped<IBusinessUserRole, BusinessUserRole>();
builder.Services.AddScoped<IRepositoryUserRole, RepositoryUserRole>();

builder.Services.AddScoped<IBusinessNotification, BusinessNotification>();
builder.Services.AddScoped<IRepositoryNotification, RepositoryNotification>();

builder.Services.AddScoped<IBusinessSupplier, BusinessSupplier>();
builder.Services.AddScoped<IRepositorySupplier, RepositorySupplier>();

// Decorator
builder.Services.AddScoped<ICatalogValidator, CatalogValidator>();
builder.Services.Decorate<IBusinessCatalog, CatalogValidationDecorator>();

builder.Services.AddScoped<ICategoryValidator, CategoryValidator>();
builder.Services.Decorate<IBusinessCategory, CategoryValidationDecorator>();

builder.Services.AddScoped<IInventoryValidator, InventoryValidator>();
builder.Services.Decorate<IBusinessInventory, InventoryValidationDecorator>();
//***************************************************************************



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
