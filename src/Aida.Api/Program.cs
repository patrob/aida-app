using Aida.Api.Extensions;
using Aida.Api.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure controllers with System.Text.Json
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = false; // compact JSON for production
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    });

// Add validation services
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddFeatures(builder.Configuration);

var app = builder.Build();

// Add exception handling middleware early in the pipeline
app.UseExceptionHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseHsts();
}

app.MapControllers();
app.UseHttpsRedirection();

app.MapFallbackToFile("index.html");
app.UseStaticFiles();

app.UseRouting();

app.Run();

public partial class Program;