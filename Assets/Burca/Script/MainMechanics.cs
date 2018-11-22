using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//TODO: etapas -> deixar o arrow anterior por 1 movimento, colocar um tap ao inves de swipe, colocar mais direções como um *, arrow e texto começam a se mover
//                pulsar continuamente.
public class MainMechanics : MonoBehaviour
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
    public SoundManager soundManager;
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
    private PossibleCombination[] combinations = new PossibleCombination[4] { new PossibleCombination("R", 0),
                                                                             new PossibleCombination("L", 180),
                                                                             new PossibleCombination("U", 90),
                                                                             new PossibleCombination("D", 270)};
    #endregion

    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        points = 1;
        arrowAnim = arrow.GetComponent<Animator>();
        timer.maxValue = timerInitialValue;
        //Arrumar
        soundManager = GameObject.Find("music").GetComponent<SoundManager>();
        //
        gameStarted = false;
        swipeTextRef.text = "R";
        arrow.rotation = Quaternion.Euler(0, 0, 0);
        //ResetSwipes();
    }
    //Todo o codigo do swipe abaixo foi totalmente trocado por apenas essa função e o asset LeanTouch

    public void Swipe(string direction)
    {
        if (!gameStarted)
        {
            StartCoroutine(StartTimer());
            gameStarted = true;
        }

        if (swipeTextRef.text.Contains(direction))
            SwipedRight();
        else
            SwipedWrong();
    }

    private void SwipedRight()
    {
        points++;
        pointsRef.text = "{" + points.ToString() + "}";
        swipeTextRef.text = GenerateSwipeString().ToString();
        arrow.rotation = GenerateArrowRotation();
        arrowAnim.Play("appear", 0, 0.0f);
        AddTime(defaultAddTime);
        ///ARRUMAR
        soundManager.PlayRight();
        ///
    }

    private void SwipedWrong()
    {
        ///ARRUMAR
        soundManager.PlayWrong();
        ///
        StartCoroutine(Restart(0.0f));
    }

    private string GenerateSwipeString()
    {
        float swipesLog2Clamped = Mathf.Clamp(Mathf.Log(points, 2) + 1, 0, combinations.Length);
        return combinations[Random.Range(0, (int)Mathf.Floor(swipesLog2Clamped))].swipe;
    }

    private Quaternion GenerateArrowRotation()
    {
        float rotationLog2Clamped = Mathf.Clamp(Mathf.Log(points, 2) + 1, 0, combinations.Length);
        Debug.Log("Points: " + points + "\nLog2Clamped: " + rotationLog2Clamped);
        return Quaternion.Euler(0, 0, combinations[Random.Range(0, (int)Mathf.Floor(rotationLog2Clamped))].rotation);
    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            timer.value -= Time.deltaTime;
            if (timer.value <= 0)
            {
                //Arrumar
                StartCoroutine(Restart(0.0f));
                soundManager.PlayWrong();
                //
            }
            yield return null;
        }
    }

    public IEnumerator Restart(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }

    public void AddTime(float value)
    {
        //timer.value = timerInitialValue;
        timer.value += value;
        timer.value = Mathf.Clamp(timer.value, 0.0f, timerInitialValue);
    }

    /*private void OnMouseDown()
    {
        beganInputPosition = Input.mousePosition;
        Debug.Log("Initial Position: " + beganInputPosition);
    }

    private void OnMouseUp()
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
            SwipedRight();
        else
            SwapedWrong();
    }*/

    /*private string CalculateSwipe(Vector3 inicial, Vector3 final)
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

    private void ResetSwipes()
    {
        swipe.Left = false;
        swipe.Right = false;
        swipe.Up = false;
        swipe.Down = false;
        swipe.Horizontal = false;
        swipe.Vertical = false;
    }*/
}
