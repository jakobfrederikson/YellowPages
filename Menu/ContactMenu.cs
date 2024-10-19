using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Controllers;
using YellowPages.Models;

namespace YellowPages.Menu;

internal class ContactMenu : IMenu
{
	private readonly ContactController _contactController;
	private readonly CategoryController _categoryController;

	public ContactMenu(YellowPagesContext context)
	{
		_contactController = new(context);
		_categoryController = new(context);
	}

	// Display all contacts and provide contact options
	public void Start()
	{
		do
		{
			MenuHelper.DisplayMenuHeader("Contacts menu");

			var allContacts = _contactController.QueryAll();
			if (allContacts != null && allContacts.Any()) MenuHelper.DisplayDataSet(allContacts);
			else Console.WriteLine("You have no contacts.");

			int choice = MenuHelper.DisplayOptionsAndGetIntResult(["Create contact", "Contact details (View/Update/Delete)", "Back"]);

			switch (choice)
			{
				case 1:
					CreateContact();
					break;
				case 2:
					QueryContact();
					break;
				case 3:
					return;
				default:
					break;
			}
		}
		while (true);
	}

	private void CreateContact()
	{
		MenuHelper.DisplayMenuHeader("Create a contact");

		var newContact = new Contact
		{
			Name = getValidName(),
			Email = getValidEmail(),
			PhoneNumber = getValidPhoneNumber(),
		};

		_contactController.Create(newContact);
	}	

