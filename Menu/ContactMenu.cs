using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Controllers;

namespace YellowPages.Menu;

internal class ContactMenu
{
	private readonly ContactController _contactController;

	public ContactMenu(YellowPagesContext context)
	{
		_contactController = new(context);
	}
}
