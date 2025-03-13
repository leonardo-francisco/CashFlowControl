using CashFlowControl.Api.Configuration;
using CashFlowControl.Api.Model;
using CashFlowControl.Application.Contracts;
using CashFlowControl.Application.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
AppNativeInjection.RegisterServices(builder.Services);

// Autenticação e JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Define políticas de retry e circuit breaker
builder.Services.AddPoliticasDeResiliencia();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Registrar os endpoints Minimal API separados
app.RegisterEndpoints();


app.Run();
