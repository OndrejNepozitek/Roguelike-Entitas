namespace Assets.Sources.Helpers.Networking
{
	using System;
	using ProtoBuf;

	/// <summary>
	/// Class to hold player's information in a multiplayer game.
	/// </summary>
	[ProtoContract]
	public class Player
	{
		public Player()
		{
			
		}

		[ProtoMember(1)]
		public string Name { get; set; }

		[ProtoMember(2)]
		public int Id { get; private set; }

		public Player(int id, string name)
		{
			Name = name;
			Id = id;

			if (string.IsNullOrEmpty(Name))
			{
				var random = new Random();
				Name = "RandomPlayer" + random.Next(1000, 9999);
			}
		}

		public override bool Equals(object obj)
		{
			var player = obj as Player;

			if (player == null)
			{
				return false;
			}

			return Id == player.Id;
		}

		protected bool Equals(Player other)
		{
			return Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, Id);
		}
	}
}