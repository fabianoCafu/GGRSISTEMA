using AutoMapper;
using GGR.CategoriaAPI.Service;
using GGR.Shared.Infra.ConfigMapper;
using GGR.Shared.Infra.Data;
using GGR.Shared.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MySQLContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("MySQLConnectionString");
    options.UseMySql(connection, ServerVersion.AutoDetect(connection), b => b.MigrationsAssembly("GGR.Shared.Infra"));
});

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();


builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy => 
    {
        var urlBase = builder.Configuration["ApiSettings:UrlBase"];
        policy.WithOrigins(urlBase, urlBase).AllowAnyHeader().AllowAnyMethod(); 
    });
});


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "GGR CategoriaAPI", Version = "v1" }); 
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("ReactPolicy");
app.MapControllers();
app.Run();

