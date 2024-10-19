using Microsoft.EntityFrameworkCore;
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
	private readonly YellowPagesContext _context;

	public ContactController(YellowPagesContext context) : base(context)
	{
		_context = context;
	}	

	/// <summary>
	/// Get a contact if it exists.
	/// </summary>
	/// <param name="id">ID for the Contact you want to query</param>
	public override Contact Query(int id)
	{
		try
		{
			var contact = _context.Contacts
				.Include(c => c.Categories)
				.FirstOrDefault(c => c.ContactId == id);

			if (contact == null)
				throw new Exception($"Unable to find Contact with ID {id}.");

			return contact;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unable to find Contact with ID {id}. Error details: {ex.Message}");
			return null;
		}
	}

	public virtual List<Contact> QueryAll()
	{
		try
		{
			var contacts = _context.Contacts
				.Include(c => c.Categories)
				.ToList();

			return contacts;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unable to find a set of Contact. Error details: {ex.Message}");
			return null;
		}
	}
}

/// <summary>
/// Contains the CRUD operations (Create, Query, Update, Delete) for the Category model.
/// </summary>
internal class CategoryController : Controller<Category>
{
	private readonly YellowPagesContext _context;

	public CategoryController(YellowPagesContext context) : base (context) 
	{
		_context = context;
	}

	/// <summary>
	/// Get a category if it exists.
	/// </summary>
	/// <param name="id">ID for the Category you want to query</param>
	public override Category Query(int id)
	{
		try
		{
			var category = _context.Categories
				.Include(c => c.Contacts)
				.FirstOrDefault(c => c.CategoryId == id);

			if (category == null)
				throw new Exception($"Unable to find Category with ID {id}.");

			return category;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unable to find Category with ID {id}. Error details: {ex.Message}");
			return null;
		}
	}

	public virtual List<Category> QueryAll()
	{
		try
		{
			var categories = _context.Categories
				.Include(c => c.Contacts)
				.ToList();

			return categories;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unable to find a set of Category. Error details: {ex.Message}");
			return null;
		}
	}
}
