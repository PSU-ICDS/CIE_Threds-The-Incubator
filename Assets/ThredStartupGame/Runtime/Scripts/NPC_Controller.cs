using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC_Controller : MonoBehaviour
{
    [SerializeField] GameObject npcObj;
    [SerializeField] SpriteChanger_Simple spriteChanger;
    [SerializeField] bool facePlayerWhenTriggered;
    //[SerializeField] bool triggerOnSelected;
    [SerializeField] bool isSelectable;
    [SerializeField] bool isSelected;
    [SerializeField] SelectableTarget selectionTarget;
    GameObject player;
    [SerializeField] bool inZone;
    [SerializeField] Animator animator;

    [Space(15)]
    [SerializeField] GameObject uiPanelObj;
    [SerializeField] bool useHUD;
    [SerializeField] GameObject hudObj;

    [Space(15)]
    [SerializeField] bool triggerZoneActive;
    [SerializeField] GameObject triggerZoneObj;
    [SerializeField] List<GameObject> extraTriggerZones;

    [Space(15)]
    [SerializeField] UnityEvent selectedEvent;
    [SerializeField] UnityEvent deselectedEvent;
    [SerializeField] UnityEvent zoneEvent_Enter;
    [SerializeField] UnityEvent zoneEvent_Exit;

    [Space(15)]
    [SerializeField] bool debug;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        FindNpcObj();
        FindSpriteChanger();
        FindHudObj();
        ConnectToTriggerZones();
        Zone_ResizeTriggerZone();
    }

    void FindNpcObj()
    {
        if (npcObj == null)
            npcObj = gameObject;
    }

    void FindSpriteChanger()
    {
        if (spriteChanger == null)
        {
            if (gameObject.TryGetComponent<SpriteChanger_Simple>(out SpriteChanger_Simple _changer))
            {
                spriteChanger = _changer;
            }
        }
    }

    void FindHudObj()
    {
        if(hudObj == null)
        {
            if (GameObject.FindObjectOfType<Marker_HUD>(true) != null)
                hudObj = GameObject.FindObjectOfType<Marker_HUD>(true).gameObject;
        }
    }

    void ConnectToTriggerZones()
    {
        if (triggerZoneObj != null)
        {
            if (triggerZoneObj.TryGetComponent<NPC_TriggerConnector>(out NPC_TriggerConnector _zone))
            {
                _zone.NPC = this;
                _zone.Active = triggerZoneActive;
            }
        }

        for (int i = 0; i < extraTriggerZones.Count; i++)
        {
            if (extraTriggerZones[i] != null)
            {
                if (extraTriggerZones[i].TryGetComponent<NPC_TriggerConnector>(out NPC_TriggerConnector _zone))
                {
                    _zone.NPC = this;
                    _zone.Active = triggerZoneActive;
                }
            }
        }

    }

    public void NPC_SetActiveState(bool _active)
    {
        if (npcObj != null)
            npcObj.SetActive(_active);
    }

    public void UI_SetPanelActiveState(bool _active)
    {
        if (uiPanelObj != null)
            uiPanelObj.SetActive(_active);
    }

    public void ZoneObj_SetTriggerZoneObjActiveState(bool _active)
    {
        if (triggerZoneObj != null)
            triggerZoneObj.SetActive(_active);

        for (int i = 0; i < extraTriggerZones.Count; i++)
        {
            if (extraTriggerZones[i] != null)
            {
                extraTriggerZones[i].SetActive(_active);
            }
        }
    }

    public void Zone_SetTriggerZoneFunctionalityActiveState(bool _active)
    {
        if (triggerZoneObj != null)
        {
            if (triggerZoneObj.TryGetComponent<NPC_TriggerConnector>(out NPC_TriggerConnector _zone))
            {
                triggerZoneActive = _active;
                _zone.Active = triggerZoneActive;
            }
        }

        for (int i = 0; i < extraTriggerZones.Count; i++)
        {
            if (extraTriggerZones[i] != null)
            {
                if (extraTriggerZones[i].TryGetComponent<NPC_TriggerConnector>(out NPC_TriggerConnector _zone))
                {
                    //triggerZoneActive = _active;
                    _zone.Active = triggerZoneActive;
                }
            }
        }
    }

    public void ZoneEvent_PlayerEntered()
    {
        if (triggerZoneActive)
        {
            zoneEvent_Enter.Invoke();
            inZone = true;

            Animator_SetActiveAnimationState(inZone);
        }

        //if (triggerOnSelected && isSelected)
        //    zoneEvent_Enter.Invoke();
        //else
        //    zoneEvent_Enter.Invoke();
    }

    public void ZoneEvent_PlayerEntered(GameObject _player = null)
    {
        player = _player;

        if (triggerZoneActive)
        {
            if (player != null && facePlayerWhenTriggered)
                Zone_FacePlayerInTriggerZone(player);

            ZoneEvent_PlayerEntered();
            //zoneEvent_Enter.Invoke();
        }

        //if ((triggerOnSelected && isSelected) || !triggerOnSelected)
        //{
        //    if (player != null && facePlayerWhenTriggered)
        //        Zone_FacePlayerInTriggerZone(player);

        //    zoneEvent_Enter.Invoke();
        //}
    }


    public void Zones_CheckInZoneStates()
    {
        bool _inZone = false;

        if (triggerZoneObj != null)
        {
            if (triggerZoneObj.TryGetComponent<NPC_TriggerConnector>(out NPC_TriggerConnector _zone))
            {
                if (_zone.InZone.Count > 0)
                    _inZone = true;
            }
        }

        for (int i = 0; i < extraTriggerZones.Count; i++)
        {
            if (extraTriggerZones[i] != null)
            {
                if (extraTriggerZones[i].TryGetComponent<NPC_TriggerConnector>(out NPC_TriggerConnector _zone))
                {
                    if (_zone.InZone.Count > 0)
                    {
                        _inZone = true;
                        break;
                    }
                }
            }
        }

        if(_inZone != inZone)
        {
            if(!_inZone)
            {
                ZoneEvent_PlayerExited();
            }
        }

    }

    public void ZoneEvent_PlayerExited()
    {
        player = null;
        Selected_SetState(false);
        zoneEvent_Exit.Invoke();
        inZone = false;

        Animator_SetActiveAnimationState(inZone);
    }

    public void Zone_FacePlayerInTriggerZone(GameObject _player)
    {
        if (_player != null)
        {
            Vector3 delta = _player.transform.position - gameObject.transform.position;
            //Vector3 delta = gameObject.transform.position - _player.transform.position;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x <= 0)
                    Sprite_ApplyNewSprite("Left");
                else
                    Sprite_ApplyNewSprite("Right");
            }
            else
            {
                if (delta.y <= 0)
                    Sprite_ApplyNewSprite("Down");
                else
                    Sprite_ApplyNewSprite("Up");
            }
        }
    }

    public void Zone_ResizeTriggerZone()
    {
        if (gameObject.TryGetComponent<RectTransform>(out RectTransform _rect))
        {
            //_zone.Rect_SetTransformSize(_rect.rect.height, _rect.rect.width);

            if (triggerZoneObj != null)
            {
                if (triggerZoneObj.TryGetComponent<NPC_TriggerConnector>(out NPC_TriggerConnector _zone))
                {
                    _zone.Rect_SetTransformSize(_rect.rect.height, _rect.rect.width);

                    //if (gameObject.TryGetComponent<RectTransform>(out RectTransform _rect))
                    //{
                    //    _zone.Rect_SetTransformSize(_rect.rect.height, _rect.rect.width);
                    //}
                }
            }

            for (int i = 0; i < extraTriggerZones.Count; i++)
            {
                if (extraTriggerZones[i] != null)
                {
                    if (extraTriggerZones[i].TryGetComponent<NPC_TriggerConnector>(out NPC_TriggerConnector _zone))
                    {
                        _zone.Rect_SetTransformSize(_rect.rect.height, _rect.rect.width);
                    }
                }
            }
        }
    }

    public void Animator_SetActiveAnimationState(bool _state)
    {
        if(animator != null) 
        {
                animator.SetBool("Active", _state);

            //if (_state)
            //    animator.SetTrigger("Active");
            //else
            //    animator.ResetTrigger("Active");
        }
    }

    public void Sprite_ApplyNewSprite(string _spriteChangerName)
    {
        if (spriteChanger != null)
            spriteChanger.Sprite_ApplyByName(_spriteChangerName);
    }

    public void Selected_SetSelectableState(bool _selectable)
    {
        isSelectable = _selectable;
        isSelected = false;

        if (!isSelectable)
        {
            if (selectionTarget != null)
                selectionTarget.Select_SetState(false);

            deselectedEvent.Invoke();
        }

    }

    public void Selected_SetState(bool _isSelected)
    {
        if (isSelectable)
        {
            if (isSelected != _isSelected)
            {
                isSelected = _isSelected;

                if (debug)
                    Debug.Log("NPC Controller: Selected State Change-> Selected: " + isSelected + ", on GameObject: " + gameObject.name);

                Selected_RunSelectionEvents();
            }
        }
        else
        {
            isSelected = false;
            if (selectionTarget != null)
                selectionTarget.Select_SetState(false);
        }
    }

    public void Selected_RunSelectionEvents()
    {
        if (isSelectable)
        {
            if (isSelected)
            {
                if (facePlayerWhenTriggered)
                    Zone_FacePlayerInTriggerZone(player);

                selectedEvent.Invoke();
            }
            else
                deselectedEvent.Invoke();

            HUD_UpdateState(isSelected);
        }
    }

    void HUD_UpdateState(bool _state)
    {
        if(useHUD)
        {
            if (hudObj == null)
                FindHudObj();

            if (hudObj != null)
                hudObj.SetActive(_state);
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
