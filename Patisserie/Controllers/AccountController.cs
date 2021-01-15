using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Patisserie.Models;
using Patisserie.ViewModels;
using System.Threading.Tasks;

namespace Patisserie.Controllers
{
  public class AccountController : Controller
  {
    private readonly PatisserieContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, PatisserieContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    public ActionResult Index()
    {
      return View();
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel mdoel)
    {
      var user = new ApplicationUser { UserName = mdoel.Email };
      IdentityResult result = await _userManager.CreateAsync(user, mdoel.Password);
      if(result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        ViewBag.Error = result.Errors;
        return View();
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
      if(result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        ViewBag.Error = "There was an issue with your log in. Please try again.";
        return View();
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}