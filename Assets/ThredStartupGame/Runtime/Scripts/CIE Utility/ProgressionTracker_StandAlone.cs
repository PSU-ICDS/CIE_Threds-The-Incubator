/*
 * Name: ProgressionTracker_StandAlone
 * Project: XR Template Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 10/31/2023
 * Description: This script holds info about the progression of the experience in this scene. 
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
    public class ProgressionTracker_StandAlone : MonoBehaviour
    {
        /// <summary>
        /// Phase number for the currently active phase
        /// </summary>
        [Tooltip("Phase number for the currently active phase")]
        [SerializeField] int phaseNum;
        [Space(10)]
        /// <summary>
        /// List of progression phases for this experience
        /// </summary>
        [Tooltip("List of progression phases for this experience")]
        [SerializeField] List<Phase> phases;
        [Space(10)]
        /// <summary>
        /// Debugging settings used for testing and debugging functionality through the inspector window while running in the editor
        /// </summary>
        [Tooltip("Debugging settings used for testing and debugging functionality through the inspector window while running in the editor")]
        [SerializeField] DebugSettings debugSettings;
        

        /// <summary>
        /// Container class which holds info and events for a specific progression phase of this experience
        /// </summary>
        [Serializable]
        public class Phase
        {
            /// <summary>
            /// Name of this phase
            /// </summary>
            [Tooltip("Name of this phase")]
            public string name;
            /// <summary>
            /// Number for this phase
            /// </summary>
            [Tooltip("Number for this phase")]
            public int phaseNum;
            /// <summary>
            /// Unity event which gets invoked at the start of this phase
            /// </summary>
            [Tooltip("Unity event which gets invoked at the start of this phase")]
            public UnityEvent startEvent;
            /// <summary>
            /// Unity event used to reset a phase
            /// </summary>
            [Tooltip("Unity event used to reset a phase")]
            public UnityEvent resetEvent;
            /// <summary>
            /// Unity event which gets invoked at the end of this phase
            /// </summary>
            [Tooltip("Unity event which gets invoked at the end of this phase")]
            public UnityEvent endEvent;

            /// <summary>
            /// Runs the startEvent or endEvent for this phase based on the given bool
            /// </summary>
            /// <param name="_startEvent">Determines if the startEvent runs (TRUE) or the endEvent runs (FALSE)</param>
            public void RunPhaseStateEvent(bool _startEvent)
            {
                if (_startEvent)
                    startEvent.Invoke();
                else
                    endEvent.Invoke();
            }

            /// <summary>
            /// Runs the resetEvent for this phase
            /// </summary>
            public void RunPhaseEvent_Reset()
            {
                resetEvent.Invoke();
            }
        }

        /// <summary>
        /// Container class which holds debugging variables and settings
        /// </summary>
        [Serializable]
        public class DebugSettings
        {
            /// <summary>
            /// Determines if debugging is active (TRUE) or not (FALSE)
            /// </summary>
            [Tooltip("Determines if debugging is active (TRUE) or not (FALSE)")]
            public bool debug;
            /// <summary>
            /// Debugging bool which activates the previous phase when set to TRUE
            /// The 'debug' bool must be TRUE for this functionality be acted on
            /// </summary>
            [Tooltip("Debugging bool which activates the previous phase when set to TRUE.\n The 'debug' bool must be TRUE for this functionality be acted on")]
            public bool debug_PreviousPhase;
            /// <summary>
            /// Debugging bool which resets the active phase when set to TRUE
            /// The 'debug' bool must be TRUE for this functionality be acted on
            /// </summary>
            [Tooltip("Debugging bool which resets the active phase when set to TRUE.\n The 'debug' bool must be TRUE for this functionality be acted on")]
            public bool debug_ResetPhase;
            /// <summary>
            /// Debugging bool which activates the next phase when set to TRUE
            /// The 'debug' bool must be TRUE for this functionality be acted on
            /// </summary>
            [Tooltip("Debugging bool which activates the next phase when set to TRUE.\n The 'debug' bool must be TRUE for this functionality be acted on")]
            public bool debug_NextPhase;
            /// <summary>
            /// Debugging bool which activates the phaseNum phase when set to TRUE
            /// The 'debug' bool must be TRUE for this functionality be acted on
            /// </summary>
            [Tooltip("Debugging bool which activates the phaseNum phase when set to TRUE.\n The 'debug' bool must be TRUE for this functionality be acted on")]
            public bool debug_ActivatePhaseNum;
        }


        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void Start()
        {
            SetPhaseState(phaseNum, true);
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update()
        {
            CheckDebugSettings();
        }

        /// <summary>
        /// Activates the phase with the given phaseNum
        /// If the given phaseNum is different from the current phase, the current phase is ended and the new one is started.
        /// If the given phaseNum matches the currently active phase, the current phase is reset
        /// </summary>
        /// <param name="_phaseNum">Phase Number of the phase to activate</param>
        public virtual void ActivatePhaseNum(int _phaseNum)
        {
            if (_phaseNum != phaseNum)
            {
                SetPhaseState(phaseNum, false);
                phaseNum = _phaseNum;
                SetPhaseState(phaseNum, true);
            }
            else
                Phase_ResetCurrent();

        }

        /// <summary>
        /// Set the active state for the phase whos phaseNumber matches the given _phaseNum param
        /// </summary>
        /// <param name="_phaseNum">Phase number for the phase to start/end</param>
        /// <param name="_startEventState">Determines if the targeted phase will be started (TRUE) or ended (FALSE)</param>
        public virtual void SetPhaseState(int _phaseNum, bool _startEventState)
        {
            for (int i = 0; i < phases.Count; i++)
            {
                if (phases[i] != null)
                {
                    if (phases[i].phaseNum == _phaseNum)
                        phases[i].RunPhaseStateEvent(_startEventState);
                }
            }
        }

        /// <summary>
        /// Activates the previous phase (based on phaseNumber)
        /// </summary>
        public virtual void Phase_ActivatePrevious()
        {
            ActivatePhaseNum(phaseNum - 1);
        }

        /// <summary>
        /// Activates the next phase (based on phaseNumber)
        /// </summary>
        public virtual void Phase_ActivateNext()
        {
            ActivatePhaseNum(phaseNum + 1);
        }

        /// <summary>
        /// Activates the previous phase (based on phaseNumber) with the condition that the currently active phase must match the given _phaseToMatch phase number
        /// </summary>
        /// <param name="_phaseToMatch">Phase number for the currently active phase required for this phase change to occur</param>
        public virtual void Phase_ActivatePrevious_Conditional(int _phaseToMatch)
        {
            if (_phaseToMatch == phaseNum)
                Phase_ActivatePrevious();
        }

        /// <summary>
        /// Activates the next phase (based on phaseNumber) with the condition that the currently active phase must match the given _phaseToMatch phase number
        /// </summary>
        /// <param name="_phaseToMatch">Phase number for the currently active phase required for this phase change to occur</param>
        public virtual void Phase_ActivateNext_Conditional(int _phaseToMatch)
        {
            if (_phaseToMatch == phaseNum)
                Phase_ActivateNext();
        }

        /// <summary>
        /// Runs the resetEvent for the currently active phase
        /// </summary>
        public virtual void Phase_ResetCurrent()
        {
            if (phases[phaseNum] != null)
                phases[phaseNum].RunPhaseEvent_Reset();
        }

        /// <summary>
        /// Checks the variables in the DebuggingSettings and takes actions accordingly
        /// </summary>
        public virtual void CheckDebugSettings()
        {
            if(debugSettings.debug)
            {
                if(debugSettings.debug_NextPhase)
                {
                    ActivatePhaseNum(phaseNum + 1);
                    debugSettings.debug_NextPhase = false;
                }

                if (debugSettings.debug_PreviousPhase)
                {
                    ActivatePhaseNum(phaseNum - 1);
                    debugSettings.debug_PreviousPhase = false;
                }

                if (debugSettings.debug_ActivatePhaseNum)
                {
                    ActivatePhaseNum(phaseNum);
                    debugSettings.debug_ActivatePhaseNum = false;
                }

                if(debugSettings.debug_ResetPhase)
                {
                    Phase_ResetCurrent();
                    debugSettings.debug_ResetPhase = false;
                }
            }
        }

    }
}