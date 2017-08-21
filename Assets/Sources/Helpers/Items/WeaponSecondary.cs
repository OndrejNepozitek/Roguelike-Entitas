namespace Assets.Sources.Helpers.Items
{
	using Features.Items;

	public class WeaponSecondary : Equipment
	{
		public override InventorySlot Slot
		{
			get
			{
				return InventorySlot.WeaponSecondary;
			}
		}

		public WeaponSecondary(ItemName name, string prefab)
		{
			Name = name;
			Prefab = prefab;
		}
	}
}