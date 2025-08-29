/*
 * Name: ToggleObject
 * Project: XR Template Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 02/09/2023
 * Description: This script toggles gameObjects On/Off
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code created for XR Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_XR_Utility
{
    public class ToggleObject : MonoBehaviour
    {
        /// <summary>
        /// Determines if this should toggle multiple target objects (TRUE) or only a single target object (FALSE)
        /// </summary>
        [Tooltip("Determines if this should toggle multiple target objects (TRUE) or only a single target object (FALSE)")]
        [SerializeField] bool multiTarget = false;
        /// <summary>
        /// Holds the object to be toggled on/off
        /// </summary>
        [Tooltip("Holds the object to be toggled on/off")]
        [SerializeField] GameObject target = null;
        /// <summary>
        /// Holds multiple targets to be toggled (only used if multiTarget = True, otherwise this can be left blank)
        /// </summary>
        [Tooltip("Holds multiple targets to be toggled (only used if multiTarget = True, otherwise this can be left blank)")]
        [SerializeField] GameObject[] targets;
        /// <summary>
        /// Holds the index of the currently active target in targets[]. Used for the CycleTarget() func.
        /// </summary>
        int activeIndex = -1;
        /// <summary>
        /// Determines if this script prints out debug messages (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if this script prints out debug messages (TRUE) or not (FALSE)")]
        [SerializeField] bool debug;

        public bool MultiTarget { get { return multiTarget; } set { multiTarget = value; } }
        public GameObject Target { get { return target; } set { target = value; } }
        public GameObject[] Targets { get { return targets; } set { targets = value; } }
        public int ActiveIndex { get { return activeIndex; } set { activeIndex = value; } }

      

        /// <summary>
        /// Toggles the active state of the target object on/off when called
        /// </summary>
        public void ToggleState()
        {
            //Checks if this is multiTarget or single target
            if (multiTarget)
            {
                //Checks that the targets[] array isnt empty
                if (targets.Length > 0)
                {
                    //Loops through all targets and toggles them one by one
                    for (int i = 0; i < targets.Length; i++)
                    {
                        //Checks that the target at this index is not null
                        if (targets[i] != null)
                            targets[i].SetActive(!targets[i].activeSelf);
                    }
                }
                else
                {
                    if (debug)
                        Debug.Log("No targets have been added to the targets[] array in ToggleObject script on the " + gameObject.name + "!!");
                }
            }
            else
            {
                if (target != null)
                    target.SetActive(!target.activeSelf);
            }
        }

        /// <summary>
        /// Turn the target object off
        /// </summary>
        public void TargetOff()
        {
            //Checks if this is multiTarget or single target
            if (multiTarget)
            {
                //Checks that the targets[] array isnt empty
                if (targets.Length > 0)
                {
                    //Loops through all targets and toggles them one by one
                    for (int i = 0; i < targets.Length; i++)
                    {
                        //Checks that the target at this index is not null
                        if (targets[i] != null)
                            targets[i].SetActive(false);
                    }
                }
                else
                {
                    if (debug)
                        Debug.Log("No targets have been added to the targets[] array in ToggleObject script on the " + gameObject.name + "!!");
                }
            }
            else
            {
                if (target != null)
                    target.SetActive(false);
            }

        }

        /// <summary>
        /// Turn the target object on
        /// </summary>
        public void TargetOn()
        {
            //Checks if this is multiTarget or single target
            if (multiTarget)
            {
                //Checks that the targets[] array isnt empty
                if (targets.Length > 0)
                {
                    //Loops through all targets and toggles them one by one
                    for (int i = 0; i < targets.Length; i++)
                    {
                        //Checks that the target at this index is not null
                        if (targets[i] != null)
                            targets[i].SetActive(true);
                    }
                }
                else
                {
                    if (debug)
                        Debug.Log("No targets have been added to the targets[] array in ToggleObject script on the " + gameObject.name + "!!");
                }
            }
            else
            {
                if (target != null)
                    target.SetActive(true);
            }

        }

        /// <summary>
        /// Cycles through the targets in the targets[] array and activates the next one in the array each time this is called
        /// </summary>
        public void CycleTarget()
        {
            //Checks if this script is set to multiTarget os has a single target
            if (multiTarget)
            {
                //Checks that the targets[] array isnt empty
                if (targets.Length > 0)
                {
                    //Incriments the active index to target the next location in the targets[] array
                    activeIndex++;

                    //Resets the active index if it fals out of bounds of the targets[] array
                    if (activeIndex >= targets.Length)
                        activeIndex = 0;

                    for (int i = 0; i < targets.Length; i++)
                    {
                        //Activates the target at the active index and deactivates the rest of the targets
                        if (i == activeIndex)
                        {
                            if (targets[i] != null)
                                targets[i].SetActive(true);
                        }
                        else
                        {
                            if (targets[i] != null)
                                targets[i].SetActive(false);
                        }
                    }

                }
                else
                {
                    if (debug)
                        Debug.Log("No targets have been added to the targets[] array in ToggleObject script on the " + gameObject.name + "!!");
                }

            }
            else
                ToggleState();
        }

        /// <summary>
        /// Activates a specific target index while disabling the rest of the objects in the targets[] array
        /// </summary>
        /// <param name="_index">The index to activate</param>
        public void ActivateSpecificTarget(int _index)
        {
            //Checks if this script is set to multiTarget os has a single target
            if (multiTarget)
            {
                //Checks that the targets[] array isnt empty and _index is within range for the targets[] array
                if (targets.Length > 0 && _index < targets.Length)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (i == _index && targets[i] != null)
                            targets[i].SetActive(true);
                        else
                        {
                            if (targets[i] != null)
                                targets[i].SetActive(false);
                        }
                    }
                }
                else
                {
                    if (debug)
                        Debug.Log("No targets have been added to the targets[] array in ToggleObject script on the " + gameObject.name + "!!");
                }

            }
        }

        /// <summary>
        /// Activates a specific target index while without changing the rest of the objects in the targets[] array
        /// </summary>
        /// <param name="_index">The index to activate</param>
        public void ActivateTargetNum(int _index)
        {
            //Checks if this script is set to multiTarget os has a single target
            if (multiTarget)
            {
                //Checks that the targets[] array isnt empty and _index is within range for the targets[] array
                if (targets.Length > 0 && _index < targets.Length)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (i == _index && targets[i] != null)
                            targets[i].SetActive(true);

                    }
                }
                else
                {
                    if (debug)
                        Debug.Log("No targets have been added to the targets[] array in ToggleObject script on the " + gameObject.name + "!!");
                }

            }
        }

        /// <summary>
        /// Deactivates a specific target index while without changing the rest of the objects in the targets[] array
        /// </summary>
        /// <param name="_index">The index to deactivate</param>
        public void DeactivateTargetNum(int _index)
        {
            //Checks if this script is set to multiTarget os has a single target
            if (multiTarget)
            {
                //Checks that the targets[] array isnt empty and _index is within range for the targets[] array
                if (targets.Length > 0 && _index < targets.Length)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (i == _index && targets[i] != null)
                            targets[i].SetActive(false);

                    }
                }
                else
                {
                    if (debug)
                        Debug.Log("No targets have been added to the targets[] array in ToggleObject script on the " + gameObject.name + "!!");
                }

            }
        }
    }
}