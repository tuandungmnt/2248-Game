using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using DG.Tweening;

public class BlockController : MonoBehaviour {
    int num = 0;
    bool clicked = false;
    System.Random rand = new System.Random();
    Tweener tween;

    public void SetPosition(int i,int j) {
        Vector3 pos = new Vector3();
        pos.x = 60 + i * 100;
        pos.y = -300 + j * 100;
        this.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void SetNum(int t) {
        num = t;
        this.GetComponentInChildren<Text>().text = num.ToString();
    }

    public int GetNum() {
        return num;
    }

    public void Click() {
        if (clicked == true) return;
        clicked = true;
        this.GetComponent<Image>().color = new Color(0, 0, 0);
    }

    public void Unclick() {
        if (clicked == false) return;
        clicked = false;
        this.GetComponent<Image>().color = new Color(255, 255, 255);
    }

    public bool GetClicked() {
        return clicked;
    }

    public void MovePosition(Vector2 x, float time) {
        this.GetComponent<RectTransform>().DOAnchorPos(x, time).SetEase(Ease.OutQuad);
    }
}
