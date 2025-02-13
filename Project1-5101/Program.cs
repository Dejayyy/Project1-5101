using Project1_5101;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {



            Console.WriteLine("\n------------------------------");
            Console.WriteLine(" Program Functionality ");
            Console.WriteLine("------------------------------");
            Console.WriteLine("- To exit program, enter 'exit' at any prompt.");
            Console.WriteLine("- To restart program, enter 'restart' at any prompt.");
            Console.WriteLine("- You will be presented with a numbered list of options. Please enter a value.");
            Console.WriteLine("Fetching list of available file names to be processed and queried...");
            Console.WriteLine("\n1) canadiancities-CSV");
            Console.WriteLine("2) canadiancities-JSON");
            Console.WriteLine("3) canadiancities-XML");
            Console.Write("\nSelect an option from the list above (e.g., 1, 2, 3): ");

            string input = Console.ReadLine().ToLower();
            if (input == "exit") break;
            if (input == "restart") break;

            string fileType = input switch
            {
                "1" => "csv",
                "2" => "json",
                "3" => "xml",
                _ => throw new ArgumentException("Invalid selection, please restart.")
            };

            string fileName = fileType switch
            {
                "csv" => "canadiancities-CSV",
                "json" => "canadiancities-JSON",
                "xml" => "canadiancities-XML",
                _ => throw new ArgumentException("Invalid file selected.")
            };

            Console.WriteLine($"\nYou selected {fileName}. Processing");
            Statistics stats = new Statistics(fileName, fileType);
        }
    }
}