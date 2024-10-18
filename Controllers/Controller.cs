using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Controllers;

/// <summary>
/// Contains CRUD operations for the Contact and Category models.
/// </summary>
/// <typeparam name="T">Use the Contact or Category class here</typeparam>
internal abstract class Controller<T> where T : class
{
	private readonly YellowPagesContext _context;

	public Controller(YellowPagesContext context)
	{
		_context = context;
	}

	public virtual void Create(T entity)
	{
		try
		{
			_context.Set<T>().Add(entity);
			_context.SaveChanges();
			Console.WriteLine($"{typeof(T).Name} created.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unable to create {typeof(T).Name}: {ex.Message}");
		}
	}

	public T Query(int id)
	{
		try
		{
			var entity = _context.Set<T>().Find(id);
			if (entity == null)
				throw new Exception($"Unable to find {typeof(T).Name} with ID {id}.");

			return entity;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unable to find {typeof(T).Name} with ID {id}. Error details: {ex.Message}");
			return null;
		}
	}

	public List<T> QueryAll()
	{
		try
		{
			var entities = _context.Set<T>().ToList();
			if (entities == null)
				throw new Exception($"Unable to find a set of {typeof(T).Name}.");

			return entities;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unable to find a set of {typeof(T).Name}. Error details: {ex.Message}");
			return null;
		}
	}

	public void Update(T entity)
	{
		try
		{
			_context.Set<T>().Update(entity);
			_context.SaveChanges();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unable to edit entity for {typeof(T).Name}. Error details: {ex.Message}");
		}
	}

	public void Delete(T entity)
	{
		try
		{
			_context.Set<T>().Remove(entity);
			_context.SaveChanges();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unable to delete entity of type {typeof(T).Name}. Error details: {ex.Message}");
		}
	}
}
