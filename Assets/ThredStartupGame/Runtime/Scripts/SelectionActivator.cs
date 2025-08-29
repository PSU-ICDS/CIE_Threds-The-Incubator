using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionActivator : MonoBehaviour
{
    [SerializeField] CharacterController_2D player;
    [SerializeField] RectTransform rect;
    RectTransform rect_Player;
    [SerializeField] bool squareSelector;
    [SerializeField] PlacementType placement;

    [Space(15)]
    [SerializeField] List<SelectableTarget> targets;

    [Space(15)]
    [SerializeField] bool debug;
    [SerializeField] bool debug_PlaceLeft;
    [SerializeField] bool debug_PlaceRight;
    [SerializeField] bool debug_PlaceUp;
    [SerializeField] bool debug_PlaceDown;
    [SerializeField] bool debug_SelectTarget;

    public enum PlacementType { NONE, LEFT, RIGHT, UP, DOWN }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        FindRectTransform();
        FindPlayer();
    }

    public void FindPlayer()
    {
        if (player == null)
        {
            if (GameObject.FindObjectOfType<CharacterController_2D>() != null)
            {
                player = GameObject.FindObjectOfType<CharacterController_2D>();               
            }
        }

        if(player != null)
        {
            if (player.gameObject.TryGetComponent<RectTransform>(out RectTransform _rect))
            {
                rect_Player = _rect;
                Rect_SetSelectorSize(rect_Player.rect.height, rect_Player.rect.width);
            }
            else
                rect_Player = null;
        }
    }

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

    public void Rect_SetSelectorSize(float _height, float _width)
    {
        if (rect == null)
            FindRectTransform();

        if (rect != null)
        {
            if (squareSelector)
            {
                if (_width < _height)
                    rect.sizeDelta = new Vector2(_width, _width);
                else
                    rect.sizeDelta = new Vector2(_height, _height);

            }
            else
                rect.sizeDelta = new Vector2(_width, _height);
        }
    }

    public void Rect_PlaceSelector(string _placementType)
    {
        switch (_placementType.ToUpper())
        {
            case "NONE":
                placement = PlacementType.NONE;
                break;
            case "LEFT":
                placement = PlacementType.LEFT;
                break;
            case "RIGHT":
                placement = PlacementType.RIGHT;
                break;
            case "UP":
                placement = PlacementType.UP;
                break;
            case "DOWN":
                placement = PlacementType.DOWN;
                break;
            default:
                break;
        }

        Selector_UpdatePlacement();
    }

    public void Rect_PlaceSelector(PlacementType _placement)
    {
        placement = _placement;
        Selector_UpdatePlacement();
    }

    void Selector_UpdatePlacement()
    {
        if (rect == null)
            FindRectTransform();

        if (player == null || rect_Player == null)
            FindPlayer();

        if (rect != null && player != null && rect_Player != null)
        {
            float offset_X = 0.0f;
            float offset_Y = 0.0f;

            switch (placement)
            {
                case PlacementType.NONE:
                    break;
                case PlacementType.LEFT:
                    offset_X = -((rect.rect.width * 0.5f) + (rect_Player.rect.width * 0.5f));
                    break;
                case PlacementType.RIGHT:
                    offset_X = (rect.rect.width * 0.5f) + (rect_Player.rect.width * 0.5f);
                    break;
                case PlacementType.UP:
                    offset_Y = (rect.rect.height * 0.5f) + (rect_Player.rect.height * 0.5f);
                    break;
                case PlacementType.DOWN:
                    offset_Y = -((rect.rect.height * 0.5f) + (rect_Player.rect.height * 0.5f));
                    break;
                default:
                    break;
            }

            Vector3 offset = new Vector3(offset_X, offset_Y, gameObject.transform.position.z);
            gameObject.transform.position = player.gameObject.transform.position + offset;

        }
    }

    public void Selector_TriggerSelectionTarget()
    {
        int _index = Selectable_FindClosestTargetIndex();

        if(_index != -1)
        {
            targets[_index].Select_ToggleSelectionState();
        }
    }

    int Selectable_FindClosestTargetIndex()
    {
        int _index = -1;

        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i] != null)
            {
                if (_index == -1)
                    _index = i;
                else
                {
                    if(Vector3.Distance(targets[i].gameObject.transform.position, gameObject.transform.position) < Vector3.Distance(targets[_index].gameObject.transform.position, gameObject.transform.position))
                    {
                        _index = i;
                    }
                }
            }
        }

        return _index;
    }


    // Update is called once per frame
    void Update()
    {
        CheckDebugSettings();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<SelectableTarget>(out SelectableTarget _target))
        {
            if (!targets.Contains(_target))
                targets.Add(_target);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<SelectableTarget>(out SelectableTarget _target))
        {
            if (targets.Contains(_target))
            {
                _target.Selected = false;
                targets.Remove(_target);
                targets.TrimExcess();
            }
        }
    }

    public void CheckDebugSettings()
    {
        if(debug)
        {
            if(debug_PlaceLeft)
            {
                Rect_PlaceSelector(PlacementType.LEFT);
                debug_PlaceLeft = false;
            }

            if (debug_PlaceRight)
            {
                Rect_PlaceSelector(PlacementType.RIGHT);
                debug_PlaceRight = false;
            }

            if (debug_PlaceUp)
            {
                Rect_PlaceSelector(PlacementType.UP);
                debug_PlaceUp = false;
            }

            if (debug_PlaceDown)
            {
                Rect_PlaceSelector(PlacementType.DOWN);
                debug_PlaceDown = false;
            }

            if (debug_SelectTarget)
            {
                Selector_TriggerSelectionTarget();
                debug_SelectTarget = false;
            }

        }
    }

}
