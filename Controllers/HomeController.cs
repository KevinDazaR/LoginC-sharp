using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployerSection.Models;
using Microsoft.EntityFrameworkCore;
using EmpleadoLogin.Models;
using EmpleadoLogin.Data;

namespace EmpleadoLogin.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
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

    // ME

    public readonly BaseContext _context;

    public HomeController(BaseContext context)
    {
        _context = context;
    }

    // INICIO DE SESIÓN
        
    public async Task<IActionResult> Login(string Correo, string Contraseña)
    {
        var empleado = await _context.Empleados.FirstOrDefaultAsync(m => m.Correo == Correo && m.Contraseña == Contraseña);

        if(empleado != null)
        {
            return RedirectToAction("EmpleadosList", "Empleados");

        }
        else
        {
            return RedirectToAction("Index");
        }
        
    }


}
