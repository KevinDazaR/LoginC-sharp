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
        


        // public IActionResult Index()
        // {
        //     return View();
        // }

            // ME LANZÒ ERROR PORQUE ERA ASYNC
        // public async Task<IActionResult> Index(int? id)
        // {
        //     return View(await _context.Empleados.FirstOrDefault(m => m.Id == id));
        // }

        public IActionResult Index()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre"); // Variable de session para la vista
            ViewBag.Apellidos = HttpContext.Session.GetString("Apellidos"); 
            ViewBag.Correo = HttpContext.Session.GetString("Correo"); 
            return View();
        }


        public async Task<IActionResult> EmpleadosList()
        {
            var empleados = await _context.Empleados.ToListAsync();
            return View(empleados);
        }

        // SISTEMA DE INGRESO DE JORNADA LABORAL

        // public async Task<IActionResult> Ingreso(int? id)
        // {
        //     var empleado = await _context.Empleados.FirstOrDefaultAsync(m => m.Id == id);
        //     return View(empleado);
        // }

         public async Task<IActionResult> Ingreso()
        {
            
            return View();
        }

      public IActionResult IngresoHora()
        {
            // Buscar al empleado en la base de datos
            var correo = HttpContext.Session.GetString("Correo");
            var usuarioLogeado = _context.Empleados.FirstOrDefault(m => m.Correo == correo);

            if (usuarioLogeado != null)
            {
                // Actualizar la hora de entrada del empleado
                usuarioLogeado.Hora_Entrada = DateTime.Now;
                
                // Guardar los cambios en la base de datos
                _context.SaveChanges();
                
                // Actualizar la sesión con la nueva hora de entrada solo si los cambios se guardaron correctamente
                HttpContext.Session.SetString("HoraEntrada", usuarioLogeado.Hora_Entrada.ToString());
                
                ViewBag.HoraEntrada = HttpContext.Session.GetString("HoraEntrada"); 
            }

            return RedirectToAction("Index"); // Redirigir a donde sea necesario después de registrar la hora de entrada
        }


         public async Task<IActionResult> Salida()
        {
            return View();
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


    }
}