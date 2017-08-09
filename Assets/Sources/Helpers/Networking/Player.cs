namespace Assets.Sources.Helpers.Networking
{
	public class Player
	{
		public Player(int id, string name)
		{
			Name = name;
			Id = id;
		}

		public string Name { get; set; }
		public int Id { get; private set; }
	}
}