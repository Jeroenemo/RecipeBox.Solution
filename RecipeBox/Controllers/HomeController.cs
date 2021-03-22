using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
  public class HomeController : Controller
  {
    private readonly RecipeBoxContext _db;
    public HomeController(RecipeBoxContext db)
    {
      _db = db;
    }
    [HttpGet("/")]
    public ActionResult Index()
    {
      List<Tag> model = _db.Tags.ToList();
      return View(model);
    }
    // example of how to link photos to a page
    [Route("/photos")]
    public ActionResult Photos()
    {
      return View();
    }

  }
}
