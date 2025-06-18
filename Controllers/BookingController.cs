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
        var bookingResult = _bookingService.AssignTrains(request);
        var trains = _bookingService.GetTrains();
        System.IO.File.WriteAllText("trains.json", System.Text.Json.JsonSerializer.Serialize(trains, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

        var response = new BookingResponse
        {
            Booking = bookingResult,
            Trains = trains
        };

        return Ok(response);
    }
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("Healthy");
    }
}
