namespace Assets.Sources.Helpers.Items
{
	using System.Text;
	using Features.Items;
	using Features.Stats.Components;

	public class Weapon : Equipment
	{
		public override InventorySlot Slot
		{
			get
			{
				return InventorySlot.Weapon;
			}
		}

		public int Attack { get; private set; }
		public int MovementSpeed { get; private set; }
		public int CriticalChance { get; private set; }

		public Weapon(ItemName name, string prefab, int attack, int movementSpeed = 0, int criticalChance = 0)
		{
			Name = name;
			Prefab = prefab;
			Attack = attack;
			MovementSpeed = movementSpeed;
			CriticalChance = criticalChance;
		}

		public override string ToInventoryString()
		{
			var sb = new StringBuilder();

			sb.AppendFormat(NiceName());

			if (Attack != 0)
			{
				sb.AppendLine();
				sb.AppendFormat("Damage: {0}", Attack);
			}

			if (MovementSpeed != 0)
			{
				sb.AppendLine();
				sb.AppendFormat("Mvmt. speed: {0}", MovementSpeed);
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
			stats.Attack += Attack;
			stats.MovementSpeed += MovementSpeed;
			stats.CriticalChance += CriticalChance;
		}
	}
}