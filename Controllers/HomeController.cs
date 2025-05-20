using Microsoft.AspNetCore.Mvc;
using LoLRandomizerWebApp.Models;
using System.Collections.Generic;

namespace LoLRandomizerWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(List<string> jugadores) 
        {
            ViewBag.Resultado = LoLRandomizer.AsignarRoles(jugadores);
            ViewBag.Jugadores = jugadores;
            return View();
        }
    }
}
