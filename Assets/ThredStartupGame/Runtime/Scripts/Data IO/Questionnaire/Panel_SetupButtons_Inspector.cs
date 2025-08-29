/*
 * Name: Panel_SetupButtons_Inspector
 * Project: Nutrition - Buffet Project (v2)
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 10/25/2023
 * Description: This script provides inspector buttons to auto setup PanelGroup_Question objects without having to enter runtime
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Code created for XR Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_XR_Utility
{
    public class Panel_SetupButtons_Inspector : MonoBehaviour
    {

        [SerializeField] List<PanelGroup_Questions> panelGroups;
        [SerializeField] bool includeInactive;
        [SerializeField] int targetIndex;

        public void FindPanelGroups()
        {
            PanelGroup_Questions[] groups = GameObject.FindObjectsOfType<PanelGroup_Questions>(includeInactive);

            for (int i = 0; i < groups.Length; i++)
            {
                if (groups[i] != null)
                {
                    if (panelGroups == null || !panelGroups.Contains(groups[i]))
                        panelGroups.Add(groups[i]);
                }
            }
        }

        public void PanelGroups_SetupAll()
        {
            if (panelGroups == null || panelGroups.Count == 0)
                FindPanelGroups();

            for (int i = 0; i < panelGroups.Count; i++)
            {
                if (panelGroups[i] != null)
                    panelGroups[i].Setup();
            }
        }

        public void PanelGroups_SetupTargetIndex()
        {
            if (panelGroups == null || panelGroups.Count == 0)
                FindPanelGroups();

            if (targetIndex >= 0 && targetIndex < panelGroups.Count)
                panelGroups[targetIndex].Setup();
        }

        //public void PanelGroups_SortByID()
        //{
        //    for (int i = 0; i < panelGroups.Count; i++)
        //    {
        //        if (panelGroups[i] != null)
        //            panelGroups[i].Data_SortGroups();
        //    }
        //}


        //// Start is called before the first frame update
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}