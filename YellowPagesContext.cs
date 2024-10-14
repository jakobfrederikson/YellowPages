using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Models;

namespace YellowPages;

internal class YellowPagesContext : DbContext
{
	public DbSet<Contact> Contacts { get; set; }
	public DbSet<Category> Categories { get; set; }

	public string DbPath { get; set; }

	public YellowPagesContext()
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		DbPath = System.IO.Path.Join(path, "YellowPages.db");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	=> options.UseSqlite($"Data Source={DbPath}");
}
