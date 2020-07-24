using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class ScoreBoardPresenter : MonoBehaviour
    {
        public GameObject scoreBoardPanel;
        public Text yourPositionText;
        private static GameObject[] _nameText;
        private static GameObject[] _scoreText;
        private static string _yourPosition;

        private void Start()
        {
            _nameText = new GameObject[5];
            _scoreText = new GameObject[5];
        
            for (var i = 0; i < 5; ++i)
            {
                _nameText[i] = FindObjectOfType<GameUiCreator>().CreateNameText(scoreBoardPanel);
                GameUiChanger.SetPosition(_nameText[i], new Vector2(0, 260 - 130 * i));
            
                _scoreText[i] = FindObjectOfType<GameUiCreator>().CreateScoreText(scoreBoardPanel);
                GameUiChanger.SetPosition(_scoreText[i], new Vector2(0, 260 - 130 * i));
            }
        }

        public static void UpdateScoreBoard(int n, string username, string score)
        {
            //GameUiChanger.SetText(_nameText[n], username);
            //GameUiChanger.SetText(_scoreText[n], score);
            _nameText[n].GetComponent<Text>().text = username;
            _scoreText[n].GetComponent<Text>().text = score;
        }

        public static void UpdateScoreBoard(string n)
        {
            _yourPosition = n;
        }

        public void ShowScoreBoard()
        {
            GameUiChanger.SetText(yourPositionText, "Your Position: " + _yourPosition);
            GameUiChanger.ChangePosition(scoreBoardPanel, new Vector2(0,0), 1f);
        }

        public void HideScoreBoard()
        {
            GameUiChanger.ChangePosition(scoreBoardPanel, new Vector2(720,0), 1f);
        }
    }
}
