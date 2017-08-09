using System.Collections;
using System.Collections.Generic;
using Assets.Sources.Helpers.Networking;
using Assets.Sources.Helpers.Networking.ControlMessages;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour
{
	public static NetworkController Instance;

	public bool IsMultiplayer { get; private set; }



	public Text HostPort;
	public Text HostName;

	public Text ClientAddress;
	public Text ClientName;

	public Text PlayersList;

	public GameObject HostPanel;
	public GameObject JoinPanel;
	public GameObject LobbyPanel;

	public NetworkEntity NetworkEntity;

	public void Awake()
	{
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
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

		HostPanel.SetActive(false);
		LobbyPanel.SetActive(true);

		NetworkEntity.RegisterHandler(typeof(ConnectMessage), RefreshPlayerList);
	}

	private void RefreshPlayerList(IControlMessage imessage, Player player)
	{
		var message = imessage as ConnectMessage;
		PlayersList.text = message.Name;
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

		var server = new Client();
		server.Connect(parts[0].Trim(), port, HostName.text);

		NetworkEntity = server;
	}

	public void StartSinglePlayer()
	{
		IsMultiplayer = false;
		SceneManager.LoadScene("Main");
	}

	void Start () {
		
	}
	
	void Update ()
	{
		if (NetworkEntity != null)
		{
			NetworkEntity.WatchNetwork();
		}
	}
}
