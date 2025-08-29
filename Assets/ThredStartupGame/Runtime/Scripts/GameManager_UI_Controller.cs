using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_UI_Controller : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] List<ManagedUI> uis;
    int current = -1;
    //[SerializeField] GameObject ui_Welcome;
    //[SerializeField] GameObject ui_Questionnaire;
    //[SerializeField] GameObject ui_Tutorial;

    [Serializable]
    public class ManagedUI
    {
        public string name;
        public bool isUsed;
        public GameObject uiObject;
        public int orderNum;

        public void Activate()
        {
            if (isUsed && uiObject != null)
                uiObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (uiObject != null)
                uiObject.SetActive(false);
        }
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
        if (active)
        {
            HideUnusedUIs();
            FindFirstUI();
        }
    }

    public void HideUnusedUIs()
    {
        for (int i = 0; i < uis.Count; i++)
        {
            if (uis[i] != null)
            {
                if (!uis[i].isUsed)
                    uis[i].Deactivate();
            }
        }
    }

    public void FindFirstUI()
    {
        int _index = -1;
        int order = -1;

        for (int i = 0; i < uis.Count; i++)
        {
            if (uis[i] != null)
            {
                if (uis[i].isUsed && uis[i].uiObject != null)
                {
                    if (_index == -1)
                    {
                        order = uis[i].orderNum;
                        _index = i;
                    }
                    else
                    {
                        if (order > uis[i].orderNum)
                        {
                            order = uis[i].orderNum;
                            _index = i;
                        }
                    }

                }
            }
        }

        //Debug.Log("UI Manager => First UI Index: " + _index.ToString());

        if (_index != -1)
            uis[_index].Activate();

    }

    public void ActivateNextUI()
    {
        int _index = FindNextValidIndex();

        if (_index != -1)
        {
            if (current >= 0 && current < uis.Count)
                uis[current].Deactivate();

            uis[_index].Activate();
        }
    }

    public int FindNextValidIndex()
    {
        int _index = -1;
        int order = 50000;

        for (int i = 0; i < uis.Count; i++)
        {
            if (uis[i] != null)
            {
                if (uis[i].isUsed && uis[i].uiObject != null)
                {
                    if (uis[i].orderNum > uis[current].orderNum && uis[i].orderNum < order)
                    {
                        order = uis[i].orderNum;
                        _index = i;
                    }
                }
            }
        }

        return _index;
    }

    public int FindPreviousValidIndex()
    {
        int _index = -1;
        int order = -50000;

        for (int i = 0; i < uis.Count; i++)
        {
            if (uis[i] != null)
            {
                if (uis[i].isUsed && uis[i].uiObject != null)
                {
                    if (uis[i].orderNum < uis[current].orderNum && uis[i].orderNum > order)
                    {
                        order = uis[i].orderNum;
                        _index = i;
                    }
                }
            }
        }

        return _index;
    }

}
