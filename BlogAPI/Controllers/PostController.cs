using BlogAPI.Models;
using BlogAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BlogContext _blogContext;

        public PostController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        // Posztok felvitele
        [HttpPost]
        public async Task<ActionResult> AddNewPost(AddPostDTO addPostDTO)
        {
            try
            {
                var post = new Post()
                {
                    BloggerId = addPostDTO.BloggerId,
                    Title = addPostDTO.Title,
                    Content = addPostDTO.Content
                };

                if (post != null)
                {
                    await _blogContext.Posts.AddAsync(post);
                    await _blogContext.SaveChangesAsync();

                    return StatusCode(201, new
                    {
                        message = "Sikeres felvitel!",
                        result = post
                    });
                }

                return StatusCode(404, new
                {
                    message = "Sikertelen felvitel!",
                    result = post
                });

            }
            catch (Exception ex)
            {
                var errorList = new List<string> { ex.Message };

                if (ex.InnerException != null)
                {
                    errorList.Add($"Részletek: {ex.InnerException.Message}");
                }

                return StatusCode(400, new
                {
                    errors = errorList
                });
            }
        }

    }
}
