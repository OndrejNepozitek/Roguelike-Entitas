namespace Assets.Scripts
{
	using UnityEngine;

	public class MapGenConfig : MonoBehaviour
	{
		public int MinHeight;
		public int MaxHeight;

		public int MinWidth;
		public int MaxWidth;

		public bool WithPools;
		public int CurrentSeed;
		public bool DoNotChangeSeed;

		public int BorderMaxDepth;
		public int BorderIterations;

		public GameObject PrimaryMaterial;
		public GameObject SecondaryMaterial;
		public GameObject WallsMaterial;
	}
}
