public class InventoryItem
{
	public IItem Item;
	public int Count;

	public InventoryItem(IItem item, int count)
	{
		Item = item;
		Count = count;
	}
}