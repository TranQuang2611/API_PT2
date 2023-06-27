using API_PT2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_PT2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookStoreContext _bookStoreContext = new BookStoreContext();

        public BookController(BookStoreContext bookStoreContext)
        {
            _bookStoreContext = bookStoreContext;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        public IActionResult Get()
        {
            var listBook = _bookStoreContext.Books.ToList();
            return Ok(listBook);
        }
    }
}
