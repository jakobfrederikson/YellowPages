using System;
using System.Collections.Generic;
using System.Linq;
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
}
