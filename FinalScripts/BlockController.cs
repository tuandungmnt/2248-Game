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

    private void Start()
    {
        this.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 0.8f);
    }

    public void SetPosition(int i,int j) {
        Vector3 pos = new Vector3();
        pos.x = -250 + i * 125;
        pos.y = 85 + j * 125;
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
        this.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
    }

    public void Unclick() {
        if (clicked == false) return;
        clicked = false;
        this.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 0.8f);
    }

    public bool GetClicked() {
        return clicked;
    }

    public void MovePosition(Vector2 x, float time) {
        this.GetComponent<RectTransform>().DOAnchorPos(x, time).SetEase(Ease.OutQuad);
    }

    IEnumerator CoChangeNum(int newNum) {
        if (newNum - num < 10) {
            for (int i = num + 1; i <= newNum; ++i) { 
                this.GetComponentInChildren<Text>().text = i.ToString();
                yield return new WaitForSeconds(0.07f);
            }
        }
        else {
            float xxx = (float) (newNum - num) / 10;
            float yyy = num;
            for (int i = 0; i < 10; ++i) {
                yyy += xxx;
                int zzz = (int) yyy;
                this.GetComponentInChildren<Text>().text = zzz.ToString();
                yield return new WaitForSeconds(0.07f);
            }
        }
        SetNum(newNum);
    }

    public void ChangeNum(int newNum) {
        StartCoroutine( CoChangeNum(newNum) );
    } 
}
