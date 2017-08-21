using Assets.Sources.Features.Items;

public interface IItem
{
	ItemName Name { get; }
	string Prefab { get; }
	InventorySlot Slot { get; }
}