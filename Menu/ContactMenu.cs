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

		string contactName = getValidName();

		string contactEmail = getValidEmail();

		string phoneNumber = getValidPhoneNumber();

		_contactController.Create(new Contact
		{
			Name = contactName,
			Email = contactEmail,
			PhoneNumber = phoneNumber
		});
	}	

	private void QueryContact()
	{
		MenuHelper.DisplayMenuHeader("View a contact");

		var allContacts = _contactController.QueryAll();
		
		// Check if user has any contacts and display them, if not tell them they have no contacts
		if (allContacts != null && allContacts.Any())
		{
			MenuHelper.DisplayDataSet(allContacts);

			Console.Write("Enter a contact ID: ");
			string contactIdInput;
			int contactId;
			Contact contact = null;

			// Ask for a contact's ID, then display options for the contact if they exist
			do
			{
				contactIdInput = Console.ReadLine();

				if (int.TryParse(contactIdInput, out contactId))
				{
					contact = _contactController.Query(contactId);

					if (contact != null)
					{						
						MenuHelper.DisplayMenuHeader("Contact options");
						MenuHelper.DisplaySingleData(contact);
						int choice = MenuHelper.DisplayOptionsAndGetIntResult(["Update contact", "Delete contact", "Back to manage contacts"]);

						switch (choice)
						{
							case 1:
								UpdateContact(contact);
								break;
							case 2:
								DeleteContact(contact);
								break;
							case 3:
								return;
							default:
								Console.WriteLine("Invalid choice. Returning to contacts menu.");
								return;
						}
					}
				}
			} while (contact == null);
		}
		else Console.WriteLine("You have no contacts.");

		Console.ReadLine();
	}

	private void UpdateContact(Contact contact)
	{
		MenuHelper.DisplayMenuHeader("Update a contact");

		MenuHelper.DisplaySingleData(contact);

		int choice = MenuHelper.DisplayOptionsAndGetIntResult(["Name", "Email", "Phone"]);

		// What would you like to update?
		// 1. Name
		// 2. Email
		// 3. Phone
		// 

		switch (choice)
		{
			case 1:
				string updatedName = getValidName();
				break;
			case 2:
				string updateEmail = getValidEmail();
				break;
			case 3:
				string updatePhone = getValidPhoneNumber();
				break;
			case 4:
				return;
			default:
				return;
		}
		
		Console.ReadLine();
	}

	private void DeleteContact(Contact contact)
	{
		MenuHelper.DisplayMenuHeader("Delete a contact");

		MenuHelper.DisplaySingleData(contact);

		// Ask are you sure
		// Get 1. Yes or 2. No
		// Delete if 1, exit method if 2.
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
}
