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

            if(usuarioLogeado != null)
            {
                
            }

            if ( usuarioLogeado.Estado =="Online" )
            {
                // Establecer el valor de una variable de sesión
       
                HttpContext.Session.SetString("Nombre", usuarioLogeado.Nombres);
                HttpContext.Session.SetString("Apellidos", usuarioLogeado.Apellidos);
                HttpContext.Session.SetString("Correo", usuarioLogeado.Correo);
                HttpContext.Session.SetString("HoraEntrada", usuarioLogeado.Hora_Entrada.ToString());
                HttpContext.Session.SetString("HoraSalida", usuarioLogeado.Hora_Salida.ToString());
                HttpContext.Session.SetString("Estado", usuarioLogeado.Estado);
            
                usuarioLogeado.Estado = "Online";
                await _context.SaveChangesAsync();

                // Obtener el valor de una variable de sesión
                // var sessionId = HttpContext.Session.GetString("ID");

                if(usuarioLogeado.Estado == "Online")
                {
                    return RedirectToAction("Index", "Empleados");
                }
                else 
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }

}
