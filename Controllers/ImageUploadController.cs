using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using carGooBackend.Services;

namespace carGooBackend.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageUploadController : ControllerBase
    {
        private readonly ImageUploadService _imageUploadService;

        public ImageUploadController(ImageUploadService imageUploadService)
        {
            _imageUploadService = imageUploadService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var result = await _imageUploadService.UploadImageAsync(file);
            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(new { url = result.Url });
        }
    }
}
