using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Controller : MonoBehaviour
{
    [SerializeField] bool useHUD;
    [SerializeField] GameObject hud_Total;
    [Space(10)]
    [SerializeField] GameObject hudItem_Resources;
    [SerializeField] GameObject hudItem_Team;
    [SerializeField] GameObject hudItem_Device;
    [SerializeField] GameObject hudItem_CustomerDisc;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        if(!useHUD)
        {
            HUD_SetItemState_Total(false);
        }
    }

    public void HUD_SetItemState_All(bool _state)
    {
        HUD_SetItemState_Total(_state);
        HUD_SetItemState_Resources(_state);
        HUD_SetItemState_Team(_state);
        HUD_SetItemState_Device(_state);
        HUD_SetItemState_CustomerDiscovery(_state);
    }

    public void HUD_SetItemState_Total(bool _state)
    {
        if (hud_Total != null)
            hud_Total.SetActive(_state);
    }
     

    public void HUD_SetItemState_Resources(bool _state)
    {
        if (hudItem_Resources != null)
            hudItem_Resources.SetActive(_state);
    }

    public void HUD_SetItemState_Team(bool _state)
    {
        if (hudItem_Team != null)
            hudItem_Team.SetActive(_state);
    }

    public void HUD_SetItemState_Device(bool _state)
    {
        if (hudItem_Device != null)
            hudItem_Device.SetActive(_state);
    }

    public void HUD_SetItemState_CustomerDiscovery(bool _state)
    {
        if (hudItem_CustomerDisc != null)
            hudItem_CustomerDisc.SetActive(_state);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
