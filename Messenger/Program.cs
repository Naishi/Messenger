using Messenger;
using Messenger.Domain;
using Messenger.Service;
using Messenger.Domain.Data;
using Messenger.Domain.Entities;
using Messenger.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDomainServices(builder.Configuration);
builder.Services.AddMessengerServices();
builder.Services.AddAutoMapper(typeof(MappingProfile), typeof(MappingModelEntity));

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
