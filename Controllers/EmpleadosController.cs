using Microsoft.AspNetCore.Mvc;
using EmployerSection.Models;
using Microsoft.EntityFrameworkCore;
using EmployerSection.Data;

namespace EmployerSection.Controllers
{
    public class EmpleadosController : Controller
    {

        public readonly BaseContext _context;
    
        public EmpleadosController(BaseContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            var estadoUser = HttpContext.Session.GetString("Estado");

            if(estadoUser=="Online")
            {
                ViewBag.Nombre = HttpContext.Session.GetString("Nombre"); // Variable de session para la vista
                ViewBag.Apellidos = HttpContext.Session.GetString("Apellidos"); 
                ViewBag.Correo = HttpContext.Session.GetString("Correo"); 

                ViewBag.HoraEntrada = HttpContext.Session.GetString("HoraEntrada"); 
                ViewBag.HoraSalida = HttpContext.Session.GetString("HoraSalida"); 
            }

            ViewBag.estadoUser = HttpContext.Session.GetString("Estado");
 
            return View();
        }


        public async Task<IActionResult> EmpleadosList()
        {
            var empleados = await _context.Empleados.ToListAsync();
            return View(empleados);
        }

        // SISTEMA DE INGRESO DE JORNADA LABORAL

         public async Task<IActionResult> Ingreso()
        {
            return View();
        }

        public async Task<IActionResult> IngresoHora()
        {

            ViewBag.HoraEntrada = HttpContext.Session.GetString("HoraEntrada"); 
            // Obtener el correo electrónico del usuario logeado de la sesión
            var correo = HttpContext.Session.GetString("Correo");
            
            // Buscar al empleado en la base de datos basado en el correo electrónico
            var usuarioLogeado = await _context.Empleados.FirstOrDefaultAsync(m => m.Correo == correo);

            // Actualizar la hora de entrada del empleado
            usuarioLogeado.Hora_Entrada = DateTime.Now;
            
            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
            
                // Actualizar la sesión con la nueva hora de entrada y salida
            ViewBag.HoraEntrada =   usuarioLogeado.Hora_Entrada;


        // Redirigir a donde sea necesario después de registrar la hora de entrada
        return RedirectToAction("Index"); 
        }

        public async Task<IActionResult> Salida()
        {
            return View();
        }



         public async Task<IActionResult> IngresoSalida()
        {
            ViewBag.Nombres = HttpContext.Session.GetString("Nombre");
            ViewBag.HoraEntrada = HttpContext.Session.GetString("HoraSalida"); 
            // Obtener el correo electrónico del usuario logeado de la sesión
            var correo = HttpContext.Session.GetString("Correo");
            
            // Buscar al empleado en la base de datos basado en el correo electrónico
            var usuarioLogeado = await _context.Empleados.FirstOrDefaultAsync(m => m.Correo == correo);

            // Actualizar la hora de entrada del empleado
            usuarioLogeado.Hora_Salida = DateTime.Now;
            
            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
            
                // Actualizar la sesión con la nueva hora de entrada y salida
            ViewBag.HoraSalida =   usuarioLogeado.Hora_Salida;


        // Redirigir a donde sea necesario después de registrar la hora de entrada
            return RedirectToAction("Index"); 
        }

        public async Task<IActionResult> EliminarEmpleado(int? id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado); 
            await _context.SaveChangesAsync(); 
            return RedirectToAction("EmpleadosList"); 
        }

        public IActionResult Create(){
            return View();
        }
        [HttpPost]
        public  IActionResult Create(Empleado empleado){
             _context.Empleados.Add(empleado);
             _context.SaveChanges();
             return RedirectToAction("EmpleadosList");
        }

       public async Task<IActionResult> CerrarSession()
        {
            var correo = HttpContext.Session.GetString("Correo");
            
            // Buscar al empleado en la base de datos basado en el correo electrónico
            var usuarioLogeado = await _context.Empleados.FirstOrDefaultAsync(m => m.Correo == correo);

            // Cambiar el estado del usuario a "Offline"
            usuarioLogeado.Estado = "Offline";
            
            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
        
            // Redirigir al usuario 
            return RedirectToAction("Index", "Home");
        }
    }
}