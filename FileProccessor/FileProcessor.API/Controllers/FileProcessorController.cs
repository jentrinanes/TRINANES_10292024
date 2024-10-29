using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FileProcessor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileProcessorController : ControllerBase
    {
        private readonly ILogger<FileProcessorController> _logger;
        private static int _fileCounter = 0;

        public FileProcessorController(ILogger<FileProcessorController> logger)
        {
            _logger = logger;            
        }

        [HttpPost]
        public async Task<IActionResult> ProcessFile(IFormFile file)
        {
            if (file == null)
            {
                _logger.Log(LogLevel.Error, "File is null");
                return BadRequest("File is null");
            }

            if (file.Length == 0)
            {
                _logger.Log(LogLevel.Error, "File is empty");
                return BadRequest("File is empty");
            }

            try
            {
                var filePath = Path.GetFileName(file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                if (Path.GetExtension(filePath) == ".csv" || file.ContentType == "text/csv")
                {
                    // calculate a simple aggregate value from the CSV file
                    var sum = 0;
                    using (var reader = new StreamReader(filePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            foreach (var value in values)
                            {
                                sum += int.Parse(value);
                            }
                        }
                    }
                }
                else if (Path.GetExtension(filePath) == ".json" || file.ContentType == "application/json")
                {
                    // perform a simple data transformation on the JSON file
                    var sum = 0;
                    using (var reader = new StreamReader(filePath))
                    {
                        var json = reader.ReadToEnd();
                        var jObject = JObject.Parse(json);
                        foreach (var item in jObject)
                        {
                            sum += item.Value.Value<int>();
                        }
                    }
                }

                _fileCounter++;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "An error occurred while processing the file");
                return StatusCode(500, "An error occurred while processing the file");
            }
           
            return Ok();
        }
    }
}