	private void QueryContact()
	{
		MenuHelper.DisplayMenuHeader("View a contact");

		var allContacts = _contactController.QueryAll();
		if (allContacts == null || !allContacts.Any())
		{
			Console.WriteLine("You have no contacts.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		MenuHelper.DisplayDataSet(allContacts);

		Console.Write("Enter a contact ID: ");
		int contactId;
		Contact contact;

		while (true)
		{
			string contactIdInput = Console.ReadLine();
			if (int.TryParse(contactIdInput, out contactId))
			{
				contact = _contactController.Query(contactId);
				if (contact != null)
				{
					DisplayContactOptions(contact);
					return;
				}
			}
			Console.WriteLine("Invalid contact ID. Please try again.");
		}
	}

	private void DisplayContactOptions(Contact contact)
	{
		MenuHelper.DisplayMenuHeader("Contact options");
		MenuHelper.DisplaySingleData(contact);

		int choice = MenuHelper.DisplayOptionsAndGetIntResult(
			["Update contact", "Delete contact", "Back to manage contacts"]
		);

		switch (choice)
		{
			case 1:
				UpdateContact(contact);
				break;
			case 2:
				DeleteContact(contact);
				break;
			case 3:
				return; // Go back to manage contacts
			default:
				Console.WriteLine("Invalid choice. Returning to contacts menu.");
				break;
		}
	}

	private void UpdateContact(Contact contact)
	{
		MenuHelper.DisplayMenuHeader("Update a contact");
		MenuHelper.DisplaySingleData(contact);

		int choice = MenuHelper.DisplayOptionsAndGetIntResult(
			["Name", "Email", "Phone", "Add Category", "Remove Category", "Back"],
			"Select option to update: "
		);

		switch (choice)
		{
			case 1:
				Console.WriteLine($"Old Name: {contact.Name}");
				contact.Name = getValidName();
				Console.WriteLine($"New Name: {contact.Name}");
				break;
			case 2:
				Console.WriteLine($"Old Email: {contact.Email}");
				contact.Email = getValidEmail();
				Console.WriteLine($"New Email: {contact.Email}");
				break;
			case 3:
				Console.WriteLine($"Old Phone Number: {contact.PhoneNumber}");
				contact.PhoneNumber = getValidPhoneNumber();
				Console.WriteLine($"New Phone Number: {contact.PhoneNumber}");
				break;
			case 4:
				addContactToCategory(contact);
				break;
			case 5:
				removeContactFromCategory(contact);
				return;
			default:
				return;
		}

		choice = MenuHelper.DisplayOptionsAndGetIntResult(["Yes", "No"], "Confirm update: ");
		if (choice == 1)
			_contactController.Update(contact);
		else 
			MenuHelper.PressAnyKeyToContinue();
	}

	private void DeleteContact(Contact contact)
	{
		MenuHelper.DisplayMenuHeader("Delete a contact");
		MenuHelper.DisplaySingleData(contact);

		// Ask for confirmation
		int choice = MenuHelper.DisplayOptionsAndGetIntResult(
			["Yes", "No"], "Are you sure you want to delete this contact? "
		);

		if (choice == 1)
		{
			_contactController.Delete(contact);
			Console.WriteLine($"Contact '{contact.Name}' has been deleted.");
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

	private string getValidName()
	{
		string contactName;
		do
		{
			Console.Write("Enter contact name (max 20 characters): ");
			contactName = Console.ReadLine();

			if (string.IsNullOrEmpty(contactName) || contactName.Length >= 20) Console.WriteLine("Contact cannot exceed 20 characters.");
		}
		while (string.IsNullOrEmpty(contactName) || contactName.Length >= 20);

		return contactName;
	}

	private string getValidEmail()
	{		
		string email;

		// I don't want this to be used outside of getValidEmail() :)
		bool isEmailValid(string nonEmptyString)
		{
			try
			{
				var emailAddress = new System.Net.Mail.MailAddress(nonEmptyString);
				return emailAddress.Address == nonEmptyString;
			}
			catch
			{
				return false;
			}
		}

		do
		{
			Console.Write("Enter a valid email: ");
			email = Console.ReadLine();
			if (string.IsNullOrEmpty(email) || !isEmailValid(email)) Console.WriteLine("Not a valid email. Please try again.");

		}
		while (string.IsNullOrEmpty(email) || !isEmailValid(email));

		return email;
	}

	private string getValidPhoneNumber()
	{
		string phoneNumber;
		do
		{
			Console.Write("Enter contact phone number: ");
			phoneNumber = Console.ReadLine();

			if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length > 12) Console.WriteLine("Invalid phone number. Try again.");

		}
		while (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length > 12);

		return phoneNumber;
	}

	private void addContactToCategory(Contact contact)
	{
		MenuHelper.DisplayMenuHeader("Add Contact to Category");

		var allCategories = _categoryController.QueryAll();
		if (allCategories == null || !allCategories.Any())
		{
			Console.WriteLine("No categories available.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		int categoryChoice = MenuHelper.DisplayOptionsAndGetIntResult(
			allCategories.Select(c => c.Name).ToArray(),
			"Select a category to add the contact to: "
		);

		if (categoryChoice < 1 || categoryChoice > allCategories.Count)
		{
			Console.WriteLine("Invalid choice.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		var selectedCategory = allCategories[categoryChoice - 1];
		if (contact.Categories.Any(c => c.CategoryId == selectedCategory.CategoryId))
		{
			Console.WriteLine($"Contact is already in the category '{selectedCategory.Name}'.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		contact.Categories.Add(selectedCategory);
		Console.WriteLine($"Contact '{contact.Name}' has been added to category '{selectedCategory.Name}'.");
		MenuHelper.PressAnyKeyToContinue();
	}

	private void removeContactFromCategory(Contact contact)
	{
		MenuHelper.DisplayMenuHeader("Remove Contact from Category");

		if (contact.Categories == null || !contact.Categories.Any())
		{
			Console.WriteLine("No categories available.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		int categoryChoice = MenuHelper.DisplayOptionsAndGetIntResult(
			contact.Categories.Select(c => c.Name).ToArray(),
			"Select a category to remove the contact from: "
		);

		if (categoryChoice < 1 || categoryChoice > contact.Categories.Count)
		{
			Console.WriteLine("Invalid choice.");
			MenuHelper.PressAnyKeyToContinue();
			return;
		}

		var selectedCategory = contact.Categories[categoryChoice - 1];
		contact.Categories.Remove(selectedCategory);
		Console.WriteLine($"Contact '{contact.Name}' has been removed from category '{selectedCategory.Name}'.");
		MenuHelper.PressAnyKeyToContinue();
	}
}
