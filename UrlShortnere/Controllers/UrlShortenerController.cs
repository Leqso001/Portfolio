using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private static Dictionary<string, UrlMapping> _urlStore = new();
        private static readonly Random _random = new();

        [HttpPost("shorten")]
        public IActionResult ShortenUrl([FromBody] string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl) || !Uri.IsWellFormedUriString(originalUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid URL.");
            }

            string shortId = GenerateShortId();
            _urlStore[shortId] = new UrlMapping { Id = shortId, OriginalUrl = originalUrl };

            string shortUrl = $"{Request.Scheme}://{Request.Host}/api/UrlShortener/{shortId}";
            return Ok(new { shortUrl, originalUrl });
        }

        [HttpGet("{shortId}")]
        public IActionResult RedirectUrl(string shortId)
        {
            if (_urlStore.TryGetValue(shortId, out UrlMapping mapping))
            {
                return Redirect(mapping.OriginalUrl);
            }

            return NotFound("Short URL not found.");
        }

        private string GenerateShortId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
