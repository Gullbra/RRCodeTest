using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("/api/")]
public class HomeController : Controller
{
  //public IActionResult Index()
  //{
  //  return View();
  //}

  [HttpGet]
  public string Index()
  {
    return "<p>Hello World!</p>";
  }
}
