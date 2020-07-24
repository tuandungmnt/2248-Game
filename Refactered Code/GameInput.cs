using UnityEngine;

namespace Presentation
{
    public class GameInput : MonoBehaviour
    {
        public GameObject topRightGameObject;
        public static bool isPressed;
        private static Vector2 _mousePosition;
        private static float _realSize;

        public static bool IsMouseOnBlock(float x, float y)
        {
            if (_mousePosition.x < x - _realSize || _mousePosition.x > x + _realSize) return false;
            if (_mousePosition.y < y - _realSize || _mousePosition.y > y + _realSize) return false;
            return true;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) isPressed = true;
            if (Input.GetMouseButtonUp(0)) isPressed = false;
            //Debug.Log("GameInput: " + isPressed);
            
            _mousePosition = Input.mousePosition;
            var p1 = topRightGameObject.GetComponent<Transform>().position;
            _realSize = p1.x * 50f / 720f;
        }
    }
}
