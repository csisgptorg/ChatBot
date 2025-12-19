using System.Reflection;
using AutoMapper;
using ChatBot.Application.Mapping;
using ChatBot.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Plug your provider here; using InMemory for demo purposes.
    options.UseInMemoryDatabase("chatbot");
});

builder.Services.AddSingleton<IMapper>(_ =>
{
    var assemblies = new[]
    {
        Assembly.Load("ChatBot.Application"),
        Assembly.Load("ChatBot.Domain")
    };
    return ReflectionMappingConfig.BuildMapper(assemblies);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
