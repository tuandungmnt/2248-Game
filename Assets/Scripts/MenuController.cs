using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    public Button buttonPlay;
    public Text welcomeText;
    public Canvas actionMenu;

    int matchWidthOrHeight = 0;
    int sh = 0;
    public static int bestScore;

    void Start() {
        buttonPlay.onClick.AddListener(() => {
            StartCoroutine( ChangeScene() ); 
        });  
        bestScore = PlayerPrefs.GetInt("bestScore");
    }

    IEnumerator ChangeScene() {
        FindObjectOfType<AudioManager>().Play("Click");
        buttonPlay.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1000, 38), 0.7f);
        welcomeText.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-1000, -65), 0.7f);
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(1);
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
