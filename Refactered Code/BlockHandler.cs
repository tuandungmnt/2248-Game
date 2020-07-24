using Data;
using Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Domain
{
    public class BlockHandler : MonoBehaviour
    {
        private static readonly Color ClickedColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        private static readonly Color NotClickedColor = new Color(0.9f, 0.9f, 0.9f, 0.8f);
        private static readonly GameObject[] Line = new GameObject[70];
        private static GameObject _parent;

        public static void Initialize(GameObject parent)
        {
            _parent = parent;
        }
 
        private static Vector2 TranslateTablePositionToVector2(int x, int y)
        {
            return new Vector2(-250 + x * 125, 85 + y * 125);
        }

        public static BlockData CreateBlock()
        {
            var block = _parent.AddComponent<BlockData>();
            block.block = FindObjectOfType<GameUiCreator>().CreateBlock(_parent);
            block.text = block.block.transform.Find("Text").gameObject.GetComponent<Text>();
            block.rectTransform = block.block.GetComponent<RectTransform>();
            block.image = block.block.GetComponent<Image>();
            
            GameUiChanger.SetColor(block.image, NotClickedColor);
            return block;
        }

        public static void SetNumber(BlockData block, int number)
        {
            block.number = number;
            GameUiChanger.SetText(block.text, number.ToString());
        }

        public static void SetPosition(BlockData block, int x, int y)
        {
            var z = TranslateTablePositionToVector2(x, y);
            GameUiChanger.SetPosition(block.rectTransform, z);
        }

        public static void Click(BlockData block)
        {
            if (block.isClicked) return;
            block.isClicked = true;
            GameUiChanger.SetColor(block.image, ClickedColor);
        }

        public static void UndoClick(BlockData block)
        {
            if (!block.isClicked) return;
            block.isClicked = false;
            GameUiChanger.SetColor(block.image, NotClickedColor);
        }

        public static void Match(int n, int block1, int block2, float time)
        {
            Line[n] = FindObjectOfType<GameUiCreator>().CreateLine(_parent);
            
            var p1 = TranslateTablePositionToVector2(block1 / 7, block1 % 7);
            var p2 = TranslateTablePositionToVector2(block2 / 7, block2 % 7);
            var p3 = new Vector2 {x = (p1.x + p2.x) / 2, y = (p1.y + p2.y) / 2};
            var p4 = new Vector2 {x = (p3.x + p1.x) / 2, y = (p3.y + p1.y) / 2};

            GameUiChanger.SetPosition(Line[n], p4);
            GameUiChanger.ChangePosition(Line[n], p3, time);
        }
        
        public static void UndoMatch(int n, int block1, int block2, float time)
        {
            var p1 = TranslateTablePositionToVector2(block1 / 7, block1 % 7);
            var p2 = TranslateTablePositionToVector2(block2 / 7, block2 % 7);
            var p3 = new Vector2 {x = (3 * p1.x + p2.x) / 4, y = (3 * p1.y + p2.y) / 4};

            GameUiChanger.ChangePosition(Line[n], p3, time);
            Destroy(Line[n], 0.1f);
        }

        public static void DestroyLine(int n)
        {
            Destroy(Line[n]);
        }
        public static void Move(BlockData block, int x, int y, float time)
        {
            var z = TranslateTablePositionToVector2(x, y);
            GameUiChanger.ChangePosition(block.rectTransform, z, time);
        }

        public static void ChangeNumber(BlockData block, int newNum)
        {
            FindObjectOfType<GameUiChanger>().ChangeNumber(block.text, block.number, newNum);
            block.number = newNum;
        }

        public static void ShakePosition(BlockData block, float time)
        {
            GameUiChanger.ShakePosition(block.block, time);
        }
    }
}
