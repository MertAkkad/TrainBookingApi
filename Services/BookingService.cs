using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

public class BookingService
{
    private List<Train> _trains;
    private int _totalPassengers;


    // Method to assign trains to passengers
    public BookingResponse AssignTrains(BookingRequest request)
    {
        BookingResponse response = new BookingResponse();
        response.Trains = new List<Train>();

        // Check if trains are provided
        if (request.Trains == null || request.Trains.Count == 0)
        {
            Console.WriteLine("No trains provided in the request.");
            response.isBooked = false;
            return response;
        }

        var _trains = request.Trains;
        _totalPassengers = request.PassengerCount;
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
                var hasRoom = (t.Capacity - t.CurrentOccupancy) >= request.PassengerCount;
                Console.WriteLine($"Checking {t.Name}: Occupancy ratio: {occupancyRatio:P2}, Has enough space: {hasRoom}");
                return occupancyRatio < 0.7 && hasRoom;//conditional return statement to return  the train has enough space and is less than 70% full
            });
            // Final check if an available train is returned before assigning passengers to it
            if (availableTrains != null && availableTrains.Count > 0)
            {   // Display the list of available trains
                Console.WriteLine("\nAvailable trains:");
                foreach (var train in availableTrains)
                {
                    var ratio = (double)train.CurrentOccupancy / train.Capacity;
                    Console.WriteLine($"- {train.Name}: {ratio:P2} full ({train.CurrentOccupancy}/{train.Capacity} seats)");
                }
                // Get and display the train with the largest occupancy ratio
                var selectedTrain = GetTrainWithLargestOccupancyRatio(availableTrains);
                if (selectedTrain != null)
                {
                    Console.WriteLine($"\nSelected train with highest occupancy: {selectedTrain.Name}");
                    selectedTrain.CurrentOccupancy += request.PassengerCount;
                    response.Trains.Add(selectedTrain);
                    response.isBooked = true;
                    Console.WriteLine($"Updated occupancy for {selectedTrain.Name}: {selectedTrain.CurrentOccupancy}/{selectedTrain.Capacity}");
                }
                else
                {
                    response.isBooked = false;
                    Console.WriteLine("No suitable train found.");
                }
                /*
                                // Assign passengers to a selected train
                                foreach (var p in request.Passengers)
                                {

                                    p.TrainId = selectedTrain.Id;    // Assigning the selected train id to the passenger


                                    Console.WriteLine($"Assigned {p.Name} to {selectedTrain.Name}");
                                }*/
                // Update the current occupancy of the selected train

                Console.WriteLine($"Updated occupancy for {selectedTrain.Name}: {selectedTrain.CurrentOccupancy}/{selectedTrain.Capacity}");
            }
            // If there is no trains available for this case
            else
            {
                Console.WriteLine("\nNo train available to accommodate all passengers together.");
                response.isBooked = false;
            }
        }
        else // If the passengers are not conditioned to be assigned to the same train
        {
            Console.WriteLine("\nAttempting to assign passengers to different trains...");
            // Assign passengers individually to any trains that have space and under 70% full
            for (var i = 0; i < request.PassengerCount; i++)
            {
                Console.WriteLine($"\nTrying to assign passenger {i + 1}...");
                var availableTrains = _trains.FindAll(t =>
                {
                    var occupancyRatio = (double)t.CurrentOccupancy / t.Capacity;
                    var hasSpace = t.CurrentOccupancy < t.Capacity;
                    Console.WriteLine($"Checking {t.Name}: Occupancy ratio: {occupancyRatio:P2}, Has space: {hasSpace}");
                    return occupancyRatio < 0.7 && hasSpace;
                });

                if (availableTrains != null && availableTrains.Count > 0)
                {
                    var selectedTrain = GetTrainWithLargestOccupancyRatio(availableTrains);
                    if (selectedTrain != null)
                    {
                        selectedTrain.CurrentOccupancy += 1;
                        response.Trains.Add(selectedTrain);
                        Console.WriteLine($"Assigned passenger {i + 1} to {selectedTrain.Name}");
                        Console.WriteLine($"Updated occupancy for {selectedTrain.Name}: {selectedTrain.CurrentOccupancy}/{selectedTrain.Capacity}");
                    }
                }
                else
                {
                    response.isBooked = false;
                    Console.WriteLine($"No available train for passenger {i + 1}.");
                }
            }
        }

        return response;
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
