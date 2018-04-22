using UnityEngine;
using UnityEngine.UI;

namespace FifteenPuzzle
{
    public class Tile : MonoBehaviour
    {
        public Text Text;

        public Canvas canvas;

        private int number;

        public float GetSize()
        {
            var rectTransform = canvas.GetComponent<RectTransform>();
            return rectTransform.rect.width * rectTransform.localScale.x;
        }

        public void Init(int newNumber)
        {
            Number = newNumber;
        }

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                Text.text = number.ToString();
            }
        }
    
        public void SetSize(float newSize)
        {
            var scaleMult = newSize / GetSize();
            transform.localScale = Vector3.one * scaleMult;
        }
    }
}