using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Helper;
using Core.Services.Base;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace Web.Controllers.Public
{
    [ApiController]
    [Route("/api/data/images")]
    public class UploadController
    {
        private readonly IConfiguration _configuration;
        private readonly ModelContext _context;
        private readonly HttpContext _httpContext;

        public UploadController(ModelContext context, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = accessor.HttpContext;
        }

        [HttpPost]
        [Route("")]
        public async Task<JsonResult> UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) throw new InvalidFileException("The image meta is invalid");
            if (string.IsNullOrWhiteSpace(upload.FileName)) throw new InvalidFileException("Invalid file name");
            var image = await Create(upload);
            return new JsonResult(new { Url = $"/api/data/images/{image.Id}" });
        }

        [HttpGet]
        [Route("{id}")]
        public FileStreamResult GetImage(string id)
        {
            var file = Get(id);
            var stream = File.OpenRead(ResolvePath(file));

            var provider = new FileExtensionContentTypeProvider();
            return provider.TryGetContentType(ResolveFileName(file), out var contentType)
                ? new FileStreamResult(stream, contentType)
                : new FileStreamResult(stream, "application/octet-stream");
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteImage(string id)
        {
            var userId = _httpContext.User.Id() ?? throw new UnauthorizedException();
            try
            {
                var current = Get(id);
                if (current.OwnerId != userId && !_httpContext.User.IsAdmin()) return new OkResult();
                File.Delete(ResolvePath(current));
                _context.Images.Remove(current);
                _context.SaveChanges();
                return new OkResult();
            }
            catch (DataNotFoundException)
            {
                return new OkResult();
            }
        }

        private Image Get(string rawId)
        {
            if (string.IsNullOrWhiteSpace(rawId)) throw new DataNotFoundException("Image not found");
            if (Guid.TryParse(rawId, out var hash))
                return _context.Images.FirstOrDefault(e => e.Id == hash) ??
                       throw new DataNotFoundException("Image not found");
            throw new DataNotFoundException("Image not found");
        }

        private async Task<Image> Create(IFormFile formFile)
        {
            var userId = _httpContext.User.Id() ?? throw new UnauthorizedException();
            if (formFile.Length > 6_000_000) throw new InvalidFileException("Image too large (max: 6MB)");
            try
            {
                System.Drawing.Image.FromStream(formFile.OpenReadStream());
            }
            catch (Exception)
            {
                throw new InvalidFileException("Invalid image file type");
            }

            var file = new Image
            {
                FileName = formFile.FileName,
                Length = formFile.Length,
                Extension = Path.GetExtension(formFile.FileName),
                OwnerId = userId,
            };
            Directory.CreateDirectory(ResolveTypeDirectory());
            await using var stream = new FileStream(ResolvePath(file), FileMode.Create);
            await formFile.CopyToAsync(stream);
            _context.Images.Add(file);
            _context.SaveChanges();
            return file;
        }

        private string ResolvePath(Image file)
        {
            return Path.Join(ResolveTypeDirectory(), ResolveFileName(file));
        }

        private static string ResolveFileName(Image file)
        {
            var name = file.Id.ToString();
            return string.IsNullOrWhiteSpace(file.Extension) ? name : $"{name}.{file.Extension}";
        }

        private string ResolveTypeDirectory()
        {
            const string type = "Images";
            return Path.Join(
                _configuration["FileStorage:BasePath"] ??
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), type);
        }
    }
}