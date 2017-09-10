namespace Assets.Sources.Helpers.Loot
{
	using System;
	using System.Collections.Generic;
	using Items;

	public class LootGroup
	{
		private readonly List<Group> items = new List<Group>();

		public void AddItem(ItemName item, float chance)
		{
			if (chance > 1 || chance < 0)
			{
				throw new ArgumentOutOfRangeException("chance", "Chance must be between 0 and 1 (inclusive)");
			}

			items.Add(new Group(item, chance));
		}

		public ItemName? GetLoot(int seed)
		{
			var chosenItems = new List<ItemName>();
			var random = new Random(seed);

			foreach (var group in items)
			{
				if (random.NextDouble() < group.Chance)
				{
					chosenItems.Add(group.Item);
				}
			}

			if (chosenItems.Count == 0)
			{
				return null;
			}

			var randomIndex = random.Next(0, chosenItems.Count);
			var item = chosenItems[randomIndex];

			return item;
		}

		private class Group
		{
			public readonly ItemName Item;
			public readonly float Chance;

			public Group(ItemName item, float chance)
			{
				Item = item;
				Chance = chance;
			}
		}
	}
}
