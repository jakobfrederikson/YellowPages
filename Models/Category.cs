using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Models;

internal class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }

    public List<Contact> Contacts { get; set; } = new();
}
