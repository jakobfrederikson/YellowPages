using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Menu;

internal class MenuHelper
{
	public static int DisplayOptionsAndGetIntResult(string[] options, string message = "Select an option: ")
	{
		for (int i = 0; i < options.Length; i++)
		{
			Console.WriteLine($"{i + 1}. {options[i]}");
		}
		Console.Write(message);

		int choice;
		do
		{
			if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= options.Length) break;
			else Console.WriteLine($"Please enter a number between 1 and { options.Length }");
		}
		while (true);

		return choice;
	}

	public static void DisplayMenuHeader(string menuTitle)
	{
		Console.Clear();
		Console.WriteLine($"====[{menuTitle}]====");
	}

	public static void DisplayDataSet<T> (List<T> list)
	{
		Type type = typeof(T);

		PropertyInfo[] properties = type.GetProperties();

		int maxPropLength = 0;
		foreach (var prop in properties)
		{
			if (prop.Name.Length > maxPropLength) maxPropLength = prop.Name.Length;
		}

		foreach (var item in list)
		{
			foreach (var prop in properties)
			{
				var value = prop.GetValue(item, null);

				// Handle the "List<Category>" or "List<Contact>" properties in each model
				if (value is System.Collections.IEnumerable modelList && value is not string)
				{
					var listItems = modelList.Cast<object>().ToList();

					if (listItems.Any())
					{
						WriteLineInColour($"{prop.Name.PadRight(maxPropLength)}: ", ConsoleColor.Yellow);
						foreach (var listItem in listItems)
						{
							var nameProp = listItem.GetType().GetProperty("Name");
							WriteInColour(" - ", ConsoleColor.Yellow);
							Console.Write(nameProp.GetValue(listItem));
							Console.WriteLine();
						}						
					}
					else
					{
						WriteInColour($"{prop.Name.PadRight(maxPropLength)}: ", ConsoleColor.Yellow);
						Console.Write("None");
						Console.WriteLine();
					}
				}
				else
				{
					WriteInColour($"{prop.Name.PadRight(maxPropLength)}: ", ConsoleColor.Yellow);
					Console.Write(value); 
					Console.WriteLine();
				}				
			}
			Console.WriteLine();
		}
	}

	public static void DisplaySingleData<T> (T entity)
	{
		Type type = typeof(T);

		PropertyInfo[] properties = type.GetProperties();

		int maxPropLength = 0;
		foreach (var prop in properties)
		{
			if (prop.Name.Length > maxPropLength) maxPropLength = prop.Name.Length;
		}

		foreach (var prop in properties)
		{
			var value = prop.GetValue(entity, null);

			// Handle the "List<Category>" or "List<Contact>" properties in each model
			if (value is System.Collections.IEnumerable modelList && value is not string)
			{
				var listItems = modelList.Cast<object>().ToList();

				if (listItems.Any())
				{
					WriteLineInColour($"{prop.Name.PadRight(maxPropLength)}: ", ConsoleColor.Yellow);
					foreach (var listItem in listItems)
					{
						var nameProp = listItem.GetType().GetProperty("Name");
						WriteInColour(" - ", ConsoleColor.Yellow);
						Console.Write(nameProp.GetValue(listItem));
						Console.WriteLine();
					}
				}
				else
				{
					WriteInColour($"{prop.Name.PadRight(maxPropLength)}: ", ConsoleColor.Yellow);
					Console.Write("None");
					Console.WriteLine();
				}
			}
			else
			{
				WriteInColour($"{prop.Name.PadRight(maxPropLength)}: ", ConsoleColor.Yellow);
				Console.Write(value);
				Console.WriteLine();
			}
		}

		Console.WriteLine();
	}

	public static void WriteInColour(string message, ConsoleColor consoleColor)
	{
		ConsoleColor defaultColor = Console.ForegroundColor;
		Console.Write(message, Console.ForegroundColor = consoleColor);
		Console.ForegroundColor = defaultColor;
	}

	public static void WriteLineInColour(string message, ConsoleColor consoleColor)
	{
		ConsoleColor defaultColor = Console.ForegroundColor;
		Console.WriteLine(message, Console.ForegroundColor = consoleColor);
		Console.ForegroundColor = defaultColor;
	}

	public static void PressAnyKeyToContinue(string message = "")
	{
		Console.Write($"{message}Press any key to continue... ");
		Console.ReadKey();
	}
}
