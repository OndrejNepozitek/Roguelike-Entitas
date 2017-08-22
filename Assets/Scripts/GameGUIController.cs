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

		public GameObject MenuCanvas;
		private bool isInMenu;

		void Start () {
			/* IGroup<GameEntity> group = Contexts.sharedInstance.game.GetGroup(GameMatcher.Log);
        var logEntity = group.GetSingleEntity();
        logEntity.OnComponentReplaced += Log_OnComponentReplaced;*/
			Debug.Log("dasda");
			GameController = GetComponent<GameController>();
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
			SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
		}
	}
}
