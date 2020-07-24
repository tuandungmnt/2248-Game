using Data;
using Presentation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Domain
{
    public class EndScene : MonoBehaviour
    {
        public Button replayButton;
        public Button scoreBoardButton;
        public Button closeScoreBoardButton;
        public Button shareButton;
        public Text scoreTagText;
        public Text scoreText;

        private void Start() {
            AddButtonListener();
            UpdateScene();
        
            FindObjectOfType<AdsManager>().PlayInitializeAds();
            FindObjectOfType<AudioManager>().Play("End");
        }

        private void AddButtonListener()
        {
            replayButton.onClick.AddListener(() => 
            {
                FindObjectOfType<AudioManager>().Play("Click");
                SceneManager.LoadScene(1);
            });  
        
            scoreBoardButton.onClick.AddListener(async () =>
            {
                FindObjectOfType<AudioManager>().Play("Click");
                await FindObjectOfType<FirebaseManager>().UpdateScoreBoard();
                Debug.Log("hello");
                ScoreBoardPresenter.UpdateScoreBoard(ScoreData.yourPosition.ToString());
                for (var i = 0; i < 5; ++i)
                    ScoreBoardPresenter.UpdateScoreBoard(i, ScoreData.scoreBoardName[i], ScoreData.scoreBoardScore[i]);
                FindObjectOfType<ScoreBoardPresenter>().ShowScoreBoard();
            });
        
            closeScoreBoardButton.onClick.AddListener(() =>
            {    
                FindObjectOfType<AudioManager>().Play("Click");
                FindObjectOfType<ScoreBoardPresenter>().HideScoreBoard();
            });
            shareButton.onClick.AddListener(() =>
            {
                FindObjectOfType<AudioManager>().Play("Click");
                FindObjectOfType<ShareManager>().Share();
            });
        }

        private void UpdateScene()
        {
            scoreText.text = ScoreData.currentScore.ToString();

            if (ScoreData.currentScore <= ScoreData.bestScore) return;
            ScoreData.bestScore = ScoreData.currentScore;
            scoreTagText.text = "New Best Score!!";
            FindObjectOfType<FirebaseManager>().SaveScore();
        }
    }
}
