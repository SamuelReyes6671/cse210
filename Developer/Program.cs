using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static List<JournalEntry> journal = new List<JournalEntry>();
    static List<string> prompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?"
    };

    static void Main()
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Journal Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a file");
            Console.WriteLine("4. Load the journal from a file");
            Console.WriteLine("5. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    WriteNewEntry();
                    break;
                case "2":
                    DisplayJournal();
                    break;
                case "3":
                    SaveJournalToFile();
                    break;
                case "4":
                    LoadJournalFromFile();
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }

    static void WriteNewEntry()
    {
        Console.WriteLine("Choose a prompt:");
        for (int i = 0; i < prompts.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {prompts[i]}");
        }

        if (int.TryParse(Console.ReadLine(), out int promptIndex) && promptIndex >= 1 && promptIndex <= prompts.Count)
        {
            Console.WriteLine("Write your response:");
            string response = Console.ReadLine();
            DateTime date = DateTime.Now;

            journal.Add(new JournalEntry(prompts[promptIndex - 1], response, date));
            Console.WriteLine("Entry saved successfully.");
        }
        else
        {
            Console.WriteLine("Invalid prompt choice. Please enter a valid number.");
        }
    }

    static void DisplayJournal()
    {
        if (journal.Count == 0)
        {
            Console.WriteLine("The journal is empty.");
        }
        else
        {
            Console.WriteLine("----- Journal Entries -----");
            foreach (var entry in journal)
            {
                Console.WriteLine($"Date: {entry.Date}");
                Console.WriteLine($"Prompt: {entry.Prompt}");
                Console.WriteLine($"Response: {entry.Response}");
                Console.WriteLine("----------------------------");
            }
        }
    }

    static void SaveJournalToFile()
    {
        Console.WriteLine("Enter the filename to save the journal:");
        string filename = Console.ReadLine();

        try
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var entry in journal)
                {
                    writer.WriteLine($"{entry.Date}: {entry.Prompt}");
                    writer.WriteLine($"{entry.Response}");
                    writer.WriteLine();
                }
            }

            Console.WriteLine("Journal saved to file successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving journal to file: {ex.Message}");
        }
    }

    static void LoadJournalFromFile()
    {
        Console.WriteLine("Enter the filename to load the journal:");
        string filename = Console.ReadLine();

        try
        {
            journal.Clear();

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                string prompt = "";
                string response = "";
                DateTime date = DateTime.MinValue;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(": "))
                    {
                        var parts = line.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 2)
                        {
                            if (parts[0].Equals("Date", StringComparison.OrdinalIgnoreCase))
                            {
                                date = DateTime.Parse(parts[1]);
                            }
                            else if (parts[0].Equals("Prompt", StringComparison.OrdinalIgnoreCase))
                            {
                                prompt = parts[1];
                            }
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        response = line;
                    }
                    else
                    {
                        if (date != DateTime.MinValue && !string.IsNullOrWhiteSpace(prompt) && !string.IsNullOrWhiteSpace(response))
                        {
                            journal.Add(new JournalEntry(prompt, response, date));
                            date = DateTime.MinValue;
                            prompt = "";
                            response = "";
                        }
                    }
                }
            }

            Console.WriteLine("Journal loaded from file successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading journal from file: {ex.Message}");
        }
    }
}

class JournalEntry
{
    public string Prompt { get; }
    public string Response { get; }
    public DateTime Date { get; }

    public JournalEntry(string prompt, string response, DateTime date)
    {
        Prompt = prompt;
        Response = response;
        Date = date;
    }
}
