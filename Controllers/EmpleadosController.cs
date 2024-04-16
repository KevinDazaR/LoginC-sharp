using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmpleadoLogin.Data;
using System.Security.Cryptography.X509Certificates;

namespace EmpleadoLogin.Controllers
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
            return View();
        }

        public async Task<IActionResult> EmpleadosList()
        {
            var empleados = await _context.Empleados.ToListAsync();
            return View(empleados);
        }



    }
}