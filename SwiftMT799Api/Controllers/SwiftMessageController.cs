using Microsoft.AspNetCore.Mvc;
using SwiftMT799Api.Helpers;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace SwiftMT799Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SwiftMessageController : ControllerBase
    {
        private readonly ILogger<SwiftMessageController> _logger;

        public SwiftMessageController(ILogger<SwiftMessageController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Upload")]
        public IActionResult Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            string field20 = string.Empty, field21 = string.Empty, field79 = string.Empty;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.StartsWith(":20:"))
                        field20 = line.Substring(4);
                    else if (line.StartsWith(":21:"))
                        field21 = line.Substring(4);
                    else if (line.StartsWith(":79:"))
                    {
                        field79 = line.Substring(4);
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine()?.Trim();
                            if (string.IsNullOrEmpty(line) || line.StartsWith(":"))
                                break;
                            field79 += " " + line;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(field20) || string.IsNullOrEmpty(field21) || string.IsNullOrEmpty(field79))
                return BadRequest("Invalid Swift MT799 format");

            DatabaseHelper.InsertSwiftMessage(field20, field21, field79);
            _logger.LogInformation("Swift message saved to database.");

            return Ok("File uploaded and processed successfully");
        }
    }
}
