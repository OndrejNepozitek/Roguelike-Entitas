using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Sources.Helpers.MapGen
{
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
