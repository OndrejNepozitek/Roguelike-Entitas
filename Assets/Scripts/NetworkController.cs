namespace Assets.Scripts
{
	using System;
	using System.Text;
	using Sources.Helpers;
	using Sources.Helpers.Networking;
	using Sources.Helpers.Networking.ControlMessages;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;

	public class NetworkController : MonoBehaviour
	{
		#region Properties and fields

		public static NetworkController Instance;

		public event Action OnGameStarted;
		public event Action OnGameEnded;

		public bool IsMultiplayer { get; private set; }
		public bool IsServer { get; private set; }

		public Text HostPort;
		public Text HostName;

		public Text ClientAddress;
		public Text ClientName;

		public Text PlayersList;

		public GameObject HostPanel;
		public GameObject JoinPanel;
		public GameObject LobbyPanel;
		public GameObject MainPanel;

		public GameObject StartGameButton;

		public NetworkEntity NetworkEntity;

		public int Seed;
		private int readyPlayers;

		#endregion

		#region Network event handlers

		/// <summary>
		/// Load Main scene when the game starts.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="player"></param>
		private void HandleStartGame(StartGameMessage message, Player player)
		{
			Seed = message.Seed;

			IsMultiplayer = true;
			SceneManager.LoadScene("Main");
		}

		/// <summary>
		/// Trigger OnGameEnded() event when host disconnects.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="player"></param>
		private void HandleHostDisconnected(HostDisconnectedMessage message, Player player)
		{
			// TODO: should check if the game started
			if (LobbyPanel != null)
			{
				LobbyPanel.SetActive(false);
				MainPanel.SetActive(true);
			}

			if (OnGameEnded != null)
			{
				OnGameEnded();
			}
		}

		/// <summary>
		/// Handle GameState changes.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="player"></param>
		private void HandleGameState(GameStateMessage message, Player player)
		{
			switch (message.State)
			{
				case GameState.WaitingForPlayers:
					readyPlayers++;
					if (readyPlayers == NetworkEntity.Players.Count)
					{
						NetworkEntity.SendMessage(new GameStateMessage() { State = GameState.Running });
					}
					break;

				case GameState.Running:
					if (OnGameStarted != null)
					{
						OnGameStarted();
					}
					break;
			}
		}

		#endregion

		#region Button callbacks

		/// <summary>
		/// Start singleplayer game.
		/// </summary>
		public void StartSinglePlayer()
		{
			IsMultiplayer = false;
			SceneManager.LoadScene("Main", LoadSceneMode.Single);
		}

		/// <summary>
		/// Try to join a game with given information.
		/// </summary>
		public void JoinGame()
		{
			// Parse information and try to connect
			var parts = ClientAddress.text.Trim().Split(':');
			var port = 0;

			if (parts.Length >= 2)
			{
				int.TryParse(parts[1], out port);
			}

			var client = new Client();
			client.Connect(parts[0].Trim(), port, HostName.text);

			NetworkEntity = client;
			IsServer = false;

			// Show lobby panel
			JoinPanel.SetActive(false);
			StartGameButton.SetActive(false);
			LobbyPanel.SetActive(true);

			// Register event handlers
			RefreshPlayerList();
			RegisterCommonHandlers();
			NetworkEntity.RegisterHandler<WelcomeMessage>((message, player) => RefreshPlayerList());
			NetworkEntity.RegisterHandler<HostDisconnectedMessage>(HandleHostDisconnected);
		}

		/// <summary>
		/// Try to host a game.
		/// </summary>
		public void HostGame()
		{
			// Parse information and try to host
			int port;
			int.TryParse(HostPort.text, out port);

			var server = new Server();
			server.Connect(null, port, HostName.text);

			NetworkEntity = server;
			IsServer = true;
			readyPlayers = 0;

			// Show lobby panel
			HostPanel.SetActive(false);
			StartGameButton.SetActive(true);
			LobbyPanel.SetActive(true);

			// Register event handlers
			RefreshPlayerList();
			RegisterCommonHandlers();
		}

		/// <summary>
		/// Start game as a host.
		/// </summary>
		public void StartGame()
		{
			var message = new StartGameMessage() { Seed = 1 }; // Seed should be random later
			NetworkEntity.SendMessage(message);
		}

		/// <summary>
		/// Leave lobby.
		/// </summary>
		public void LeaveLobby()
		{
			NetworkEntity.Disconnect();
		}

		#endregion

		#region Other methods

		/// <summary>
		/// Register common event handlers
		/// </summary>
		private void RegisterCommonHandlers()
		{
			NetworkEntity.RegisterHandler<ConnectedMessage>((message, player) => RefreshPlayerList());
			NetworkEntity.RegisterHandler<DisconnectedMessage>((message, player) => RefreshPlayerList());
			NetworkEntity.RegisterHandler<StartGameMessage>(HandleStartGame);
			NetworkEntity.RegisterHandler<GameStateMessage>(HandleGameState);
		}

		/// <summary>
		/// Unity hook.
		/// </summary>
		private void Update()
		{
			if (NetworkEntity != null)
			{
				NetworkEntity.WatchNetwork();
			}
		}

		/// <summary>
		/// Refresh the list of players in the lobby.
		/// </summary>
		private void RefreshPlayerList()
		{
			var stringBuilder = new StringBuilder();

			foreach (var player in NetworkEntity.Players)
			{
				stringBuilder.Append(player);
				stringBuilder.Append(Environment.NewLine);
			}

			if (PlayersList != null)
			{
				PlayersList.text = stringBuilder.ToString();
			}
		}

		/// <summary>
		/// Handle initial load and make this instance a singleton.
		/// </summary>
		public void Awake()
		{
			if (Instance == null)
			{
				DontDestroyOnLoad(gameObject);
				Instance = this;
			}
			else if (Instance != this)
			{
				// GUI elements are rebinded after scene loads
				Instance.HostPort = HostPort;
				Instance.HostName = HostName;
				Instance.ClientAddress = ClientAddress;
				Instance.ClientName = ClientName;
				Instance.PlayersList = PlayersList;
				Instance.HostPanel = HostPanel;
				Instance.JoinPanel = JoinPanel;
				Instance.LobbyPanel = LobbyPanel;
				Instance.MainPanel = MainPanel;
				Instance.StartGameButton = StartGameButton;

				Destroy(gameObject);
			}
		}

		/// <summary>
		/// Send a message that a client is ready and waiting for other players.
		/// </summary>
		/// <remarks>
		/// TODO: this method is currently called from the GameController which may not be the best idea
		/// as other methods are events.
		/// </remarks>
		public void SendWaitingForPlayers()
		{
			NetworkEntity.SendMessage(new GameStateMessage() { State = GameState.WaitingForPlayers });
		}

		#endregion
	}
}
