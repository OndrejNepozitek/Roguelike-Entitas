using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Sources.Helpers
{
	public enum GameState
	{
		Unknown, // For protobuf
		NotStarted, WaitingForPlayers, Running, Paused
	}
}
