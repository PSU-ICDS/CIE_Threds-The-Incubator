/*
 * Name: EnableEvent
 * Project: XR Template Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 05/10/2023
 * Description: This script runs an event when the object it is attached to gets disabled
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
    public class DisableEvent : MonoBehaviour
    {
        /// <summary>
        /// Unity Event which gets invoked when this gameObject gets disabled
        /// </summary>
        [Tooltip("Unity Event which gets invoked when this gameObject gets disabled")]
        [SerializeField] UnityEvent disableEvent;

        private void OnDisable()
        {
            disableEvent.Invoke();
        }

    }
}