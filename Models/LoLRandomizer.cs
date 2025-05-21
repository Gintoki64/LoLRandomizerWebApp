using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoLRandomizerWebApp.Models
{
    public class LoLRandomizer
    {
        static readonly string[] roles = { "TOP", "JUNGLA", "MID", "ADC", "SUPPORT" };

        public static string AsignarRoles(List<string> jugadores, string rutaCampeones)
        {

            Dictionary<string, List<string>> campeonesPorRol = LeerCampeones(rutaCampeones);
            Random rand = new Random();
            List<string> rolesDisponibles = roles.OrderBy(x => rand.Next()).ToList();

            List<string> resultado = new List<string>();
            for (int i = 0; i < jugadores.Count; i++)
            {
                string rol = rolesDisponibles[i];
                string campeon = campeonesPorRol[rol][rand.Next(campeonesPorRol[rol].Count)];
                string nombreImagen = campeon.ToLower().Replace(" ", "").Replace("'", "").Replace(".", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
                string iconoUrl = $"/images/Icons/{nombreImagen}-illustration-icon.png";
                resultado.Add($"<img src='{iconoUrl}' width='48' style='vertical-align:middle;margin-right:8px' /> {rol}: <strong>{jugadores[i]}</strong> → {campeon}");

            }

            if (jugadores.Count < 5)
            {
                string lineaVacia = rolesDisponibles[jugadores.Count];
                resultado.Add($"{lineaVacia}: ❌ (vacante)");
            }

            return string.Join(Environment.NewLine, resultado);
        }

        private static Dictionary<string, List<string>> LeerCampeones(string ruta)
        {
            string[] lineas = File.ReadAllLines(ruta);
            Dictionary<string, List<string>> resultado = new Dictionary<string, List<string>>();
            string rolActual = null;

            foreach (string linea in lineas)
            {
                string l = linea.Trim();
                if (string.IsNullOrEmpty(l) || l.StartsWith("-")) continue;

                if (roles.Contains(l))
                {
                    rolActual = l;
                    resultado[rolActual] = new List<string>();
                }
                else if (rolActual != null)
                {
                    resultado[rolActual].Add(l);
                }
            }

            return resultado;
        }
        public static List<ResultadoViewModel> GenerarAsignacion(List<string> jugadores, string rutaCampeones)
        {
            Dictionary<string, List<string>> campeonesPorRol = LeerCampeones(rutaCampeones);
            Random rand = new Random();
            List<string> rolesDisponibles = roles.OrderBy(x => rand.Next()).ToList();

            var resultado = new List<ResultadoViewModel>();

            for (int i = 0; i < jugadores.Count; i++)
            {
                string rol = rolesDisponibles[i];
                string campeon = campeonesPorRol[rol][rand.Next(campeonesPorRol[rol].Count)];
                resultado.Add(new ResultadoViewModel
                {
                    Jugador = jugadores[i],
                    Rol = rol,
                    Campeon = campeon
                });
            }

            if (jugadores.Count < 5)
            {
                string rolVacio = rolesDisponibles[jugadores.Count];
                resultado.Add(new ResultadoViewModel
                {
                    Jugador = "❌ (vacante)",
                    Rol = rolVacio,
                    Campeon = "-"
                });
            }

            return resultado;
        }

    }
}
