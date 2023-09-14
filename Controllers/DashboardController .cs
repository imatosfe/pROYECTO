using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pROYECTO.Models;

namespace pROYECTO.Controllers
{

   /* [Authorize]*/
    public class DashboardController : Controller
    {
        public IActionResult Index(string responseData)
        {
            var responseDataArray = JsonConvert.DeserializeObject<ConsultaResponse[]>(responseData);

            // Aquí puedes usar los datos para generar la vista
            return View(responseDataArray);
        }

        /*
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("IniciarSesion", "Inicio");
        }*/
    }
}
