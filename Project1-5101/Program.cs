using Project1_5101;
/*
 * Authors: Ayden Nicholson, William Mouhtouris, Logan McCallum 
 */

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            DisplayProgamHeader();
            Statistics stats = InitalizeStatistics();
            if (stats == null)
            {
                continue;
            }

            while (true)
            {
                Console.Clear();
                DisplayMainMenu();

                string input = Console.ReadLine()?.ToLower();
                if (input == "exit")
                {
                    return;
                }
                if (input == "restart" || input == "6")
                {
                    break;
                }

                try
                {
                    ProcessMenuChoice(input, stats);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                Console.WriteLine("\nPress any key to continue.");
                Console.ReadKey();
            }



        }
    }

    private static void DisplayProgamHeader()
    {
        Console.WriteLine("\n------------------------------");
        Console.WriteLine(" Program Functionality ");
        Console.WriteLine("------------------------------");
        Console.WriteLine("- To exit program, enter 'exit' at any prompt.");
        Console.WriteLine("- To restart program, enter 'restart' at any prompt.");
        Console.WriteLine("- You will be presented with a numbered list of options. Please enter a value.");
        Console.WriteLine("Fetching list of available file names to be processed and queried...");
        Console.WriteLine("\n1) Canadacities.csv");
        Console.WriteLine("2) Canadacities-JSON.json");
        Console.WriteLine("3) Canadacities-XML.xml");
        Console.Write("\nSelect an option from the list above (e.g., 1, 2, 3): ");

    }

    private static Statistics InitalizeStatistics()
    {
        string input = Console.ReadLine()?.ToLower();
        if (input == "exit")
        {
            Environment.Exit(0);
        }
        if (input == "restart")
        {
            return null;
        }

        try
        {
            string fileType = input switch
            {
                "1" => "csv",
                "2" => "json",
                "3" => "xml",
                _ => throw new ArgumentException("Invalid selection, please restart.")
            };

            string fileName = fileType switch
            {
                "csv" => "Canadacities.csv",
                "json" => "Canadacities-JSON.json",
                "xml" => "Canadacities-XML.xml",
                _ => throw new ArgumentException("Invalid file selected.")
            };

            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data", fileName);
            dataPath = Path.GetFullPath(dataPath);

            if (!File.Exists(dataPath))
            {
                throw new FileNotFoundException($"Could not find the file: {dataPath}");
            }

            Console.WriteLine($"\nYou selected {Path.GetFileName(dataPath)}. Processing...");
            return new Statistics(dataPath, fileType);
        }
        catch(Exception ex) 
        {
            Console.WriteLine($"\nError: {ex.Message}");
            Console.WriteLine("Press any key to restart...");
            Console.ReadKey();
            return null;
        }
    }
    private static void DisplayMainMenu()
    {
        Console.WriteLine($"\nSelect an option from the list below for the file (e.g., 1, 2):");
        Console.WriteLine("1) Display City Information");
        Console.WriteLine("2) Display Province Cities");
        Console.WriteLine("3) Calculate Province Population");
        Console.WriteLine("4) Match Cities Population");
        Console.WriteLine("5) Distance Between Cities");
        Console.WriteLine("6) Restart Program And Choose Another File Or File Type To Query");
        Console.Write("\nEnter your choice: ");
    }
    private static void ProcessMenuChoice(string input, Statistics stats)
    {
        switch (input)
        {
            case "1":
                CityInformation(stats);
                break;
            case "2":
                ProvinceCities(stats);
                break;
            case "3":
                ProvincePopulation(stats);
                break;
            case "4":
                CityComparison(stats);
                break;
            case "5":
                DistanceCalculation(stats);
                break;
            default:
                Console.WriteLine("Invalid option. Please enter a number between 1 and 6.");
                break;
        }
    }
    private static void PrintCityDetails(CityInfo city)
    {
        Console.WriteLine($"\nCity Details:");
        Console.WriteLine($"ID: {city.CityID}");
        Console.WriteLine($"Name: {city.CityName}");
        Console.WriteLine($"Province: {city.Province}");
        Console.WriteLine($"Population: {city.Population:N0}");
        Console.WriteLine($"Location: ({city.Latitude}, {city.Longitude})");
    }

    private static void CityInformation(Statistics stats)
    {
        Console.WriteLine("\nEnter a city name: ");
        string input = Console.ReadLine();
        if (input == "exit")
        {
            Environment.Exit(0);
        }
        if (input == "restart")
        {
            return;
        }
        var city = stats.DisplayCityInformation(input);
        PrintCityDetails(city);
    }
    private static void ProvinceCities(Statistics stats)
    {
        Console.Write("\nEnter a province name: ");
        string? input = Console.ReadLine();
        if (input == "exit")
        {
            Environment.Exit(0);
        }
        if (input == "restart")
        {
            return;
        }

        var cities = stats.DisplayProvinceCities(input);
        Console.WriteLine($"\nCities in {input}:");
        foreach (var city in cities)
        {
            Console.WriteLine($"- {city.CityName}: {city.Population} people");
        }
    }
    private static void ProvincePopulation(Statistics stats)
    {
        Console.Write("\nEnter a province name: ");
        string? input = Console.ReadLine();
        if (input == "exit")
        {
            Environment.Exit(0);
        }
        if (input == "restart")
        {
            return;
        }
        int population = stats.DisplayProvincePopulation(input);
        Console.WriteLine($"The total population of {input} is {population} people.");
    }
    private static void CityComparison(Statistics stats)
    {
        Console.Write("\nEnter two city names, separated by a comma, to see which city has the larger population (eg. London, Toronto): ");
        string? input = Console.ReadLine();
        if (input == "exit")
        {
            Environment.Exit(0);
        }
        if (input == "restart")
        {
            return;
        }

        string[]? cities = input?.Split(',').Select(x => x.Trim()).ToArray();

        if (cities?.Length != 2)
        {
            Console.WriteLine("Please enter two cities seperated by a comma");
            return;
        }
        var result = stats.CompareCitiesPopluation(cities[0], cities[1]);
        Console.WriteLine($"\n{result.largerCity.CityName} has a larger population than {(result.largerCity.CityName == cities[0] ? cities[1] : cities[0])} with a population of {result.largerCity.Population} people.");

    }

    private static void DistanceCalculation(Statistics stats)
    {
        Console.Write("\nEnter the first city name: ");
        string city1 = Console.ReadLine();

        if (city1 == "exit")
        {
            Environment.Exit(0);
        }
        if (city1 == "restart")
        {
            return;
        }

        Console.Write("\nEnter the second city name: ");
        string city2 = Console.ReadLine();

        if (city2 == "exit")
        {
            Environment.Exit(0);
        }
        if (city2 == "restart")
        {
            return;
        }

        double result = stats.CalculateDistanceBetweenCities(city1, city2);
        Console.WriteLine($"\nThe distance between {city1} and {city2} is {result} km.");
    }
}