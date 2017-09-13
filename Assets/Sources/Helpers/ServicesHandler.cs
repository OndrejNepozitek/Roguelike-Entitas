namespace Assets.Sources.Helpers
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Class to handle services that are meant to be singletons in the game.
	/// </summary>
	public class ServicesHandler
	{
		private readonly Dictionary<Type, object> items = new Dictionary<Type, object>();

		public T GetItem<T>()
		{
			return (T) items[typeof(T)];
		}

		public void AddItem<T>(T item)
		{
			items.Add(typeof(T), item);
		}
	}
}
