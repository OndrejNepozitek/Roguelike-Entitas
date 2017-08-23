using System.Collections.Generic;
using Entitas;

namespace Assets.Sources.Features.Items.Components
{
	public sealed class InventoryComponent : IComponent
	{
		public Dictionary<InventorySlot, InventoryItem> Items = new Dictionary<InventorySlot, InventoryItem>();
	}
}