using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Controllers;

namespace YellowPages.Menu;

internal class CategoryMenu
{
	private readonly CategoryController _categoryController;

	public CategoryMenu(YellowPagesContext context)
	{
		_categoryController = new(context);
	}
}
