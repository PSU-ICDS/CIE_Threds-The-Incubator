using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMode_Object : MonoBehaviour
{
    [SerializeField] bool devModeState;
    [SerializeField] bool autoEnable;
    //[SerializeField] GameObject targetObj;

    public bool DevModeState { get=>devModeState; set {devModeState = value; } }
    public bool AutoEnable { get=>autoEnable; }


    void UpdateDevModeFromController()
    {
        DevMode_Controller controller = GameObject.FindObjectOfType<DevMode_Controller>(true);
        if (controller != null)
        {
            DevMode_SetState(controller.DevModeActive);
        }
    }

    public void DevMode_SetState(bool _state)
    {
        devModeState = _state;
        if (!devModeState)
        {
            gameObject.SetActive(false);
        }

        if(devModeState && autoEnable)
        {
            gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateDevModeFromController();
    }

    private void OnEnable()
    {
        UpdateDevModeFromController();
    }
   
}
