using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button buttonPlay;
    public Canvas actionMenu;

    int matchWidthOrHeight = 0;
    int sh = 0;

    void Start() {
        buttonPlay.onClick.AddListener(() => {
            SceneManager.LoadScene(1);
        });   
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
