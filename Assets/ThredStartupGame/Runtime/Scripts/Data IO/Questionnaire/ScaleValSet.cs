/*
 * Name: ScaleValSet
 * Project: Butterfly Experience Project (v2)
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 10/24/2023
 * Description: This script manages settings related for a set of scale value objects
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
    public class ScaleValSet : MonoBehaviour
    {
        public enum ScaleTypes { NONE, VAL_2, VAL_3, VAL_4, VAL_5, VAL_6, SLIDER, PANAS, INPUT_FIELD, MULTI_CHOICE }
        [SerializeField] ScaleTypes scaleType;
        [SerializeField] bool displayLabels;
        [SerializeField] bool displayText;
        [SerializeField] bool displayNumber;
        [SerializeField] bool displayImage;
        [SerializeField] int multiChoice_Count;
        [SerializeField] bool multiChoice_SingleResponse;
        [SerializeField] string multiChoice_Response;
        [SerializeField] string multiChoice_Delimiter;
        [SerializeField] Panel_QuestionSettings questionManager;

        [SerializeField] bool overrideResponseType;
        [SerializeField] ScaleValObj.ResponseOverrideType responseOverride;

        [SerializeField] List<ScaleValObj> scaleObjs;


        public ScaleTypes Type { get { return scaleType; } set { scaleType = value; } }
        public int MultiChoice_Count { get => multiChoice_Count; set { multiChoice_Count = value; } }
        public Panel_QuestionSettings QuestionManager { get { return questionManager; } set { questionManager = value; } }
        public string MultiChoice_Delimiter { get=>multiChoice_Delimiter; set { multiChoice_Delimiter = value; } }



        private void Awake()
        {
            Setup();
        }

        public void Setup()
        {
            FindScaleValObjs();
            SetObjectStates();
            SetResponseOverrideState();
            UpdateMultiChoiceButtons();
        }


        void FindScaleValObjs()
        {
            ScaleValObj[] childrenObjs = GetComponentsInChildren<ScaleValObj>(true);

            for (int i = 0; i < childrenObjs.Length; i++)
            {
                if (childrenObjs[i] != null)
                {
                    if (!scaleObjs.Contains(childrenObjs[i]))
                    {
                        scaleObjs.Add(childrenObjs[i]);
                        childrenObjs[i].ValueSet = this;
                        if (multiChoice_Count > 0)
                        {
                            childrenObjs[i].MultiChoice = true;
                            if (!multiChoice_SingleResponse)
                                childrenObjs[i].IsTrueFalse = true;
                        }
                    }
                }
            }
        }

        public void SetObjectStates()
        {
            for (int i = 0; i < scaleObjs.Count; i++)
            {
                if (scaleObjs[i] != null)
                {
                    scaleObjs[i].SetActive_Label(displayLabels);
                    scaleObjs[i].SetActive_Number(displayNumber);
                    scaleObjs[i].SetActive_Text(displayText);
                    scaleObjs[i].SetActive_Image(displayImage);
                }
            }
        }

        public void SetScaleTextStrings(List<string> _strings)
        {

            for (int i = 0; i < scaleObjs.Count; i++)
            {
                //Debug.Log("ScaleTextString Index: " + i + ". ScaleTextStringsCount: " + scaleTextStrings.Count);

                if (i < scaleObjs.Count)
                {
                    SetScaleText(i, _strings[i]);
                    //SetScaleText(i, _strings[i]);
                }
            }
        }

        public void SetFontSize_Text(int _size)
        {

            for (int i = 0; i < scaleObjs.Count; i++)
            {
                if (scaleObjs[i] != null)
                {
                    scaleObjs[i].SetTextSize(_size);
                }
            }
        }

        public void SetFontSize_LabelText(int _size)
        {

            for (int i = 0; i < scaleObjs.Count; i++)
            {
                if (scaleObjs[i] != null)
                {
                    scaleObjs[i].SetLabelTextSize(_size);
                }
            }
        }

        public void SetFontSize_Numbers(int _size)
        {

            for (int i = 0; i < scaleObjs.Count; i++)
            {
                if (scaleObjs[i] != null)
                {
                    scaleObjs[i].SetNumberSize(_size);
                }
            }
        }


        public void SetScaleNumber(int _index, string _val)
        {
            if (_index >= 0 && _index < scaleObjs.Count)
            {
                if (scaleObjs[_index] != null)
                {
                    scaleObjs[_index].SetNumber(_val);
                }
            }
        }

        public void SetScaleNumber(int _index, int _val)
        {
            if (_index >= 0 && _index < scaleObjs.Count)
            {
                if (scaleObjs[_index] != null)
                {
                    scaleObjs[_index].SetNumber(_val.ToString());
                }
            }
        }

        public void SetScaleNumber(int _index, float _val)
        {
            if (_index >= 0 && _index < scaleObjs.Count)
            {
                if (scaleObjs[_index] != null)
                {
                    scaleObjs[_index].SetNumber(_val.ToString());
                }
            }
        }

        public void SetScaleText(int _index, string _val)
        {
            if (_index >= 0 && _index < scaleObjs.Count)
            {
                if (scaleObjs[_index] != null)
                {
                    scaleObjs[_index].SetText(_val);
                }
            }
        }

        public void SetScaleText(List<string> _textSet)
        {
            if (_textSet != null)
            {
                for (int i = 0; i < _textSet.Count; i++)
                {
                    if (_textSet[i] != null)
                    {
                        if (i < scaleObjs.Count)
                        {
                            if (scaleObjs[i] != null)
                                scaleObjs[i].SetText(_textSet[i]);
                        }
                    }
                }
            }

            //for (int i = 0; i < scaleObjs.Count; i++)
            //{
            //    if(scaleObjs[i] != null)
            //    {

            //    }
            //}
        }

        public void SetResponseOverrideState()
        {
            if (overrideResponseType)
            {
                for (int i = 0; i < scaleObjs.Count; i++)
                {
                    if (scaleObjs[i] != null)
                    {
                        scaleObjs[i].ResponseOverride_SetState(responseOverride);
                    }
                }
            }
        }


        public void UpdateMultiChoiceButtons()
        {
            if (scaleType == ScaleTypes.MULTI_CHOICE)
            {
                for (int i = 0; i < scaleObjs.Count; i++)
                {
                    if (scaleObjs[i] != null)
                    {
                        if (i < multiChoice_Count)
                            scaleObjs[i].gameObject.SetActive(true);
                        else
                            scaleObjs[i].gameObject.SetActive(false);
                        //scaleObjs[i].ResponseOverride_SetState(responseOverride);
                    }
                }
            }
        }



        public void MultiChoice_CollectAndSendResponses()
        {
            MultiChoice_CollectAllResponses();
            MultiChoice_SendCollectedResponses();
        }

        public void MultiChoice_CollectAndSendResponses(string _singleResponse)
        {
            if (multiChoice_SingleResponse)
                multiChoice_Response = _singleResponse;
            else
                MultiChoice_CollectAllResponses();

            MultiChoice_SendCollectedResponses();
        }


        public void MultiChoice_CollectAllResponses()
        {
            multiChoice_Response = "";
            bool firstAdded = false;

            for (int i = 0; i < scaleObjs.Count; i++)
            {
                if (scaleObjs[i] != null)
                {
                    //if (questionManager == null)
                    //    questionManager = scaleObjs[i].QuestionManager;

                    if (scaleObjs[i].gameObject.activeSelf)
                    {
                        if (firstAdded)
                            multiChoice_Response = multiChoice_Response + multiChoice_Delimiter;
                            //multiChoice_Response = multiChoice_Response + ",";

                        if (scaleObjs[i].IsTrueFalse)
                        {
                            if (scaleObjs[i].TrueFalseValue)
                                multiChoice_Response = multiChoice_Response + scaleObjs[i].Response;
                            else
                                multiChoice_Response = multiChoice_Response + "null";

                            if (!firstAdded)
                                firstAdded = true;
                        }
                        else
                        {
                            multiChoice_Response = multiChoice_Response + scaleObjs[i].Response;
                            if (!firstAdded)
                                firstAdded = true;
                        }

                       

                        //if (scaleObjs[i].IsTrueFalse)
                        //{
                        //    if (scaleObjs[i].TrueFalseValue)
                        //        multiChoice_Response = multiChoice_Response + scaleObjs[i].Response + ",";
                        //    else
                        //        multiChoice_Response = multiChoice_Response + "null,";
                        //}
                        //else
                        //    multiChoice_Response = multiChoice_Response + scaleObjs[i].Response + ",";
                    }
                }
            }
        }

        public void MultiChoice_SendCollectedResponses()
        {
            if (questionManager != null)
            {
                questionManager.Response_UpdateResponseData(multiChoice_Response);
            }
        }

        public void MultiChoice_SendAllResponses()
        {
            for (int i = 0; i < scaleObjs.Count; i++)
            {
                if (scaleObjs[i] != null)
                {
                    if (scaleObjs[i].gameObject.activeSelf)
                    {
                        scaleObjs[i].Response_SendResponse();
                    }
                }
            }
        }

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