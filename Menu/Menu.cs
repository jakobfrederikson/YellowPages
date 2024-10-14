using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Controllers;
using YellowPages.Models;

namespace YellowPages.Menu;

internal class Menu
{
	private readonly YellowPagesContext _context;

	public Menu(YellowPagesContext context)
	{
		_context = context;
	}

	public void StartMenu()
	{
		while (true)
		{
			Console.Clear();
			Console.WriteLine($"Database path: {_context.DbPath}");
			int result = MenuOptions.DisplayOptionsAndGetResult(["Manage contacts", "Manage categories"]);

			switch (result)
			{
				case 1:
					var contactCoontroller = new ContactController(_context);
					var menuContact = new MenuContact(contactCoontroller);
					menuContact.StartMenuContact();
					break;
				case 2:
					var categoriesController = new CategoryController(_context);
					var menuCategory = new MenuCategory(categoriesController);
					menuCategory.StartMenuCategory();
					break;
				case 3:
					Environment.Exit(0);
					break;
				default:
					StartMenu();
					break;
			}
		}
		
	}
}
