﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//TODO: etapas -> deixar o arrow anterior por 1 movimento, colocar um tap ao inves de swipe, colocar mais direções como um *, arrow e texto começam a se mover
//                pulsar continuamente.
public class MainMechanics : MonoBehaviour
{
    #region Class PossibleCombination definition
    [SerializeField]
    public class PossibleCombination
    {
        public int type;
        public string swipe;
        public float rotation;

        public PossibleCombination(int swipeType, string swipe, float rotation)
        {
            this.type = swipeType;
            this.swipe = swipe;
            this.rotation = rotation;
        }
    }
    #endregion
    
    #region Public Variables

    public Text swipeTextRef, pointsRef, highscoreRef;
    public Transform arrow;
    public Timer timer;

    #endregion

    #region Private Variables
    private int points = 0, highscore = 0;
    private bool gameStarted = false;
    private Animator arrowAnim;
    private PossibleCombination combination;
    private PossibleCombination[] combinations = new PossibleCombination[4] { new PossibleCombination(2, "R", 0),
                                                                             new PossibleCombination(4, "L", 180),
                                                                             new PossibleCombination(1, "U", 90),
                                                                             new PossibleCombination(3, "D", 270)};
                                                                            //new PossibleCombination(5, "LD", 270),
                                                                            //new PossibleCombination(7, "LU", 270),
                                                                            //new PossibleCombination(4, "RU", 270),
                                                                            //new PossibleCombination(6, "RD", 270)};

    public SoundManager soundManager;
    #endregion

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        Setup();
    }

    private void Setup()
    {
        gameStarted = false;

        points = 1;

        highscore = PlayerPrefs.GetInt("highscore");
        highscoreRef.text = "{" + highscore.ToString() + "}";

        arrowAnim = arrow.GetComponent<Animator>();

        soundManager = FindObjectOfType<SoundManager>();

        //timer.ResetTimer();

        swipeTextRef.text = "R";

        arrow.rotation = Quaternion.Euler(0, 0, 0);

        combination = combinations[0];
    }

    public void Swipe(int direction)
    {
        if (!gameStarted)
        {
            StartCoroutine(timer.StartTimer());
            gameStarted = true;
        }

        if (combination.type == direction)
            SwipedRight();
        else
            SwipedWrong();
    }

    private void SwipedRight()
    {
        points++;
        CheckHighscore();
        pointsRef.text = "{" + points.ToString() + "}";
        highscoreRef.text = "{" + highscore.ToString() + "}";
        combination = GenerateCombination();
        swipeTextRef.text = combination.swipe;
        arrow.rotation = Quaternion.Euler(new Vector3(0,0,combination.rotation));
        arrowAnim.Play("appear", 0, 0.0f);
        timer.AddTime();
        soundManager.PlayRight();
    }

    private void SwipedWrong()
    {
        GameOver();
    }
    
    private PossibleCombination GenerateCombination()
    {
        float swipesLog2Clamped = Mathf.Clamp(Mathf.FloorToInt(Mathf.Log(points, 2)) + 1, 0, combinations.Length);
        //Debug.Log("Log2: " + swipesLog2Clamped.ToString());

        int index1, index2;
        index1 = Random.Range(0, (int)swipesLog2Clamped);
        index2 = Random.Range(0, (int)swipesLog2Clamped);


        PossibleCombination newCombination = new PossibleCombination(combinations[index1].type, combinations[index1].swipe, combinations[index2].rotation);
        //print("Type: " + newCombination.type.ToString() + " Swipe: " + newCombination.swipe + " Rot: " + newCombination.rotation);
        return newCombination;
    }
    
    public IEnumerator Restart(float delay)
    {
        PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void CheckHighscore()
    {
        if(points > highscore)
        {
            highscore = points;
            highscoreRef.text = "{" + highscore.ToString() + "}";
        }
    }

    public void GameOver()
    {
        soundManager.PlayWrong();
        Restart();
    }
}
