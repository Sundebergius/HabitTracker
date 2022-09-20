using Microsoft.Data.Sqlite;

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
    }

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

void Update()
{
    throw new NotImplementedException();
}

void Delete()
{
    throw new NotImplementedException();
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

    int finalInput = Convert.ToInt32(numberInput);

    return finalInput; 
}

void GetAllRecords()
{
    throw new NotImplementedException();
}

 string GetDateInput()
{
    Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yyyy). Type 0 to return to the main menu.\n");
    string dateInput = Console.ReadLine();
    if (dateInput == "0") GetUserInput();
    {
        return dateInput;
    }
}