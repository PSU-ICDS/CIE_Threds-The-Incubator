using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questions_InfoList : MonoBehaviour
{
    [SerializeField] int groupNum;
    [SerializeField] List<QuestionItem> questions;

    public enum QuestionTypes {NONE, TRUE_FALSE, INPUT_FIELD, SLIDER, MULTI_SELECT_SINGLE, MULTI_SELECT_MULTI }

    [Serializable]
    public class QuestionItem
    {
        public string name;
        public bool active;
        public int id_Group;
        public int id_QuestionNum;
        public QuestionTypes questionType;
        public string instructionsInfo;
        public string question;
        public List<string> answers;
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Setup()
    {
        Questions_Setup();
    }

    public void Questions_Setup()
    {
        int activeNum = 1;

        for (int i = 0; i < questions.Count; i++)
        {
            if(questions[i] != null)
            {
                questions[i].id_Group = groupNum;
                if(questions[i].active)
                {
                    questions[i].id_QuestionNum = activeNum;
                    questions[i].name = "Q" + groupNum.ToString() + "_" + activeNum.ToString();

                    activeNum++;
                }
            }
        }
    }

}
