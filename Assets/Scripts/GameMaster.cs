using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameMaster : MonoBehaviour {
    public GameObject blockPrefab;
    public Canvas actionMenu;
    public Text scoreText;
    public Button buttonEnd;

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
    int matchWidthOrHeight = 0;
    int sh = 0;

    void Start() {
        buttonEnd.onClick.AddListener(() => {
            SceneManager.LoadScene(2);
        }); 

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
        block[x].GetComponent<BlockController>().setNum(t);
    }
    
    int getNum(int x) {
        return block[x].GetComponent<BlockController>().getNum();
    }

    void setPosition(int x, int i, int j) {
        block[x].GetComponent<BlockController>().setPosition(i,j);
    }

    void movePosition(int x, int cnt) {
        block[x].GetComponent<BlockController>().movePosition(cnt);
    }

    void click(int x) {
        block[x].GetComponent<BlockController>().click();
    }

    void unclick(int x) {
        block[x].GetComponent<BlockController>().unclick();
    }

    bool getclicked(int x) {
        return block[x].GetComponent<BlockController>().getClicked();
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
        scoreText.text = score.ToString();
    }

    int Rand() {
        int x = rand.Next(1, 5);
        return (int) Math.Pow(2, x);
    }

    bool checkEndgame() {
        for (int i = 0; i < 5; ++i)
        for (int j = 0; j < 7; ++j) {
            int b = i * 7 + j;
            for (int k = 1; k < 8; k += 2) {
                int x = i + s1[k];
                int y = j + s2[k];
                int z = x * 7 + y;
                if (x < 0 || x >= 5) continue;
                if (y < 0 || y >= 7) continue;
                if (getNum(b) == getNum(z)) return false;
            }
        }
        return true;
    }

    IEnumerator waiter(float ss) {
        yield return new WaitForSeconds(20f);
    }

    void match() {
        
    }

    void unmatch() {

    }
 
    void Update() {
        if (Camera.main.aspect > 16f / 9f) sh = 1;
            else sh = 0;
        if (matchWidthOrHeight != sh) {
            matchWidthOrHeight = sh;
            actionMenu.GetComponent<CanvasScaler>().matchWidthOrHeight = sh;
        }

        if (Input.GetMouseButtonDown(0)) pressed = true;
        if (Input.GetMouseButtonUp(0)) pressed = false;

        if (pressed) {
            mousePos = Input.mousePosition;
            Vector3 p1 = sss.GetComponent<Transform>().position;
            float xx = p1.x;
            float yy = p1.y;

            if (matchWidthOrHeight == 0) w = xx * 40f / 1280f;
                else w = yy * 40f / 720f;

            int b = -1;
            
            for (int i = 0; i < 35; ++i) {
                Vector3 p = block[i].GetComponent<Transform>().position;
                if (MouseInBlock(p.x, p.y)) {
                    b = i;
                    break;
                }
            }
            
            if (b == -1) return;

            if (n > 1 && st[n-2] == b) {
                unmatch();        
                unclick(st[n-1]);
                n--;
                return;
            }

            if (getclicked(b) == false)
            if (n == 0 || checkAdject(st[n-1], b))
            if (n == 0 || getNum(b) == getNum(st[n-1]) || getNum(b) == 2 * getNum(st[n-1])) 
            if (n != 1 || getNum(b) == getNum(st[0])) {
                click(b);
                st[n] = b;
                n++;
                match();
            }

        }
        else {
            if (n>0) {
                for (int i = 1; i < n; ++i) unmatch();
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
                            movePosition(x, cnt);
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
                        movePosition(x, cnt);
                    }
                }

            }
            n = 0;

            if (checkEndgame()) {
                //StartCoroutine(waiter(20f));
                SceneManager.LoadScene(2);
            }
            
        }

    }
}

