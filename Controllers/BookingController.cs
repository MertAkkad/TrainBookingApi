using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingController()
    {
        // Load trains from file (or hardcoded for now)
        var trains = System.Text.Json.JsonSerializer.Deserialize<List<Train>>(System.IO.File.ReadAllText("trains.json"));
        _bookingService = new BookingService(trains);
    }

    [HttpPost]
    public IActionResult Book([FromBody] BookingRequest request)
    {
        var result = _bookingService.AssignTrains(request);
        System.IO.File.WriteAllText("trains.json", System.Text.Json.JsonSerializer.Serialize(_bookingService.GetTrains(), new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
        return Ok(result);
    }
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("Healthy");
    }
}
