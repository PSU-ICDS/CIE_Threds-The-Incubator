using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Code created for Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_Utility
{
    public class DelayEvent : MonoBehaviour
    {
        public enum DelayTypes { NONE, FRAMES, SECONDS }

        [SerializeField] bool active;
        [Space(15)]
        [SerializeField] bool running;
        [SerializeField] bool singleRun;
        [SerializeField] bool continuousRun;
        [SerializeField] bool hasRun = false;
        [Space(15)]
        [SerializeField] DelayTypes delayType;
        [Space(15)]
        [Header("Delay Using Frames")]
        [SerializeField] int delayFrames;
        [SerializeField] int delayCount = 0;
        [Space(15)]
        [Header("Delay Using Seconds")]
        [SerializeField] float delayDuration;
        //[SerializeField] float delayStartTime = 0.0f;
        [SerializeField] float delayCount_Time = 0.0f;
        [Space(15)]
        [SerializeField] UnityEvent delayedEvent;

        //// Start is called before the first frame update
        //void Start()
        //{

        //}

        // Update is called once per frame
        void Update()
        {
            if (active && running)
                CheckDelayCount();
        }

        public void Event_RunDelayEvent()
        {
            if (active && !running)
            {
                if (singleRun)
                {
                    if (!hasRun)
                    {
                        StartDelayRunning();
                        //running = true;
                        //delayCount = 0;
                    }
                }
                else
                {
                    StartDelayRunning();
                    //running = true;
                    //delayCount = 0;
                }
            }
        }

        void StartDelayRunning()
        {
            running = true;
            delayCount = 0;
            delayCount_Time = 0.0f;
        }

        public void Event_CancelDelayEvent()
        {
            if(active && running)
            {
                running = false;
            }
        }

        void CheckDelayCount()
        {
            switch (delayType)
            {
                case DelayTypes.NONE:
                    {
                        delayCount++;

                        if (delayCount >= delayFrames)
                        {
                            delayedEvent.Invoke();

                            delayCount = 0;
                            hasRun = true;
                            running = false;
                        }
                    }
                    break;
                case DelayTypes.FRAMES:
                    {
                        delayCount++;

                        if (delayCount >= delayFrames)
                        {
                            delayedEvent.Invoke();

                            delayCount = 0;
                            hasRun = true;
                            running = false;
                        }
                    }
                    break;
                case DelayTypes.SECONDS:
                    {
                        delayCount_Time += Time.deltaTime;

                        if (delayCount_Time >= delayDuration)
                        {
                            delayedEvent.Invoke();

                            delayCount_Time = 0.0f;
                            hasRun = true;
                            running = false;
                        }
                    }
                    break;
                default:
                    break;
            }

            if (continuousRun)
                Event_RunDelayEvent();

        }
    }
}