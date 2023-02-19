using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.EaseingUtility
{
    public static class Easing
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;
        const float c3 = c1 + 1;

        //https://easings.net/#easeInQuad
        public static float EaseIn(float a, float b, float t)
        {
            //Quad
            float value = t * t;
            return a + (b - a) * value;
        }

        //https://easings.net/#easeOutQuad
        public static float EaseOut(float a, float b, float t)
        {
            //Quad
            float value = 1 - (1 - t) * (1 - t);
            return a + (b - a) * value;
        }

        //https://easings.net/#easeInOutQuad
        public static float EaseInOut(float a, float b, float t)
        {
            float value = t < .5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
            return a + (b - a) * value;
        }

        //https://easings.net/#easeInBack
        public static float EaseInBack(float a, float b, float t)
        {
            float value = c3 * t * t * t - c1 * t * t;
            return a + (b - a) * value;
        }

        //https://easings.net/#easeOutBack
        public static float EaseOutBack(float a, float b, float t)
        {
            float value = 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
            return a + (b - a) * value;
        }


        //https://easings.net/#easeInOutBack
        public static float EaseInOutBack(float a, float b, float t)
        {

            float value = t < .5f
                ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
                : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;

            return a + (b - a) * value;
        }
    }
}