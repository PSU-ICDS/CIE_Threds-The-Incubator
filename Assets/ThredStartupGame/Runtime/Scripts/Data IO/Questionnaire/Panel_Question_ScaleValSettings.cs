/*
 * Name: Panel_Question_ScaleValSettings
 * Project: Butterfly Experience Project (v2)
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 09/06/2023
 * Description: This script manages settings related to the scale values for a questions panel
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
    public class Panel_Question_ScaleValSettings : MonoBehaviour
    {
        [SerializeField] int scaleNumbersFontSize;
        [SerializeField] int scaleTextFontSize;
        [SerializeField] int scaleLabelTextFontSize;
        [SerializeField] List<ScaleValSet> scaleSets;
        [SerializeField] bool overrideTextOnSetup;
        [SerializeField] List<string> scaleTextStrings;

        public List<string> ScaleTextStrings { get { return scaleTextStrings; } set { SetScaleTextStrings(value); } }

        //[Serializable]
        //public class ScaleValObj
        //{
        //   public Text number;
        //   public Text text;

        //    public void SetNumber(string _val)
        //    {
        //        if (number != null)
        //            number.text = _val;
        //    }

        //    public void SetText(string _text)
        //    {
        //        if (text != null)
        //            text.text = _text;
        //    }

        //    public void SetTextSize(int _size)
        //    {
        //        if (text != null)
        //            text.fontSize = _size;
        //    }

        //    public void SetNumberSize(int _size)
        //    {
        //        if (number != null)
        //            number.fontSize = _size;
        //    }

        //}


        private void Awake()
        {
            Setup();
        }

        public void Setup()
        {
            FindScaleValObjs();
            if(overrideTextOnSetup)
                ApplyScaleTextStrings();
        }

        void FindScaleValObjs()
        {
            ScaleValSet[] childrenObjs = GetComponentsInChildren<ScaleValSet>(true);

            for (int i = 0; i < childrenObjs.Length; i++)
            {
                if(childrenObjs[i] != null)
                {
                    if (!scaleSets.Contains(childrenObjs[i]))
                    {
                        scaleSets.Add(childrenObjs[i]);
                        childrenObjs[i].Setup();
                    }
                }
            }
        }

        public void ScaleObjs_DisableAll()
        {
            for (int i = 0; i < scaleSets.Count; i++)
            {
                if (scaleSets[i] != null)
                    scaleSets[i].gameObject.SetActive(false);
            }
        }

        public void ScaleObjs_ActivateByType(ScaleValSet.ScaleTypes _type)
        {
            for (int i = 0; i < scaleSets.Count; i++)
            {
                if(scaleSets[i] != null)
                {
                    scaleSets[i].gameObject.SetActive(scaleSets[i].Type == _type);

                    //if (scaleObjs[i].Type == _type)
                    //    scaleObjs[i].gameObject.SetActive(true);
                    //else
                    //    scaleObjs[i].gameObject.SetActive(false);

                }
            }
        }

        //  Text and Value Functions    -------------------------------------------------------------------------------------
        #region Text and Value Functions

        public void ApplyScaleTextStrings()
        {
            for (int i = 0; i < scaleSets.Count; i++)
            {
                if(scaleSets[i] != null)
                {
                    scaleSets[i].SetScaleText(scaleTextStrings);
                }

                ////Debug.Log("ScaleTextString Index: " + i + ". ScaleTextStringsCount: " + scaleTextStrings.Count);

                //if (i < scaleTextStrings.Count)
                //{
                //    SetScaleText(i, scaleTextStrings[i]);
                //    //SetScaleText(i, _strings[i]);
                //}
            }
        }


        public void SetScaleTextStrings(List<string> _strings)
        {
            scaleTextStrings = _strings;

            for (int i = 0; i < scaleSets.Count; i++)
            {
                //Debug.Log("ScaleTextString Index: " + i + ". ScaleTextStringsCount: " + scaleTextStrings.Count);

                if(i < scaleTextStrings.Count)
                {
                    SetScaleText(i, scaleTextStrings[i]);
                    //SetScaleText(i, _strings[i]);
                }
            }
        }

        public void SetFontSize_Text(int _size)
        {
            scaleTextFontSize = _size;

            for (int i = 0; i < scaleSets.Count; i++)
            {
                if(scaleSets[i] != null)
                {
                    scaleSets[i].SetFontSize_Text(scaleTextFontSize);
                }
            }
        }
        public void SetFontSize_LabelText(int _size)
        {
            scaleLabelTextFontSize = _size;

            for (int i = 0; i < scaleSets.Count; i++)
            {
                if (scaleSets[i] != null)
                {
                    scaleSets[i].SetFontSize_LabelText(scaleLabelTextFontSize);
                }
            }
        }


        public void SetFontSize_Numbers(int _size)
        {
            scaleNumbersFontSize = _size;

            for (int i = 0; i < scaleSets.Count; i++)
            {
                if (scaleSets[i] != null)
                {
                    scaleSets[i].SetFontSize_Numbers(scaleNumbersFontSize);
                }
            }
        }


        public void SetScaleNumber(int _index, string _val)
        {
            if(_index >= 0 && _index < scaleSets.Count)
            {
                if(scaleSets[_index] != null)
                {
                    scaleSets[_index].SetScaleNumber(_index, _val);
                }
            }
        }

        public void SetScaleNumber(int _index, int _val)
        {
            if (_index >= 0 && _index < scaleSets.Count)
            {
                if (scaleSets[_index] != null)
                {
                    scaleSets[_index].SetScaleNumber(_index, _val.ToString());
                }
            }
        }

        public void SetScaleNumber(int _index, float _val)
        {
            if (_index >= 0 && _index < scaleSets.Count)
            {
                if (scaleSets[_index] != null)
                {
                    scaleSets[_index].SetScaleNumber(_index, _val.ToString());
                }
            }
        }

        public void SetScaleText(int _index, string _val)
        {
            if (_index >= 0 && _index < scaleSets.Count)
            {
                if (scaleSets[_index] != null)
                {
                    scaleSets[_index].SetScaleText(_index, _val);
                }
            }
        }

        #endregion

    }
}