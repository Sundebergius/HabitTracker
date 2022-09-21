using Microsoft.Data.Sqlite;
using System.Globalization;

string connectionString = @"Data Source=habit-Tracker.db";

CreateDatabase();

void CreateDatabase()
{
    /*Creating a connection passing the connection string as an argument
    This will create the database for you, there's no need to manually create it.
    And no need to use File.Create().*/
    using (var connection = new SqliteConnection(connectionString))
    {
        //Creating the command that will be sent to the database
        using (var tableCmd = connection.CreateCommand()) 
        {
            connection.Open();
            //Declaring what is that command (in SQL syntax)
            tableCmd.CommandText = 
                @"CREATE TABLE IF NOT EXISTS Drinking_Water (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER
                    )";

            // Executing the command, which isn't a query, it's not asking to return data from the database.
            tableCmd.ExecuteNonQuery();
        }
        // We don't need to close the connection or the command. The 'using statement' does that for us.
    }

    /* Once we check if the database exists and create it (or not),
    we will call the next method, which will handle the user's input. Your next step is to create this method*/
    GetUserInput();
}

void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while (closeApp == false)
    {
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("\nWhat would youy like to do?");
        Console.WriteLine("\nType 0 to close the application.");
        Console.WriteLine("Type 1 to view all records.");
        Console.WriteLine("Type 2 to insert a record.");
        Console.WriteLine("Type 3 to delete a record.");
        Console.WriteLine("Type 4 to update a record.");
        Console.WriteLine("------------------------------------------\n");


        string command = Console.ReadLine();

        switch (command)
        {
            case "0":
                Console.WriteLine("\nGoodbye, bitch!");
                closeApp = true;
                break;

            case "1":
                GetAllRecords();
                break;

            case "2":
                Insert();
                break;

            case "3":
                Delete();
                break;

            case "4":
                Update();
                break;

            default:
                Console.WriteLine("\nInvalid Command. Please type a number between 0-4. \n");
                break;
        }
    }
}

void Update()
{
    GetAllRecords();

    var recordId = GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to return to the main menu.\n\n");
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM Drinking_Water WHERE Id = {recordId})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"\n\nRecord with the Id {recordId} doesn't exist.\n\n");
            connection.Close();
            Update();
        }
    }
}

void Delete()
{
    Console.Clear();
    GetAllRecords();

    var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"DELETE from Drinking_Water WHERE Id = '{recordId}'";

        int rowCount = tableCmd.ExecuteNonQuery();

        if (rowCount == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
            Delete();
        }
    }
    Console.WriteLine($"\n\nRecord with Id {recordId} was deleted \n\n");

    GetUserInput();
}

void Insert()
{
    string date = GetDateInput();

    int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");
}

int GetNumberInput(string message)
{
    Console.WriteLine(message);

    string numberInput = Console.ReadLine();

    if (numberInput == "0") 
    {
        GetUserInput();
    }

    while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
    {
        Console.WriteLine("\n\nInvalid number. Try again.\n\n");
        numberInput = Console.ReadLine();
    }
    int finalInput = Convert.ToInt32(numberInput);

    return finalInput; 
}

void GetAllRecords()
{
    Console.Clear();
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            $"SELECT * FROM Drinking_Water ";

        List<DrinkingWater> tableData = new();

        SqliteDataReader reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-mm-yyyy", new CultureInfo("en_US")),
                        Quantity = reader.GetInt32(2)
                    }); ;
            }
        }
        connection.Close();
    }
}

 string GetDateInput()
{
    Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yyyy). Type 0 to return to the main menu.\n");
    string dateInput = Console.ReadLine();
    if (dateInput == "0") GetUserInput();
    {
        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }
}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}