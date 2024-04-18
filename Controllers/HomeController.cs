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
        // public async Task<IActionResult> Login(string Correo, string Contraseña)
        // {
        //     var usuarioLogeado = await _context.Empleados.FirstOrDefaultAsync(m => m.Correo == Correo && m.Contraseña == Contraseña);

        //     if (usuarioLogeado != null) 
        //     {
        //         // Establecer el valor de las variables de sesión
        //         HttpContext.Session.SetString("Nombre", usuarioLogeado.Nombres);
        //         HttpContext.Session.SetString("Apellidos", usuarioLogeado.Apellidos);
        //         HttpContext.Session.SetString("Correo", usuarioLogeado.Correo);
        //         HttpContext.Session.SetString("HoraEntrada", usuarioLogeado.Hora_Entrada.ToString());
        //         HttpContext.Session.SetString("HoraSalida", usuarioLogeado.Hora_Salida.ToString());
        //         HttpContext.Session.SetString("Estado", usuarioLogeado.Estado);
            
        //         // Cambiar el estado del usuario a "Online"
        //         usuarioLogeado.Estado = "Online";
        //         await _context.SaveChangesAsync();

        //         // Redirigir al usuario
        //         return RedirectToAction("Index", "Empleados");
        //     } 
        //     else
        //     {
        //         // Usuario no encontrado, redirigir a la página de inicio de sesión
        //         return RedirectToAction("Index");
        //     }
        // }
        public IActionResult Login(string Correo, string Contraseña)
        {
            var usuarioLogeado =  _context.Empleados.FirstOrDefault(m => m.Correo == Correo && m.Contraseña == Contraseña);

            if (usuarioLogeado != null) 
            {
                // Establecer el valor de las variables de sesión
                HttpContext.Session.SetString("Nombre", usuarioLogeado.Nombres);
                HttpContext.Session.SetString("Apellidos", usuarioLogeado.Apellidos);
                HttpContext.Session.SetString("Correo", usuarioLogeado.Correo);
                HttpContext.Session.SetString("HoraEntrada", usuarioLogeado.Hora_Entrada.ToString());
                HttpContext.Session.SetString("HoraSalida", usuarioLogeado.Hora_Salida.ToString());
                HttpContext.Session.SetString("Estado", usuarioLogeado.Estado);
            
                // Cambiar el estado del usuario a "Online"
                usuarioLogeado.Estado = "Online";
                _context.SaveChanges();

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
