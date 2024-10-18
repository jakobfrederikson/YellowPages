using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Controllers;

namespace YellowPages.Menu;

internal class CategoryMenu : IMenu
{
	private readonly CategoryController _categoryController;
	private readonly ContactController _contactController;

	public CategoryMenu(YellowPagesContext context)
	{
		_categoryController = new(context);
		_contactController = new(context);
	}

	public void Start()
	{
		MenuHelper.DisplayMenuHeader("Categories menu");
	}
}
