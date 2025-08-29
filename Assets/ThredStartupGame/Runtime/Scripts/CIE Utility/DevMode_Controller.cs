using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DevMode_Controller : MonoBehaviour
{
    [SerializeField] bool devModeActive;
    [SerializeField] bool usePlayerPrefs;
    [SerializeField] bool autoDisableOnStart;
    bool prevState;
    [Space(15)]
    /// <summary>
    /// When TRUE, Hold the SHIFT,D,E, and V keys simultaneously to activate developer mode.
    /// Ignores keyboard input for this script when set to FALSE
    /// </summary>
    [Tooltip("When TRUE, Hold the SHIFT,D,E, and V keys simultaneously to activate developer mode.\n" +
             "Ignores keyboard input for this script when set to FALSE.")]
    [SerializeField] bool useKeyboardInputActivation;
    [Space(15)]
    [SerializeField] UnityEvent devModeEvent_Active;
    [SerializeField] UnityEvent devModeEvent_Disabled;

    [SerializeField] bool debug_updateDevModeObjs;

    public bool DevModeActive { get => devModeActive; }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
        DevMode_RunEvents();
        //DevObjs_UpdateState();
    }

    private void OnEnable()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        DevMode_CheckKeyboardInput();
        CheckDebugging();
    }

    public void Setup()
    {
        if (autoDisableOnStart)
        {
            DevObjs_SetDevModeActiveState(false);
            DevMode_SaveToPlayerPrefs();
        }

        if (usePlayerPrefs && DevMode_CheckPlayerPrefs())
            DevMode_GetStoredPlayerPrefs();

        DevObjs_UpdateState();
        DevMode_RunEvents(devModeActive);
    }

    public void DevObjs_SetDevModeActiveState(bool _active)
    {
        prevState = devModeActive;
        devModeActive = _active;
        if (usePlayerPrefs)
            DevMode_SaveToPlayerPrefs();

        DevObjs_UpdateState();
        DevMode_RunEvents(devModeActive);
    }

    public void DevObjs_ToggleDevModeState()
    {
        DevObjs_SetDevModeActiveState(!devModeActive);
        //devModeActive = !devModeActive;
        //DevObjs_UpdateState();
    }

    public void DevObjs_UpdateState()
    {
        DevMode_Object[] objs = GameObject.FindObjectsOfType<DevMode_Object>(true);

        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i] != null)
            {
                objs[i].DevMode_SetState(devModeActive);
                if (!devModeActive)
                    objs[i].gameObject.SetActive(false);

                if (devModeActive && objs[i].AutoEnable)
                    objs[i].gameObject.SetActive(true);

            }
        }
    }

    public void DevMode_CheckKeyboardInput()
    {
        if (useKeyboardInputActivation && !devModeActive)
        {
            bool _activate = true;

            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                _activate = false;

            if (!Input.GetKey(KeyCode.D))
                _activate = false;

            if (!Input.GetKey(KeyCode.E))
                _activate = false;

            if (!Input.GetKey(KeyCode.V))
                _activate = false;

            if (_activate)
                DevObjs_SetDevModeActiveState(true);
        }
    }

    public void DevMode_DisableDevMode()
    {
        DevObjs_SetDevModeActiveState(false);
        DevMode_SaveToPlayerPrefs();
    }

    //  Player Prefs Functions          ---------------------------------------------------------------------------------------------
    #region Player Prefs Functions

    /// <summary>
    /// Checks PlayerPrefs for a saved DevMode state
    /// </summary>
    /// <returns>Returns TRUE if a saved DevMode state is found in PlayerPrefs.
    ///          Returns FALSE if no saved DevMode state is found.</returns>
    public bool DevMode_CheckPlayerPrefs()
    {
        return PlayerPrefs.HasKey("DevMode");
    }

    /// <summary>
    /// Pulls the saved DevMode state from PlayerPrefs
    /// </summary>
    public void DevMode_GetStoredPlayerPrefs()
    {
        if (usePlayerPrefs)
            DevObjs_SetDevModeActiveState((PlayerPrefs.GetInt("DevMode") != 0));
        //devModeActive = (PlayerPrefs.GetInt("DevMode") != 0);
    }

    /// <summary>
    /// Saves the DevMode state to PlayerPrefs
    /// </summary>
    public void DevMode_SaveToPlayerPrefs()
    {
        if (usePlayerPrefs)
            PlayerPrefs.SetInt("DevMode", (devModeActive ? 1 : 0));
    }

    /// <summary>
    /// Deletes the DevMode state saved in PlayerPrefs
    /// </summary>
    public void DevMode_ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("DevMode");
    }

    #endregion Player Prefs Functions

    public void DevMode_RunEvents(bool _state)
    {
        if (_state != prevState)
        {
            if (devModeActive)
                devModeEvent_Active.Invoke();
            else
                devModeEvent_Disabled.Invoke();
        }
    }

    public void DevMode_RunEvents()
    {
        if (devModeActive)
            devModeEvent_Active.Invoke();
        else
            devModeEvent_Disabled.Invoke();
    }

    public void CheckDebugging()
    {
        if (debug_updateDevModeObjs)
        {
            if (usePlayerPrefs)
                DevMode_SaveToPlayerPrefs();

            DevObjs_UpdateState();
            debug_updateDevModeObjs = false;
        }
    }

}
