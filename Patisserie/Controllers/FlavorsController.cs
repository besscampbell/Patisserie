using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patisserie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Patisserie.Controllers
{
  [Authorize]
  public class FlavorsController : Controller 
  {
    private readonly PatisserieContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public FlavorsController(UserManager<ApplicationUser> userManager, PatisserieContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userFlavors = _db.Flavors.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(userFlavors);
    }

     public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Flavor flavor)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      flavor.User = currentUser;
      _db.Flavors.Add(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}