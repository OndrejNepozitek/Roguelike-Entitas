namespace Assets.Sources.Helpers
{
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Helper functions to be used in coroutines
	/// </summary>
	public static class CoroutineHelpers
	{
		public static IEnumerable<Vector3> MoveTowards(Vector3 from, Vector3 to, float totalTime)
		{
			var currentTime = 0f;

			while (currentTime < totalTime)
			{
				currentTime += Time.deltaTime;
				var t = currentTime / totalTime; // Progress percentage

				t = t * t * t * (t * (6f * t - 15f) + 10f);

				yield return Vector3.Lerp(from, to, t);
			}
		}

		public static IEnumerable<Vector3> MoveTowardsAndBack(Vector3 from, Vector3 to, float totalTime)
		{
			foreach (var position in MoveTowards(from, to, totalTime / 2))
			{
				yield return position;
			}

			foreach (var position in MoveTowards(to, from, totalTime / 2))
			{
				yield return position;
			}
		}
	}
}
