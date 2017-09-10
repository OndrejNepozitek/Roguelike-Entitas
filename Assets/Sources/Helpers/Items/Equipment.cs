using System.Text.RegularExpressions;
using Assets.Sources.Features.Items;
using Assets.Sources.Features.Stats.Components;
using Assets.Sources.Helpers.Items;

public abstract class Equipment : IItem
{
	public ItemName Name { get; protected set; }
	public string Prefab { get; protected set; }
	public abstract InventorySlot Slot { get; }

	public virtual string ToInventoryString()
	{
		return ToString();
	}

	public virtual void ModifyStats(StatsComponent stats)
	{

	}

	public virtual string NiceName()
	{
		var split = Regex.Split(Name.ToString(), @"(?<!^)(?=[A-Z])");

		return string.Join(" ", split);
	}
}