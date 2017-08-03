public class Weapon : IArmor
{
	public InventorySlot Slot
	{
		get
		{
			return InventorySlot.Weapon;
		}
	}

	public string Prefab { get; private set; }

	public float Attack { get; private set; }

	public Weapon(string prefab, float attack)
	{
		Prefab = prefab;
		Attack = attack;
	}
}