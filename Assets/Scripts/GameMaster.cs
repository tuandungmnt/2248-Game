using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMaster : MonoBehaviour {
    public GameObject blockPrefab;
    public Canvas actionMenu;
    public Text txt;

    public GameObject[] block;
    public GameObject tmp;
    public GameObject sss;
    Vector3 mousePos;
    bool pressed = false;
    int[] st = new int[40];
    int n = 0;
    System.Random rand = new System.Random();
    public static int score = 0;
    int[] s1 = {-1, 0, 1, 1, 1, 0, -1, -1};
    int[] s2 = {1, 1, 1, 0, -1, -1, -1, 0};
    int cnt;
    float w;

    void Start() {
        score = 0;
        block = new GameObject[35];        

        for (int i = 0; i < 5; ++i)
        for (int j = 0; j < 7; ++j) {
            int x = i * 7 + j;
            block[x] = Instantiate(blockPrefab) as GameObject;
            block[x].transform.SetParent(actionMenu.transform,false);
            setNum(x, Rand());
            setPosition(x, i, j);
        }
    }

    void setNum(int x, int t) {
        block[x].GetComponent<blockcontroler>().setText(t);
    }

    int getNum(int x) {
        return block[x].GetComponent<blockcontroler>().getNum();
    }

    void setPosition(int x, int i, int j) {
        block[x].GetComponent<blockcontroler>().setPosition(i,j);
    }

    void movetoPosition(int x, int cnt) {
        block[x].GetComponent<blockcontroler>().movetoPosition(cnt);
    }

    void click(int x) {
        block[x].GetComponent<blockcontroler>().click();
    }

    void unclick(int x) {
        block[x].GetComponent<blockcontroler>().unclick();
    }

    bool getclicked(int x) {
        return block[x].GetComponent<blockcontroler>().getClicked();
    }

    bool MouseInBlock(float x, float y) {
        if (mousePos.x < x - w || mousePos.x > x + w) return false;
        if (mousePos.y < y - w || mousePos.y > y + w) return false;
        return true;
    }

    bool checkAdject(int x,int y) {
        Vector2 xx = new Vector2(x / 7, x % 7);
        Vector2 yy = new Vector2(y / 7, y % 7);
        if (Math.Abs(xx.x - yy.x) + Math.Abs(xx.y - yy.y) == 1) return true;
        return false;
    }

    void scoring(int sum) {
        score += sum;
        txt.text = score.ToString();
    }

    int Rand() {
        int x = rand.Next(1, 5);
        return (int) Math.Pow(2, x);
    }

    bool checkEndgame() {
        for (int i = 0; i < 5; ++i)
        for (int j = 0; j < 7; ++j) {
            int b = i * 7 + j;
            for (int k = 0; k < 8; ++k) {
                int x = i + s1[k];
                int y = j + s2[k];
                int z = x * 7 + y;
                if (x < 0 || x >= 5) continue;
                if (y < 0 || y >= 7) continue;
                if (getNum(b) == getNum(z) || getNum(b) == getNum(z) * 2) return true;
            }
        }
        return true;
    }

    void Update() {
        mousePos = Input.mousePosition;
        Vector3 p1 = sss.GetComponent<Transform>().position;
        w = p1.x * 40f / 1280f;

        if (Input.GetMouseButtonDown(0)) pressed = true;
        if (Input.GetMouseButtonUp(0)) pressed = false;

        if (pressed) {
            //Debug.Log("Mouse: " + mousePos.x + " " + mousePos.y);
            //Debug.Log("Size: " + w);

            int b = -1;
            for (int i = 0; i < 35; ++i) {
                Vector3 p = block[i].GetComponent<Transform>().position;
                if (MouseInBlock(p.x, p.y)) {
                    b = i;
                    break;
                }
            }
            
            //Debug.Log("Chi vao o: " + b + " " + n);
            if (b == -1) return;

            if (n > 1 && st[n-2] == b) {        
                unclick(st[n-1]);
                n--;
                //Debug.Log("Giam " + n);
                return;
            }

            if (getclicked(b) == false)
            if (n == 0 || checkAdject(st[n-1], b))
            if (n == 0 || getNum(b) == getNum(st[n-1]) || getNum(b) == 2 * getNum(st[n-1])) 
            if (n != 1 || getNum(b) == getNum(st[0])) {
                click(b);
                st[n] = b;
                n++;
                //Debug.Log("Tang " + n);
            }

        }
        else {
            if (n>0) {
                int sum = 0;
                for (int i = 0; i < n; ++i) sum += getNum(st[i]);
                    
                if (n > 1) scoring(sum);
                int s = 2;
                while (s <= sum) s *= 2; s /= 2;
                setNum(st[n-1], s);
                unclick(st[n-1]);

                for (int i = 0; i < 5; ++i) {
                    int p = 0;
                    int cnt = 0;
                    for (int j = 0; j < 7; ++j) {
                        int x = i * 7 + j;
                        int y = i * 7 + p;

                        if (getclicked(x) == false) {
                            movetoPosition(x, cnt);
                            tmp = block[x];
                            block[x] = block[y];
                            block[y] = tmp;
                            p++;
                        }
                        else {
                            unclick(x);
                            cnt++;
                        }
                    }

                    for (int j = p; j < 7; ++j) {
                        int x = i * 7 + j;
                        setNum(x, Rand());
                        setPosition(x, i, j + cnt);
                        movetoPosition(x, cnt);
                    }
                }

                //Debug.Log("Vua xoa "+ n);
            }
            n = 0;
            //Debug.Log("Process Delete");

            
        }

    }
}

