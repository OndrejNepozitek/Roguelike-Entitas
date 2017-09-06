namespace Assets.Scripts
{
	using System.Text;
	using Entitas;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;

	public class GameGUIController : MonoBehaviour {

		public Text log;
		public GameController GameController;
		public InventoryController InventoryController;

		public GameObject MenuCanvas;
		private bool isInMenu;

		public GameObject RespawnCanvas;
		public Text RespawnText;

		void Start () {
			GameController = GetComponent<GameController>();
			InventoryController = GetComponent<InventoryController>();
		}

		private void Log_OnComponentReplaced(IEntity entity, int index, IComponent previous, IComponent next)
		{
			var logComp = (LogComponent)next;
			var builder = new StringBuilder();

			foreach (var message in logComp.queue)
			{
				builder.Append(message + System.Environment.NewLine);
			}

			log.text = builder.ToString();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (InventoryController.IsOpened)
				{
					InventoryController.Close();
					return;
				}

				if (isInMenu)
				{
					GameController.UnpauseGame();
				}
				else
				{
					GameController.PauseGame();
				}

				MenuCanvas.SetActive(!isInMenu);
				isInMenu = !isInMenu;
			}
		}

		public void BackToMainMenu()
		{
			GameController.StopGame();

			if (NetworkController.Instance.IsMultiplayer)
			{
				NetworkController.Instance.NetworkEntity.Disconnect();
			}

			SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
		}

		public void ShowPlayerRespawning()
		{
			SetRespawnTime(5);
			RespawnCanvas.SetActive(true);
		}

		public void HidePlayerRespawning()
		{
			RespawnCanvas.SetActive(false);
		}

		public void SetRespawnTime(int seconds)
		{
			RespawnText.text = "You will respawn in " + seconds;
		}
	}
}
