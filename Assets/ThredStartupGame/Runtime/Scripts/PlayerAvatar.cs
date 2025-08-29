using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    [SerializeField] GameObject avatarObj;
    [SerializeField] int roomNum;
    [SerializeField] GameObject linkedPlayer;
    [SerializeField] bool remainHidden;
    [SerializeField] bool matchLinkedPlayerPosition;
    [Space(15)]
    [SerializeField] bool isKinematic;
    Rigidbody2D rb;

    public int RoomNum { get => roomNum; set { roomNum = value; } }
    public bool RemainHidden { get=>remainHidden; set {Avatar_SetRemainHiddenState(value); } }


    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        FindAvatarObj();
        FindRigidBody2D();
        ApplyKinematicState();
    }

    void FindAvatarObj()
    {
        if (avatarObj == null)
            avatarObj = gameObject;
    }

    void FindRigidBody2D()
    {
        if (rb == null)
        {
            if (avatarObj != null)
            {
                if (avatarObj.TryGetComponent<Rigidbody2D>(out Rigidbody2D _rb))
                    rb = _rb;
            }

            if (rb == null)
            {
                if (gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D _rb))
                    rb = _rb;
            }
        }
    }

    public void ApplyKinematicState()
    {
        if (rb != null)
        {
            if (isKinematic)
                rb.bodyType = RigidbodyType2D.Kinematic;
            else
                rb.bodyType = RigidbodyType2D.Dynamic;
            //rb.isKinematic = isKinematic;
        }
    }

    public bool IsAvatarValidAndActive()
    {
        if (avatarObj != null && avatarObj.activeSelf && avatarObj.activeInHierarchy)
            return true;

        return false;
    }

    public void Avatar_SetRemainHiddenState(bool _remainHidden)
    {
        remainHidden = _remainHidden;
        Avatar_CheckRemainHiddenState();
    }

    public void Player_NewRoomEntered(int _newRoomNum)
    {
        Avatar_SetActiveState(_newRoomNum == roomNum);
    }

    public void Avatar_SetActiveState(bool _active)
    {
        if (avatarObj != null)
        {
            if (remainHidden)
                avatarObj.SetActive(false);
            else
                avatarObj.SetActive(_active);
        }
    }

    void Avatar_CheckRemainHiddenState()
    {
        if (avatarObj != null && remainHidden)
            Avatar_SetActiveState(false);
    }

    public void Avatar_LinkedPlayerRoomChange(int _roomNum)
    {
        roomNum = _roomNum;
    }

    public void Avatar_SetLinkedPlayer(GameObject _player)
    {
        if (_player != null && linkedPlayer == null)
            linkedPlayer = _player;
    }

    void Avatar_MatchLinkedPlayerPosition()
    {
        if (matchLinkedPlayerPosition && avatarObj != null && linkedPlayer != null)
            avatarObj.transform.position = linkedPlayer.transform.position;
    }

    private void LateUpdate()
    {
        if (IsAvatarValidAndActive())
            Avatar_MatchLinkedPlayerPosition();
    }



    //// Update is called once per frame
    //void Update()
    //{

    //}
}
