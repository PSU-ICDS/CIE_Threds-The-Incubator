using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSet_Named : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] List<NamedEvent> events;
    [SerializeField] bool debug;

    public bool ActiveState { get => active; set { SetActiveState(value); } }

    [Serializable]
    public class NamedEvent
    {
        public string name;
        public bool isUsable;
        public UnityEvent enableEvent;
        public UnityEvent disableEvent;

        public void RunEvent_Enable()
        {
            if (isUsable)
                enableEvent.Invoke();
        }

        public void RunEvent_Disable()
        {            
            Debug.Log("Event=> Disable Event named: " + name);
            enableEvent.Invoke();
        }
    }


    private void Start()
    {
        if (!active)
            Event_RunDisableEvent_All();
    }

    public void SetActiveState(bool _state)
    {
        active = _state;

        if (!active)
            Event_RunDisableEvent_All();
    }

    public void Event_RunEnableEventByName(string _name)
    {
        if (active)
        {
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i] != null)
                {
                    if (events[i].name.ToLower() == _name.ToLower())
                        events[i].RunEvent_Enable();
                }
            }
        }
    }

    public void Event_RunDisableEventByName(string _name)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i] != null)
            {
                if (events[i].name.ToLower() == _name.ToLower())
                {
                    if (debug)
                        Debug.Log("Event=> Disabling Event named: " + _name);

                    events[i].RunEvent_Disable();
                }
            }
        }
    }

    public void Event_RunEnableEvent_All()
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i] != null)
                events[i].RunEvent_Enable();
        }
    }

    public void Event_RunDisableEvent_All()
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i] != null)
                events[i].RunEvent_Disable();
        }
    }


}
