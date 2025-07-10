using System.Text.Json.Serialization;

// Models to hold train data
public class Train
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public int CurrentOccupancy { get; set; }

    public override string ToString()
    {
        return $"Train {Id} ({Name}): {CurrentOccupancy}/{Capacity} seats";
    }
}
// Model to hold single passenger data

// Model to hold booking record
public class BookingRequest
{
    // 1 if all passengers are assigned to the same train, 0 if they are assigned to different trains
    [JsonPropertyName("IsSame")]
    public bool IsSame { get; set; }
    // List of passengers
    [JsonPropertyName("PassengerCount")]
    public int PassengerCount { get; set; }
    [JsonPropertyName("trains")]
    public List<Train>? Trains { get; set; }
}

// Model to hold booking response with train data
public class BookingResponse
{
    [JsonPropertyName("isBooked")]
    public bool isBooked { get; set; }
    [JsonPropertyName("Trains")]
    public List<Train>? Trains { get; set; }
}