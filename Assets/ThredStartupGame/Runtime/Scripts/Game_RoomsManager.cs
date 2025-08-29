using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_RoomsManager : MonoBehaviour
{
    [SerializeField] bool autoFindRooms;
    [SerializeField] bool closeAllOnStart;
    [SerializeField] bool enterMainRoomOnStart;
    [SerializeField] int mainRoomIndex = -1;
    [Space(15)]
    [SerializeField] List<GameRoom> rooms;
    [Space(15)]
    [SerializeField] List<RoomRefInfo> roomsInfo;
    [Space(15)]
    //[SerializeField] List<RoomTransitionConnectors> roomTransitions;
    int current = 0;

    //[Serializable]
    //public class RoomTransitionConnectors
    //{
    //    public string name;
    //    public string roomName_01;
    //    public string roomName_02;
    //    public int roomId_01;
    //    public int roomId_02;
    //    public Vector3 entryPOS_Room01;
    //    public Vector3 entryPOS_Room02;

    //    public bool ValidRoomIds()
    //    {
    //        if (roomId_01 != -1 && roomId_02 != -1)
    //            return true;

    //        return false;
    //    }

    //    public bool ConnectionMatchFound(int _index_01, int _index_02)
    //    {
    //        if (_index_01 == roomId_01 && _index_02 == roomId_02)
    //            return true;

    //        if (_index_01 == roomId_02 && _index_02 == roomId_01)
    //            return true;

    //        return false;
    //    }

    //    public Vector3 GetEntryPointByID(int _id)
    //    {
    //        if (_id == roomId_01)
    //            return entryPOS_Room01;

    //        if (_id == roomId_02)
    //            return entryPOS_Room02;

    //        return Vector3.zero;
    //    }

    //}

    [Serializable]
    public class RoomRefInfo
    {
        public string name;
        public int id;
    }


    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        FindGameRooms();
        FindMainRoomIndex();
        SetRoomIDs();
        //SetupRoomTransitions();
        if (closeAllOnStart)
            CloseAllRoomGameObjects();

        if (enterMainRoomOnStart && mainRoomIndex != -1)
            Rooms_EnterRoomByIndex(mainRoomIndex);
    }

    void FindGameRooms()
    {
        if (autoFindRooms)
        {
            GameRoom[] _rooms = gameObject.transform.GetComponentsInChildren<GameRoom>(true);

            for (int i = 0; i < _rooms.Length; i++)
            {
                if (_rooms[i] != null)
                {
                    if (!rooms.Contains(_rooms[i]))
                        rooms.Add(_rooms[i]);
                }
            }
        }
    }

    void FindMainRoomIndex()
    {
        if (mainRoomIndex == -1)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i] != null)
                {
                    if (rooms[i].IsMainRoom)
                    {
                        mainRoomIndex = i;
                        break;
                    }
                }
            }
        }
    }

    void SetRoomIDs()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if(rooms[i]!= null)
            {
                rooms[i].RoomID = i;
                int _index = GetExistingRoomInfoIndex(rooms[i].RoomName);

                if(_index != -1)
                {
                    roomsInfo[_index].id = i;
                }
                else
                {
                    RoomRefInfo newInfo = new RoomRefInfo();
                    newInfo.id = i;
                    newInfo.name = rooms[i].RoomName;
                    roomsInfo.Add(newInfo);
                }
            }
        }
    }

    int GetExistingRoomInfoIndex(string _roomName)
    {
        for (int i = 0; i < roomsInfo.Count; i++)
        {
            if (roomsInfo[i] != null && roomsInfo[i].name.ToLower() == _roomName.ToLower())
                return i;
        }

        return -1;
    }

    //void SetupRoomTransitions()
    //{
    //    for (int i = 0; i < roomTransitions.Count; i++)
    //    {
    //        if(roomTransitions[i] != null)
    //        {
    //            int _roomIndex_01 = GetExistingRoomInfoIndex(roomTransitions[i].roomName_01);
    //            int _roomIndex_02 = GetExistingRoomInfoIndex(roomTransitions[i].roomName_02);

    //            if(_roomIndex_01 != -1)
    //            {
    //                roomTransitions[i].roomId_01 = roomsInfo[_roomIndex_01].id;
    //            }
    //            else
    //            {
    //                roomTransitions[i].roomId_01 = -1;
    //            }

    //            if (_roomIndex_02 != -1)
    //            {
    //                roomTransitions[i].roomId_02 = roomsInfo[_roomIndex_02].id;
    //            }
    //            else
    //            {
    //                roomTransitions[i].roomId_02 = -1;
    //            }

    //            if(roomTransitions[i].ValidRoomIds())
    //            {
    //                roomTransitions[i].entryPOS_Room01 = Transition_GetLocation(roomTransitions[i].roomName_02, roomTransitions[i].roomName_01);
    //                roomTransitions[i].entryPOS_Room02 = Transition_GetLocation(roomTransitions[i].roomName_01, roomTransitions[i].roomName_02);
    //            }

    //        }
    //    }
    //}

    //public Vector3 Transition_GetLocation(string _roomName_Leaving, string _roomName_Entering)
    //{
    //    int _index_Leaving = GetRoomIndexByName(_roomName_Leaving);
    //    //int _index_Entering = GetRoomIndexByName(_roomName_Entering);

    //    if(_index_Leaving != -1)
    //    {
    //        int _subIndex = rooms[_index_Leaving].GetTransitionIndexByConnectedRoomName(_roomName_Entering);
    //        if (_subIndex != -1)
    //            return rooms[_index_Leaving].GetTransitionLocationByIndex(_subIndex);
    //    }

    //    return Vector3.zero;
    //}

    int GetRoomIndexByName(string _roomName)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if(rooms[i] != null)
            {
                if (rooms[i].name.ToLower() == _roomName.ToLower())
                    return i;
            }
        }


        return -1;
    }

    //public Vector3 GetTransitionLocation(int _index_From, int _index_To)
    //{
    //    for (int i = 0; i < roomTransitions.Count; i++)
    //    {
    //        if(roomTransitions[i] != null)
    //        {
    //            if(roomTransitions[i].ConnectionMatchFound(_index_From, _index_To))
    //            {
    //                return roomTransitions[i].GetEntryPointByID(_index_To);
    //            }
    //        }
    //    }

    //    return Vector3.zero;
    //}

    
    public void CloseAllRoomGameObjects()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] != null && i != mainRoomIndex)
                rooms[i].gameObject.SetActive(false);
        }
    }

    public void Rooms_ReturnToMain()
    {
        if (mainRoomIndex > -1 && mainRoomIndex < rooms.Count)
        {
            if (current != mainRoomIndex)
            {
                Rooms_ExitRoomByIndex(current);
                Rooms_EnterRoomByIndex(mainRoomIndex);
            }
        }
    }

    public void Rooms_EnterRoom(int _index)
    {
        if (_index > -1 && _index < rooms.Count && _index != current)
        {
            if (rooms[_index] != null && !rooms[_index].Locked)
            {
                Rooms_ExitRoomByIndex(current);
                Rooms_EnterRoomByIndex(_index);
            }
        }
    }

    void Rooms_EnterRoomByIndex(int _index)
    {
        if (_index > -1 && _index < rooms.Count)
        {
            if (rooms[_index] != null)
            {
                rooms[_index].Room_Enter();
                current = _index;
            }
        }
    }

    void Rooms_ExitRoomByIndex(int _index)
    {
        if (_index > -1 && _index < rooms.Count)
        {
            if (rooms[_index] != null)
            {
                rooms[_index].Room_Exit();
            }
        }
    }

    public void Rooms_EnterRoomByName(string _name)
    {
        int _index = -1;

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] != null)
            {
                if (rooms[i].RoomName.ToLower() == _name.ToLower())
                {
                    _index = i;
                    break;
                }
            }
        }

        Rooms_EnterRoom(_index);
    }

    public void Rooms_LockByName(string _name)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] != null)
            {
                if (rooms[i].RoomName.ToLower() == _name.ToLower())
                {
                    Rooms_SetLockState(i, true);
                    break;
                }
            }
        }
    }

    public void Rooms_UnlockByName(string _name)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] != null)
            {
                if (rooms[i].RoomName.ToLower() == _name.ToLower())
                {
                    Rooms_SetLockState(i, false);
                    break;
                }
            }
        }
    }

    public void Rooms_LockByIndex(int _index)
    {
        Rooms_SetLockState(_index, true);
    }

    public void Rooms_UnlockByIndex(int _index)
    {
        Rooms_SetLockState(_index, false);
    }

    void Rooms_SetLockState(int _index, bool _locked)
    {
        if (_index > -1 && _index < rooms.Count)
        {
            if (rooms[_index] != null)
                rooms[_index].Locked = _locked;
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
