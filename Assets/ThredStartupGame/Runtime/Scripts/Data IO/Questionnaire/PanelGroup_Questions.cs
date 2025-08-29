/*
 * Name: PanelGroup_Questions
 * Project: Butterfly Experience Project (v2)
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 08/29/2023
 * Description: This script manages and navigates group of question panels
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
    public class PanelGroup_Questions : MonoBehaviour
    {
        //  Variables and Properties    -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Variables and Properties

        [SerializeField] List<GameObject> panels;
        [SerializeField] bool includeInDataSet;
        [SerializeField] int groupNumber;
        [SerializeField] bool autoSetGroupNum;
        [SerializeField] bool autoFindPanels;
        [SerializeField] bool looping;
        [SerializeField] string outPutText;
        [SerializeField] Text printoutTextObj;
        [Space(10)]
        [SerializeField] string multiChoiceDelimiter;
        [Space(10)]
        [SerializeField] QuestionsSettings questionSettings;
        [Space(10)]
        [SerializeField] DataOutputSettings dataSettings;
        [Space(10)]
        [SerializeField] GroupEvents eventSet;
        [Space(10)]
        [SerializeField] DebugSettings debugSettings;
        int current;

        public bool IncludeInDataSet { get { return includeInDataSet; } set { includeInDataSet = value; } }
        public int GroupNumber { get { return groupNumber; } set { groupNumber = value; } }
        public bool AddToDataSet { get { return includeInDataSet; } set { includeInDataSet = value; } }

        #endregion

        //  Container Classes           -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Container Classes

        [Serializable]
        public class QuestionsSettings
        {
            public bool useOverrides;
            public bool overrideInstructions;
            public string instructionsText;
            public bool overrideScaleInfo;
            public string scaleInfoText;
            public bool overrideQuestions;
            public List<string> questions;
            public bool overrideScaleText;
            public List<string> scaleTextStrings;
            [Header("Override Settings")]
            public bool overrideScaleFontSize_Numbers;
            public int fontSize_Numbers;
            public bool overrideScaleFontSize_Text;
            public int fontSize_Text;
            public bool overrideScaleLabelFontSize_Text;
            public int labelFontSize_Text;
        }

        [Serializable]
        public class DataOutputSettings
        {
            public DataOutput_Questionnaire dataHolder;
            public string dataSetName;
            public bool autoSet_TotalResponseNum;
            public int totalResponseNum;

            public void Setup()
            {
                if (dataHolder == null)
                {
                    if (GameObject.FindObjectOfType<DataOutput_Questionnaire>() != null)
                        dataHolder = GameObject.FindObjectOfType<DataOutput_Questionnaire>();
                }
            }

            public void SetTotalResponseNum(int _totalResponseNum)
            {
                if (autoSet_TotalResponseNum)
                    totalResponseNum = _totalResponseNum;
            }
        }

        [Serializable]
        public class GroupEvents
        {
            public UnityEvent startEvent;
            public UnityEvent endEvent;

            public void RunEvent_Start()
            {
                startEvent.Invoke();
            }

            public void RunEvent_End()
            {
                endEvent.Invoke();
            }
        }

        [Serializable]
        public class DebugSettings
        {
            public bool debug_nextPanel;
            public bool debug_prevPanel;
        }

        #endregion

        //  Unity Functions             -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Unity Functions

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            if (debugSettings.debug_nextPanel)
            {
                Panels_Next();
                debugSettings.debug_nextPanel = false;
            }

            if (debugSettings.debug_prevPanel)
            {
                Panels_Previous();
                debugSettings.debug_prevPanel = false;
            }
        }

        #endregion

        //  Panel State Functions       -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Panel State Functions

        public void Panels_SetActivePanel(int _index)
        {
            _index = BoundsCheck(_index);

            if (_index >= 0 && _index < panels.Count)
            {
                if (panels[_index] != null)
                {
                    Panels_CloseAll();
                    current = _index;
                    panels[current].SetActive(true);
                }
            }
        }

        public void Panels_CloseAll()
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i] != null)
                    panels[i].SetActive(false);
            }
        }

        public void Panels_Next()
        {
            Panels_SetActivePanel(current + 1);
        }

        public void Panels_Previous()
        {
            Panels_SetActivePanel(current - 1);
        }


        #endregion

        //  Output String Functions     -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Output String Functions

        public void AddToOutputText(string _text)
        {
            outPutText = outPutText + _text + "\n";
        }

        public void PrintOutputText(bool _append)
        {
            if (printoutTextObj != null)
            {
                if (_append)
                    printoutTextObj.text = printoutTextObj.text + outPutText;
                else
                    printoutTextObj.text = outPutText;
            }
        }

        void SetupOutputText()
        {
            outPutText = "Question Group: " + groupNumber + "\n";
        }

        #endregion

        //  Data Output Functions       -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Data Output Functions

        public void CountQuestionPanels()
        {
            int questionPanelCount = 0;

            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i] != null)
                {
                    if (panels[i].TryGetComponent<Panel_QuestionSettings>(out Panel_QuestionSettings _settings))
                    {
                        _settings.PanelGroup = this;

                        if (_settings.isQuestionPanel)
                        {
                            _settings.QuestionNumber = questionPanelCount;
                            questionPanelCount++;
                        }
                    }
                }
            }

            dataSettings.SetTotalResponseNum(questionPanelCount);
        }

        public void Data_SetupDataSet()
        {
            if (dataSettings.dataHolder == null)
                dataSettings.Setup();

            if (dataSettings.dataHolder != null)
            {
                //Data_CheckAndAddSet();
                if (includeInDataSet)
                {
                    dataSettings.dataHolder.Data_AddNewSet(groupNumber, dataSettings.dataSetName);
                    dataSettings.dataHolder.SetInfo_UpdateSet(groupNumber, dataSettings.dataSetName, dataSettings.totalResponseNum);
                }
                else
                    dataSettings.dataHolder.Data_RemoveSetByID(groupNumber);
            }

        }

        public void Data_SetResponse(int _questionNum, string _response)
        {
            if (dataSettings.dataHolder == null)
                dataSettings.Setup();

            if (dataSettings.dataHolder != null)
            {
                //Data_CheckAndAddSet();
                if (includeInDataSet)
                {
                    dataSettings.dataHolder.Data_AddNewSet(groupNumber, dataSettings.dataSetName);
                    dataSettings.dataHolder.Data_SetResponse(groupNumber, _questionNum, _response);
                }
                else
                    dataSettings.dataHolder.Data_RemoveSetByID(groupNumber);
            }

        }

        //public void Data_SortGroups()
        //{
        //    if (dataSettings.dataHolder == null)
        //        dataSettings.Setup();

        //    if (dataSettings.dataHolder != null)
        //    {
        //        dataSettings.dataHolder.Data_SortSetsByID();
        //    }
        //}

        //public void Data_CheckAndAddSet()
        //{
        //    if (dataSettings.dataHolder != null)
        //    {
        //        if (!dataSettings.dataHolder.Data_ContainsSetByID(groupNumber))
        //            dataSettings.dataHolder.Data_AddNewSet(groupNumber, dataSettings.dataSetName);
        //    }
        //}

        #endregion

        //  Panel Setup Functions       -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Panel Setup Functions

        public void Setup()
        {
            if (autoSetGroupNum)
                FindAvailableGroupNumber();

            if (autoFindPanels)
                Panels_AutoFindPanels();

            Panels_SetupSettings();
            Panels_SetupQuestions();
            Panels_SetupScaleValStrings();
            Panels_SetupScaleValFontSize_Numbers();
            Panels_SetupScaleValFontSize_Text();
            Panels_SetupScaleValLabelFontSize_Text();

            SetupOutputText();

            Panels_SetActivePanel(current);

            CountQuestionPanels();
            Data_SetupDataSet();

            GetDelimiterString();
            UpdateMiltuChoiceDelimiter();
        }

        void FindAvailableGroupNumber()
        {
            if (GameObject.FindObjectsOfType<PanelGroup_Questions>() != null)
            {
                PanelGroup_Questions[] qPanels = GameObject.FindObjectsOfType<PanelGroup_Questions>();

                int newGroupNum = -1;

                for (int i = 0; i < qPanels.Length; i++)
                {
                    if (qPanels[i] != null)
                    {
                        if (qPanels[i].groupNumber >= newGroupNum)
                            newGroupNum = qPanels[i].groupNumber + 1;
                    }
                }

                if (newGroupNum == -1)
                    newGroupNum = 0;

                groupNumber = newGroupNum;
            }
        }

        int BoundsCheck(int _index)
        {
            if (_index >= panels.Count)
            {
                if (looping)
                    _index = 0;
                else
                    _index = panels.Count - 1;
            }

            if (_index < 0)
            {
                if (looping)
                    _index = panels.Count - 1;

                else
                    _index = 0;
            }

            return _index;
        }


        void Panels_AutoFindPanels()
        {
            if (panels != null)
                panels.Clear();
                //panels.RemoveAll(null);

            Panel_QuestionSettings[] panelSettings = GetComponentsInChildren<Panel_QuestionSettings>(true);

            for (int i = 0; i < panelSettings.Length; i++)
            {
                if (panelSettings[i] != null)
                {
                    if (!panels.Contains(panelSettings[i].gameObject))
                        panels.Add(panelSettings[i].gameObject);
                }
            }
        }

        void Panels_SetupSettings()
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i] != null)
                {
                    if (panels[i].TryGetComponent<Panel_QuestionSettings>(out Panel_QuestionSettings _settings))
                    {
                        _settings.SetLabelText("Question Group " + groupNumber);
                        _settings.Setup();
                    }
                }
            }
        }

        void Panels_SetupQuestions()
        {
            if (questionSettings.useOverrides)         //(questionSettings.overrideQuestions)
            {
                int qNum = 0;

                for (int i = 0; i < panels.Count; i++)
                {
                    if (panels[i] != null)
                    {
                        if (panels[i].TryGetComponent<Panel_QuestionSettings>(out Panel_QuestionSettings _qSettings))
                        {
                            if (_qSettings.isQuestionPanel)
                            {
                                if (qNum >= 0 && qNum < questionSettings.questions.Count)
                                {
                                    if(questionSettings.overrideInstructions)
                                    _qSettings.SetInstructionsText(questionSettings.instructionsText);

                                    if(questionSettings.overrideScaleInfo)
                                    _qSettings.SetScaleInfoText(questionSettings.scaleInfoText);

                                    if(questionSettings.overrideQuestions)
                                    _qSettings.SetQuestionText(questionSettings.questions[qNum]);
                                    qNum++;
                                }
                            }
                        }
                    }
                }
            }
        }

        void Panels_SetupScaleValStrings()
        {
            if (questionSettings.overrideScaleText)
            {
                for (int i = 0; i < panels.Count; i++)
                {
                    if (panels[i] != null)
                    {
                        if (panels[i].TryGetComponent<Panel_QuestionSettings>(out Panel_QuestionSettings _settings))
                        {
                            _settings.SetScaleValTextStrings(questionSettings.scaleTextStrings);
                        }
                    }
                }
            }
        }

        void Panels_SetupScaleValFontSize_Text()
        {
            if (questionSettings.overrideScaleFontSize_Text)
            {
                for (int i = 0; i < panels.Count; i++)
                {
                    if (panels[i] != null)
                    {
                        if (panels[i].TryGetComponent<Panel_QuestionSettings>(out Panel_QuestionSettings _settings))
                        {
                            _settings.SetScaleVal_TextFontSize(questionSettings.fontSize_Text);
                        }
                    }
                }
            }
        }

        void Panels_SetupScaleValFontSize_Numbers()
        {
            if (questionSettings.overrideScaleFontSize_Numbers)
            {
                for (int i = 0; i < panels.Count; i++)
                {
                    if (panels[i] != null)
                    {
                        if (panels[i].TryGetComponent<Panel_QuestionSettings>(out Panel_QuestionSettings _settings))
                        {
                            _settings.SetScaleVal_NumbersFontSize(questionSettings.fontSize_Numbers);
                        }
                    }
                }
            }
        }

        void Panels_SetupScaleValLabelFontSize_Text()
        {
            if (questionSettings.overrideScaleLabelFontSize_Text)
            {
                for (int i = 0; i < panels.Count; i++)
                {
                    if (panels[i] != null)
                    {
                        if (panels[i].TryGetComponent<Panel_QuestionSettings>(out Panel_QuestionSettings _settings))
                        {
                            _settings.SetScaleVal_LabelTextFontSize(questionSettings.labelFontSize_Text);
                        }
                    }
                }
            }
        }

        void GetDelimiterString()
        {
            if (dataSettings.dataHolder != null)
                multiChoiceDelimiter = dataSettings.dataHolder.GetDelimiterString();
            else
                multiChoiceDelimiter = ",";
        }

        void UpdateMiltuChoiceDelimiter()
        {
            ScaleValSet[] scaleSets = gameObject.GetComponentsInChildren<ScaleValSet>(true);

            for (int i = 0; i < scaleSets.Length; i++)
            {
                if (scaleSets[i] != null)
                    scaleSets[i].MultiChoice_Delimiter = multiChoiceDelimiter;
            }
        }

        #endregion


        #region Panel Event Functions

        public void RunEvent_Start()
        {
            eventSet.RunEvent_Start();
        }

        public void RunEvent_End()
        {
            eventSet.RunEvent_End();
        }

        #endregion


    }
}