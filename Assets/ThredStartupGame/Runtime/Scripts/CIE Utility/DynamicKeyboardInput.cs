/*
 * Name: DynamicKeyboardInput
 * Project: LightingVR Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 12/12/2023
 * Description: Checks for keyboard input from dynamically set keys and runs corresponding events
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
    public class DynamicKeyboardInput : MonoBehaviour
    {
        /// <summary>
        /// List of input sets, each determines an input key and corresponding events
        /// </summary>
        [Tooltip("List of input sets, each determines an input key and corresponding events")]
        [SerializeField] List<InputSet> inputSets;

        /// <summary>
        /// Container class which holds an input key and events corresponding to its pressed/held/up state
        /// </summary>
        [Serializable]
        public class InputSet
        {
            /// <summary>
            /// Name of this input set (auto assigned to the name of the input key on Start())
            /// </summary>
            [Tooltip("Name of this input set (auto assigned to the name of the input key on Start())")]
            public string name;
            /// <summary>
            /// Determines if this input set reacts to key inputs (TRUE) or ignores inputs (FALSE)
            /// </summary>
            [Tooltip("Determines if this input set reacts to key inputs (TRUE) or ignores inputs (FALSE)")]
            public bool active;
            /// <summary>
            /// Key to check for pressed/hold/up state
            /// </summary>
            [Tooltip("Key to check for pressed/hold/up state")]
            public KeyCode inputKey;
            /// <summary>
            /// Indicated if the inputKey is currently pressed/down (TRUE) or is released/up (FALSE)
            /// </summary>
            [Tooltip("Indicated if the inputKey is currently pressed/down (TRUE) or is released/up (FALSE)")]
            public bool keyDown;
            /// <summary>
            /// Determines if the name of this inputSet gets auto updated to match the name of the inputKey (TRUE)
            /// or keeps the currently set name (FALSE) when Start() is called
            /// </summary>
            [Tooltip("Determines if the name of this inputSet gets auto updated to match the name of the inputKey (TRUE) " +
                "or keeps the currently set name (FALSE) when Start() is called")]
            public bool autoUpdateName;
            /// <summary>
            /// Holds the keyDown state for the previous check
            /// </summary>
            bool prevState;
            /// <summary>
            /// Unity Event which gets invoked on the first frame which the inputKey is pressed
            /// </summary>
            [Tooltip("Unity Event which gets invoked on the first frame which the inputKey is pressed")]
            public UnityEvent keyPressEvent;
            /// <summary>
            /// Unity Event which gets invoked on every frame while the inputKey is pressed
            /// </summary>
            [Tooltip("Unity Event which gets invoked on every frame while the inputKey is pressed")]
            public UnityEvent keyHoldEvent;
            /// <summary>
            /// Unity Event which gets invoked on the first frame which the inputKey is no longer being pressed (after being pressed)
            /// </summary>
            [Tooltip("Unity Event which gets invoked on the first frame which the inputKey is no longer being pressed (after being pressed)")]
            public UnityEvent keyUpEvent;

            /// <summary>
            /// Updates the inputSet name to match the name of the input key used
            /// </summary>
            public void UpdateName()
            {
                if (autoUpdateName || name == null || name == "")
                    name = inputKey.ToString();
            }

            /// <summary>
            /// Checks the state of the inputKey and runs corresponding events accordingly
            /// </summary>
            public void CheckKeyState()
            {
                //  Only checks input states when this inputSet is active
                if (active)
                {
                    //  First frame which the inputKey is pressed/down
                    if (Input.GetKeyDown(inputKey))
                    {
                        keyDown = true;
                        RunEvent_KeyPress();
                    }
                    //  Every frame the inputKey is pressed/down
                    else if (Input.GetKey(inputKey))
                    {
                        keyDown = true;
                        RunEvent_KeyHold();
                    }
                    //  First frame which the inputKey is released/up (after having been pressed during the previous check)
                    else
                    {
                        keyDown = false;
                        if (prevState != keyDown)
                            RunEvent_KeyUp();
                    }

                    //  Updates the previous keyDown state for the next check
                    prevState = keyDown;
                }
            }

            /// <summary>
            /// Runs the keyPressEvent used for the first frame that the inputKey is pressed/down 
            /// </summary>
            public void RunEvent_KeyPress()
            {
                Debug.Log("KeyInput Event -> KeyPress for key: " + name);
                keyPressEvent.Invoke();
            }

            /// <summary>
            /// Runs the keyPressEvent used for every frame that the inputKey is pressed/down 
            /// </summary>
            public void RunEvent_KeyHold()
            {
                Debug.Log("KeyInput Event -> KeyHold for key: " + name);
                keyHoldEvent.Invoke();
            }

            /// <summary>
            /// Runs the keyPressEvent used for the first frame that the inputKey is released/up after being pressed/down 
            /// </summary>
            public void RunEvent_KeyUp()
            {
                Debug.Log("KeyInput Event -> KeyUp for key: " + name);
                keyUpEvent.Invoke();
            }

        }


        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void Start()
        {
            UpdateInputNames();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update()
        {
            CheckInputs();
        }

        /// <summary>
        /// Updates the named of all inputSets to match the name of their inputKeys
        /// </summary>
        void UpdateInputNames()
        {
            for (int i = 0; i < inputSets.Count; i++)
            {
                if (inputSets[i] != null)
                {
                    inputSets[i].UpdateName();
                }
            }
        }

        /// <summary>
        /// Checks the inputs for all inputSets
        /// </summary>
        void CheckInputs()
        {
            for (int i = 0; i < inputSets.Count; i++)
            {
                if (inputSets[i] != null)
                {
                    inputSets[i].CheckKeyState();
                }
            }
        }

        /// <summary>
        /// Set the inputSet at the given index to be active
        /// </summary>
        /// <param name="_index">Index of the inputSet to set to be active</param>
        public void InputSet_SetActive(int _index)
        {
            if (_index >= 0 && _index < inputSets.Count)
            {
                if (inputSets[_index] != null)
                    inputSets[_index].active = true;
            }
        }

        /// <summary>
        /// Set the inputSet at the given index to be inactive
        /// </summary>
        /// <param name="_index">Index of the inputSet to set to be inactive</param>
        public void InputSet_SetInactive(int _index)
        {
            if (_index >= 0 && _index < inputSets.Count)
            {
                if (inputSets[_index] != null)
                    inputSets[_index].active = false;
            }
        }

        /// <summary>
        /// Exits the program
        /// </summary>
        public void ExitProgram()
        {
            Application.Quit();
        }
    }
}