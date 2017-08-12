using System.Collections.Generic;

public class ItemDatabase
{
	private Dictionary<ItemName, IItem> items = new Dictionary<ItemName, IItem>();

	private static ItemDatabase _instance;
	public static ItemDatabase Instance
	{
		get { return _instance ?? (_instance = new ItemDatabase()); }
	}

	public void Reset()
	{
		items = new Dictionary<ItemName, IItem>();
	}

	public IItem GetItem(ItemName name)
	{
		return items[name];
	}

	public void RegisterItem(IItem item)
	{
		items.Add(item.Name, item);
	}
}