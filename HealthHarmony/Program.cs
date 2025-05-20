using HealthHarmony.Extensions;
using HealthHarmony.Presentation.Controllers;
using HealthHarmony.Repositories.Contracts;
using HealthHarmony.Repositories;
using HealthHarmony.Service;
using HealthHarmony.Services.Contracts;
using Microsoft.OpenApi.Models;
using HealthHarmony.Hubs;


var builder = WebApplication.CreateBuilder(args);

//Servis kayýtlarý ve Controller yollarý
builder.Services.AddControllerPatch();
builder.Services.ServiceRegister();

builder.Services.AddCorsPolicy();

//JWT
builder.Services.AddAuthenticationJWT(builder);
builder.Services.AddAuthorization();
builder.Services.JWTSwagger();

//SignalR
builder.Services.AddSignalR();


// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");  

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.MapHub<MessageHub>("/messagehub");

app.Run();
