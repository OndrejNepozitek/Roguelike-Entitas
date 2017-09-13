namespace Assets.Sources.Helpers
{
	/// <summary>
	/// States of the game
	/// </summary>
	public enum GameState
	{
		Unknown, // For protobuf
		NotStarted, WaitingForPlayers, Running, Paused
	}
}
