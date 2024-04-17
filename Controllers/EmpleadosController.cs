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

       public IActionResult IngresoHora(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscar al empleado en la base de datos
            var empleado = _context.Empleados.Find(id);

            if (empleado == null)
            {
                return NotFound();
            }

            // Actualizar la hora de entrada del empleado
            empleado.Hora_Entrada = DateTime.Now;
            
            // Guardar los cambios en la base de datos
            _context.SaveChanges();

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