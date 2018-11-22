using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class swiper2 : MonoBehaviour
{
    public Transform arrow;
    public Text swipeTextRef, pointsRef;
    public Slider timer;
    public float timerInitialValue = 3.0f;
    public Treshold treshold = new Treshold(0.0f, 0.0f);

    private int points = 0;
    private string swipeDirection;
    private Vector3 beganInputPosition, finishedInputPosition;
    private PossibleCombination[] combinations = new PossibleCombination[4] { new PossibleCombination("Right", 90),
                                                                             new PossibleCombination("Left", 270),
                                                                             new PossibleCombination("Up", 180),
                                                                             new PossibleCombination("Down", 0)};
    private Swipes swipe = new Swipes(false, false, false, false, false, false);

    void Start()
    {
        Random.seed = (int)System.DateTime.Now.Ticks;
        points = 1;
        swipeTextRef.text = "Swipe Right";
        arrow.rotation = Quaternion.Euler(0, 0, 90);
        StartCoroutine(StartTimer());
        ResetSwipes();
    }

    void OnMouseDown()
    {
        beganInputPosition = Input.mousePosition;
        Debug.Log("Initial Position: " + beganInputPosition);
    }

    void OnMouseUp()
    {
        finishedInputPosition = Input.mousePosition;
        Debug.Log("Final Position: " + finishedInputPosition);
        swipeDirection = CalculateSwipe(beganInputPosition, finishedInputPosition);
        Debug.Log(swipeDirection);
        ResetSwipes();
        if (swipeTextRef.text.Contains(swipeDirection))
        {
            points++;
            pointsRef.text = points.ToString();
            swipeTextRef.text = "Swipe " + GenerateSwipe();
            arrow.rotation = GenerateRotation();
            arrow.GetComponent<Animator>().ForceStateNormalizedTime(0.0f);
            ResetTimer();
        }
        else
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    string CalculateSwipe(Vector3 inicial, Vector3 final)
    {
        string returnStringHorizontal = "";
        string returnStringVertical = "";
        if (Mathf.Abs(inicial.x - final.x) >= treshold.x * 9)
        {
            swipe.Horizontal = true;
            if (final.x > inicial.x)
            {
                //Debug.Log("swipedLeft");
                returnStringHorizontal = "Right";
                swipe.Right = true;
            }
            else
            {
                //Debug.Log("SwipedRight");
                returnStringHorizontal = "Left";
                swipe.Left = true;
            }
        }
        if (Mathf.Abs(inicial.y - final.y) >= treshold.y * 16)
        {
            swipe.Vertical = true;
            if (final.y > inicial.y)
            {
                //Debug.Log("swipedDown");
                returnStringVertical = "Up";
                swipe.Up = true;
            }
            else
            {
                //Debug.Log("swipedUp");
                returnStringVertical = "Down";
                swipe.Down = true;
            }
        }
        if (Mathf.Abs(inicial.x - final.x) > Mathf.Abs(inicial.y - final.y))
        {
            return returnStringHorizontal;
        }
        else
        {
            return returnStringVertical;
        }
    }

    void ResetSwipes()
    {
        swipe.Left = false;
        swipe.Right = false;
        swipe.Up = false;
        swipe.Down = false;
        swipe.Horizontal = false;
        swipe.Vertical = false;
    }

    string GenerateSwipe()
    {
        float swipesLog2Clamped = Mathf.Clamp(Mathf.Log(points, 2) + 1, 0, combinations.Length);
        return combinations[Random.Range(0, (int)Mathf.Floor(swipesLog2Clamped))].swipe;
    }

    Quaternion GenerateRotation()
    {
        float rotationLog2Clamped = Mathf.Clamp(Mathf.Log(points, 2) + 1, 0, combinations.Length);
        Debug.Log("Points: " + points + "\nLog2Clamped: " + rotationLog2Clamped);
        return Quaternion.Euler(0, 0, combinations[Random.Range(0, (int)Mathf.Floor(rotationLog2Clamped))].rotation);
    }

    public IEnumerator StartTimer()
    {
        while (true)
        {
            timer.value -= Time.deltaTime;
            //if (timer.value <= 0)
            // Application.LoadLevel(Application.loadedLevel);
            yield return null;
        }
    }

    public void ResetTimer()
    {
        timer.value = timerInitialValue;
    }


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

    [SerializeField]
    public class PossibleCombination
    {
        public string swipe;
        public float rotation;

        public PossibleCombination(string swipeValue, float rotationValue)
        {
            swipe = swipeValue;
            rotation = rotationValue;
        }
    }

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
}
