namespace Assets.Sources.Helpers.Items
{
	using System.Text;
	using Features.Items;
	using Features.Stats.Components;

	public class BodyArmor : Equipment
	{
		public override InventorySlot Slot
		{
			get { return InventorySlot.Body; }
		}

		public int Defense { get; private set; }

		public int MaxHealth { get; private set; }

		public BodyArmor(ItemName name, string prefab, int defense, int maxHealth = 0)
		{
			Name = name;
			Prefab = prefab;
			Defense = defense;
			MaxHealth = maxHealth;
		}

		public override string ToInventoryString()
		{
			var sb = new StringBuilder();

			sb.AppendFormat(NiceName());

			if (Defense != 0)
			{
				sb.AppendLine();
				sb.AppendFormat("Defense: {0}", Defense);
			}

			if (MaxHealth != 0)
			{
				sb.AppendLine();
				sb.AppendFormat("Max health: {0}", MaxHealth);
			}

			return sb.ToString();
		}

		public override void ModifyStats(StatsComponent stats)
		{
			stats.Defense += Defense;
			stats.MaxHealth += MaxHealth;
		}
	}
}
