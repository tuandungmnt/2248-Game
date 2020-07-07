using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using DG.Tweening;

public class GameMaster : MonoBehaviour {
    public GameObject blockPrefab;
    public GameObject linePrefab;
    public Canvas actionMenu;
    public Text scoreText;
    public Text bestScoreText;
    public Button buttonEnd;
    public Toggle toggle;

    public GameObject[] block;
    public GameObject[] line;
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
    int toggleIsOn = 0;
    int[] md = new int[40];

    void Start() {
        buttonEnd.onClick.AddListener(() => {
            FindObjectOfType<AudioManager>().Play("Click");
            SceneManager.LoadScene(2);
        }); 

        toggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            FindObjectOfType<AudioManager>().Play("Click");
            toggleIsOn = 1 - toggleIsOn;
        });

        score = 0;
        block = new GameObject[35];    
        line = new GameObject[70];    

        for (int i = 0; i < 5; ++i)
        for (int j = 0; j < 7; ++j) {
            int x = i * 7 + j;
            block[x] = Instantiate(blockPrefab) as GameObject;
            block[x].transform.SetParent(actionMenu.transform,false);
            SetNum(x, Rand());
            SetPosition(x, i, j + 4);
            MoveDown(x, 4);
        }

        StartCoroutine( Up() ); 
        bestScoreText.text = "Best Score: " + MenuController.bestScore.ToString();
    }

    void SetNum(int x, int t) {
        block[x].GetComponent<BlockController>().SetNum(t);
    }
    
    int GetNum(int x) {
        return block[x].GetComponent<BlockController>().GetNum();
    }

    void SetPosition(int x, int i, int j) {
        block[x].GetComponent<BlockController>().SetPosition(i,j);
    }

    void MovePosition(int x, Vector3 pos, float time) {
        Vector2 v = new Vector2(pos.x, pos.y);
        block[x].GetComponent<BlockController>().MovePosition(v, time);
    }

    void MoveDown(int x, int cnt) {
        Vector3 pos = block[x].GetComponent<RectTransform>().anchoredPosition;
        Vector2 v = new Vector2(pos.x, pos.y - cnt * 100);
        block[x].GetComponent<BlockController>().MovePosition(v, 0.4f);
    }

    void Click(int x) {
        block[x].GetComponent<BlockController>().Click();
    }

    void Unclick(int x) {
        block[x].GetComponent<BlockController>().Unclick();
    }

    bool Getclicked(int x) {
        return block[x].GetComponent<BlockController>().GetClicked();
    }

    bool MouseInBlock(float x, float y) {
        if (mousePos.x < x - w || mousePos.x > x + w) return false;
        if (mousePos.y < y - w || mousePos.y > y + w) return false;
        return true;
    }

    bool CheckAdject(int x,int y) {
        Vector2 xx = new Vector2(x / 7, x % 7);
        Vector2 yy = new Vector2(y / 7, y % 7);
        if (Math.Abs(xx.x - yy.x) + Math.Abs(xx.y - yy.y) == 1) return true;
        return false;
    }

    void Scoring(int sum) {
        score += sum;
        scoreText.text = score.ToString();
    }

    int Rand() {
        int x;
        if (toggleIsOn == 0) x = rand.Next(1, 5);
            else x = rand.Next(4, 11);
        return (int) Math.Pow(2, x);
    }

    bool CheckEndgame() {
        for (int i = 0; i < 5; ++i)
        for (int j = 0; j < 7; ++j) {
            int b = i * 7 + j;
            for (int k = 1; k < 8; k += 2) {
                int x = i + s1[k];
                int y = j + s2[k];
                int z = x * 7 + y;
                if (x < 0 || x >= 5) continue;
                if (y < 0 || y >= 7) continue;
                if (GetNum(b) == GetNum(z)) return false;
            }
        }
        return true;
    }

    IEnumerator waiter(float ss) {
        yield return new WaitForSeconds(20f);
    }

    void Match(int n) {
        if (n <= 1) return;
        line[n-1] = Instantiate(linePrefab) as GameObject;
        line[n-1].transform.SetParent(actionMenu.transform,false);

        Vector3 p1 = block[st[n-1]].GetComponent<RectTransform>().anchoredPosition;
        Vector3 p2 = block[st[n-2]].GetComponent<RectTransform>().anchoredPosition;
        Vector2 p3 = new Vector2();
        Vector3 p4 = new Vector3();
        p3.x = (p1.x + p2.x) / 2;
        p3.y = (p1.y + p2.y) / 2;

        p4.x = (p3.x + p2.x) / 2;
        p4.y = (p3.y + p2.y) / 2;
        Debug.Log(p3.x + " " + p3.y + " / " + p4.x + " " + p4.y);

        line[n-1].GetComponent<RectTransform>().anchoredPosition = p4;
        line[n-1].GetComponent<RectTransform>().DOAnchorPos(p3, 0.1f);
    }

    void Unmatch(int n) {
        if (n <= 1) return;
        Vector3 p1 = block[st[n-1]].GetComponent<RectTransform>().anchoredPosition;
        Vector3 p2 = block[st[n-2]].GetComponent<RectTransform>().anchoredPosition;
        Vector2 p4 = new Vector2();
        
        p4.x = (p1.x + 3 * p2.x) / 4;
        p4.y = (p1.y + 3 * p2.y) / 4;
        Debug.Log(p4.x + " " + p4.y);
        line[n-1].GetComponent<RectTransform>().DOAnchorPos(p4, 0.2f);
        Destroy(line[n-1], 0.1f);
    }
 
    IEnumerator Up() {
    while(true) {
        yield return new WaitForSeconds(0.01f);
        //if (pressed) Debug.Log(Time.time + ".........................");
          //  else Debug.Log(Time.time + "**");

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
            
            if (b == -1) continue;

            if (n > 1 && st[n-2] == b) {
                Unmatch(n);        
                

                Unclick(st[n-1]);
                FindObjectOfType<AudioManager>().Play("Turn");
                n--;
                continue;
            }

            if (Getclicked(b) == false)
            if (n == 0 || CheckAdject(st[n-1], b))
            if (n == 0 || GetNum(b) == GetNum(st[n-1]) || GetNum(b) == 2 * GetNum(st[n-1])) 
            if (n != 1 || GetNum(b) == GetNum(st[0])) {
                Click(b);
                FindObjectOfType<AudioManager>().Play("Turn");
                st[n] = b;
                n++;
                Match(n);
            }

        }
        else {
            if (n == 1) {
                Unclick(st[0]);
            }

            if (n>1) {
                FindObjectOfType<AudioManager>().Play("Delete");
                //for (int i = 1; i < n; ++i) Destroy(line[i]);
                //for (int i = 0; i < n-1; ++i) MovePosition(st[i], block[st[n-1]].GetComponent<RectTransform>().anchoredPosition);
                //yield return new WaitForSeconds(0.45f);
                //cnt1++; Debug.Log(cnt1);
                for (int i = 1; i < n; ++i) {
                    Destroy(line[i]);
                    for (int j = 0; j < i; ++j)
                        MovePosition(st[j], block[st[i]].GetComponent<RectTransform>().anchoredPosition,0.08f);
                    yield return new WaitForSeconds(0.08f);
                }

                int sum = 0;
                int k = 0;
                for (int i = 0; i < n; ++i) sum += GetNum(st[i]);
                
                Scoring(sum);
                int s = 2;
                while (s <= sum) s *= 2; 
                s /= 2;

                Unclick(st[n-1]);

                for (int i = 0; i < 5; ++i) {
                    int p = 0;
                    int cnt = 0;
                    for (int j = 0; j < 7; ++j) {
                        int x = i * 7 + j;
                        int y = i * 7 + p;

                        if (Getclicked(x) == false) {
                            tmp = block[x];
                            block[x] = block[y];
                            block[y] = tmp;
                            md[y] = cnt;
                            p++;
                            if (x == st[n-1]) k = y;
                        }
                        else {
                            Unclick(x);
                            cnt++;
                        }
                    }

                    for (int j = p; j < 7; ++j) {
                        int x = i * 7 + j;
                        SetNum(x, Rand());
                        SetPosition(x, i, j + cnt);
                        md[x] = cnt;
                    }
                }

                Debug.Log(k);
                
                //yield return new WaitForSeconds(1f);
                block[k].GetComponent<BlockController>().ChangeNum(s);
                for (int i = 0; i < 35; ++i) MoveDown(i, md[i]);

                //yield return new WaitForSeconds(0.2f);
                //cnt1++; Debug.Log(cnt1);

            }
            n = 0;

            if (CheckEndgame()) {
                yield return new WaitForSeconds(2f);
                for (int i = 0; i < 35; ++i) 
                    block[i].GetComponent<Transform>().DOShakePosition(2f, new Vector3(6f, 3f, 3f), 5, 20f, false, true);
                yield return new WaitForSeconds(1.8f);
                SceneManager.LoadScene(2);
            }
            
        }
    }
    Debug.Log("ENDED");
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
    }
}

