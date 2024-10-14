
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Menu;

internal class MenuOptions
{
	public static int GetIntInput(string message)
	{
		Console.WriteLine(message);
		int result;
		while (!int.TryParse(Console.ReadLine(), out result))
		{
			Console.WriteLine("Invalid input, please enter a valid number");
		}
		return result;
	}

	public static string GetStringInput(string message)
	{
		Console.WriteLine(message);
		string result = Console.ReadLine();
		while (string.IsNullOrWhiteSpace(result))
		{
			Console.WriteLine("Input cannot be empty. Please try again.");
			result = Console.ReadLine();
		}
		return result;
	}

	public static bool GetYesNoInput(string message)
	{
		Console.WriteLine($"{message} (y/n)");
		string response = Console.ReadLine().ToLower();
		while (response != "y" && response != "n")
		{
			Console.WriteLine("Invalid input. Please enter 'y' for Yes or 'n' for No.");
			response = Console.ReadLine().ToLower();
		}
		return response == "y";
	}

	public static int DisplayOptionsAndGetResult(List<string> options, string message = "Select an option:")
	{
		for (int i = 0; i < options.Count; i++) Console.WriteLine($"{i + 1}. {options[i]}");

		Console.Write(message + " ");

		int result;
		while (!int.TryParse(Console.ReadLine(), out result) && result < 1 && result > options.Count)
		{
			Console.WriteLine("Invalid input, please enter a valid number");
		}

		return result;
	}
}
