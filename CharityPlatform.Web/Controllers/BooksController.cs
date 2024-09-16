using CharityPlatform.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CharityPlatform.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            return View(_bookService.GetAllBooks());
        }
    }
}
