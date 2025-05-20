using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoLRandomizerWebApp.Models
{
    public class LoLRandomizer
    {
        static readonly string[] roles = { "TOP", "JUNGLA", "MID", "ADC", "SUPPORT" };

        public static string AsignarRoles(List<string> jugadores)
        {
            Dictionary<string, List<string>> campeonesPorRol = LeerCampeones();
            Random rand = new Random();
            List<string> rolesDisponibles = roles.OrderBy(x => rand.Next()).ToList();

            List<string> resultado = new List<string>();
            for (int i = 0; i < jugadores.Count; i++)
            {
                string rol = rolesDisponibles[i];
                string campeon = campeonesPorRol[rol][rand.Next(campeonesPorRol[rol].Count)];
                resultado.Add($"{rol}: {jugadores[i]} → {campeon}");
            }

            if (jugadores.Count < 5)
            {
                string lineaVacia = rolesDisponibles[jugadores.Count];
                resultado.Add($"{lineaVacia}: ❌ (vacante)");
            }

            return string.Join(Environment.NewLine, resultado);
        }

        private static Dictionary<string, List<string>> LeerCampeones()
        {
            var root = AppContext.BaseDirectory;
            string ruta = Path.Combine(root, "wwwroot", "data", "champions_por_rol.txt");

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

    }
}
