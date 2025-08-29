using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSet_BuiltTarget : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] bool runOnStart;
    [SerializeField] bool runOnEnable;

    [SerializeField] UnityEvent platformEvent_WindowsPlayer;
    [SerializeField] UnityEvent platformEvent_WindowsEditor;
    [SerializeField] UnityEvent platformEvent_Android;
    [SerializeField] UnityEvent platformEvent_WebGL;

    // Start is called before the first frame update
    void Start()
    {
        if (active && runOnStart)
            CheckBuildPlatform();
    }

    private void OnEnable()
    {
        if (active && runOnEnable)
            CheckBuildPlatform();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}


    public void CheckBuildPlatform()
    {
        switch (Application.platform)
        {      
            case RuntimePlatform.WindowsPlayer:
                PlatformEvent_WindowsPlayer();
                break;  
            case RuntimePlatform.WindowsEditor:
                PlatformEvent_WindowsEditor();
                break;              
            case RuntimePlatform.Android:
                PlatformEvent_Android();
                break;        
            case RuntimePlatform.WebGLPlayer:
                PlatformEvent_WebGL();
                break;          
            default:
                break;
        }
    }

    public void PlatformEvent_WindowsPlayer()
    {
        Debug.Log("EventSet_BuildTarget => Current Platform: WindowsPlayer, running build target event.");
        platformEvent_WindowsPlayer.Invoke();
    }

    public void PlatformEvent_WindowsEditor()
    {
        Debug.Log("EventSet_BuildTarget => Current Platform: WindowsEditor, running build target event.");
        platformEvent_WindowsEditor.Invoke();
    }

    public void PlatformEvent_Android()
    {
        Debug.Log("EventSet_BuildTarget => Current Platform: Android, running build target event.");
        platformEvent_Android.Invoke();
    }

    public void PlatformEvent_WebGL()
    {
        Debug.Log("EventSet_BuildTarget => Current Platform: WebGL, running build target event.");
        platformEvent_WebGL.Invoke();
    }

}
