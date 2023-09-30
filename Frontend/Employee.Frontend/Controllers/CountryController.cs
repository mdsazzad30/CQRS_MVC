using Employee.Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Frontend.Controllers;

public class CountryController : Controller
{
    private readonly HttpClient _httpClient;
    public CountryController(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.CreateClient("EmployeeApiBase");

    public async Task<IActionResult> Index() => View(await GetAllCountry());

    public async Task<List<Country>> GetAllCountry()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Country>>("Country");
        return response is not null ? response : new List<Country>();
    }

    [HttpGet]
    public async Task<IActionResult> AddOrEdit(int Id)
    {
        if (Id == 0)
        {
            //Creat From
            return View(new Country());
        }
        else
        {
            // Find By Id
            var data = await _httpClient.GetAsync($"Country/{Id}");
            if (data.IsSuccessStatusCode)
            {
                var result = await data.Content.ReadFromJsonAsync<Country>();
                return View(result);
            }
        }
        return View(new Country());
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> AddOrEdit(int Id, Country country)
    {
        if (ModelState.IsValid)
        {
            if (Id == 0)
            {
                var result = await _httpClient.PostAsJsonAsync("Country", country);
                if (result.IsSuccessStatusCode) return RedirectToAction("Index");
            }
            else
            {
                var result = await _httpClient.PutAsJsonAsync($"Country/{Id}", country);
                if (result.IsSuccessStatusCode) return RedirectToAction("Index");
            }
        }
        return View(new Country());
    }
    public async Task<IActionResult> Delete(int Id)
    {
        var data = await _httpClient.DeleteAsync($"Country/{Id}");
        if (data.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View(new Country());
        }
    }
}


