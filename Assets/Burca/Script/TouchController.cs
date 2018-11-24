using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    #region Class Treshold definition
    [SerializeField]
    public class Treshold
    {
        public float x;
        public float y;

        public Treshold() { }
        public Treshold(float xValue, float yValue)
        {
            x = xValue;
            y = yValue;
        }
    }
    #endregion

    #region Class Swipes definition
    [SerializeField]
    public class Swipes
    {
        private bool right;
        private bool left;
        private bool up;
        private bool down;
        private bool horizontal;
        private bool vertical;
        public bool Right
        {
            get { return right; }
            set { right = value; }
        }
        public bool Left
        {
            get { return left; }
            set { left = value; }
        }
        public bool Up
        {
            get { return up; }
            set { up = value; }
        }
        public bool Down
        {
            get { return down; }
            set { down = value; }
        }
        public bool Horizontal
        {
            get { return horizontal; }
            set { horizontal = value; }
        }
        public bool Vertical
        {
            get { return vertical; }
            set { vertical = value; }
        }

        public Swipes(bool rightValue, bool leftValue, bool upValue, bool downValue, bool horizontalValue, bool verticalValue)
        {
            right = rightValue;
            left = leftValue;
            up = upValue;
            down = downValue;
            horizontal = horizontalValue;
            vertical = verticalValue;
        }
    }
    #endregion
}
