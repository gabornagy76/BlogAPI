using BlogAPI.Models;
using BlogAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<ActionResult> GetAllPost()
        {
            try
            {
                return Ok(new
                {
                    message = "Sikeres lekérdezés!",
                    result = await _blogContext.Posts.ToListAsync()
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

        [HttpGet("id")]
        // 
        public async Task<ActionResult> GetPostById([FromQuery] int id)
        {
            try
            {
                var post = await _blogContext.Posts.FindAsync(id);

                if (post != null)
                {
                    return Ok(new
                    {
                        message = "Sikeres lekérdezés!",
                        result = post
                    });
                }

                return StatusCode(404, new
                {
                    message = "Sikertelen lekérdezés!",
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

        // Frissítő lekerédezés - Update
        [HttpPut]
        // 2 paraméter kell a frissítő lekérdezéshez: id - query-kénet, és az adatok a törzsben
        public async Task<ActionResult> UpdatePost([FromQuery] int id, [FromBody] UpdatePostDTO updatePostDTO)
        {
            try
            {
                var post = await _blogContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

                if (post != null)
                {
                    post.Title = updatePostDTO.Title;
                    post.BloggerId = updatePostDTO.BloggerId;
                    post.Content = updatePostDTO.Content;

                    _blogContext.Posts.Update(post);
                    await _blogContext.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "Sikeres frissítés!",
                        result = post
                    });
                }

                return StatusCode(404, new
                {
                    message = "Nincs ilyen poszt!"
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
