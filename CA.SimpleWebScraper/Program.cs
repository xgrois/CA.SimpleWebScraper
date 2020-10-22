using System;
using System.Net;
using System.Text.RegularExpressions;

namespace CA.SimpleWebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("::: BASIC WEB SCRAPER FOR CRAIGLIST.ORG :::\n\r");

            string city, category;
            bool exit = false;

            // Repeat unless the user wants to exit
            do
            {
                // City
                Console.Write("Please, enter a city (boston, newyork, chicago, etc.): ");
                city = Console.ReadLine().ToLower() ?? String.Empty;

                // Category
                Console.Write("Please, enter a category (cta, ata, boo, etc.): ");
                category = Console.ReadLine().ToLower() ?? String.Empty;

                // URL
                string url = "https://" + city + ".craigslist.org/search/" + category;

                // Download HTML website
                string content;
                using (WebClient wc = new WebClient())
                {
                    try
                    {
                        content = wc.DownloadString(url);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\n\rNo results have been found for the selected combination of city and category.\n\r{ex.Message}\n\r");

                        // Ask the user for exit
                        exit = ExitQuestion();
                        if (!exit) Console.Clear();

                        continue;
                    }
                    
                }

                // Find all matches in HTML document
                string regularExpression = @"<a href=(.*?) data-id=(.*?) class=(.*?) id=(.*?)>(.*?)<\/a>";
                MatchCollection matches = Regex.Matches(content, regularExpression);

                // Output all items found
                // There are 6 fields per match (item)
                // Only show interested fields (link = 1, description = 5)
                Console.WriteLine();
                for (int i = 0; i < matches.Count; i++)
                {
                    Console.WriteLine($">> Item {i}: ");

                    Console.WriteLine($"\tLink: {matches[i].Groups[1].Value}");
                    Console.WriteLine($"\tDescription: {matches[i].Groups[5].Value}\n\r");

                }

                // Ask the user for exit
                exit = ExitQuestion();
                if (!exit) Console.Clear();

            } while (!exit);

        }

        static bool ExitQuestion()
        {
            bool exit = false;

            // Ask the user for exit
            Console.Write("Would you like to search for another city? [y/n]: ");
            string answer = Console.ReadLine().ToLower() ?? String.Empty;

            if (!answer.Equals("y"))
                exit = true;
            
            return exit;

        }

    }
}

