using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Domain
{
    public class GameScene : MonoBehaviour
    {
        public Button endButton;
        public Text bestScoreText;
        private void Start()
        {
            AddButtonListener();
            UpdateScene();
        }

        private void AddButtonListener()
        {
            endButton.onClick.AddListener( () =>
            {
                FindObjectOfType<AudioManager>().Play("Click");
                ChangeScene();
            });
        }

        private void UpdateScene()
        {
            Debug.Log("Update Game Scene: " + ScoreData.bestScore);
            bestScoreText.text = "Best Score: " + ScoreData.bestScore;
        }

        public void ChangeScene()
        {
            SceneManager.LoadScene(2);
        }
    } 
}
