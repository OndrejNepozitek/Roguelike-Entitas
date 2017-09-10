namespace Assets.Sources.Helpers.Items
{
	using System.Text;
	using Features.Items;
	using Features.Stats.Components;

	public class Helmet : Equipment
	{
		public override InventorySlot Slot
		{
			get { return InventorySlot.Head; }
		}

		public int Defense { get; private set; }

		public int CriticalChance { get; private set; }

		public Helmet(ItemName name, string prefab, int defense, int criticalChance = 0)
		{
			Name = name;
			Prefab = prefab;
			Defense = defense;
			CriticalChance = criticalChance;
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

			if (CriticalChance != 0)
			{
				sb.AppendLine();
				sb.AppendFormat("Critical chance: {0}", CriticalChance);
			}

			return sb.ToString();
		}

		public override void ModifyStats(StatsComponent stats)
		{
			stats.Defense += Defense;
			stats.CriticalChance += CriticalChance;
		}
	}
}
