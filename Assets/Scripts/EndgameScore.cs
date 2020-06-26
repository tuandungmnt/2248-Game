using UnityEngine;
using UnityEngine.UI;

public class EndgameScore : MonoBehaviour
{   
    public Text txt;

    void Start()
    {
        txt.text = GameMaster.score.ToString();   
    }
}
