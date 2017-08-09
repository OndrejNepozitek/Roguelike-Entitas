using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGUIController : MonoBehaviour {

	public void StartSinglePlayer()
	{
		NetworkController.Instance.StartSinglePlayer();
	}

	public void HostGame()
	{
		NetworkController.Instance.HostGame();
	}

	public void JoinGame()
	{
		NetworkController.Instance.JoinGame();
	}
}
