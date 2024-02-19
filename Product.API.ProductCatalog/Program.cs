using Microsoft.EntityFrameworkCore;
using Product.API.ProductCatalog.Application;
using Product.API.ProductCatalog.Infrastructure.Configuration;
using Product.API.ProductCatalog.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Add DbContext
builder.Services.AddDbContext<ProductDbContext>(x=>x.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

//Add AutoMapper
builder.Services.AddAutoMapper(typeof(CustomMap));

//Add Services
builder.Services.AddScoped<IProductCatalog, ProductCatalog>();
builder.Services.AddScoped<CRUDService>();


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

// Configure the HTTP request pipeline For Dockerization
app.UseSwagger();
app.UseSwaggerUI();

//Add app StaticFiles
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
