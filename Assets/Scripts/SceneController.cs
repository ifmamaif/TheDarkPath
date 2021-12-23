using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheDarkPath
{
    public class SceneController : EventBehaviour
    {
        public string sceneName;
        private string gameplayScene = "Gameplay";
        private string gameOverScene = "GameOver";
        private string mainMenuScene = "Menu";
        public Transform playerTransform;


        private void Start()
        {
            RegisterEvent(EventSystem.EventType.PlayerDeath, () => { LoadGameOverScene(); });
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == gameplayScene)
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        public void LoadGameplayScene()
        {
            SceneManager.LoadScene(gameplayScene);
        }

        public void LoadGameOverScene()
        {
            SceneManager.LoadScene(gameOverScene);
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void TeleportPlayer(Transform newPosition)
        {
            playerTransform.position = newPosition.position;
        }
    }
}