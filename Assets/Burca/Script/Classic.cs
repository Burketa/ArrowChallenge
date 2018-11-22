using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Classic : MonoBehaviour
{
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

    #region Class PossibleCombination definition
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

    #region Public Variables
    public float timerInitialValue = 2.0f;
    public float defaultAddTime = 0.80f;
    //////////////////////
    public PlayAudios musicNSound;
    ////////////////////////
    public Slider timer;
    public Text swipeTextRef, pointsRef;
    public Transform arrow;
    public Treshold treshold = new Treshold(0.0f, 0.0f);
    #endregion

    #region Private Variables
    private int points = 0;
    private bool gameStarted = false;
    private bool tap = false;
    private string swipeDirection;
    private Animator arrowAnim;
    private Vector3 beganInputPosition, finishedInputPosition;
    private Swipes swipe = new Swipes(false, false, false, false, false, false);
    private PossibleCombination[] combinations = new PossibleCombination[4] { new PossibleCombination("Right", 0),
                                                                             new PossibleCombination("Left", 180),
                                                                             new PossibleCombination("Up", 90),
                                                                             new PossibleCombination("Down", 270)};
    #endregion

    void Awake()
    {
        Random.seed = (int)System.DateTime.Now.Ticks;
    }

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        points = 1;
        arrowAnim = arrow.GetComponent<Animator>();
        timer.maxValue = timerInitialValue;
        //Arrumar
        musicNSound = GameObject.Find("music").GetComponent<PlayAudios>();
        //
        gameStarted = false;
        swipeTextRef.text = "Swipe Right";
        arrow.rotation = Quaternion.Euler(0, 0, 0);
        ResetSwipes();
    }

    void OnMouseDown()
    {
        beganInputPosition = Input.mousePosition;
        Debug.Log("Initial Position: " + beganInputPosition);
    }

    void OnMouseUp()
    {
        if(!gameStarted)
        {
            StartCoroutine(StartTimer());
            gameStarted = true;
        }
        finishedInputPosition = Input.mousePosition;
        Debug.Log("Final Position: " + finishedInputPosition);
        swipeDirection = CalculateSwipe(beganInputPosition, finishedInputPosition);
        Debug.Log(swipeDirection);
        ResetSwipes();
        if (swipeTextRef.text.Contains(swipeDirection))
        {
            SwipedRight();
        }
        else
        {
            SwapedWrong();
        }
    }

    void SwipedRight()
    {
        points++;
        pointsRef.text = "{" + points.ToString() + "}";
        swipeTextRef.text = "Swipe " + GenerateSwipe();
        arrow.rotation = GenerateRotation();
        arrowAnim.Play("appear", 0, 0.0f);
        AddTime(defaultAddTime);
        ///ARRUMAR
        musicNSound.PlayRight();
        ///
    }
    void SwapedWrong()
    {
        ///ARRUMAR
        musicNSound.PlayWrong();
        ///
        StartCoroutine(Restart(0.0f));
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
            if (timer.value <= 0)
            {
                //Arrumar
                StartCoroutine(Restart(0.0f));
                musicNSound.PlayWrong();
                //
            }
            yield return null;
        }
    }

    public IEnumerator Restart(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(Application.loadedLevel);
        yield return null;
    }

    public void AddTime(float value)
    {
        //timer.value = timerInitialValue;
        timer.value += value;
        timer.value = Mathf.Clamp(timer.value, 0.0f, timerInitialValue);
    }
}
