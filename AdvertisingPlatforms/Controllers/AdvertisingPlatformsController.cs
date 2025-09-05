using AdvertisingPlatforms.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingPlatforms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertisingPlatformsController : ControllerBase
    {
        private readonly PlatformsTreeNode _root;

        public AdvertisingPlatformsController(PlatformsTreeNode root)
        {
            _root = root;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoadPlatforms([FromForm] FileUploadModel model)
        {
            var file = model.File;
            if (file == null || file.Length == 0) return BadRequest("File is empty");

            _root.Location = string.Empty;
            _root.Platforms = new();
            _root.Children = new();

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var platformAndLocations = line.Split(':');
                if (platformAndLocations.Length != 2) return BadRequest("Incorrect form of file");
                if (string.IsNullOrWhiteSpace(platformAndLocations[0])) BadRequest("Incorrect form of file");
                var locations = platformAndLocations[1].Split(',');
                foreach (var location in locations)
                {
                    if (string.IsNullOrWhiteSpace(location) || location[0] != '/') return BadRequest("Incorrect form of file");
                    var parts = location.Substring(1).Split('/');
                    if (parts.Length == 0) BadRequest("Incorrect form of file");
                    if (!string.IsNullOrWhiteSpace(_root.Location) && parts[0] != _root.Location) BadRequest("Incorrect form of file");
                    _root.Location = parts[0];
                    var lastNode = _root;
                    foreach (var part in parts.Skip(1))
                    {
                        var currentNode = lastNode.Children.FirstOrDefault(x => x.Location == part);
                        if (currentNode == null)
                        {
                            currentNode = new PlatformsTreeNode(part);
                            lastNode.Children.Add(currentNode);
                        }
                        lastNode = currentNode;
                    }
                    lastNode.Platforms.Add(platformAndLocations[0]);
                }
            }

            return Ok();
        }

        [HttpGet]
        public ActionResult<List<string>> GetPlatformsByLocation([FromQuery] string location)
        {
            if (string.IsNullOrWhiteSpace(_root.Location)) return BadRequest("System is not initialized");
            if (string.IsNullOrWhiteSpace(location) || location[0] != '/') return BadRequest("Incorrect location");
            var parts = location.Substring(1).Split('/');
            if (_root.Location != parts[0]) return BadRequest("Incorrect location");

            var result = new List<string>(_root.Platforms);
            var lastNode = _root;
            foreach (var part in parts.Skip(1))
            {
                var currentNode = lastNode.Children.FirstOrDefault(x => x.Location == part);
                if (currentNode == null) return BadRequest("Incorrect location");
                result.AddRange(currentNode.Platforms);
                lastNode = currentNode;
            }

            return result.Distinct().ToList();
        }
    }
}
