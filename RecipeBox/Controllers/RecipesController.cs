// using System.Collections.Generic;
// using System;
// using Microsoft.AspNetCore.Mvc;
// using ProjectName.Models;

// namespace ProjectName.Controllers
// {
//   public class ClassNameController : Controller
//   {
//     [HttpGet("/classname")]
//     public ActionResult Index()
//     {
//       List<Class> allClass = Class.GetAll();
//       return View(allClass);
//     }

//     [HttpGet("/classname/new")]
//     public ActionResult New()
//     {
//       return View();
//     }

//     [HttpPost("/classname")]
//     public ActionResult Create(string classDescription)
//     {
//       Class newClass = new Class(classDescription);
//       return RedirectToAction("Index");
//     }

//     [HttpGet("/classname/{id}")]
//     public ActionResult Show(int id)
//     {
//       Class foundClass = Class.Find(id);
//       return View(foundClass);
//     }

//     [HttpPost("/classname/delete")]
//     public ActionResult DeleteAll()
//     {
//       Class.ClearAll();
//       return View();
//     }