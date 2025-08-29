/*
 * Name: ToggleEventSet
 * Project: XR Template Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 05/01/2023
 * Description: This script holds a set of events which get invoked as a toggle bool changes
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Code created for XR Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_XR_Utility
{
    public class ToggleEventSet : MonoBehaviour
    {
        /// <summary>
        /// Determines if this script runs (TRUE) nor not (FALSE)
        /// </summary>
        [Tooltip("Determines if this script runs (TRUE) nor not (FALSE)")]
        [SerializeField] bool active;
        /// <summary>
        /// Bool determining the state of the toggle
        /// </summary>
        [Tooltip("Bool determining the state of the toggle")]
        [SerializeField] bool toggleState;
        /// <summary>
        /// Unity Event which gets invoked when the toggleState is TRUE and the RunToggleEvent() function is called
        /// </summary>
        [Tooltip("Unity Event which gets invoked when the toggleState is TRUE and the RunToggleEvent() function is called")]
        [SerializeField] UnityEvent toggleEvent_Active;
        /// <summary>
        /// Unity Event which gets invoked when the toggleState is FALSE and the RunToggleEvent() function is called
        /// </summary>
        [Tooltip("Unity Event which gets invoked when the toggleState is FALSE and the RunToggleEvent() function is called")]
        [SerializeField] UnityEvent toggleEvent_Inactive;

        public bool Active { get { return active; } set { active = value; } }

        /// <summary>
        /// Sets the state of the toggleState bool
        /// </summary>
        /// <param name="_state">State to set the toggleState bool to</param>
        public void Toggle_SetToggleState(bool _state)
        {
            toggleState = _state;
        }

        /// <summary>
        /// Sets the state of the toggleState bool then runs the RunToggleEvent() function to invoke
        /// the corresponding event
        /// </summary>
        /// <param name="_state"></param>
        public void Toggle_SetStateAndRunEvent(bool _state)
        {
            toggleState = _state;
            RunToggleEvent();
        }

        /// <summary>
        /// Toggles the active state for this script then runs the corresponding toggle event
        /// </summary>
        /// <param name="_state">Active state and event to run</param>
        public void Toggle_SetActiveStateAndRunEvent(bool _state)
        {
            if (_state)
            {
                active = _state;
                toggleState = _state;
                RunToggleEvent();
            }
            else
            {
                toggleState = _state;
                RunToggleEvent();
                active = _state;
            }
        }

        /// <summary>
        /// Sets the toggleState bool to the opposite of its current state then runs the corresponding toggle event
        /// </summary>
        public void ToggleAndRunEvent()
        {
            if (active)
            {
                toggleState = !toggleState;
                RunToggleEvent();
            }
        }

        /// <summary>
        /// Invokes the toggle event which corresponds to the current state of the toggleState bool
        /// </summary>
        void RunToggleEvent()
        {
            if (active)
            {
                if (toggleState)
                    toggleEvent_Active.Invoke();
                else
                    toggleEvent_Inactive.Invoke();
            }
        }


    }
}