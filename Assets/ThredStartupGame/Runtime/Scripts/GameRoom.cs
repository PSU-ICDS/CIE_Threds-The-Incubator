using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameRoom : MonoBehaviour
{
    [SerializeField] string roomName;
    [SerializeField] int roomId;
    [SerializeField] GameObject gameRoomObj;
    [SerializeField] bool isMainRoom;
    [SerializeField] bool hasBeenEntered;
    [SerializeField] bool disableOnExit;
    [SerializeField] bool locked;
    [Space(15)]
    //[SerializeField] bool multipleEntryPoints;
    //[SerializeField] List<RoomTransitions> transitions;
    [Space(15)]
    [SerializeField] UnityEvent enterEvent_FirstTime;
    [SerializeField] UnityEvent enterEvent;
    [SerializeField] UnityEvent exitEvent;
    [Space(15)]
    [SerializeField] UnityEvent lockRoomEvent;
    [SerializeField] UnityEvent unlockRoomEvent;
    bool lockedEventRan = false;
    [Space(15)]
    [SerializeField] RoomTime roomTime;
    [Space(15)]
    [SerializeField] List<TimeTriggeredEvent> timeEvents;

    public bool IsMainRoom { get => isMainRoom; set { isMainRoom = value; } }
    public int RoomID { get=> roomId; set {SetRoomID(value); } }
    public string RoomName { get => roomName; set { roomName = value; } }
    public bool Locked { get => locked; set { SetLockedState(value); } }

    public float TimeInRoom_Total { get=>roomTime.timeInRoom_Total; }

    public enum TimedTriggerTypes { NONE, VISIT, TOTAL }

    [Serializable]
    public class RoomTime
    {
        public bool trackRoomTime;
        public float timeInRoom_CurrentVisit;
        public float timeInRoom_Total;
        public bool pauseTime;

        public void PauseTimeTracking(bool _pauseState)
        {
            pauseTime = _pauseState;
        }

        public void UpdateTime()
        {
            if (trackRoomTime && !pauseTime)
            {
                timeInRoom_CurrentVisit += Time.deltaTime;
                timeInRoom_Total += Time.deltaTime;
            }
        }

        public void UpdateTime_Total()
        {
            if (trackRoomTime)
            {
                //timeInRoom_Total += timeInRoom_CurrentVisit;
                timeInRoom_CurrentVisit = 0.0f;
            }
        }

        public void ResetTime_Current()
        {
            if (trackRoomTime)
                timeInRoom_CurrentVisit = 0.0f;
        }
    }

    [Serializable]
    public class TimeTriggeredEvent
    {
        public string name;
        public float triggerTime;
        public TimedTriggerTypes triggerType;
        public bool repeating;
        public bool hasRunEvent;
        public UnityEvent timedEvent;

        public void CheckTriggerTime(float _timeInRoom_Visit, float _timeInRoom_Total)
        {
            switch (triggerType)
            {
                case TimedTriggerTypes.NONE:
                    break;
                case TimedTriggerTypes.VISIT:
                    CheckTime_Visit(_timeInRoom_Visit);
                    break;
                case TimedTriggerTypes.TOTAL:
                    CheckTime_Total(_timeInRoom_Total);
                    break;
                default:
                    break;
            }
        }

        void CheckTime_Visit(float _timeInRoom_Visit)
        {
            if (_timeInRoom_Visit >= triggerTime)
            {
                if (!hasRunEvent)
                    RunTimedEvent();
                else if (hasRunEvent && repeating)
                    RunTimedEvent();
            }
        }

        void CheckTime_Total(float _timeInRoom_Total)
        {
            if (_timeInRoom_Total >= triggerTime)
            {
                if (!hasRunEvent)
                    RunTimedEvent();
                else if (hasRunEvent && repeating)
                    RunTimedEvent();
            }
        }


        void RunTimedEvent()
        {
            timedEvent.Invoke();
            hasRunEvent = true;
        }

    }

    //[Serializable]
    //public class RoomTransitions
    //{
    //    public string name;
    //    //public int id;
    //    public string connectedRoomName;
    //    public bool defualtEntryPoint;
    //    public GameObject targetPoint;

    //    public bool HasTargetPoint()
    //    {
    //        if (targetPoint != null)
    //            return true;

    //        return false;
    //    }

    //    public Vector3 GetTargetPoint()
    //    {
    //        //Vector3 _position = Vector3.zero;

    //        if (targetPoint != null)
    //            return targetPoint.transform.position;

    //        return Vector3.zero;
    //    }   
        
    //    public bool MatchFound_ConnectedRoom(string _connectedRoomName)
    //    {
    //        if (connectedRoomName.ToLower() == _connectedRoomName.ToLower() && HasTargetPoint())
    //            return true;

    //        return false;
    //    }

    //}

    void FindGameRoomObject()
    {
        if (gameRoomObj == null)
            gameRoomObj = this.gameObject;
    }


    //private void OnEnable()
    //{        
    //    Room_Enter();
    //}

    //private void OnDisable()
    //{
    //    Room_Exit();
    //}


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    public void Room_Enter()
    {
        FindGameRoomObject();
        if (gameRoomObj != null)
            gameRoomObj.SetActive(true);

        //hasBeenEntered = true;
        roomTime.PauseTimeTracking(false);
        RunEvents_EntryExit(true);
        //gameObject.SetActive(true);
    }

    public void Room_Exit()
    {
        if (hasBeenEntered)
        {
            roomTime.PauseTimeTracking(true);
            RunEvents_EntryExit(false);
        }

        if (disableOnExit && gameRoomObj != null)
            gameRoomObj.SetActive(false);
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeTracking();
    }

    public void UpdateTimeTracking()
    {
        if (roomTime.trackRoomTime)
        {
            roomTime.UpdateTime();

            for (int i = 0; i < timeEvents.Count; i++)
            {
                if (timeEvents[i] != null)
                    timeEvents[i].CheckTriggerTime(roomTime.timeInRoom_CurrentVisit, roomTime.timeInRoom_Total);
            }
        }
    }

    public void RunEvents_EntryExit(bool _entering)
    {
        if (_entering)
        {
            if (!hasBeenEntered)
            {
                enterEvent_FirstTime.Invoke();
                hasBeenEntered = true;
                Debug.Log("Entered Room: " + roomName + " for the first time");
            }
            else
                enterEvent.Invoke();
        }
        else
        {
            exitEvent.Invoke();
            roomTime.ResetTime_Current();
            //roomTime.UpdateTime_Total();
        }
    }

    public void SetLockedState(bool _state)
    {
        if (_state != locked)
        {
            lockedEventRan = false;
            Debug.Log("Room Lock State Change-> Room: " + roomName + ", Locked: " + locked);
            locked = _state;
            RunEvents_LockedStateChange();
        }
    }

    public void RunEvents_LockedStateChange()
    {
        if (!lockedEventRan)
        {
            if (locked)
                lockRoomEvent.Invoke();
            else
                unlockRoomEvent.Invoke();

            lockedEventRan = true;
        }
    }

    public void SetRoomID(int _roomID)
    {
        roomId = _roomID;

        RoomTransitionConnector[] _connecters = gameObject.transform.GetComponentsInChildren<RoomTransitionConnector>(true);

        for (int i = 0; i < _connecters.Length; i++)
        {
            if(_connecters[i] != null)
            {
                _connecters[i].RoomNum = roomId;
            }
        }
    }

    //public int GetTransitionIndexByConnectedRoomName(string _connectedRoomName)
    //{
    //    for (int i = 0; i < transitions.Count; i++)
    //    {
    //        if(transitions[i] != null)
    //        {
    //            if (transitions[i].MatchFound_ConnectedRoom(_connectedRoomName))
    //                return i;
    //        }
    //    }

    //    return -1;
    //}

    //public Vector3 GetTransitionLocationByIndex(int _index)
    //{
    //    if(_index >= 0 && _index < transitions.Count)
    //    {
    //        if (transitions[_index] != null)
    //            return transitions[_index].GetTargetPoint();
    //    }

    //    return Vector3.zero;
    //}

    //public void UpdateTransitionIDs()
    //{
    //    for (int i = 0; i < transitions.Count; i++)
    //    {
    //        if(transitions[i] != null)
    //        {
    //            transitions[i].id = i;
    //        }
    //    }
    //}

    //public void SetTransitionID(int _index, int _id)
    //{
    //    if(_index >= 0 && _index < transitions.Count)
    //    {
    //        if (transitions[_index] != null)
    //            transitions[_index].id = _id;
    //    }
    //}
}
