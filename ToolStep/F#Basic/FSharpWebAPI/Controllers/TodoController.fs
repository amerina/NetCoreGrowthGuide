namespace FSharpWebAPI.Controllers
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open FSharpWebAPI

module TodoController
type TodoController (logger : ILogger<WeatherForecastController>)= 
    inherit ControllerBase()
