using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Models;

namespace YellowPages.Controllers;

/// <summary>
/// Contains the CRUD operations (Create, Query, Update, Delete) for the Contact model.
/// </summary>
internal class ContactController : Controller<Contact>
{
	public ContactController(YellowPagesContext context) : base(context) { }
}

/// <summary>
/// Contains the CRUD operations (Create, Query, Update, Delete) for the Category model.
/// </summary>
internal class CategoryController : Controller<Category>
{
	public CategoryController(YellowPagesContext context) : base (context) { }
}
