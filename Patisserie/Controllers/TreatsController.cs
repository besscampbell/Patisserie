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
  public class TreatsController : Controller 
  {
    private readonly PatisserieContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public TreatsController(UserManager<ApplicationUser> userManager, PatisserieContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    [Authorize]
    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userTreats = _db.Treats.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(userTreats);
    }

    [Authorize]
     public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      treat.User = currentUser;
      _db.Treats.Add(treat);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = treat.TreatId });
    }
    public ActionResult Details(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ViewBag.IsCurrentUser = userId != null? userId == thisTreat.User.Id : false;
      return View(thisTreat);
    }

    [Authorize]
    public async Task<ActionResult> Edit(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var thisTreat = _db.Treats.Where(entry => entry.User.Id == currentUser.Id).FirstOrDefault(treats => treats.TreatId == id);
      if(thisTreat == null)
      {
        return RedirectToAction("Details", new { id = id});
      }
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = treat.TreatId});
    }

    [Authorize]
    public async Task<ActionResult> AddFlavor(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var thisTreat = _db.Treats.Where(entry => entry.User.Id == currentUser.Id).FirstOrDefault(treats => treats.TreatId == id);
      if(thisTreat == null)
      {
        return RedirectToAction("Details", new { id = id});
      }
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int FlavorId)
    {
      if(FlavorId != 0)
      {
        var returnedJoin = _db.Pastries
          .Any(join => join.FlavorId == FlavorId && join.TreatId == treat.TreatId);
          if(!returnedJoin)
          {
            _db.Pastries.Add(new Pastry() { TreatId = treat.TreatId, FlavorId = FlavorId });
          }
      }
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = treat.TreatId});
    }
  }
}
