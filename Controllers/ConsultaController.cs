
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using pROYECTO.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace pROYECTO.Controllers
{
    public class ConsultaController : Controller
    {
        private readonly HttpClient _httpClient;

      

        public ConsultaController( IHttpClientFactory httpClientFactory)
        {
        
            _httpClient = httpClientFactory.CreateClient();
       

        }


        public IActionResult Index()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewData["ShowErrorMessage"] = true;
            }

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
    
        public async Task<IActionResult> RealizarConsulta(ConsultaRequest request)
        {
            try
            {
           
                var apiUrl = "https://prod-182.westus.logic.azure.com/workflows/c42acb4b096d486587566e4b0c6a67a1/triggers/manual/paths/invoke/ConsultaEntid?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=drf4Bp1qYmJdYjAgCzTjs4CkK2m-cyOE-pTjLWnlGgU";



                var jsonRequest = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonRequest);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _httpClient.PostAsync(apiUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ConsultaResponse[]>(jsonResponse);

                    // Verificar si ciertos campos coinciden con los valores proporcionados
                    foreach (var data in responseData)
                    {
                        if (data.Identifica == request.param.Identifica && data.TipoIdentifica == int.Parse(request.param.TipoIdentifica))
                        {

                         
                            // Los campos coinciden, realizar acciones específicas
                            return RedirectToAction("Index", "Dashboard", new { responseData = JsonConvert.SerializeObject(responseData) });

                        }
                    }

                    // Si no se encontró coincidencia
                
                    TempData["ErrorMessage"] = "Los campos no coinciden con los valores proporcionados.";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.ErrorMessage = "Error en la solicitud a la API.";
          
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "El reCAPTCHA no se pasó correctamente.";
                return RedirectToAction("Index");

            }
            TempData["ErrorMessage"] = "Los campos no coinciden con los valores proporcionados.";
            return RedirectToAction("Index");
        }
    }
}


/*
 * 
 * esto es Con recapcha
 * 
 * 
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using pROYECTO.Models; // Asegúrate de que el espacio de nombres sea correcto
using reCAPTCHA.AspNetCore;

namespace pROYECTO.Controllers
{
    public class ConsultaController : Controller
    {
        private readonly HttpClient _httpClient;

        public ConsultaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealizarConsulta(ConsultaRequest request)
        {
            try
            {
                var recaptchaResponse = Request.Form["g-recaptcha-response"];
                var secretKey = "6LdwABsoAAAAAGtw55nNfGTfEtPrGbMkZUg9kY4o"; // Reemplaza con tu clave secreta

                var verificationResult = await VerifyRecaptchaAsync(recaptchaResponse, secretKey);

                if (verificationResult != null && verificationResult.success)
                {
                    // El reCAPTCHA se pasó correctamente, procesa el formulario

                    var apiUrl = "https://prod-182.westus.logic.azure.com/workflows/c42acb4b096d486587566e4b0c6a67a1/triggers/manual/paths/invoke/ConsultaEntid?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=drf4Bp1qYmJdYjAgCzTjs4CkK2m-cyOE-pTjLWnlGgU";

                    var jsonRequest = JsonConvert.SerializeObject(request);
                    var httpContent = new StringContent(jsonRequest);
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var response = await _httpClient.PostAsync(apiUrl, httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<ConsultaResponse[]>(jsonResponse);

                        // Verificar si ciertos campos coinciden con los valores proporcionados
                        foreach (var data in responseData)
                        {
                            if (data.Identifica == request.param.Identifica && data.TipoIdentifica == int.Parse(request.param.TipoIdentifica))
                            {
                                // Los campos coinciden, realizar acciones específicas
                                return RedirectToAction("Index", "Dashboard", new { responseData = JsonConvert.SerializeObject(responseData) });
                            }
                        }

                        // Si no se encontró coincidencia
                        ViewBag.ErrorMessage = "Los campos no coinciden con los valores proporcionados.";
                        return View("Error");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error en la solicitud a la API.";
                        return View("Error");
                    }
                }
                else
                {
                    // El reCAPTCHA no se pasó correctamente, maneja el error aquí
                    ViewBag.ErrorMessage = "El reCAPTCHA no se pasó correctamente.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        private async Task<RecaptchaResponse> VerifyRecaptchaAsync(string recaptchaResponse, string secretKey)
        {
            try
            {
                var httpClient = new HttpClient();
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"secret", secretKey},
                    {"response", recaptchaResponse}
                });

                var response = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RecaptchaResponse>(jsonString);

                return result;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir al verificar el reCAPTCHA
                return null;
            }
        }
    }
}
*/