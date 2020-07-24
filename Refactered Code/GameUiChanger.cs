using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Presentation
{
    public class GameUiChanger : MonoBehaviour
    {
        public static void SetPosition(RectTransform rectTransform, Vector2 x)
        {
            rectTransform.anchoredPosition = x;
        }

        public static void SetPosition(GameObject gameObject, Vector2 x)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = x;
        }

        public static void SetText(Text text, string t)
        {
            text.text = t;
        }

        public static void SetText(GameObject gameObject, string t)
        {
            gameObject.GetComponent<Text>().text = t;
        }

        public static void SetColor(Image image, Color color)
        {
            image.color = color;
        }

        public static void ChangePosition(RectTransform rectTransform, Vector2 x, float time)
        {
            rectTransform.DOAnchorPos(x, time);
        }
        
        public static void ChangePosition(GameObject gameObject, Vector2 x, float time)
        {
            gameObject.GetComponent<RectTransform>().DOAnchorPos(x, time);
        }

        public static void ShakePosition(GameObject gameObject, float time)
        {
            gameObject.GetComponent<Transform>().DOShakePosition(time, new Vector3(6f, 3f, 3f), 5, 20);
        }

        public void ChangeNumber(Text text, int oldNum, int newNum)
        {
            StartCoroutine(CoChangeNumber(text, oldNum, newNum));
        }

        public static IEnumerator CoChangeNumber(Text text, int oldNum, int newNum) {
            if (newNum - oldNum < 10) {
                for (var i = oldNum + 1; i <= newNum; ++i) { 
                    text.text = i.ToString();
                    yield return new WaitForSeconds(0.07f);
                }
            }
            else {
                var xxx = (float) (newNum - oldNum) / 10;
                float yyy = oldNum;
                for (var i = 0; i < 10; ++i) {
                    yyy += xxx;
                    var zzz = (int) yyy;
                    text.text = zzz.ToString();
                    yield return new WaitForSeconds(0.07f);
                }
                text.text = newNum.ToString();
            }
        }
    }
}
