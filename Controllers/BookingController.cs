using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private BookingService _bookingService;

    public BookingController()
    {

        _bookingService = new BookingService();
    }

    [HttpPost]
    public IActionResult Book([FromBody] BookingRequest request)
    {
        var result = _bookingService.AssignTrains(request);

        // Write to file
        System.IO.File.WriteAllText("trains.json", System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
        return Ok(result);
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("Healthy");
    }

    [HttpPost("reset")]
    public IActionResult ResetTrains()
    {
        var originalTrains = new List<Train>
        {
            new Train { Id = 1, Name = "Train A", Capacity = 150, CurrentOccupancy = 60 },
            new Train { Id = 2, Name = "Train B", Capacity = 100, CurrentOccupancy = 60 },
            new Train { Id = 3, Name = "Train C", Capacity = 120, CurrentOccupancy = 90 }
        };

        System.IO.File.WriteAllText("trains.json", System.Text.Json.JsonSerializer.Serialize(originalTrains, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

        // Reload the booking service with the reset data
        _bookingService = new BookingService();

        return Ok(new { message = "Trains reset to original state", trains = originalTrains });
    }
}
