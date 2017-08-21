using Assets.Sources.Features.Items;

public abstract class Equipment : IItem
{
	public ItemName Name { get; protected set; }
	public string Prefab { get; protected set; }
	public abstract InventorySlot Slot { get; }
}