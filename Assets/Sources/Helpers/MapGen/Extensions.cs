namespace Assets.Sources.Helpers.MapGen
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Useful extensions for map generation
	/// </summary>
	internal static class Extensions
	{
		public static T GetRandom<T>(this IEnumerable<T> tiles, Random random)
		{
			var list = tiles.ToList();

			if (list.Count == 0)
			{
				throw new InvalidOperationException("Given enumerable must not be empty");
			}

			var index = random.Next(list.Count);
			var item = list[index];

			return item;
		}
	}
}
