using System;
using System.Collections.Generic;
using System.Linq;

public class BookingService
{
    private List<Train> _trains;
    private int _totalPassengers;

    public BookingService(List<Train> trains)
    {
        _trains = trains;
        _totalPassengers = 0;
    }
    // Method to assign trains to passengers
    public BookingRequest AssignTrains(BookingRequest request)
    {
        _totalPassengers = request.Passengers.Count;
        Console.WriteLine($"\nProcessing booking request - IsSame: {request.IsSame}");
        Console.WriteLine($"Number of passengers: {_totalPassengers}");
        Console.WriteLine("\nCurrent train status:");
        foreach (var train in _trains)
        {
            Console.WriteLine(train); // Prints the overriden ToString() method of the train object (line 14 and 26)
        }

        if (request.IsSame)
        {
            Console.WriteLine("\nAttempting to find a single train for all passengers...");
            // Find a train with enough capacity and less than 70% full
            var availableTrains = _trains.FindAll(t =>
            {                                    // in case the train can only take one or two passengers before it is 70% full
                var occupancyRatio = (double)(t.CurrentOccupancy + (_totalPassengers - 1)) / t.Capacity;
                var hasRoom = (t.Capacity - t.CurrentOccupancy) >= request.Passengers.Count;
                Console.WriteLine($"Checking {t.Name}: Occupancy ratio: {occupancyRatio:P2}, Has enough space: {hasRoom}");
                return occupancyRatio < 0.7 && hasRoom;//conditional return statement to return  the train has enough space and is less than 70% full
            });
            // Final check if an available train is returned before assigning passengers to it
            if (availableTrains != null)
            {   // Display the list of available trains
                Console.WriteLine("\nAvailable trains:");
                foreach (var train in availableTrains)
                {
                    var ratio = (double)train.CurrentOccupancy / train.Capacity;
                    Console.WriteLine($"- {train.Name}: {ratio:P2} full ({train.CurrentOccupancy}/{train.Capacity} seats)");
                }
                // Get and display the train with the largest occupancy ratio
                var selectedTrain = GetTrainWithLargestOccupancyRatio(availableTrains);
                Console.WriteLine($"\nSelected train with highest occupancy: {selectedTrain.Name}");

                // Assign passengers to a selected train
                foreach (var p in request.Passengers)
                {

                    p.TrainId = selectedTrain.Id;    // Assigning the selected train id to the passenger


                    Console.WriteLine($"Assigned {p.Name} to {selectedTrain.Name}");
                }
                // Update the current occupancy of the selected train
                selectedTrain.CurrentOccupancy += request.Passengers.Count;
                Console.WriteLine($"Updated occupancy for {selectedTrain.Name}: {selectedTrain.CurrentOccupancy}/{selectedTrain.Capacity}");
            }
            // If there is no trains available for this case
            else
            {
                Console.WriteLine("\nNo train available to accommodate all passengers together.");
            }
        }
        else // If the passengers are not conditioned to be assigned to the same train
        {
            Console.WriteLine("\nAttempting to assign passengers to different trains...");
            // Assign passengers individually to any trains that have space and under 70% full
            foreach (var p in request.Passengers)
            {
                Console.WriteLine($"\nTrying to assign {p.Name}...");
                var availableTrains = _trains.FindAll(t =>
                {
                    var occupancyRatio = (double)t.CurrentOccupancy / t.Capacity;
                    var hasSpace = t.CurrentOccupancy < t.Capacity;
                    Console.WriteLine($"Checking {t.Name}: Occupancy ratio: {occupancyRatio:P2}, Has space: {hasSpace}");
                    return occupancyRatio < 0.7 && hasSpace;
                });

                if (availableTrains != null)
                {
                    var selectedTrain = GetTrainWithLargestOccupancyRatio(availableTrains);
                    p.TrainId = selectedTrain.Id;
                    selectedTrain.CurrentOccupancy += 1;

                    Console.WriteLine($"Assigned {p.Name} to {selectedTrain.Name}");
                    Console.WriteLine($"Updated occupancy for {selectedTrain.Name}: {selectedTrain.CurrentOccupancy}/{selectedTrain.Capacity}");
                }
                else
                {
                    Console.WriteLine($"No available train for passenger {p.Name}.");
                }
            }
        }

        Console.WriteLine("\nFinal booking result:");
        foreach (var p in request.Passengers)
        {
            Console.WriteLine(p);
        }

        return request;
    }
    // Method to get the train with the largest occupancy ratio(optional)
    private Train GetTrainWithLargestOccupancyRatio(List<Train> availableTrains)
    {
        if (availableTrains == null || availableTrains.Count == 0)
            return null;

        var random = new Random();
        return availableTrains[random.Next(availableTrains.Count)];
    }

    public List<Train> GetTrains() => _trains;
}
