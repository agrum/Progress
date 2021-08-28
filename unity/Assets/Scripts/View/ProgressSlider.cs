using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
    public class ProgressSlider : MonoBehaviour
    {
        public Slider mainSlider = null;
        public Slider progressSlider = null;

        public double Main
        {
            get
            {
                return mainSlider.value;
            }
            set
            {
                mainSlider.value = (float) value;
                progressSlider.value = (float) System.Math.Max(mainSlider.value, value);
            }
        }

        public double Progress
        {
            get
            {
                return progressSlider.value;
            }
            set
            {
                progressSlider.value = (float) System.Math.Max(mainSlider.value, value);
            }
        }
    }
}
