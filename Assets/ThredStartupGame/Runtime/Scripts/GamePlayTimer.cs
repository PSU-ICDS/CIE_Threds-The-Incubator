using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePlayTimer : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] float gameDuration = 30.0f;
    [SerializeField] float tick;
    [SerializeField] bool isRunning;
    [SerializeField] bool paused;
    [SerializeField] TrackedVariables dataTracker;
    [Space(10)]
    [SerializeField] float gameMidpointVal;
    [SerializeField] bool midpointNotification;
    [Space(10)]
    //[SerializeField] float gameOneMinLeftVal = 60.0f;
    [SerializeField] bool oneMinLeftNotification;
    [Space(10)]
    [SerializeField] bool endGame_UseTime;
    [SerializeField] bool endGame_UseBudget;
    [SerializeField] bool endGame_EndNow = false;
    [Space(10)]
    [SerializeField] UnityEvent gameStartEvent;
    [SerializeField] UnityEvent gameMidpointEvent;
    [SerializeField] UnityEvent gameOneMinLeftEvent;
    [SerializeField] UnityEvent gameOverEvent;
    [SerializeField] UnityEvent gameResetEvent;
    [Space(10)]
    [SerializeField] bool debug;
    float timerElapsed;
    float timeResourceResetVal;

    public float GameDuration { get => gameDuration; set => Game_SetGameDuration(value); }
    public bool IsRunning { get => isRunning; set { isRunning = value; } }


    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        Time_ClockTick();
    }

    public void Setup()
    {
        FindDataTracker();
        Time_CalculateTickValue();
    }

    public void FindDataTracker()
    {
        if (active && dataTracker == null)
        {
            if (GameObject.FindObjectOfType<TrackedVariables>() != null)
                dataTracker = GameObject.FindObjectOfType<TrackedVariables>();
        }
    }

    public void Game_SetGameDuration(float _gameDuration)
    {
        gameDuration = _gameDuration;
        gameMidpointVal = gameDuration * 0.5f;
        oneMinLeftNotification = (_gameDuration <= 60.0f);

        //if (_gameDuration >= 60.0f)
        //    oneMinLeftNotification = false;
        //else
        //    oneMinLeftNotification = true;

        Time_CalculateTickValue();
    }

    public void Time_CalculateTickValue()
    {
        if (dataTracker == null)
            FindDataTracker();

        if (dataTracker != null)
        {
            timeResourceResetVal = dataTracker.PrimaryStat.time;
            tick = timeResourceResetVal / gameDuration;
            //tick = (float)Mathf.Floor(tick);

            if (debug)
                Debug.Log("GameTime: Tick Set: " + tick.ToString());
        }
    }

    public void Time_ClockTick()
    {
        if (active && isRunning)
        {
            if (!paused)
            {
                timerElapsed += Time.deltaTime;
                if (timerElapsed >= 1.0f)
                {
                    dataTracker.PrimaryStat.Spend_Time(tick);

                    if (debug)
                        Debug.Log("GameTimer Tick: " + tick + ", Total Duration: " + gameDuration + ", Timestamp: " + Time.time);

                    timerElapsed = 0.0f;
                    Time_CheckForGameMidpoint();
                    Time_CheckForOneMinLeft();
                    Time_CheckForGameOver();
                }
            }
        }
    }

    void Time_CheckForGameMidpoint()
    {
        if (!midpointNotification && dataTracker.PrimaryStat.time <= gameMidpointVal)
        {
            Game_Pause();
            gameMidpointEvent.Invoke();
            midpointNotification = true;

            //if (debug)
            //    Debug.Log("Game Over!! Time Resource has run out!");
        }
    }

    void Time_CheckForOneMinLeft()
    {
        if (midpointNotification && !paused)
        {
            if (!oneMinLeftNotification && dataTracker.PrimaryStat.time <= 60.0f)  //gameOneMinLeftVal
            {
                Game_Pause();
                gameOneMinLeftEvent.Invoke();
                oneMinLeftNotification = true;

                if (debug)
                    Debug.Log("One Minute Left!! Time Resource has run out!");
            }
        }
    }

    void Time_CheckForGameOver()
    {
        if (GameOver_CheckTime() || GameOver_CheckBudget())
            GameOver_SetEndGameNowState(true);

        if (endGame_EndNow)
            Game_TriggerGameOver();


        //if (dataTracker.PrimaryStat.time <= 0.0f || dataTracker.PrimaryStat.budget <= 0.0f)
        //{
        //    Game_TriggerGameOver();
        //    //gameOverEvent.Invoke();
        //    //isRunning = false;

        //    //if (debug)
        //    //    Debug.Log("Game Over!! Time Resource has run out!");
        //}
    }

    public bool GameOver_CheckTime()
    {
        if (endGame_UseTime)
        {
            if (dataTracker != null && dataTracker.PrimaryStat.time <= 0.0f)
                return true;
        }
        return false;
    }

    public bool GameOver_CheckBudget()
    {
        if (endGame_UseBudget)
        {
            if (dataTracker != null && dataTracker.PrimaryStat.budget <= 0.0f)
                return true;
        }
        return false;
    }

    public void GameOver_SetEndGameNowState(bool _state)
    {
        endGame_EndNow = _state;
    }

    public void Game_Start()
    {
        Game_SetRunningState(true);
        paused = false;
        midpointNotification = false;
        gameStartEvent.Invoke();
    }

    public void Game_Stop()
    {
        Game_SetRunningState(false);
        paused = false;

        if (debug)
            Debug.Log("Game Over!! User selected to end game.");

        Game_TriggerGameOver();
        //gameOverEvent.Invoke();
    }

    public void Game_SetRunningState(bool _running)
    {
        isRunning = _running;

    }

    public void Game_Pause()
    {
        Game_SetPausedState(true);
    }

    public void Game_Unpause()
    {
        Game_SetPausedState(false);
    }

    public void Game_SetPausedState(bool _paused)
    {
        paused = _paused;
    }

    public void Game_Reset()
    {
        dataTracker.Game_Reset();
        gameResetEvent.Invoke();
        midpointNotification = false;
        endGame_EndNow = false;
        //dataTracker.PrimaryStat.time = timeResourceResetVal;
    }

    public void Game_TriggerGameOver()
    {
        gameOverEvent.Invoke();
        isRunning = false;

        if (debug)
        {
            if (dataTracker.PrimaryStat.time <= 0.0f)
                Debug.Log("Game Over!! Time Resource has run out!");

            if (dataTracker.PrimaryStat.budget <= 0.0f)
                Debug.Log("Game Over!! Budget Resource has run out!");
        }
    }

}
