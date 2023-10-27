using BookSeparation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//×¢²áSwaggerDoc
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookSeparation", Version = "v1" });
});

//×¢²á DbContext
builder.Services.AddDbContext<TodoContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //add Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookSeparation v1"));

    //add Roc
    app.UseReDoc(c =>
    {
        c.DocumentTitle = "BookSeparation API Documentation";
        c.ConfigObject.HideHostname = true;
        c.SpecUrl = "/swagger/v1/swagger.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
