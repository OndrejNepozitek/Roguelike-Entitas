namespace Assets.Sources.Helpers
{
	using System.Collections.Generic;

	public abstract class AbstractDatabase<TKey, TValue>
	{
		protected readonly Dictionary<TKey, TValue> Items = new Dictionary<TKey, TValue>();

		public virtual TValue GetItem(TKey key)
		{
			return Items[key];
		}

		public virtual void RegisterItem(TKey key, TValue value)
		{
			Items.Add(key, value);
		}
	}
}
