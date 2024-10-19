using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Controllers;
using YellowPages.Models;

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
		do
		{
			MenuHelper.DisplayMenuHeader("Categories menu");

			var allCategories = _categoryController.QueryAll();
			if (allCategories != null && allCategories.Any()) MenuHelper.DisplayDataSet(allCategories);
			else Console.WriteLine("You have no contacts.");

			int choice = MenuHelper.DisplayOptionsAndGetIntResult(["Create category", "Category details (View/Update/Delete)", "Back"]);

			switch (choice)
			{
				case 1:
					CreateCategory();
					break;
				case 2:
					QueryCategory();
					break;
				case 3:
					return; // Exit the category menu
				default:
					Console.WriteLine("Invalid option, please try again.");
					break;
			}
		}
		while (true);		
	}

	private void CreateCategory()
	{
		MenuHelper.DisplayMenuHeader("Create a category");

		string categoryName = getValidCategoryName();
		var newCategory = new Category
		{
			Name = categoryName
		};

		_categoryController.Create(newCategory);
	}

	private void QueryCategory()
	{
		MenuHelper.DisplayMenuHeader("View a category");

		var allCategories = _categoryController.QueryAll();
		if (allCategories == null && !allCategories.Any())
		{
			Console.WriteLine("You have no categories.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		MenuHelper.DisplayDataSet(allCategories);

		Console.Write("Enter a category ID: ");
		int categoryId;
		Category category = null;

		while (true)
		{
			string categoryIdInput = Console.ReadLine();
			if (int.TryParse(categoryIdInput, out categoryId))
			{
				category = _categoryController.Query(categoryId);
				if (category != null)
				{
					DisplayCategoryOptions(category);
					return;
				}
			}
			Console.WriteLine("Invalid category ID. Please try again.");
		}
	}

	private void DisplayCategoryOptions(Category category)
	{
		MenuHelper.DisplayMenuHeader("Category options");
		MenuHelper.DisplaySingleData(category);

		int choice = MenuHelper.DisplayOptionsAndGetIntResult(
			["Update category", "Delete category", "Back to manage categories"]
		);

		switch (choice)
		{
			case 1:
				UpdateCategory(category);
				break;
			case 2:
				DeleteCategory(category);
				break;
			case 3:
				return; // Go back to manage categories
			default:
				Console.WriteLine("Invalid choice. Returning to categories menu.");
				break;
		}
	}

	private void UpdateCategory(Category category)
	{
		MenuHelper.DisplayMenuHeader("Update a category");
		MenuHelper.DisplaySingleData(category);

		int choice = MenuHelper.DisplayOptionsAndGetIntResult(
			["Name", "Add Contact", "Remove Contact", "Back"],
			"Select option to update: ");

		switch (choice)
		{
			case 1:
				Console.WriteLine($"Old name: {category.Name}");
				category.Name = getValidCategoryName();
				Console.WriteLine($"New name: {category.Name}");
				break;
			case 2:
				addContactToCategory(category);
				break;
			case 3:
				removeContactFromCategory(category);
				break;
			case 4:
				return;
			default:
				return;
		}

		choice = MenuHelper.DisplayOptionsAndGetIntResult(["Yes", "No"], "Confirm update: ");
		if (choice == 1)
			_categoryController.Update(category);
		else
			MenuHelper.PressAnyKeyToContinue();
	}

	private void DeleteCategory(Category category)
	{
		MenuHelper.DisplayMenuHeader("Delete a category");
		MenuHelper.DisplaySingleData(category);

		int choice = MenuHelper.DisplayOptionsAndGetIntResult(
			["Yes", "No"], "Are you sure you want to delete this category? "
		);

		if (choice == 1)
		{
			_categoryController.Delete(category);
			Console.WriteLine($"Category '{category.Name}' has been deleted.");
		}	
		else
		{
			Console.WriteLine("Deletion cancelled.");
		}

		MenuHelper.PressAnyKeyToContinue();
	}

	//
	// ----------------------------------- vv Helper Methods vv ---------------------------------------------------------------------
	// 

	private string getValidCategoryName()
	{
		string categoryName;
		do
		{
			Console.Write("Enter contact name (max 20 characters): ");
			categoryName = Console.ReadLine();

			if (string.IsNullOrEmpty(categoryName) || categoryName.Length >= 20) Console.WriteLine("Contact cannot exceed 20 characters.");
		}
		while (string.IsNullOrEmpty(categoryName) || categoryName.Length >= 20);

		return categoryName;
	}

	private void addContactToCategory(Category category)
	{
		MenuHelper.DisplayMenuHeader("Add Contact to Category");

		var allContacts = _contactController.QueryAll();
		if (allContacts == null || !allContacts.Any())
		{
			Console.WriteLine("No contacts available.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		int contactChoice = MenuHelper.DisplayOptionsAndGetIntResult(
			allContacts.Select(c => c.Name).ToArray(),
			"Select a contact to add the category to: "
		);

		if (contactChoice < 1 || contactChoice > allContacts.Count)
		{
			Console.WriteLine("Invalid choice.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		var selectedContact = allContacts[contactChoice - 1];
		if (category.Contacts.Any(c => c.ContactId == selectedContact.ContactId))
		{
			Console.WriteLine($"Category is already allocated to contact '{selectedContact.Name}'.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		category.Contacts.Add(selectedContact);
		Console.WriteLine($"Category '{category.Name}' has been allocated to '{selectedContact.Name}'.");
		MenuHelper.PressAnyKeyToContinue();
	}

	private void removeContactFromCategory(Category category)
	{
		MenuHelper.DisplayMenuHeader("Remove Contact from Category");

		if (category.Contacts == null || !category.Contacts.Any())
		{
			Console.WriteLine("No contacts available in this category.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		int contactChoice = MenuHelper.DisplayOptionsAndGetIntResult(
			category.Contacts.Select(c => c.Name).ToArray(),
			"Select a contact to remove from the category: ");

		if (contactChoice < 1 || contactChoice > category.Contacts.Count)
		{
			Console.WriteLine("Invalid choice.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		var selectedContact = category.Contacts[contactChoice - 1];
		category.Contacts.Remove(selectedContact);
		Console.WriteLine($"Contact '{selectedContact.Name}' has been removed from category '{category.Name}'.");
		MenuHelper.PressAnyKeyToContinue();
	}
}
