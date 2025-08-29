using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectableTarget : MonoBehaviour
{
    [SerializeField] bool isSelected;

    [Space(15)]
    [SerializeField] UnityEvent selectedEvent;
    [SerializeField] UnityEvent deselectedEvent;

    [Space(15)]
    [SerializeField] bool debug;

    public bool Selected { get => isSelected; set { Select_SetState(value); } }


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void Select_SetState(bool _selected)
    {
        if (_selected != isSelected)
        {
            isSelected = _selected;
            Selection_RunEvent();
        }
    }

    public void Select_ToggleSelectionState()
    {
        isSelected = !isSelected;

        if (debug)
            Debug.Log("SelectableTarget: State Change-> Selected: " + isSelected + ", on GameObject: " + gameObject.name);

        Selection_RunEvent();
    }

    public void Selection_RunEvent()
    {
        if (isSelected)
            selectedEvent.Invoke();
        else
            deselectedEvent.Invoke();
    }

}
