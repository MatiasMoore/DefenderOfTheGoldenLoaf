using UnityEngine;
using UnityEngine.UI;

namespace DefenderOfTheGoldenLoaf.UI
{
    public class Scroller : MonoBehaviour
    {
        [SerializeField]
        private RawImage _image;

        [SerializeField]
        private float _time;

        [SerializeField]
        private float _x;

        [SerializeField]
        private float _y;

        private void Update()
        {
            _image.uvRect = new Rect(_image.uvRect.position + new Vector2(_x, _y) * Time.deltaTime * _time, _image.uvRect.size);
        }
    }
}
