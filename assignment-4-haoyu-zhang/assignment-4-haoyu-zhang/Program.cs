using System;
using System.IO;

class Program
{
    private static Client[] clients = new Client[10];
    private static int clientCount = 0;
    private static string filePath = "clients.csv";

    static void Main()
    {
        LoadClientsFromCsv();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("/-----------------------------------------/");
            Console.WriteLine("           Personal Training App           ");
            Console.WriteLine("/-----------------------------------------/");
            Console.WriteLine();
            Console.WriteLine("Menu Options");
            Console.WriteLine("============");
            Console.WriteLine("[L]ist all clients");
            Console.WriteLine("[F]ind client");
            Console.WriteLine("[N]ew client");
            Console.WriteLine("[R]emove client");
            Console.WriteLine("[S]how client BMI info");
            Console.WriteLine("[E]dit client");
            Console.WriteLine("[Q]uit");
            Console.WriteLine();
            Console.Write("Enter menu selection: ");

            var input = Console.ReadKey().Key;
            Console.WriteLine();

            switch (input)
            {
                case ConsoleKey.L:
                    ListAllClients();
                    break;
                case ConsoleKey.F:
                    FindClient();
                    break;
                case ConsoleKey.R:
                    RemoveClient();
                    break;
                case ConsoleKey.N:
                    AddNewClient();
                    break;
                case ConsoleKey.S:
                    ShowBmiInfo();
                    break;
                case ConsoleKey.E:
                    EditClient();
                    break;
                case ConsoleKey.Q:
                    SaveClientsToCsv();
                    return;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
    }

    static void LoadClientsFromCsv()
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var data = line.Split(',');
                if (data.Length == 4)
                {
                    AddClientToArray(new Client(data[0], data[1], int.Parse(data[2]), int.Parse(data[3])));
                }
            }
        }
    }

    static void SaveClientsToCsv()
    {
        var lines = new string[clientCount];
        for (int i = 0; i < clientCount; i++)
        {
            lines[i] = $"{clients[i].FirstName},{clients[i].LastName},{clients[i].Weight},{clients[i].Height}";
        }
        File.WriteAllLines(filePath, lines);
    }

    static void ListAllClients()
    {
        if (clientCount > 0)
        {
            Console.WriteLine("\nClient List:");
            for (int i = 0; i < clientCount; i++)
            {
                Console.WriteLine($"{clients[i].FullName} - Weight: {clients[i].Weight} lbs, Height: {clients[i].Height} in");
            }
        }
        else
        {
            Console.WriteLine("No clients to display.");
        }
    }

    static void FindClient()
    {
        Console.Write("\nEnter the name to search for: ");
        string searchTerm = Console.ReadLine().Trim().ToLower();
        bool found = false;

        for (int i = 0; i < clientCount; i++)
        {
            if (clients[i].FirstName.ToLower().Contains(searchTerm) || clients[i].LastName.ToLower().Contains(searchTerm))
            {
                Console.WriteLine("\nMatched Client:");
                Console.WriteLine($"{clients[i].FullName} - Weight: {clients[i].Weight} lbs, Height: {clients[i].Height} in");
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No matching clients found.");
        }
    }

    static void RemoveClient()
    {
        Console.Write("\nEnter the full name of the client to remove: ");
        string nameToRemove = Console.ReadLine().Trim();
        for (int i = 0; i < clientCount; i++)
        {
            if ($"{clients[i].FirstName} {clients[i].LastName}".Equals(nameToRemove, StringComparison.OrdinalIgnoreCase))
            {
             
                for (int j = i; j < clientCount - 1; j++)
                {
                    clients[j] = clients[j + 1];
                }
                clients[clientCount - 1] = null;
                clientCount--;
                Console.WriteLine($"{nameToRemove} has been removed from the client list.");
                SaveClientsToCsv();
                return;
            }
        }

        Console.WriteLine("Client not found.");
    }

    static void AddNewClient()
    {
        if (clientCount >= clients.Length)
        {
            ResizeClientArray();
        }

        Console.Write("Enter client's first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter client's last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter client's weight in pounds: ");
        int weight = int.Parse(Console.ReadLine());
        Console.Write("Enter client's height in inches: ");
        int height = int.Parse(Console.ReadLine());

        AddClientToArray(new Client(firstName, lastName, weight, height));
        Console.WriteLine();
        Console.WriteLine("Client successfully created.");
    }

    static void ShowBmiInfo()
    {
        Console.WriteLine("=== Client Info ===");

        for (int i = 0; i < clientCount; i++)
        {
            Console.WriteLine($"Client Name: {clients[i].FullName}");
            Console.WriteLine($"BMI Score: {clients[i].BmiScore:F2}");
            Console.WriteLine($"BMI Status: {clients[i].BmiStatus}");
            Console.WriteLine("-------------------------------------------");
        }
    }

    static void EditClient()
    {
        if (clientCount == 0)
        {
            Console.WriteLine("No clients available to edit.");
            return;
        }

        Console.WriteLine("Select a client to edit:");
        for (int i = 0; i < clientCount; i++)
        {
            Console.WriteLine($"{i + 1}. {clients[i].FullName}");
        }

        Console.Write("Enter client number: ");
        Console.WriteLine();
        int index;
        if (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > clientCount)
        {
            Console.WriteLine("Invalid client number.");
            return;
        }

        var client = clients[index - 1];
        while (true)
        {
            Console.WriteLine("Edit Client");
            Console.WriteLine("===========");
            Console.WriteLine("[F]irst name");
            Console.WriteLine("[L]ast name");
            Console.WriteLine("[H]eight");
            Console.WriteLine("[W]eight");
            Console.WriteLine("[R]eturn to main menu");
            Console.WriteLine();
            Console.Write("What would you like to edit? ");

            var input = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (input)
            {
                case 'F':
                case 'f':
                    Console.WriteLine();
                    Console.Write("Enter new first name: ");
                    client.FirstName = Console.ReadLine();
                    break;
                case 'L':
                case 'l':
                    Console.WriteLine();
                    Console.Write("Enter new last name: ");
                    client.LastName = Console.ReadLine();
                    break;
                case 'H':
                case 'h':
                    Console.WriteLine();
                    Console.Write($"Enter new height: ");
                    client.Height = int.Parse(Console.ReadLine());
                    break;
                case 'W':
                case 'w':
                    Console.WriteLine();
                    Console.Write($"Enter new weight: ");
                    client.Weight = int.Parse(Console.ReadLine());
                    break;
                case 'R':
                case 'r':
                    Console.WriteLine();
                    Console.WriteLine("Returning to main menu.");
                    return;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }

            Console.WriteLine("Client information updated successfully.");
        }
    }

    private static void AddClientToArray(Client client)
    {
        if (clientCount >= clients.Length)
        {
            ResizeClientArray();
        }
        clients[clientCount++] = client;
    }

    private static void ResizeClientArray()
    {
        int newSize = clients.Length * 2;
        Client[] newClients = new Client[newSize];
        for (int i = 0; i < clients.Length; i++)
        {
            newClients[i] = clients[i];
        }
        clients = newClients;
    }
}

class Client
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }

    public Client(string firstName, string lastName, int weight, int height)
    {
        FirstName = firstName;
        LastName = lastName;
        Weight = weight;
        Height = height;
    }

    public double BmiScore => (Weight / Math.Pow(Height, 2)) * 703;

    public string BmiStatus
    {
        get
        {
            var bmi = BmiScore;
            if (bmi <= 18.4) return "Underweight";
            if (bmi <= 24.9) return "Normal";
            if (bmi <= 39.9) return "Overweight";
            return "Obese";
        }
    }

    public string FullName => $"{FirstName} {LastName}";
}
