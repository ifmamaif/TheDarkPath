using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheDarkPath
{
    public class SceneController : EventBehaviour
    {
        private const string GAMEPLAY_SCENE = "Gameplay";
        private const string GAME_OVER_SCENE = "GameOver";
        private const string MAIN_MENU_SCENE = "Menu";
        private const string PLAYER_GAMEOBJECT_NAME = "Player";

        public Transform PlayerTransform { get; private set; }

        private void Start()
        {
            var playerGameObject = GameObject.Find(PLAYER_GAMEOBJECT_NAME);
            PlayerTransform = playerGameObject == null ? null : playerGameObject.transform;
            RegisterEvent(EventSystem.EventType.PlayerDeath, () => { LoadGameOverScene(); });
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == GAMEPLAY_SCENE)
            {
                ScoreScript.scoreValue = 0;
                SceneManager.LoadScene(MAIN_MENU_SCENE);
            }
        }

        // Used in Menu scene, called by StartGameButton
        public void LoadGameplayScene()
        {
            SceneManager.LoadScene(GAMEPLAY_SCENE);
        }

        public void LoadGameOverScene()
        {
            SceneManager.LoadScene(GAME_OVER_SCENE);
        }

        // Used in GameOver Scened, called by MainMenuButton
        public void LoadMainMenuScene()
        {
            ScoreScript.scoreValue = 0;
            SceneManager.LoadScene(MAIN_MENU_SCENE);
        }

        // Used in Menu scene, called by QuitGameButton
        public void QuitGame()
        {
            Application.Quit();
        }

        public void TeleportPlayer(Transform newPosition)
        {
            if (PlayerTransform == null)
            {
                Debug.LogError("Couldn't find the gameobject with the name " + PLAYER_GAMEOBJECT_NAME);
            }
            else
            {
                PlayerTransform.position = newPosition.position;
            }
        }
    }
}