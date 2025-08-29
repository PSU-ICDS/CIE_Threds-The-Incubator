/*
 * Name: EnableEvent
 * Project: XR Template Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 04/28/2023
 * Description: This script runs an event when the object it is attached to gets enabled
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
    public class EnableEvent : MonoBehaviour
    {
        /// <summary>
        /// Unity Event which gets invoked when this gameObject gets enabled
        /// </summary>
        [Tooltip("Unity Event which gets invoked when this gameObject gets enabled")]
        [SerializeField] UnityEvent enableEvent;

        private void OnEnable()
        {
            enableEvent.Invoke();
        }
    }
}