using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class blockcontroler : MonoBehaviour {
    float x;
    float y;
    float z = 1000f;
    float speed = 3000f;
    float delaytime = -1f;
    public int num = 0;
    public bool clicked = false;
    System.Random rand = new System.Random();

    void Start() {
    }

    public void setPosition(int i,int j) {
        Vector3 pos = new Vector3();
        pos.x = 660 + i * 100;
        pos.y = -300 + j * 100;
        this.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void setText(int t) {
        num = t;
        this.GetComponentInChildren<Text>().text = num.ToString();
    }

    public int getNum() {
        return num;
    }

    public void click() {
        if (clicked == true) return;
        clicked = true;
        this.GetComponent<Image>().color = new Color(0, 0, 0);
    }

    public void unclick() {
        if (clicked == false) return;
        clicked = false;
        this.GetComponent<Image>().color = new Color(255, 255, 255);
    }

    public bool getClicked() {
        return clicked;
    }

    public void movetoPosition(int n) {
        Vector3 pos = this.GetComponent<RectTransform>().anchoredPosition;
        z = pos.y - n * 100;
        delaytime = 0f;
    }

    void Update() {
        if (delaytime < 0f) return;

        if (delaytime < 0.3f) {
            delaytime += Time.deltaTime;
            return;
        }
        
        Vector3 pos = this.GetComponent<RectTransform>().anchoredPosition;
        if (pos.y > z) {
            pos.y -= speed * Time.deltaTime;
            if (pos.y <= z) {
                pos.y = z;
                delaytime = -1f;
            }
        }
        this.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
