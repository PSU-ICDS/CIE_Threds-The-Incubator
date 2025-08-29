using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_MenuItem : MonoBehaviour
{
    public string menuName;
    public int menuID;
    public GameObject panel;
    public bool autoSkip;
    [Space(15)]
    public UnityEvent startEvent;
    public UnityEvent endEvent;
    public UnityEvent autoSkipEvent;

    private void Awake()
    {
        if (panel == null)
            panel = this.gameObject;
    }

    public void OpenMenu(bool _runEvent)
    {
        if (autoSkip)
            autoSkipEvent.Invoke();
        else
        {
            if (panel != null)
            {
                panel.SetActive(true);

                if (_runEvent)
                    startEvent.Invoke();
            }
        }
    }

    public void CloseMenu(bool _runEvent)
    {
        if (panel != null)
        {
            panel.SetActive(false);

            if (_runEvent)
                endEvent.Invoke();
        }
    }

    public void AutoSkip_SetState(bool _autoSkip)
    {
        autoSkip = _autoSkip;
    }
}
