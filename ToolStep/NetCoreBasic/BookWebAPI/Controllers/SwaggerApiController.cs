using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.IO;
using Xceed.Words.NET;

namespace BookWebAPI.Controllers
{
    [ApiController]
    [Route("swagger/api-docs")]
    public class SwaggerApiController : ControllerBase
    {
        [HttpGet("word")]
        public IActionResult GenerateWordDocument()
        {
            // Generate the Word document using a library like DocX, iTextSharp, etc.
            // Return the generated Word document as a file download.
            // Example code using DocX:
            var doc = DocX.Create("Auto API Doc");
            doc.InsertParagraph("This is the generated Word document.");
            MemoryStream stream = new MemoryStream();
            doc.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "API_Documentation.docx");
        }
    }
}
