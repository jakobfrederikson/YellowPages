using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Controllers;
using YellowPages.Models;

namespace YellowPages.Menu;

internal class MenuCategory
{
	private readonly CategoryController _categoryController;

	public MenuCategory(CategoryController categoryController)
	{
		_categoryController = categoryController;
	}

	public void StartMenuCategory()
	{
		Console.Clear();
		var categories = _categoryController.QueryAll();

		if (categories.Any())
		{
			Console.WriteLine("All categories:");
			foreach (var category in categories) Console.WriteLine($"ID: {category.CategoryId}, Name: {category.Name}, Contact(s): {category.Contacts.ToList()} ");
		}
		else Console.WriteLine("No categories added.");

		int result = MenuOptions.DisplayOptionsAndGetResult(["Create category", "Query category by ID", "Update category by ID", "Delete category", "Back to main menu"]);

		switch (result)
		{
			case 1:
				CreateCategory();
				break;
			case 2:
				QueryCategory();
				break;
			case 3:
				UpdateCategory();
				break;
			case 4:
				DeleteCategory();
				break;
			case 5:
				break;
			default:
				Console.WriteLine("Invalid option. Returning to categories menu.");
				StartMenuCategory();
				break;
		}
	}

	private void CreateCategory()
	{
		string categoryName = MenuOptions.GetStringInput("Enter category name: ");

		_categoryController.Create(new Category
		{
			Name = categoryName,
		});
	}

	private void QueryCategory()
	{
		throw new NotImplementedException();
	}

	private void UpdateCategory()
	{
		throw new NotImplementedException();
	}

	private void DeleteCategory()
	{
		throw new NotImplementedException();
	}
}
