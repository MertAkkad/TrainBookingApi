
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
public class Passenger
{
    public string Name { get; set; }
    // The train id that the passenger is assigned to
    public int? TrainId { get; set; }
    // Passenger information as a string    
    public override string ToString()
    {
        return $"{Name} (Train: {TrainId?.ToString() ?? "Not Assigned"})";
    }
}
// Model to hold booking record
public class BookingRequest
{
    // 1 if all passengers are assigned to the same train, 0 if they are assigned to different trains
    public bool IsSame { get; set; }
    // List of passengers
    public List<Passenger> Passengers { get; set; }
}