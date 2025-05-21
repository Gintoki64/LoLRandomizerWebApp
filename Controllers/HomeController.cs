using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using LoLRandomizerWebApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LoLRandomizerWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public HomeController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(List<string> jugadores)
        {
            string rutaCampeones = Path.Combine(_env.WebRootPath, "data", "champions_por_rol.txt");

            // Generar el resultado como lista de objetos
            var resultado = LoLRandomizer.GenerarAsignacion(jugadores, rutaCampeones);

            // Guardar el resultado serializado en TempData usando System.Text.Json
            TempData["Jugadores"] = JsonSerializer.Serialize(resultado);

            return RedirectToAction("Resultado");
        }

        public IActionResult Resultado()
        {
            if (!TempData.ContainsKey("Jugadores"))
                return RedirectToAction("Index");

            var json = TempData["Jugadores"] as string;
            var lista = JsonSerializer.Deserialize<List<ResultadoViewModel>>(json);

            return View(lista);
        }
    }
}
