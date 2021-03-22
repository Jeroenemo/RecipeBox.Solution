using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
    // example of how to link photos to a page
    [Route("/photos")]
    public ActionResult Photos()
    {
      return View();
    }

  }
}
