using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSqlInjection.Models;
using MvcNetCoreLinqToSqlInjection.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Deportivo coye = new Deportivo();
//coye.Marca = "PONTIAC";
//coye.Modelo = "FIREBIRD";
//coye.Imagen = "kratosCar.jpg";
//coye.Velocidad = 0;
//coye.VelocidadMaxima = 900;
//builder.Services.AddTransient<ICoche, Deportivo>(x => coye);

//Resolvemos el Servicio Coche para la inyección
//builder.Services.AddTransient<Coche>();
//builder.Services.AddSingleton<Deportivo>();
//builder.Services.AddSingleton<ICoche, Deportivo>();

//NOTA, LOS REPOS SUELEN IR COMO TRANSIENT
builder.Services.AddTransient<RepositoryDoctoresSQLServer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
