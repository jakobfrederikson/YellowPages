using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Controllers;
using YellowPages.Models;

namespace YellowPages.Menu;

internal class MenuContact
{
	private readonly ContactController _contactController;
	public MenuContact(ContactController contactController)
	{
		_contactController = contactController;
	}

	public void StartMenuContact()
	{
		Console.Clear();
		var contacts = _contactController.QueryAll();

		if (contacts.Any())
		{
			Console.WriteLine("All contacts:");
			foreach (var contact in contacts) Console.WriteLine($"ID: {contact.ContactId}, Name: {contact.Name}, Email: {contact.Email}, Phone #: {contact.PhoneNumber}, Category(s): {contact.Categories.ToList()} ");
		}
		else Console.WriteLine("No contacts added.");

		int result = MenuOptions.DisplayOptionsAndGetResult(["Create contact", "Query contact by ID", "Update contact by ID", "Delete contact", "Back to main menu"]);

		switch (result)
		{
			case 1:
				CreateContact();
				break;
			case 2:
				QueryContact();
				break;
			case 3:
				UpdateContact();
				break;
			case 4:
				DeleteContact();
				break;
			case 5:
				break;
			default:
				Console.WriteLine("Invalid option. Returning to contacts menu.");
				StartMenuContact();
				break;
		}
	}

	private void CreateContact()
	{
		string contactName = MenuOptions.GetStringInput("Enter contact name: ");
		string contactEmail = MenuOptions.GetStringInput("Enter their email: ");
		string contactPhone = MenuOptions.GetStringInput("Enter their phone");

		_contactController.Create(new Contact
		{
			Name = contactName,
			Email = contactEmail,
			PhoneNumber = contactPhone,
		});
	}

	private void QueryContact()
	{
		throw new NotImplementedException();
	}

	private void UpdateContact()
	{
		throw new NotImplementedException();
	}

	private void DeleteContact()
	{
		throw new NotImplementedException();
	}
}
