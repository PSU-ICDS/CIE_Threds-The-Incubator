using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController_2D : MonoBehaviour
{
    public enum MovementType { NONE, PHYSICS, DIRECT }
    public enum MoveDirectionType { NONE, UP, DOWN, LEFT, RIGHT }

    [SerializeField] GameObject player;
    [SerializeField] MovementType moveType;
    [SerializeField] float speed_Direct;
    [SerializeField] float speed_Physics;
    [SerializeField] bool sprinting;
    [SerializeField] bool sprintingEnabled;
    [SerializeField] float sprintScalar = 2.0f;
    Rigidbody2D rb2D;
    Vector2 moveVec = Vector2.zero;
    bool inputDetected = false;

    [Space(15)]
    [SerializeField] SelectionActivator selector;

    [Space(15)]
    [SerializeField] int roomNum;
    [Space(10)]
    [SerializeField] UnityEvent newRoomEnteredEvent;

    [Space(15)]
    [SerializeField] bool useKeys_Arrows;
    [SerializeField] bool useKeys_WASD;
    [SerializeField] bool inputsPaused;

    [Space(15)]
    [SerializeField] bool autoPilot;
    [SerializeField] bool autoPilot_Moving;
    [SerializeField] GameObject autoPilotTarget;
    [SerializeField] bool autoSetMargin;
    [SerializeField] float autoPilot_MarginRatio = 0.25f;
    [SerializeField] float autoPilot_Margin;
    [SerializeField] UnityEvent autoPilot_Arrived;
    [SerializeField] List<GameObject> autoPilotTargets;
    int targetIndex = 0;

    [Space(15)]
    [SerializeField] bool useScreenSpaceLock;
    [SerializeField] RectTransform parentCanvas;
    //[SerializeField] Canvas parentCanvas;
    [SerializeField] float savedWidth;
    [SerializeField] float savedHeight;
    [SerializeField] Vector2 relativePOS;
    [SerializeField] bool useRelativeSpeed;
    [SerializeField] float relativeSpeedMovementVal;
    [SerializeField] float relativeSpeedCanvasWidthVal;
    [SerializeField] float relativeSpeed;

    [Space(15)]
    [SerializeField] bool useRelativeSpriteScaling;
    //[SerializeField] Vector3 originalSpriteScaling;
    [SerializeField] Vector3 relativeSpriteScaling;
    [SerializeField] GameObject spriteObj;

    [Space(15)]
    [SerializeField] MoveDirectionType moveDirection;
    [SerializeField] Animator animator;

    [Space(15)]
    [SerializeField] UnityEvent moveEvent_Up;
    [SerializeField] UnityEvent moveEvent_Down;
    [SerializeField] UnityEvent moveEvent_Left;
    [SerializeField] UnityEvent moveEvent_Right;
    [Space(15)]
    [SerializeField] UnityEvent moveEvent_Moving;
    [SerializeField] UnityEvent moveEvent_Standing;

    [Space(15)]
    [SerializeField] PlayerAvatar avatar;
    [SerializeField] bool hideAvatar;

    [Space(15)]
    public bool debug_Up;
    public bool debug_Down;
    public bool debug_Left;
    public bool debug_Right;
    [Space(5)]
    public bool debug_GetAutoPilotMargin;
    public bool debug_AutoPilot_PrevTarget;
    public bool debug_AutoPilot_NextTarget;
    [Space(5)]
    public bool debug_PrintInfo;

    public int RoomNum { get => roomNum; set { Player_SetRoomNumber(value); } }
    public bool PauseInputs { get => inputsPaused; set { inputsPaused = value; } }


    void FindPlayer()
    {
        if (player == null)
            player = this.gameObject;
    }

    void FindRigidBody2D()
    {
        if (player != null)
        {
            if (player.TryGetComponent<Rigidbody2D>(out Rigidbody2D _rb))
            {
                rb2D = _rb;
            }
        }
    }

    void FindSelector()
    {
        if (selector == null)
        {
            selector = gameObject.transform.GetComponentInChildren<SelectionActivator>();
        }
    }

    void AddPlayerToGameManager()
    {
        TrackedVariables _manager = GameObject.FindObjectOfType<TrackedVariables>();

        if (_manager != null)
            _manager.Player_AddSelf(this);
    }

    void RemovePlayerFromGameManager()
    {
        TrackedVariables _manager = GameObject.FindObjectOfType<TrackedVariables>();

        if (_manager != null)
            _manager.Player_RemoveSelf(this);
    }

    public void Setup()
    {
        FindPlayer();
        FindRigidBody2D();
        FindSelector();
        AddPlayerToGameManager();
        Avatar_UpdateRoomNum_All();
        newRoomEnteredEvent.Invoke();

        if (useScreenSpaceLock)
        {
            //ScreenSpace_SaveOriginalValues();
            ScreenSpace_SaveCurrentRatioValues();
            ScreenSpace_SavePlayerRelativeLocation();
        }

        AutoPilot_SetupTargetList();

        if (autoPilot)
            AutoPilot_SetTargetListIndex(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckDebugging();

        if (!inputsPaused)
            CheckKeyInputs_Selection();
    }

    private void FixedUpdate()
    {
        CheckDebugging();

        if (useScreenSpaceLock)
            ScreenSpace_CheckRatioValuesChange();

        if (!inputsPaused)
            CheckKeyInputs_Movement();

        if (autoPilot)
            AutoPilot_MoveToTarget();

        KillVelocity();

        if (useScreenSpaceLock)
        {
            ScreenSpace_SaveCurrentRatioValues();
            ScreenSpace_SavePlayerRelativeLocation();
        }
    }

    public void Player_SetRoomNumber(int _roomNum)
    {
        if (roomNum != _roomNum)
        {
            roomNum = _roomNum;
            newRoomEnteredEvent.Invoke();
            Avatar_UpdateRoomNum_All();
            //Avatar_UpdateLinkedRoomNum();
        }
    }

    //  Player Movement         -------------------------------------------------------------------------------------
    #region Player Movement

    public void CheckKeyInputs_Movement()
    {
        if (sprintingEnabled)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                sprinting = true;
            else
                sprinting = false;
        }
        else
            sprinting = false;

        bool input_Previous = inputDetected;

        inputDetected = false;
        moveDirection = MoveDirectionType.NONE;

        //  Up
        if ((useKeys_Arrows && Input.GetKey(KeyCode.UpArrow)) || (useKeys_WASD && Input.GetKey(KeyCode.W)))
        {
            MovePlayer_Up();
            moveDirection = MoveDirectionType.UP;
            inputDetected = true;
        }

        //  Down
        if ((useKeys_Arrows && Input.GetKey(KeyCode.DownArrow)) || (useKeys_WASD && Input.GetKey(KeyCode.S)))
        {
            MovePlayer_Down();
            moveDirection = MoveDirectionType.DOWN;
            inputDetected = true;
        }

        //  Left
        if ((useKeys_Arrows && Input.GetKey(KeyCode.LeftArrow)) || (useKeys_WASD && Input.GetKey(KeyCode.A)))
        {
            MovePlayer_Left();
            moveDirection = MoveDirectionType.LEFT;
            inputDetected = true;
        }

        //  Right
        if ((useKeys_Arrows && Input.GetKey(KeyCode.RightArrow)) || (useKeys_WASD && Input.GetKey(KeyCode.D)))
        {
            MovePlayer_Right();
            moveDirection = MoveDirectionType.RIGHT;
            inputDetected = true;
        }

        if (moveType == MovementType.PHYSICS && moveVec != Vector2.zero)
            MovePlayer_ApplyMoveVec();

        //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        //    Selector_ToggleSelectionState();

        if (input_Previous != inputDetected)
            MovePlayerState_EventUpdate();

        MovePlayer_SpriteAnimatorUpdate();

    }


    public void MovePlayer_Up()
    {
        if (player != null)
        {
            switch (moveType)
            {
                case MovementType.NONE:
                    break;
                case MovementType.PHYSICS:
                    moveVec = moveVec + new Vector2(0.0f, 1.0f);
                    break;
                case MovementType.DIRECT:
                    if (sprinting)
                        player.transform.position = player.transform.position + new Vector3(0.0f, (speed_Direct * sprintScalar), 0.0f);
                    else
                        player.transform.position = player.transform.position + new Vector3(0.0f, speed_Direct, 0.0f);
                    break;
                default:
                    break;
            }


            moveEvent_Up.Invoke();

            Selector_PlaceSelector("UP");
        }
    }

    public void MovePlayer_Down()
    {
        if (player != null)
        {
            switch (moveType)
            {
                case MovementType.NONE:
                    break;
                case MovementType.PHYSICS:
                    moveVec = moveVec + new Vector2(0.0f, -1.0f);
                    break;
                case MovementType.DIRECT:
                    if (sprinting)
                        player.transform.position = player.transform.position + new Vector3(0.0f, (-speed_Direct * sprintScalar), 0.0f);
                    else
                        player.transform.position = player.transform.position + new Vector3(0.0f, -speed_Direct, 0.0f);
                    break;
                default:
                    break;
            }

            //player.transform.position = player.transform.position + new Vector3(0.0f, -speed_Direct, 0.0f);
            moveEvent_Down.Invoke();

            Selector_PlaceSelector("DOWN");
        }
    }

    public void MovePlayer_Right()
    {
        if (player != null)
        {
            switch (moveType)
            {
                case MovementType.NONE:
                    break;
                case MovementType.PHYSICS:
                    moveVec = moveVec + new Vector2(1.0f, 0.0f);
                    break;
                case MovementType.DIRECT:
                    if (sprinting)
                        player.transform.position = player.transform.position + new Vector3((speed_Direct * sprintScalar), 0.0f, 0.0f);
                    else
                        player.transform.position = player.transform.position + new Vector3(speed_Direct, 0.0f, 0.0f);
                    break;
                default:
                    break;
            }

            //player.transform.position = player.transform.position + new Vector3(speed_Direct, 0.0f, 0.0f);
            moveEvent_Right.Invoke();

            Selector_PlaceSelector("RIGHT");
        }
    }

    public void MovePlayer_Left()
    {
        if (player != null)
        {

            switch (moveType)
            {
                case MovementType.NONE:
                    break;
                case MovementType.PHYSICS:
                    moveVec = moveVec + new Vector2(-1.0f, 0.0f);
                    break;
                case MovementType.DIRECT:
                    if (sprinting)
                        player.transform.position = player.transform.position + new Vector3((-speed_Direct * sprintScalar), 0.0f, 0.0f);
                    else
                        player.transform.position = player.transform.position + new Vector3(-speed_Direct, 0.0f, 0.0f);
                    break;
                default:
                    break;
            }

            //player.transform.position = player.transform.position + new Vector3(-speed_Direct, 0.0f, 0.0f);
            moveEvent_Left.Invoke();

            Selector_PlaceSelector("LEFT");
        }
    }

    void MovePlayer_ApplyMoveVec()
    {
        moveVec.Normalize();

        if (useScreenSpaceLock && useRelativeSpeed)
        {
            if (sprinting)
                rb2D.velocity = new Vector2(moveVec.x * (relativeSpeed * sprintScalar), moveVec.y * (relativeSpeed * sprintScalar));
            else
                rb2D.velocity = new Vector2(moveVec.x * relativeSpeed, moveVec.y * relativeSpeed);
        }
        else
        {
            if (sprinting)
                rb2D.velocity = new Vector2(moveVec.x * (speed_Physics * sprintScalar), moveVec.y * (speed_Physics * sprintScalar));
            else
                rb2D.velocity = new Vector2(moveVec.x * speed_Physics, moveVec.y * speed_Physics);
        }

        moveVec = Vector2.zero;
    }

    void KillVelocity()
    {
        if (autoPilot)
        {
            if (!autoPilot_Moving)
            {
                if (rb2D != null)
                    rb2D.velocity = Vector2.zero;
            }
        }
        else
        {
            if (!inputDetected)
            {
                if (rb2D != null)
                    rb2D.velocity = Vector2.zero;
            }
        }
    }

    public void Sprinting_ToggleEnabledState()
    {
        sprintingEnabled = !sprintingEnabled;
    }

    public void MovePlayerState_EventUpdate()
    {
        if (inputDetected)
            moveEvent_Moving.Invoke();
        else
            moveEvent_Standing.Invoke();

    }

    public void MovePlayer_SpriteAnimatorUpdate()
    {
        if (animator != null)
        {
            switch (moveDirection)
            {
                case MoveDirectionType.NONE:
                    break;
                case MoveDirectionType.UP:
                    animator.SetTrigger("Up");
                    break;
                case MoveDirectionType.DOWN:
                    animator.SetTrigger("Down");
                    break;
                case MoveDirectionType.LEFT:
                    animator.SetTrigger("Left");
                    break;
                case MoveDirectionType.RIGHT:
                    animator.SetTrigger("Right");
                    break;
                default:
                    break;
            }
        }
    }

    public void MovePlayer_TeleportToLocation(Vector3 _position)
    {
        if (player != null)
        {
            player.transform.position = _position;
            ScreenSpace_SavePlayerRelativeLocation();
        }
    }

    public void MovePlayer_TeleportToLocation(GameObject _target)
    {
        if (player != null && _target != null)
        {
            player.transform.position = _target.transform.position;
            ScreenSpace_SavePlayerRelativeLocation();
        }
    }

    #endregion Player Movement

    //  AutoPilot               -------------------------------------------------------------------------------------
    #region AutoPilot 


    public void AutoPilot_SetActiveState(bool _active)
    {
        autoPilot = _active;
    }

    public void AutoPilot_SetTargetListIndex(int _index)
    {
        if (_index >= 0 && _index < autoPilotTargets.Count)
        {
            targetIndex = _index;

            if (autoPilotTargets[targetIndex] != null)
                AutoPilot_SetTarget(autoPilotTargets[targetIndex]);
        }
    }

    public void AutoPilot_SetPreviousTarget()
    {
        int _next = targetIndex - 1;

        if (_next >= autoPilotTargets.Count)
            AutoPilot_SetTargetListIndex(autoPilotTargets.Count - 1);
        else if (_next < 0)
            AutoPilot_SetTargetListIndex(0);
        else
            AutoPilot_SetTargetListIndex(_next);
    }

    public void AutoPilot_SetNextTarget()
    {
        int _next = targetIndex + 1;

        if (_next >= autoPilotTargets.Count)
            AutoPilot_SetTargetListIndex(autoPilotTargets.Count - 1);
        else if (_next < 0)
            AutoPilot_SetTargetListIndex(0);
        else
            AutoPilot_SetTargetListIndex(_next);
    }

    public void AutoPilot_SetTarget(GameObject _target)
    {
        if (_target != null)
        {
            autoPilotTarget = _target;

            AutoPilot_AutoFindMargin();
        }
    }

    public void AutoPilot_ClearTarget()
    {
        autoPilotTarget = null;
    }

    public void AutoPilot_SetMovementState(bool _moving)
    {
        autoPilot_Moving = _moving;
    }

    public void AutoPilot_TargetReached()
    {
        if (debug_PrintInfo)
            Debug.Log("Player_AutoPilotTargetReached");

        autoPilot_Moving = false;
        //AutoPilot_LookDown();
        //moveEvent_Standing.Invoke();
        //moveDirection = MoveDirectionType.DOWN;
        //MovePlayer_SpriteAnimatorUpdate();
        autoPilot_Arrived.Invoke();
    }

    public void AutoPilot_LookDown()
    {
        autoPilot_Moving = false;
        moveEvent_Standing.Invoke();
        moveDirection = MoveDirectionType.DOWN;
        MovePlayer_SpriteAnimatorUpdate();
    }

    public void AutoPilot_AutoFindMargin()
    {
        if (autoSetMargin && autoPilotTarget != null)
        {
            if (autoPilotTarget.TryGetComponent<RectTransform>(out RectTransform _rect))
            {
                autoPilot_Margin = (_rect.rect.width * autoPilot_MarginRatio);

                if (debug_PrintInfo)
                    Debug.Log("Player_AutoPilotMArginSet=> Margin: " + autoPilot_Margin + ", Ratio: " + autoPilot_MarginRatio);
                //autoPilot_Margin = (_rect.rect.width * 0.25f);
            }

            //if(autoPilotTarget.TryGetComponent<Rect>(out Rect _rect))
            //{
            //    autoPilot_Margin = (_rect.width * 0.25f);
            //}
        }
    }

    public void AutoPilot_MoveToTarget()
    {
        if (debug_PrintInfo)
            Debug.Log("PlayerAutoPilot=> AutoPilot On");

        if ((autoPilot && autoPilot_Moving && autoPilotTarget != null && player != null))
        {
            AutoPilot_AutoFindMargin();

            Vector3 delta = autoPilotTarget.transform.position - player.transform.position;

            if (debug_PrintInfo)
                Debug.Log("PlayerAutoPilot=> VectToTarget-> X: " + delta.x + ", Y: " + delta.y);

            if ((delta.y <= autoPilot_Margin && delta.y >= -autoPilot_Margin) && (delta.x <= autoPilot_Margin && delta.x >= -autoPilot_Margin))
            {
                //autoPilot_Moving = false;
                //moveEvent_Standing.Invoke();
                //moveDirection = MoveDirectionType.DOWN;
                //MovePlayer_SpriteAnimatorUpdate();
                AutoPilot_TargetReached();
            }
            else
            {
                //  Up
                if (delta.y > 0)
                {
                    if (delta.y > autoPilot_Margin)
                    {
                        MovePlayer_Up();
                        moveDirection = MoveDirectionType.UP;
                    }
                }

                //  Down
                if (delta.y < 0)
                {
                    if (delta.y < -autoPilot_Margin)
                    {
                        MovePlayer_Down();
                        moveDirection = MoveDirectionType.DOWN;
                    }
                }

                //  Right
                if (delta.x > 0)
                {
                    if (delta.x > autoPilot_Margin)
                    {
                        MovePlayer_Right();
                        moveDirection = MoveDirectionType.RIGHT;
                    }
                }

                //  Left
                if (delta.x < 0)
                {
                    if (delta.x < -autoPilot_Margin)
                    {
                        MovePlayer_Left();
                        moveDirection = MoveDirectionType.LEFT;
                    }
                }


                if (moveType == MovementType.PHYSICS && moveVec != Vector2.zero)
                    MovePlayer_ApplyMoveVec();

                MovePlayer_SpriteAnimatorUpdate();

                moveEvent_Moving.Invoke();

            }
        }
    }

    public void AutoPilot_SetupTargetList()
    {
        for (int i = 0; i < autoPilotTargets.Count; i++)
        {
            if (autoPilotTargets[i] != null)
            {
                if (autoPilotTargets[i].TryGetComponent<AutoPilotTarget>(out AutoPilotTarget _target))
                {
                    _target.SetPlayer(this);
                }
            }
        }
    }

    public void AutoPilot_CheckTargetMatch(GameObject _target)
    {
        if (_target == null)
        {
            if (_target == autoPilotTarget)
            {
                Debug.Log("AutoPilot_CheckTargetMatch=> Match Found, Target Reached");
                AutoPilot_TargetReached();
            }
            else
                Debug.Log("AutoPilot_CheckTargetMatch=> Match Not Found");
        }
        else
            Debug.Log("AutoPilot_CheckTargetMatch=> Target to check was Null!!");
    }

    #endregion AutoPilot

    //  Selector / Selection    -------------------------------------------------------------------------------------
    #region Selector / Selection

    public void CheckKeyInputs_Selection()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            Selector_ToggleSelectionState();
    }

    public void Selector_ToggleSelectionState()
    {
        if (selector == null)
            FindSelector();

        if (selector != null)
            selector.Selector_TriggerSelectionTarget();
    }

    public void Selector_PlaceSelector(string _placementType)
    {
        if (selector != null)
            selector.Rect_PlaceSelector(_placementType);
    }

    #endregion Selector / Selection


    //  Player Avatar           -------------------------------------------------------------------------------------
    #region Player Avatar

    //  Screenspace Lock           -------------------------------------------------------------------------------------
    #region

    //public void ScreenSpace_SaveOriginalValues()
    //{
    //    if (useScreenSpaceLock && parentCanvas != null && spriteObj != null)
    //    {
    //        float _scaleX = spriteObj.transform.localScale.x / parentCanvas.rect.x;
    //        float _scaleY = spriteObj.transform.localScale.y / parentCanvas.rect.y;
    //        originalSpriteScaling = new Vector3(_scaleX, _scaleY, 1.0f);
    //    }
    //}

    public void ScreenSpace_SaveCurrentRatioValues()
    {
        if (useScreenSpaceLock && parentCanvas != null)
        {
            savedWidth = parentCanvas.rect.width;
            savedHeight = parentCanvas.rect.height;

            if (useRelativeSpeed)
            {
                if (moveType == MovementType.PHYSICS)
                    relativeSpeed = (relativeSpeedMovementVal / relativeSpeedCanvasWidthVal) * savedWidth;
                //relativeSpeed = 0.0015f * savedWidth;
                //relativeSpeed = speed_Physics / savedWidth;
                //if (moveType == MovementType.DIRECT)
                //    relativeSpeed = speed_Direct / savedWidth;
            }

            if (useRelativeSpriteScaling && spriteObj != null)
                ScreenSpace_UpdateSpriteScale();

            //ScreenSpace_SavePlayerRelativeLocation();
        }
    }

    public void ScreenSpace_SavePlayerRelativeLocation()
    {
        if (useScreenSpaceLock && savedWidth != 0.0f && savedHeight != 0.0f)
        {
            float _ratioX = player.transform.position.x / savedWidth;
            float _ratioY = player.transform.position.y / savedHeight;
            relativePOS = new Vector2(_ratioX, _ratioY);
        }
    }

    public void ScreenSpace_CheckRatioValuesChange()
    {
        if (useScreenSpaceLock && parentCanvas != null)
        {
            if (savedWidth != parentCanvas.rect.width || savedHeight != parentCanvas.rect.height)
            {
                float _newX = relativePOS.x * parentCanvas.rect.width;
                float _newY = relativePOS.y * parentCanvas.rect.height;
                player.transform.position = new Vector3(_newX, _newY, player.transform.position.z);

                ScreenSpace_UpdateSpriteScale();
            }

            ScreenSpace_SaveCurrentRatioValues();
            ScreenSpace_SavePlayerRelativeLocation();

            //savedWidth = parentCanvas.rect.width;
            //savedHeight = parentCanvas.rect.height;

            //float _ratioX = player.transform.position.x / savedWidth;
            //float _ratioY = player.transform.position.y / savedHeight;
            //relativePOS = new Vector2(_ratioX, _ratioY);
        }
    }

    public void ScreenSpace_UpdateSpriteScale()
    {
        if (spriteObj != null)
        {
            if (useRelativeSpriteScaling)
            {
                float _scalarX = savedWidth / 1469.0f;
                spriteObj.transform.localScale = Vector3.one * _scalarX;
                relativeSpriteScaling = Vector3.one * _scalarX;
            }
            else
                spriteObj.transform.localScale = Vector3.one;

        }
    }

    #endregion

    public void Avatar_SetPlayerAvatar(PlayerAvatar _avatar)
    {
        if (_avatar != null && avatar == null)
        {
            avatar = _avatar;
            avatar.Avatar_SetLinkedPlayer(gameObject);
            Avatar_UpdateLinkedRoomNum();
        }
    }

    public void Avatar_UpdateLinkedAvatarState()
    {
        if (avatar != null)
        {
            avatar.RemainHidden = hideAvatar;
            avatar.Avatar_SetActiveState(!hideAvatar);
        }
    }

    public void Avatar_UpdateRoomNum_All()
    {
        Avatar_UpdateLinkedRoomNum();
        Avatar_NewRoomEntered();
    }

    public void Avatar_UpdateLinkedRoomNum()
    {
        if (avatar != null)
            avatar.Avatar_LinkedPlayerRoomChange(roomNum);

        Avatar_UpdateLinkedAvatarState();
    }

    public void Avatar_NewRoomEntered()
    {
        PlayerAvatar[] _avatars = GameObject.FindObjectsOfType<PlayerAvatar>(true);

        for (int i = 0; i < _avatars.Length; i++)
        {
            if (_avatars[i] != null)
            {
                if (avatar != null && _avatars[i] == avatar)
                {
                    continue;
                }
                else
                    _avatars[i].Player_NewRoomEntered(roomNum);
            }
        }
    }

    #endregion Player Avatar

    public void CheckDebugging()
    {
        if (debug_Up)
        {
            MovePlayer_Up();
            debug_Up = false;
        }

        if (debug_Down)
        {
            MovePlayer_Down();
            debug_Down = false;
        }

        if (debug_Left)
        {
            MovePlayer_Left();
            debug_Left = false;
        }

        if (debug_Right)
        {
            MovePlayer_Right();
            debug_Right = false;
        }

        if (debug_GetAutoPilotMargin)
        {
            AutoPilot_AutoFindMargin();
            debug_GetAutoPilotMargin = false;
        }

        if (debug_AutoPilot_NextTarget)
        {
            AutoPilot_SetNextTarget();
            debug_AutoPilot_NextTarget = false;
        }

        if (debug_AutoPilot_PrevTarget)
        {
            AutoPilot_SetPreviousTarget();
            debug_AutoPilot_PrevTarget = false;
        }

    }


}
