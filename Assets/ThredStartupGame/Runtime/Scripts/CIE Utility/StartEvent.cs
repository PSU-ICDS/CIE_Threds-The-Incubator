/*
 * Name: StartEvent
 * Project: XR Template Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 04/09/2024
 * Description: This script runs an event at the start of the scene
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
    public class StartEvent : MonoBehaviour
    {
        [SerializeField] bool active;
        /// <summary>
        /// Unity Event which gets invoked when this gameObject gets enabled
        /// </summary>
        [Tooltip("Unity Event which gets invoked when this gameObject gets enabled")]
        [SerializeField] UnityEvent startEvent;

        // Start is called before the first frame update
        void Start()
        {
            if (active)
                startEvent.Invoke();
        }
    }
}