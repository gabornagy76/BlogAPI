using BlogAPI.Models;
using BlogAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        private readonly BlogContext _blogContext;

        public BloggerController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewBlogger(AddBloggerDTO addBloggerDTO)
        {
            try
            {
                var blogger = new Blogger()
                {
                    UserName = addBloggerDTO.UserName,
                    Password = addBloggerDTO.Password,
                    Email = addBloggerDTO.Email
                };

                if (blogger != null)
                {
                    await _blogContext.Bloggers.AddAsync(blogger);
                    await _blogContext.SaveChangesAsync();

                    return StatusCode(201, new
                    {
                        message = "Sikeres felvitel!",
                        result = blogger
                    });
                }

                return StatusCode(404, new
                {
                    message = "Sikertelen felvitel!",
                    result = blogger
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
        public async Task<ActionResult> GetAllBlogger()
        {
            try
            {
                return Ok(new
                {
                    message = "Sikeres lekérdezés!",
                    result = await _blogContext.Bloggers.ToListAsync()
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

        // A végpont megszólításánál fog várni egy paramétert, ami alapján ki fogjuk szűrni a megfelelő rekordot.
        // Id alapján szűrünk, tehát a kérés úgy fog kinézni, hogy pl.: https://localhost:7208/Blogger?id=5
        // Ilyen megadásnál nem kötelező a [FromQuery] paraméter, automatikusan így várja az adatot.
        [HttpGet("id")]
        // 
        public async Task<ActionResult> GetBloggerById([FromQuery] int id)
        {
            try
            {
                var blogger = await _blogContext.Bloggers.FindAsync(id);

                if (blogger != null)
                {
                    return Ok(new
                    {
                        message = "Sikeres lekérdezés!",
                        result = blogger
                    });
                }

                return StatusCode(404, new
                {
                    message = "Sikertelen lekérdezés!",
                    result = blogger
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
