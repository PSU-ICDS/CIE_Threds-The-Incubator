/*
 * Name: Panel_QuestionSettings
 * Project: Butterfly Experience Project (v2)
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 08/29/2023
 * Description: This script manages settings for a specific question panel
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Code created for XR Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_XR_Utility
{
    public class Panel_QuestionSettings : MonoBehaviour
    {
        [SerializeField] bool questionPanel;
        [SerializeField] int questionNumber;
        [SerializeField] TextSettings textSettings;
        [SerializeField] ButtonSettings buttonSettings;
        [SerializeField] ScaleSettings scaleSettings;
        PanelGroup_Questions panelGroup;

        public bool isQuestionPanel { get { return questionPanel; } set { questionPanel = value; } }
        public int QuestionNumber { get { return questionNumber; } set { questionNumber = value; } }
        public PanelGroup_Questions PanelGroup { get { return panelGroup; } set { panelGroup = value; } }

        //public enum ScaleType { NONE, VALUES_4, VALUES_5 }
        public enum ButtonOption { NONE, SINGLE, DUAL }

        [Serializable]
        public class TextSettings
        {
            public bool applyLabelText;
            public TMP_Text labelTextObj;
            public string labelText;
            public bool applyInstructionText;
            public TMP_Text instructionTextObj;
            public string instructionText;
            public bool applyScaleInfoText;
            public TMP_Text scaleInfoTextObj;
            public string scaleInfoText;
            public bool applyQuestionText;
            public TMP_Text questionTextObj;
            public string questionText;

            public void Setup()
            {
                if (applyLabelText)
                    SetLabelText(labelText);

                if (applyInstructionText)
                    SetInstructionsText(instructionText);

                if (applyQuestionText)
                    SetQuestionText(questionText);

                if (applyScaleInfoText)
                    SetScaleInfoText(scaleInfoText);
            }

            public void SetLabelText(string _text)
            {
                if (labelTextObj != null)
                {
                    labelTextObj.text = _text;
                    labelText = _text;
                }
            }

            public void SetInstructionsText(string _text)
            {
                if (instructionTextObj != null)
                {
                    instructionTextObj.text = _text;
                    instructionText = _text;
                }
            }

            public void SetScaleInfoText(string _text)
            {
                if (scaleInfoTextObj != null)
                {
                    scaleInfoTextObj.text = _text;
                    scaleInfoText = _text;
                }
            }

            public void SetQuestionText(string _text)
            {
                if (questionTextObj != null)
                {
                    questionTextObj.text = _text;
                    questionText = _text;
                }
            }

        }

        [Serializable]
        public class ButtonSettings
        {
            public ButtonOption buttonSettings;
            public GameObject continueButtons_single;
            public GameObject continueButtons_dual;
            public bool hideButtonsOnEnable;

            public void ApplyButtonSettings()
            {
                switch (buttonSettings)
                {
                    case ButtonOption.NONE:
                        break;
                    case ButtonOption.SINGLE:
                        SetButtonState(true);
                        break;
                    case ButtonOption.DUAL:
                        SetButtonState(false);
                        break;
                    default:
                        break;
                }

            }

            void SetButtonState(bool _singleActive)
            {
                if (continueButtons_single != null)
                    continueButtons_single.SetActive(_singleActive);

                if (continueButtons_dual != null)
                    continueButtons_dual.SetActive(!_singleActive);
            }

            public void SetButtonState_HideAll()
            {
                if (continueButtons_single != null)
                    continueButtons_single.SetActive(false);

                if (continueButtons_dual != null)
                    continueButtons_dual.SetActive(false);
            }
        }

        [Serializable]
        public class ScaleSettings
        {
            public ScaleValSet.ScaleTypes scaleType;
            public Panel_Question_ScaleValSettings scaleValSettings;
            public int scaleTextFontSize;
            public int scaleNumberFontSize;
            public int scaleLabelFontSize;



            public void ApplyScaleSettings()
            {
                if (scaleValSettings != null)
                {
                    scaleValSettings.ScaleObjs_ActivateByType(scaleType);
                }
            }

            public void ApplyScaleValue_TextSize()
            {
                if (scaleValSettings != null)
                    scaleValSettings.SetFontSize_Text(scaleTextFontSize);
            }

            public void ApplyScaleValue_NumberSize()
            {
                if (scaleValSettings != null)
                    scaleValSettings.SetFontSize_Text(scaleNumberFontSize);
            }

            public void ApplyScaleValue_LabelTextSize()
            {
                if (scaleValSettings != null)
                    scaleValSettings.SetFontSize_LabelText(scaleLabelFontSize);
            }

            public void SetScaleTextStrings(List<string> _strings)
            {
                if (scaleValSettings != null)
                    scaleValSettings.ScaleTextStrings = _strings;
            }

        }

        public void Setup()
        {
            buttonSettings.ApplyButtonSettings();
            FindScaleValSettings();
            scaleSettings.ApplyScaleSettings();
            textSettings.Setup();
            UpdateQuestionManager();

            if (buttonSettings.hideButtonsOnEnable)
                PanelGroup_HideNavigationButtons();
        }

        public void FindScaleValSettings()
        {
            if (scaleSettings.scaleValSettings == null)
            {
                scaleSettings.scaleValSettings = GetComponentInChildren<Panel_Question_ScaleValSettings>();
                scaleSettings.scaleValSettings.Setup();
            }
        }

        public void UpdateQuestionManager()
        {

            ScaleValSet[] scaleVals = GetComponentsInChildren<ScaleValSet>(true);

            for (int i = 0; i < scaleVals.Length; i++)
            {
                if (scaleVals[i] != null)
                    scaleVals[i].QuestionManager = this;
            }

            ScaleValObj[] scaleObjs = GetComponentsInChildren<ScaleValObj>(true);

            for (int i = 0; i < scaleObjs.Length; i++)
            {
                if (scaleObjs[i] != null)
                    scaleObjs[i].QuestionManager = this;
            }
        }

        public void SetLabelText(string _labelText)
        {
            textSettings.SetLabelText(_labelText);
        }

        public void SetInstructionsText(string _instructionsText)
        {
            textSettings.SetInstructionsText(_instructionsText);
        }
        public void SetScaleInfoText(string _scaleInfoText)
        {
            textSettings.SetScaleInfoText(_scaleInfoText);
        }

        public void SetQuestionText(string _questionText)
        {
            textSettings.SetQuestionText(_questionText);
        }

        public void SetScaleVal_TextFontSize(int _size)
        {
            scaleSettings.scaleTextFontSize = _size;
            scaleSettings.ApplyScaleValue_TextSize();
        }

        public void SetScaleVal_LabelTextFontSize(int _size)
        {
            scaleSettings.scaleLabelFontSize = _size;
            scaleSettings.ApplyScaleValue_LabelTextSize();
        }

        public void SetScaleVal_NumbersFontSize(int _size)
        {
            scaleSettings.scaleNumberFontSize = _size;
            scaleSettings.ApplyScaleValue_NumberSize();
        }

        public void SetScaleValTextStrings(List<string> _strings)
        {
            scaleSettings.SetScaleTextStrings(_strings);
        }


        public void Response_UpdateResponseData(string _response)
        {
            if (isQuestionPanel && panelGroup != null)
            {
                panelGroup.Data_SetResponse(questionNumber, _response);
            }
        }

        public void PanelGroup_NextPanel()
        {
            if (panelGroup != null)
                panelGroup.Panels_Next();
        }

        public void PanelGroup_PreviousPanel()
        {
            if (panelGroup != null)
                panelGroup.Panels_Previous();
        }

        public void PanelGroup_RunStartEvent()
        {
            if (panelGroup != null)
                panelGroup.RunEvent_Start();
        }

        public void PanelGroup_RunEndEvent()
        {
            if (panelGroup != null)
                panelGroup.RunEvent_End();
        }

        private void OnEnable()
        {
            if (buttonSettings.hideButtonsOnEnable)
                PanelGroup_HideNavigationButtons();
        }

        public void PanelGroup_HideNavigationButtons()
        {
            buttonSettings.SetButtonState_HideAll();
        }

        public void PanelGroup_DisplayNavigationButtons()
        {
            buttonSettings.ApplyButtonSettings();
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