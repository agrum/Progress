using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
    public class ProgressSlider : MonoBehaviour
    {
        public Slider mainSlider = null;
        public Slider progressSlider = null;

        public float Main
        {
            get
            {
                return mainSlider.value;
            }
            set
            {
                mainSlider.value = value;
                progressSlider.value = System.Math.Max(mainSlider.value, value);
            }
        }

        public float Progress
        {
            get
            {
                return progressSlider.value;
            }
            set
            {
                progressSlider.value = System.Math.Max(mainSlider.value, value);
            }
        }
    }
}
