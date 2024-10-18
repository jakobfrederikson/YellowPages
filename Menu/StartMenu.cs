using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Controllers;

namespace YellowPages.Menu;

internal class StartMenu : IMenu
{
	private readonly YellowPagesContext _context;

	public StartMenu(YellowPagesContext context)
	{
		_context = context;
	}

	// Start Menu Loop
	public void Start()
	{
		do
		{
			Console.Clear();
			int choice = MenuHelper.DisplayOptionsAndGetIntResult(["Manage contacts", "Manage categories", "Exit"]);

			switch (choice)
			{
				case 1:
					ContactMenu contactMenu = new(_context);
					contactMenu.Start();
					break;
				case 2:
					CategoryMenu categoryMenu = new(_context);
					categoryMenu.Start();
					break;
				case 3:
					Environment.Exit(0);
					break;
				default:
					break;
			}
		}
		while (true);		
	}
}