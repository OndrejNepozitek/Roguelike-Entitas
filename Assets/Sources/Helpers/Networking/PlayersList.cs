namespace Assets.Sources.Helpers.Networking
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// List of players to be used in multiplayer games.
	/// </summary>
	public class PlayersList : IEnumerable<Player>
	{
		private readonly Dictionary<int, Player> players = new Dictionary<int, Player>();

		public int Count
		{
			get { return players.Count; }
		}

		public Player GetPlayer(int id)
		{
			Player player;
			if (players.TryGetValue(id, out player))
			{
				return player;
			}

			return null;
		}

		public bool AddPlayer(Player player)
		{
			if (players.ContainsKey(player.Id))
			{
				return false;
			}

			players.Add(player.Id, player);
			return true;
		}

		public void RemovePlayer(Player player)
		{
			players.Remove(player.Id);
		}

		private void Add(Player player)
		{
			AddPlayer(player);
		}

		public IEnumerator<Player> GetEnumerator()
		{
			return players.Select(pair => pair.Value).OrderBy(player => player.Id).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
