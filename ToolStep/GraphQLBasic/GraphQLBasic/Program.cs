//https://developer.okta.com/blog/2020/08/24/simple-graphql-csharp
//https://chillicream.com/docs/hotchocolate/v13/get-started-with-graphql-in-net-core

using GraphQLBasic.Database;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.AspNetCore;
using Microsoft.EntityFrameworkCore;
using GraphQLBasic.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TimeGraphContext>(context =>
{
    context.UseInMemoryDatabase("TimeGraphServer");
});

builder.Services.AddGraphQLServer()
                //.AddType<ProjectType>()
                //.AddType<TimeLogType>()
                .AddQueryType<Query>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UsePlayground(new PlaygroundOptions
    {
        QueryPath = "/api",
        Path = "/playground"
    });
}

app.MapGraphQL("/api");

//app.MapGet("/", () => "Hello World!");

app.Run();
