using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployerSection.Models;
using Microsoft.EntityFrameworkCore;
using EmployerSection.Data;
using System.Threading.Tasks;

namespace EmployerSection.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BaseContext _context;

        public HomeController(ILogger<HomeController> logger, BaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // INICIO DE SESIÓN

        [HttpPost]       
        public async Task<IActionResult> Login(string Correo, string Contraseña)
        {
            var usuarioLogeado = await _context.Empleados.FirstOrDefaultAsync(m => m.Correo == Correo && m.Contraseña == Contraseña);

            if (usuarioLogeado != null)
            {
                // Establecer el valor de las variables de sesión para el usuario logeado
                HttpContext.Session.SetString("Nombre", usuarioLogeado.Nombres);
                HttpContext.Session.SetString("Apellidos", usuarioLogeado.Apellidos);
                HttpContext.Session.SetString("Correo", usuarioLogeado.Correo);
                HttpContext.Session.SetString("Ultima_Hora_Entrada", usuarioLogeado.Ultima_Hora_Entrada.ToString());
                HttpContext.Session.SetString("Ultima_Hora_Salida", usuarioLogeado.Ultima_Hora_Salida.ToString());

                // Obtener el historial de conexión del usuario logeado
                var historialConexionEmpleado = await _context.HistorialConexionEmpleado
                    .FirstOrDefaultAsync(h => h.Id_Empleado == usuarioLogeado.Id);

                if (historialConexionEmpleado != null)
                {
                    // Establecer las variables de sesión para la última hora de entrada y salida del usuario logeado
                    HttpContext.Session.SetString("Hora_Entrada", historialConexionEmpleado.Hora_Entrada.ToString());
                    HttpContext.Session.SetString("Hora_Salida", historialConexionEmpleado.Hora_Salida.ToString());
                }

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Redirigir al usuario
                return RedirectToAction("Index", "Empleados");
            }
            else
            {
                // Usuario no encontrado, redirigir a la página de inicio de sesión
                return RedirectToAction("Index");
            }
        }

    }

}
