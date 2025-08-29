/*
 * Name: EventSet_IntTriggered
 * Project: ?
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 09/06/2024
 * Description: This script holds sets of UnityEvents which can be triggered by providing a specific int
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSet_IntTriggered : MonoBehaviour
{
    /// <summary>
    /// Current int used to check and trigger events
    /// </summary>
    [Tooltip("Current int used to check and trigger events")]
    [SerializeField] int current;
    /// <summary>
    /// List of events which each have their own triggering int to check against the "current" int
    /// </summary>
    [Tooltip("List of events which each have their own triggering int to check against the 'current' int")]
    [SerializeField] List<IntEvent> intEvents;

    /// <summary>
    /// Current int used to check and trigger events
    /// </summary>
    public int CurrentInt { get=> current; set {SetCurrentInt(value); } }

    /// <summary>
    /// Container class which holds a triggering int value, an activated state, and events to run when activated/disabled
    /// </summary>
    [Serializable]
    public class IntEvent
    {
        /// <summary>
        /// Determines if this event is currently activated from having a matching trigger int to the "current" int
        /// </summary>
        [Tooltip("Determines if this event is currently activated from having a matching trigger int to the 'current' int")]
        public bool activated;
        /// <summary>
        /// Int which will trigger this event to activate when matching the "current" int
        /// </summary>
        [Tooltip("Int which will trigger this event to activate when matching the 'current' int")]
        public int triggerInt;
        /// <summary>
        /// Unity event which gets invoked when this event is activated
        /// </summary>
        [Tooltip("Unity event which gets invoked when this event is activated")]
        public UnityEvent activeEvent;
        /// <summary>
        /// Unity event which gets invoked when this event is disabled
        /// </summary>
        [Tooltip("Unity event which gets invoked when this event is disabled")]
        public UnityEvent disableEvent;
       
        /// <summary>
        /// Checks this events triggerInt against the given int, then runs corresponding events based on match/mismatch states
        /// </summary>
        /// <param name="_int">Current int to check against the triggerInt</param>
        public void CheckIntTriggerMatch(int _int)
        {
            if(_int == triggerInt)
            {
                if(!activated)
                {
                    activated = true;
                    activeEvent.Invoke();
                }
            }
            else
            {
                if(activated)
                {
                    activated = false;
                    disableEvent.Invoke();
                }
            }
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        //  Add connecting code here to get the int from the server before applying it to the "CurrentInt" var
        //  of applying it using the "SetCurrentInt()" function.
    }

    /// <summary>
    /// Set the current int used to check/trigger events
    /// </summary>
    /// <param name="_int">It to set the current int to</param>
    public void SetCurrentInt(int _int)
    {
        if(_int != current)
        {
            current = _int;
            CheckIntEvents();
        }
    }

    /// <summary>
    /// Checks all intEvents for triggerInts that match the "current" int
    /// </summary>
    public void CheckIntEvents()
    {
        for (int i = 0; i < intEvents.Count; i++)
        {
            if(intEvents[i] != null)
            {
                intEvents[i].CheckIntTriggerMatch(current);
            }
        }
    }
}
