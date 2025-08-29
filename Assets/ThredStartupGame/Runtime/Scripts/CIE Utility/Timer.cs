/*
 * Name: Timer
 * Project: Architecture Education Template Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 04/21/2023
 * Description: This script runs a countdown timer and invokes a timer event when finished
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Code created for XR Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_XR_Utility
{

    /// <summary>
    /// Runs a countdown timer and invokes a timer event when finished
    /// </summary>
    public class Timer : MonoBehaviour
    {
        [Header("Timer States")]
        /// <summary>
        /// Determines if the timer should run (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if the timer should run (TRUE) or not (FALSE)")]
        [SerializeField] bool runTimer = false;

        /// <summary>
        /// Determines if the timer runs a single time and stops (TRUE) or continuously runs (FALSE)
        /// </summary>
        [Tooltip("Determines if the timer runs a single time and stops (TRUE) or continuously runs (FALSE)")]
        [SerializeField] bool singleRun;

        /// <summary>
        /// Determines if the timer has finished running (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if the timer has finished running (TRUE) or not (FALSE)")]
        [SerializeField] bool finished = false;

        [Header("Timer Settings")]
        /// <summary>
        /// The duration of the timer
        /// </summary>
        [Tooltip("The duration of the timer")]
        [SerializeField] float duration;
        /// <summary>
        /// Determines if this timer uses a randomized duration each time (TRUE) or a preset fixed duration (FALSE) 
        /// </summary>
        [Tooltip("Determines if this timer uses a randomized duration each time (TRUE) or a preset fixed duration (FALSE) ")]
        [SerializeField] bool randomDuration;
        /// <summary>
        /// Minimum timer duration used when randomized duration (randomDuration) is active
        /// </summary>
        [Tooltip("Minimum timer duration used when randomized duration (randomDuration) is active")]
        [SerializeField] float durationMin;
        /// <summary>
        /// Maximum timer duration used when randomized duration (randomDuration) is active
        /// </summary>
        [Tooltip("Maximum timer duration used when randomized duration (randomDuration) is active")]
        [SerializeField] float durationMax;

        /// <summary>
        /// Determines if the timer duration is measured in frames (TRUE) or in time (FALSE)
        /// </summary>
        [Tooltip("Determines if this delay is measured in frames (TRUE) or in time (FALSE)")]
        [SerializeField] bool framesBased;

        [Header("Events")]
        /// <summary>
        /// Event that gets invoked when the timer is finished running
        /// </summary>
        [Tooltip("Event that gets invoked when the timer is finished running")]
        [SerializeField] public UnityEvent timerEvent;

        [Header("Settings")]
        /// <summary>
        /// Determines if this script prints out debug log statements (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if this script prints out debug log statements (TRUE) or not (FALSE)")]
        [SerializeField] bool debug;

        /// <summary>
        /// The start time for the timer
        /// </summary>
        protected float startTime;

        /// <summary>
        /// Get/Set the bool that determines if the timer should run (TRUE) or not (FALSE)
        /// </summary>
        public bool RunTimer
        {
            get { return runTimer; }
            set { runTimer = value; }
        }

        /// <summary>
        /// Get/Set the bool that determines if the timer runs a single time and stops (TRUE) or continuously runs (FALSE)
        /// </summary>
        public bool SingleRun
        {
            get { return singleRun; }
            set { singleRun = value; }
        }

        /// <summary>
        /// Get/Set the bool that determines if the timer has finished running (TRUE) or not (FALSE)
        /// </summary>
        public bool TimerFinished
        {
            get { return finished; }
            set { finished = value; }
        }

        /// <summary>
        /// Get/Set the duration of the timer
        /// </summary>
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        /// Get/Set the bool that determines if the timer duration is measured in frames (TRUE) or in time (FALSE)
        /// </summary>
        public bool FrameBasedTimer
        {
            get { return framesBased; }
            set { framesBased = value; }
        }

        /// <summary>
        /// Get/Set the event that gets invoked when the timer is finished running
        /// </summary>
        public UnityEvent TimerEvent
        {
            get { return timerEvent; }
            set { timerEvent = value; }
        }



        /// <summary>
        /// Update is called once per frame
        /// </summary>
        protected virtual void Update()
        {
            CountDownTimer();
        }

        /// <summary>
        /// Starts the timer running
        /// </summary>
        public virtual void StartTimer()
        {
            runTimer = true;
            finished = false;

            if (randomDuration)
                RandomDuration();

            if (framesBased)
                startTime = 0f;
            else
                startTime = Time.time;

            if (debug)
                Debug.Log("StartTimer() - Timer Started");
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public virtual void StopTimer()
        {
            runTimer = false;
            if (debug)
                Debug.Log("StopTimer() - Timer Stopped");
        }

        /// <summary>
        /// Called when the timer is finished running
        /// </summary>
        protected virtual void CountDownFinished()
        {
            finished = true;
            timerEvent.Invoke();
            if (debug)
                Debug.Log("CountDownFinished() - Timer Countdown Finished");
        }

        /// <summary>
        /// Runs a countdown timer 
        /// </summary>
        protected virtual void CountDownTimer()
        {
            if (runTimer)
            {
                if (framesBased)
                {
                    if (startTime < duration)
                        startTime += 1.0f;
                    else
                    {
                        CountDownFinished();

                        //  Reset the timer for non single run timers
                        if (!singleRun)
                            startTime = 0f;
                    }
                }
                else
                {
                    //Log(Time.time.ToString());
                    if (Time.time - startTime > duration)
                    {
                        CountDownFinished();

                        //  Reset the timer for non single run timers
                        if (!singleRun)
                            startTime = Time.time;
                    }

                }
            }
        }

        /// <summary>
        /// Sets a random timer diration between the durationMin and durationMax values
        /// </summary>
        void RandomDuration()
        {
            duration = Random.Range(durationMin, durationMax);
        }

    }
}