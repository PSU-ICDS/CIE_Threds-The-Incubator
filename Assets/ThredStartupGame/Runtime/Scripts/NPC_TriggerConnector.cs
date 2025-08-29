using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_TriggerConnector : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] NPC_Controller npc;
    [SerializeField] List<GameObject> inZone;
    [SerializeField] bool ignoreRescale;
    [SerializeField] bool useAlternateScaleRef;
    [SerializeField] RectTransform alternateScaleRef;
    [SerializeField] Vector2 rectScalar = Vector2.one;
    [SerializeField] RectTransform rect;

    public bool Active { get => active; set { SetActiveState(value); } }
    public List<GameObject> InZone { get => inZone; }
    public NPC_Controller NPC { get => npc; set { npc = value; } }

    public void FindRectTransform()
    {
        if (rect == null)
        {
            if (gameObject.TryGetComponent<RectTransform>(out RectTransform _rect))
            {
                rect = _rect;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<CharacterController_2D>(out CharacterController_2D _player))
        {
            if (active && npc != null)
            {
                npc.ZoneEvent_PlayerEntered(_player.gameObject);

                if (!inZone.Contains(_player.gameObject))
                    inZone.Add(_player.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<CharacterController_2D>(out CharacterController_2D _player))
        {
            if (active && npc != null)
            {
                npc.ZoneEvent_PlayerExited();
                //npc.ZoneEvent_PlayerEntered(_player.gameObject);

                if (inZone.Contains(_player.gameObject))
                {
                    inZone.Remove(_player.gameObject);
                    inZone.TrimExcess();
                }

                CheckInZoneState();
            }
        }
    }

    public void CheckInZoneState()
    {
        if (npc != null)
            npc.Zones_CheckInZoneStates();

        //if (inZone.Count == 0)
        //{
        //    if (npc != null)
        //        npc.ZoneEvent_PlayerExited();
        //}
    }

    public void Rect_SetTransformSize(float _height, float _width)
    {
        if (rect == null)
            FindRectTransform();


        if (!ignoreRescale)
        {
            if (rect != null)
            {
                if (useAlternateScaleRef)
                {
                    if (alternateScaleRef != null)
                    {
                        float newHeight = alternateScaleRef.rect.height;
                        float newWidth = alternateScaleRef.rect.width;

                        //float newHeight = _height;
                        //float newWidth = _width;

                        if (rectScalar.y >= 1.0f && alternateScaleRef.rect.height != 0.0f)
                            newHeight = (rectScalar.y * alternateScaleRef.rect.height);

                        if (rectScalar.x >= 1.0f && alternateScaleRef.rect.width != 0.0f)
                            newWidth = (rectScalar.x * alternateScaleRef.rect.width);

                        rect.sizeDelta = new Vector2(newWidth, newHeight);
                    }
                }
                else
                {
                    float newHeight = _height;
                    float newWidth = _width;

                    if (rectScalar.y >= 1.0f && _height != 0.0f)
                        newHeight = (rectScalar.y * _height);

                    if (rectScalar.x >= 1.0f && _width != 0.0f)
                        newWidth = (rectScalar.x * _width);

                    rect.sizeDelta = new Vector2(newWidth, newHeight);
                    // rect.sizeDelta = new Vector2(newHeight, newWidth);
                }
            }
        }
    }

    public void SetActiveState(bool _active)
    {
        active = _active;
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
