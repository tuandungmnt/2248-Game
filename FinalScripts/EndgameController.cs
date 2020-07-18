using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndgameController : MonoBehaviour
{
    public Button buttonReplay;
    public Button buttonScoreboard;
    public Button buttonCloseSB;
    public Text scoreTagText;
    public Text scoreText;
    public Canvas actionMenu;
    public GameObject panelSB;
    public Text yourPosText;
    public GameObject textFrefabL;
    public GameObject textFrefabR;
    public GameObject[] textSBL;
    public GameObject[] textSBR;

    public static string[] username = new string[5];
    public static string[] userscore = new string[5];
    public static int yourPos;

    int matchWidthOrHeight = 0;
    int sh = 0;

    void Start() {
        FindObjectOfType<AdsManager>().PlayInitializeAds();
        buttonReplay.onClick.AddListener(() => {
            FindObjectOfType<AudioManager>().Play("Click");
            SceneManager.LoadScene(1);
        });  
        FindObjectOfType<AudioManager>().Play("End");
        
        buttonScoreboard.onClick.AddListener(() =>
        {
            FindObjectOfType<FirebaseManager>().ScoreBoard();
        });

        buttonCloseSB.onClick.AddListener(() =>
        {
            Vector2 x = new Vector2(720, 0);
            panelSB.GetComponent<RectTransform>().DOAnchorPos(x, 1f);
        });

        scoreText.text = GameMaster.score.ToString();  
        if (GameMaster.score > MenuController.bestScore) {
            MenuController.bestScore = GameMaster.score;
            scoreTagText.text = "New Best Score!!";
            FindObjectOfType<FirebaseManager>().SaveScore();
        }
        
        textSBL = new GameObject[5];
        textSBR = new GameObject[5];
        
        for (int i = 0; i < 5; ++i)
        {
            textSBL[i] = Instantiate(textFrefabL) as GameObject;
            textSBR[i] = Instantiate(textFrefabR) as GameObject;

            textSBL[i].transform.SetParent(panelSB.transform,false);
            textSBR[i].transform.SetParent(panelSB.transform,false);

            textSBL[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 260 - 130 * i);
            textSBR[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 260 - 130 * i);
        }

    }
    
    void Update() {
        if (Camera.main.aspect > 16f / 9f) sh = 1;
            else sh = 0;
        if (matchWidthOrHeight != sh) {
            matchWidthOrHeight = sh;
            actionMenu.GetComponent<CanvasScaler>().matchWidthOrHeight = sh;
        }
    }

    public void WriteScoreBoard()
    {
        for (int i = 0; i < 5; ++i)
        {
            Debug.Log("writescoreboard" + username[i] + " " + userscore[i]);
            textSBL[i].GetComponent<Text>().text = username[i] + ": ";
            textSBR[i].GetComponent<Text>().text = userscore[i];
        }

        yourPosText.text = "Your Position: "+ yourPos.ToString();
        
        Vector2 x = new Vector2(0, 0);
        panelSB.GetComponent<RectTransform>().DOAnchorPos(x, 1f);
    }
}
