using System.Collections;
using Presentation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Domain
{
    public class MenuScene : MonoBehaviour
    {
        public Text welcomeText;
        public Button playButton;
        public Text inputText;

        private void Start() 
        {
            AddButtonListener(); 
        }

        private void AddButtonListener()
        {
            playButton.onClick.AddListener(() => {
                FindObjectOfType<AudioManager>().Play("Click");
                FindObjectOfType<FirebaseManager>().SignIn(inputText.text);
                StartCoroutine(ChangeScene());
            });
        }

        private IEnumerator ChangeScene() 
        {
            GameUiChanger.ChangePosition(playButton.GetComponent<RectTransform>(), new Vector2(1000, 38), 0.7f);
            GameUiChanger.ChangePosition(welcomeText.GetComponent<RectTransform>(), new Vector2(-1000, 65), 0.7f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }
    }
}
