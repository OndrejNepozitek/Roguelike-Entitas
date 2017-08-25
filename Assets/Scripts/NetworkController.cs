using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Sources.Helpers;
using Assets.Sources.Helpers.Networking;
using Assets.Sources.Helpers.Networking.ControlMessages;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour
{
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

	public void Awake()
	{
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
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

	public void HostGame()
	{
		int port;
		int.TryParse(HostPort.text, out port);

		var server = new Server();
		server.Connect(null, port, HostName.text);

		NetworkEntity = server;
		IsServer = true;
		readyPlayers = 0;

		HostPanel.SetActive(false);
		StartGameButton.SetActive(true);
		LobbyPanel.SetActive(true);

		RefreshPlayerList();
		NetworkEntity.RegisterHandler(typeof(ConnectedMessage), (message, player) => RefreshPlayerList());
		NetworkEntity.RegisterHandler(typeof(DisconnectedMessage), (message, player) => RefreshPlayerList());
		NetworkEntity.RegisterHandler(typeof(StartGameMessage), HandleStartGame);
		NetworkEntity.RegisterHandler(typeof(GameStateMessage), HandleGameState);
	}

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

	public void StartGame()
	{
		var message = new StartGameMessage() {Seed = 1};
		var server = NetworkEntity as Server; // TODO: stupid
		server.SendMessage(message);
	}

	private void HandleStartGame(IControlMessage rawMessage, Player player)
	{
		var message = rawMessage as StartGameMessage;
		Seed = message.Seed;

		IsMultiplayer = true;
		SceneManager.LoadScene("Main");
	}

	public void SendWaitingForPlayers()
	{
		NetworkEntity.SendMessage(new GameStateMessage() {State = GameState.WaitingForPlayers});
	}

	private void HandleGameState(IControlMessage rawMessage, Player player)
	{
		var message = rawMessage as GameStateMessage;

		switch (message.State)
		{
			case GameState.WaitingForPlayers:
				readyPlayers++;
				if (readyPlayers == NetworkEntity.Players.Count()) // TODO: should it be linq?
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

	private void HandleHostDisconnected(IControlMessage rawMessage, Player player)
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

	public void JoinGame()
	{
		var parts = ClientAddress.text.Trim().Split(':');
		var port = 0;

		if (parts.Length >= 2)
		{
			int.TryParse(parts[1], out port);
		}
		
		Debug.Log(parts[0]);
		Debug.Log(port);

		var client = new Client();
		client.Connect(parts[0].Trim(), port, HostName.text);

		NetworkEntity = client;
		IsServer = false;

		JoinPanel.SetActive(false);
		StartGameButton.SetActive(false);
		LobbyPanel.SetActive(true);

		RefreshPlayerList();
		NetworkEntity.RegisterHandler(typeof(ConnectedMessage), (message, player) => RefreshPlayerList());
		NetworkEntity.RegisterHandler(typeof(DisconnectedMessage), (message, player) => RefreshPlayerList());
		NetworkEntity.RegisterHandler(typeof(WelcomeMessage), (message, player) => RefreshPlayerList());
		NetworkEntity.RegisterHandler(typeof(StartGameMessage), HandleStartGame);
		NetworkEntity.RegisterHandler(typeof(GameStateMessage), HandleGameState);
		NetworkEntity.RegisterHandler(typeof(HostDisconnectedMessage), HandleHostDisconnected);
	}

	public void StartSinglePlayer()
	{
		IsMultiplayer = false;
		SceneManager.LoadScene("Main", LoadSceneMode.Single);
	}

	private void Update()
	{
		if (NetworkEntity != null)
		{
			NetworkEntity.WatchNetwork();
		}
	}

	public void LeaveLobby()
	{
		NetworkEntity.Disconnect();
	}
}
