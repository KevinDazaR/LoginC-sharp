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
        
        public async Task<IActionResult> Index()
        {

            var conexion = await _context.HistorialConexionEmpleado.ToListAsync();


            ViewBag.Id = HttpContext.Session.GetString("Id"); // Variable de session para la vista
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre"); // Variable de session para la vista
            ViewBag.Apellidos = HttpContext.Session.GetString("Apellidos"); 
            ViewBag.Correo = HttpContext.Session.GetString("Correo"); 

            ViewBag.Ultima_Hora_Entrada = HttpContext.Session.GetString("Ultima_Hora_Entrada"); 
            ViewBag.Ultima_Hora_Salida = HttpContext.Session.GetString("Ultima_Hora_Salida"); 
            
            // BD DE HISTORIAL
             ViewBag.Id_Empleado = HttpContext.Session.GetString("Id_Empleado");
            ViewBag.Hora_Entrada = HttpContext.Session.GetString("Hora_Entrada"); 
            ViewBag.Hora_Salida = HttpContext.Session.GetString("Hora_Salida"); 

            // Igualo Id a Id_Empleado
            
 
            
            return View(conexion);
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

            ViewBag.Ultima_Hora_Entrada = HttpContext.Session.GetString("Ultima_Hora_Entrada"); 

            // Obtener el correo electrónico del usuario logeado de la sesión
            var correo = HttpContext.Session.GetString("Correo");
            
            // Buscar al empleado en la base de datos basado en el correo electrónico
            var usuarioLogeado = await _context.Empleados.FirstOrDefaultAsync(m => m.Correo == correo);

            // Actualizar la hora de entrada del empleado
            usuarioLogeado.Ultima_Hora_Entrada = DateTime.Now;

            // Actualizar la sesión con la nueva hora de entrada y salida
            ViewBag.Ultima_Hora_Entrada =   usuarioLogeado.Ultima_Hora_Entrada;
            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            var historialConexionEmpleado = await _context.HistorialConexionEmpleado.FirstOrDefaultAsync(h => h.Id_Empleado == usuarioLogeado.Id);
            
            if(historialConexionEmpleado != null)
            {
                ViewBag.Hora_Entrada = HttpContext.Session.GetString("Hora_Entrada");

                var nuevaConexionEntrada = new HistorialConexionEmpleadoModel
                 {
                    Id_Empleado = usuarioLogeado.Id,
                    Hora_Entrada = usuarioLogeado.Ultima_Hora_Entrada
                };

                _context.HistorialConexionEmpleado.Add(nuevaConexionEntrada);

                ViewBag.Hora_Entrada = historialConexionEmpleado.Hora_Entrada;
                await _context.SaveChangesAsync();

            }

        return RedirectToAction("Index"); 
        }



        public async Task<IActionResult> Salida()
        {
            return View();
        }



        
        public async Task<IActionResult> IngresoSalida()
        {
            ViewBag.Nombres = HttpContext.Session.GetString("Nombre");
            ViewBag.Ultima_Hora_Salida = HttpContext.Session.GetString("Ultima_Hora_Salida"); 
            // Obtener el correo electrónico del usuario logeado de la sesión
            var correo = HttpContext.Session.GetString("Correo");
            
            // Buscar al empleado en la base de datos basado en el correo electrónico
            var usuarioLogeado = await _context.Empleados.FirstOrDefaultAsync(m => m.Correo == correo);

            // Actualizar la hora de salida del empleado
            usuarioLogeado.Ultima_Hora_Salida = DateTime.Now;
            
            ViewBag.Ultima_Hora_Salida = usuarioLogeado.Ultima_Hora_Salida;
            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
            
            // Obtener el último historial de conexión del empleado
            var ultimoHistorialConexion = await _context.HistorialConexionEmpleado
                .OrderByDescending(h => h.Hora_Entrada)
                .FirstOrDefaultAsync(h => h.Id_Empleado == usuarioLogeado.Id);
            
            if(ultimoHistorialConexion == null)
            {
                // Si no hay historial de conexión previo, crea uno nuevo
                var nuevaConexionSalida = new HistorialConexionEmpleadoModel
                {
                    Id_Empleado = usuarioLogeado.Id,
                    Hora_Entrada = usuarioLogeado.Ultima_Hora_Entrada,
                    Hora_Salida = usuarioLogeado.Ultima_Hora_Salida
                };
                _context.HistorialConexionEmpleado.Add(nuevaConexionSalida);
                }
                else
                {
                    // Si ya existe historial de conexión, actualiza la hora de salida
                    ultimoHistorialConexion.Hora_Salida = usuarioLogeado.Ultima_Hora_Salida;
                }

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Redirigir a donde sea necesario después de registrar la hora de salida
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

        // CERRA SESSION NO FUNCIONAL DEL TODO
        public async Task<IActionResult> CerrarSession()
        {
            // Redirigir al usuario 
            return RedirectToAction("Index", "Home");
        }

    }
}