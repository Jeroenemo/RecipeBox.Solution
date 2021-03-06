using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System;

namespace RecipeBox.Controllers
{
  [Authorize]
  public class RecipesController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public RecipesController(RecipeBoxContext db, UserManager<ApplicationUser> userManager)
    {
      _db = db;
      _userManager = userManager;
    }

    // public async Task<ActionResult> Index()
    // {
    //   var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   var currentUser = await _userManager.FindByIdAsync(userId);
    //   var userRecipes = _db.Recipes.Where(entry => entry.User.Id == currentUser.Id).ToList();
    //   return View(userRecipes);
    // }

    // public async Task<IActionResult> Index(string userInput)
    // {
    //   var items = from model in _db.Items
    //               select model;

    //   // This if
    //   if (!(String.IsNullOrEmpty(userInput)))
    //   {
    //     items = items.Where(model => model.Description.Contains(userInput));
    //   }
    //   return View(await items.ToListAsync());
    // }

    public async Task<ActionResult> Index(string userInput)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      if (userInput == "Rating")
      {
        var userRecipes = _db.Recipes.Where(entry => entry.User.Id == currentUser.Id).OrderByDescending(model => model.Rating).ToList();
        ModelState.Clear();
        return View(userRecipes);
      }
      else
      {
        if (!(String.IsNullOrEmpty(userInput)))
        {
          var userRecipes = _db.Recipes.Where(entry => entry.User.Id == currentUser.Id).Where(model => model.Ingredients.Contains(userInput) || model.Name.Contains(userInput)).ToList();
          return View(userRecipes);
        }
        else
        {
          var userRecipes = _db.Recipes.Where(entry => entry.User.Id == currentUser.Id).ToList();
          return View(userRecipes);
        }
      }
    }

    public ActionResult Create()
    {
      ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Recipe recipe, int TagId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      recipe.User = currentUser;
      _db.Recipes.Add(recipe);
      _db.SaveChanges();
      if (TagId != 0)
      {
        _db.TagRecipe.Add(new TagRecipe() { TagId = TagId, RecipeId = recipe.RecipeId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisRecipe = _db.Recipes
        .Include(recipe => recipe.JoinEntities)
        .ThenInclude(join => join.Tag)
        .FirstOrDefault(recipe => recipe.RecipeId == id);
      return View(thisRecipe);
    }

    public ActionResult Edit(int id)
    {
      var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Name");
      return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult Edit(Recipe recipe, int TagId)
    {
      if (TagId != 0)
      {
        _db.TagRecipe.Add(new TagRecipe() { TagId = TagId, RecipeId = recipe.RecipeId });
      }
      _db.Entry(recipe).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    // public async Task<ActionResult> AddTag(int id)
    // {
    //   var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   var currentUser = await _userManager.FindByIdAsync(userId);

    //   var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
    //   // Think the ViewBag is the way to go here - it just lists all tags
    //   ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Name");
    //   return View(thisRecipe);
    // }
    public async Task<ActionResult> AddTag(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      var userTags = _db.Tags.Where(entry => entry.User.Id == currentUser.Id).ToList(); // new line
      ViewBag.TagId = new SelectList(userTags, "TagId", "Name"); // targeting userTags instead of _db.Tags
      return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult AddTag(Recipe recipe, int TagId)
    {
      if (TagId != 0)
      {
        _db.TagRecipe.Add(new TagRecipe() { TagId = TagId, RecipeId = recipe.RecipeId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      return View(thisRecipe);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      _db.Recipes.Remove(thisRecipe);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteTag(int joinId)
    {
      var joinEntry = _db.TagRecipe.FirstOrDefault(entry => entry.TagRecipeId == joinId);
      int recipeId = joinEntry.RecipeId;
      _db.TagRecipe.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", "Recipes", new { id = recipeId });
    }
  }
}