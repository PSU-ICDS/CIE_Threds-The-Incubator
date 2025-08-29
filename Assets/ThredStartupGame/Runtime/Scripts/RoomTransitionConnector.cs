using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTransitionConnector : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] RoomTransitionConnector linkedConnector;
    [SerializeField] GameObject transitionLocation;
    [SerializeField] int roomNum;
    [Space(15)]
    [SerializeField] bool locked;
    [SerializeField] UnityEvent lockedEvent;
    [SerializeField] UnityEvent unlockedEvent;
    //[SerializeField] GameObject returnLocation;
    //[SerializeField] bool useFade;

    //[Space(15)]

   
    //[SerializeField] bool track2D;
    //[SerializeField] bool track3D;

    [Space(15)]
    //[SerializeField] bool ignoreCollisionEvents;
    [SerializeField] bool useColliders;
    [SerializeField] UnityEvent collisionEnterEvent;
    [SerializeField] UnityEvent collisionExitEvent;

    [Space(15)]
    //[SerializeField] bool ignoreTriggerEvents;
    [SerializeField] bool useTriggers;
    [SerializeField] UnityEvent triggerEnterEvent;
    [SerializeField] UnityEvent triggerExitEvent;

    public bool Locked { get => locked; set { SetLockedState(value); } }
    public int RoomNum { get => roomNum; set { roomNum = value; } }

    private void OnEnable()
    {
        RunEvent_UpdateLockedState();
    }

    public bool HasLinkedConnector()
    {
        if (linkedConnector != null)
            return true;

        return false;
    }

    public Vector3 GetLinkedTransitionLocation()
    {
        if (HasLinkedConnector() && linkedConnector.HasTransitionLocation())
            return linkedConnector.GetTransitionLocation();

        return Vector3.zero;
    }

    public int GetLinkedRoomNumber()
    {
        if (HasLinkedConnector() && linkedConnector.HasTransitionLocation())
            return linkedConnector.RoomNum;

        return -1;
    }

    public bool TransitionAvailable()
    {
        if (!locked && HasTransitionLocation() && HasLinkedConnector())
            return true;

        return false;
    }

    public void SetLockedState(bool _locked)
    {
        if (locked != _locked)
        {
            locked = _locked;
            RunEvent_UpdateLockedState();
        }
    }

    public void RunEvent_UpdateLockedState()
    {
        if (locked)
            lockedEvent.Invoke();
        else
            unlockedEvent.Invoke();
    }

    public bool HasTransitionLocation()
    {
        if (transitionLocation != null)
            return true;

        return false;
    }

    public Vector3 GetTransitionLocation()
    {
        if (transitionLocation != null)
            return transitionLocation.transform.position;

        return Vector3.zero;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (active && TransitionAvailable() && useTriggers)
        {
            if (other.gameObject.TryGetComponent<CharacterController_2D>(out CharacterController_2D _cc_2D))
            {
                if (_cc_2D.RoomNum == roomNum)
                {
                    Debug.Log("RoomTransition-> TriggerEnter in room: " + roomNum);
                    UpdatePlayerPosition(other.gameObject);
                    UpdatePlayerRoomNumber(_cc_2D);
                    triggerEnterEvent.Invoke();
                }

                //other.gameObject.transform.position = GetLinkedTransitionLocation();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && TransitionAvailable() && useTriggers)
        {
            if (collision.gameObject.TryGetComponent<CharacterController_2D>(out CharacterController_2D _cc_2D))
            {

                if (_cc_2D.RoomNum == roomNum)
                {
                    Debug.Log("RoomTransition-> TriggerEnter2D in room: " + roomNum);
                    UpdatePlayerPosition(collision.gameObject);
                    UpdatePlayerRoomNumber(_cc_2D);
                    triggerEnterEvent.Invoke();
                }

                //collision.gameObject.transform.position = GetLinkedTransitionLocation();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (active && TransitionAvailable() && useColliders)
        {
            if (collision.gameObject.TryGetComponent<CharacterController_2D>(out CharacterController_2D _cc_2D))
            {
                if (_cc_2D.RoomNum == roomNum)
                {
                    Debug.Log("RoomTransition-> CollisionEnter in room: " + roomNum);
                    UpdatePlayerPosition(collision.gameObject);
                    UpdatePlayerRoomNumber(_cc_2D);
                    collisionEnterEvent.Invoke();
                }

                //collision.gameObject.transform.position = GetLinkedTransitionLocation();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (active && TransitionAvailable() && useColliders)
        {
            if (collision.gameObject.TryGetComponent<CharacterController_2D>(out CharacterController_2D _cc_2D))
            {

                if (_cc_2D.RoomNum == roomNum)
                {
                    Debug.Log("RoomTransition-> CollisionEnter2D in room: " + roomNum);
                    UpdatePlayerPosition(collision.gameObject);
                    UpdatePlayerRoomNumber(_cc_2D);
                    collisionEnterEvent.Invoke();
                }

                // collision.gameObject.transform.position = GetLinkedTransitionLocation();
            }
        }
    }

    public void UpdatePlayerPosition(GameObject _playerObj)
    {
        if (_playerObj != null)
        {
            _playerObj.transform.position = GetLinkedTransitionLocation();
        }
    }

    public void UpdatePlayerRoomNumber(CharacterController_2D _player)
    {
        if (_player != null)
        {
            int newRoomNum = GetLinkedRoomNumber();
            if (newRoomNum != -1)
            {
                Debug.Log("RoomTransition-> Player moving to room: " + newRoomNum);
                _player.Player_SetRoomNumber(newRoomNum);
            }
        }
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
