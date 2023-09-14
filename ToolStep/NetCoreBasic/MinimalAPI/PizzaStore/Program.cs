using Microsoft.OpenApi.Models;
using PizzaStore.DB;

//构建器
var builder = WebApplication.CreateBuilder(args);
//构建应用实例
//构建器有一个Services属性。通过使用Services属性，可以添加CORS、EFCore或Swagger等特性。
//如builder.Services.AddCors(options => {});
//In the Services property, you tell the API that here's a capability to use.

//启用断点预览
builder.Services.AddEndpointsApiExplorer();
//This method sets up header information on your API
builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
   });

//the app instance is used to actually use it
//You can also use app instance to add middleware. Here's an example of how you would use a capability like CORS:
//app.UseCors("some unique string");
var app = builder.Build();

//tell the API project to use Swagger and also where to find the specification file swagger.json.
app.UseSwagger();
app.UseSwaggerUI(c =>
   {
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
   });

app.MapGet("/", () => "Hello World!");

//Sample CRUD Operation
//app.MapGet("/products", () => data);
//app.MapGet("/products/{id}", (int id) => data.SingleOrDefault(product => product.Id == id));
//app.MapPost("/products", (Product product) => /**/);
//app.MapPut("/products", (Product product) => /* Update the data store using the `product` instance */);
//app.MapDelete("/products/{id}", (int id) => /* Remove the record whose unique identifier matches `id` */);

app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));
app.MapGet("/pizzas", () => PizzaDB.GetPizzas());
app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));
app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));
app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));


//app.Run() starts your API and makes it listen for requests from the client.
app.Run();
