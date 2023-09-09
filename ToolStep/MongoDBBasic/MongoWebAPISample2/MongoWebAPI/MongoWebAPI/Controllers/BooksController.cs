using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoWebAPI.Models;
using MongoWebAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-5.0&tabs=visual-studio
/// </summary>
namespace MongoWebAPI.Controllers
{
    [Produces("application/json")]/*声明控制器的操作支持 application/json 的响应内容类型*/
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Get All Books from MongoDB
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [SwaggerOperation(
            Summary = "Get All Books",
            Description = "All Data from MongoDb",
            OperationId = "Get",
            Tags = new[] { "Test Swagger Operation Tags", "Books" }

        )]
        public ActionResult<List<Book>> Get() =>
            _bookService.Get();

        /// <summary>
        /// Get one Book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        /// <summary>
        /// Creates a Book.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Create
        ///     {
        ///        "id": 1,
        ///        "bookName": "Item1"
        ///     }
        ///
        /// </remarks>
        /// <param name="Book"></param>
        /// <returns>A newly created Book</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Book> Create(Book book)
        {
            _bookService.Create(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update([FromQuery, SwaggerParameter("Search keywords", Required = true)] string id, Book bookIn)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);

            return NoContent();
        }
    }
}
