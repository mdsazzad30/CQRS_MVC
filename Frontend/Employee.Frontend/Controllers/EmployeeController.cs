using Employee.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Employee.Frontend.Controllers;

public class EmployeeController : Controller
{
    private readonly HttpClient _httpClient;
    public EmployeeController(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.CreateClient("EmployeeApiBase");

    public async Task<IActionResult> Index()
    {
        var data = await GetAllEmployee();
        return View(data);
    }
    public async Task<IEnumerable<Employeess>> GetAllEmployee()
    {
        var response = await _httpClient.GetAsync("Employee");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var emp = JsonConvert.DeserializeObject<IEnumerable<Employeess>>(content);
            return emp;
        }
        return new List<Employeess>();
    }
}
