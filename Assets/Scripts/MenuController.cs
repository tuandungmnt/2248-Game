using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button buttonPlay;
    public Canvas actionMenu;

    int matchWidthOrHeight = 0;
    int sh = 0;
    public static int bestScore;

    void Start() {
        buttonPlay.onClick.AddListener(() => {
            FindObjectOfType<AudioManager>().Play("Click");
            SceneManager.LoadScene(1);
        });  
        bestScore = PlayerPrefs.GetInt("bestScore");
    }

    void Update() {
        if (Camera.main.aspect > 16f / 9f) sh = 1;
            else sh = 0;
        if (matchWidthOrHeight != sh) {
            matchWidthOrHeight = sh;
            actionMenu.GetComponent<CanvasScaler>().matchWidthOrHeight = sh;
        }
    }
}
