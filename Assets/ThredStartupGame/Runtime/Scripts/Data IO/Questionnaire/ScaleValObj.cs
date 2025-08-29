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
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Code created for XR Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_XR_Utility
{
    public class ScaleValObj : MonoBehaviour
    {
        public enum ObjType { NONE, BUTTON, SLIDER, INPUT_FIELD }
        [SerializeField] ObjType type;
        [SerializeField] bool multiChoice;
        [SerializeField] ScaleValSet valSet;
        [SerializeField] ResponseSettings responseSettings;

        [SerializeField] TMP_Text number;
        [SerializeField] TMP_Text text;
        [SerializeField] TMP_Text label;
        [SerializeField] GameObject image;
        [Space(10)]
        [SerializeField] bool isTrueFalse;
        [SerializeField] bool trueFalseValue;
        [Space(10)]
        [SerializeField] SliderSettings sliderSettings;
        [Space(10)]
        [SerializeField] ButtonColorSettings buttonColorSettings;
        [SerializeField] bool debug_sendResponse;

        public enum ResponseOverrideType { NONE, NUMBER, TEXT, LABEL, SLIDER }

        public Panel_QuestionSettings QuestionManager { get { return responseSettings.questionManager; } set { responseSettings.questionManager = value; } }
        public ScaleValSet ValueSet { get => valSet; set { valSet = value; } }
        public bool MultiChoice { get => multiChoice; set { multiChoice = value; } }

        public string Response { get => responseSettings.response; set { responseSettings.response = value; } }
        public bool IsTrueFalse { get=>isTrueFalse; set {isTrueFalse = value; } }
        public bool TrueFalseValue { get=>trueFalseValue; set {trueFalseValue = value; } }


        //  Container Classes           ----------------------------------------------------------------------------------------------------------------------------------
        #region Container Classes

        [Serializable]
        public class SliderSettings
        {
            public Slider slider;

            public enum OutputFormat { NONE, INT, DECIMAL_1, DECIMAL_2, DECIMAL_3, DECIMAL_4, DECIMAL_5, DECIMAL_6, DECIMAL_7 }
            public OutputFormat responseFormat;

            [Header("Slider Min")]
            public TMP_Text number_SliderMin;
            public TMP_Text text_SliderMin;
            public TMP_Text label_SliderMin;

            [Header("Slider Max")]
            public TMP_Text number_SliderMax;
            public TMP_Text text_SliderMax;
            public TMP_Text label_SliderMax;

            public string Slider_FormatResponse(float _inputVal)
            {
                string _response = _inputVal.ToString();

                switch (responseFormat)
                {
                    case OutputFormat.NONE:
                        _response = _inputVal.ToString();
                        break;
                    case OutputFormat.INT:
                        _response = _inputVal.ToString("F0");
                        break;
                    case OutputFormat.DECIMAL_1:
                        _response = _inputVal.ToString("F1");
                        break;
                    case OutputFormat.DECIMAL_2:
                        _response = _inputVal.ToString("F2");
                        break;
                    case OutputFormat.DECIMAL_3:
                        _response = _inputVal.ToString("F3");
                        break;
                    case OutputFormat.DECIMAL_4:
                        _response = _inputVal.ToString("F4");
                        break;
                    case OutputFormat.DECIMAL_5:
                        _response = _inputVal.ToString("F5");
                        break;
                    case OutputFormat.DECIMAL_6:
                        _response = _inputVal.ToString("F6");
                        break;
                    case OutputFormat.DECIMAL_7:
                        _response = _inputVal.ToString("F7");
                        break;
                    default:
                        _response = _inputVal.ToString();
                        break;
                }

                return _response;
            }
        }

        [Serializable]
        public class ResponseSettings
        {
            public ResponseOverrideType overrideType;
            public string response;
            public Panel_QuestionSettings questionManager;
        }

        [Serializable]
        public class ButtonColorSettings
        {
            public bool useButtonColors;
            public Color colorSelected;
            public Color colorDeselected;
            public Button button;
            public Image buttonImage;

            public void SetupButtonColor()
            {
                if(useButtonColors && button != null)
                {
                    ColorBlock cb = button.colors;
                    cb.selectedColor = colorDeselected;
                    button.colors = cb;
                }
            }

            public void ApplyColorBySelectedState(bool _selected)
            {
                if (_selected)
                    ApplyColor_Selected();
                else
                    ApplyColor_Deselected();
            }

            public void ApplyColor_Selected()
            {
                if (useButtonColors && buttonImage != null)
                    buttonImage.color = colorSelected;
            }

            public void ApplyColor_Deselected()
            {
                if (useButtonColors && buttonImage != null)
                    buttonImage.color = colorDeselected;
            }
        }

        #endregion

        //  Unity Functions                 ----------------------------------------------------------------------------------------------------------------------------------
        #region Unity Functions

        private void Update()
        {
            if (debug_sendResponse)
            {
                Response_SendResponse();
                debug_sendResponse = false;
            }
        }

        #endregion

        //  Active State Functions          ----------------------------------------------------------------------------------------------------------------------------------
        #region Active State Functions

        public void SetActive_Number(bool _state)
        {
            if (number != null)
                number.gameObject.SetActive(_state);
        }

        public void SetActive_Text(bool _state)
        {
            if (text != null)
                text.gameObject.SetActive(_state);
        }

        public void SetActive_Label(bool _state)
        {
            if (label != null)
                label.gameObject.SetActive(_state);
        }

        public void SetActive_Image(bool _state)
        {
            if (image != null)
                image.SetActive(_state);
        }

        #endregion

        //  Text Update Functions           ----------------------------------------------------------------------------------------------------------------------------------
        #region Text Update Functions

        public void SetNumber(string _val)
        {
            if (number != null)
            {
                number.text = _val;
                ResponseOverride_Update();
            }
        }

        public void SetText(string _text)
        {
            if (text != null)
            {
                text.text = _text;
                ResponseOverride_Update();
            }
        }

        public void SetLabelText(string _text)
        {
            if (label != null)
            {
                label.text = _text;
                ResponseOverride_Update();
            }
        }

        #endregion

        //  Font Size Functions             ----------------------------------------------------------------------------------------------------------------------------------
        #region Font Size Functions

        public void SetTextSize(int _size)
        {
            if (text != null)
                text.fontSize = _size;

            if (type == ObjType.SLIDER)
            {
                if (sliderSettings.text_SliderMin != null)
                    sliderSettings.text_SliderMin.fontSize = _size;

                if (sliderSettings.text_SliderMax != null)
                    sliderSettings.text_SliderMax.fontSize = _size;
            }
        }

        public void SetLabelTextSize(int _size)
        {
            if (label != null)
                label.fontSize = _size;

            if (type == ObjType.SLIDER)
            {
                if (sliderSettings.label_SliderMin != null)
                    sliderSettings.label_SliderMin.fontSize = _size;

                if (sliderSettings.label_SliderMax != null)
                    sliderSettings.label_SliderMax.fontSize = _size;
            }
        }

        public void SetNumberSize(int _size)
        {
            if (number != null)
                number.fontSize = _size;

            if (type == ObjType.SLIDER)
            {
                if (sliderSettings.number_SliderMin != null)
                    sliderSettings.number_SliderMin.fontSize = _size;

                if (sliderSettings.number_SliderMax != null)
                    sliderSettings.number_SliderMax.fontSize = _size;
            }
        }

        #endregion

        //  Response Functions              ----------------------------------------------------------------------------------------------------------------------------------
        #region Response Functions

        public void ResponseOverride_SetState(ResponseOverrideType _type)
        {
            responseSettings.overrideType = _type;
            ResponseOverride_Update();
        }

        public void ResponseOverride_Update()
        {
            switch (responseSettings.overrideType)
            {
                case ResponseOverrideType.NONE:
                    break;
                case ResponseOverrideType.NUMBER:
                    if (number != null)
                        responseSettings.response = number.text;
                    break;
                case ResponseOverrideType.TEXT:
                    if (text != null)
                        responseSettings.response = text.text;
                    break;
                case ResponseOverrideType.LABEL:
                    if (label != null)
                        responseSettings.response = label.text;
                    break;
                case ResponseOverrideType.SLIDER:
                    if (sliderSettings.slider != null)
                    {
                        responseSettings.response = sliderSettings.Slider_FormatResponse(sliderSettings.slider.value);
                        //responseSettings.response = sliderSettings.slider.value.ToString();

                    }
                    break;
                //case ResponseOverrideType.MULTI_CHOICE:
                //    if (text != null)
                //        responseSettings.response = text.text;
                //    break;
                default:
                    break;
            }

            //if (isTrueFalse && !trueFalseValue)
            //    responseSettings.response = "null";

        }

        public void Response_SendResponse()
        {
            Response_ToggleTrueFalseValue();

            ResponseOverride_Update();

            if (multiChoice && valSet != null)
            {
                valSet.MultiChoice_CollectAndSendResponses(Response);
            }
            else
            {
                if (responseSettings.questionManager != null)
                    responseSettings.questionManager.Response_UpdateResponseData(responseSettings.response);
            }
        }

        public void Response_ToggleTrueFalseValue()
        {
            if (isTrueFalse)
            {
                trueFalseValue = !trueFalseValue;
                responseSettings.response = trueFalseValue.ToString();

                buttonColorSettings.useButtonColors = true;
                buttonColorSettings.SetupButtonColor();
                buttonColorSettings.ApplyColorBySelectedState(trueFalseValue);
            }
        }

        #endregion
    }
}