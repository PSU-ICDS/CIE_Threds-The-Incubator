using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoPilot_TeleportLocation : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] GameObject targetLocation;
    [SerializeField] CharacterController_2D player;
    [SerializeField] UnityEvent teleportEvent;

    // Start is called before the first frame update
    void Start()
    {
        FindTarget();
        FindPlayer();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void FindTarget()
    {
        if (targetLocation == null)
            targetLocation = gameObject;
    }

    void FindPlayer()
    {
        if (player == null)
        {
            if (GameObject.FindObjectOfType<CharacterController_2D>() != null)
            {
                player = GameObject.FindObjectOfType<CharacterController_2D>();
            }
        }
    }

    public void TeleportPlayerToTarget()
    {
        if (active)
        {
            if (player != null && targetLocation != null)
            {
                player.MovePlayer_TeleportToLocation(targetLocation);
                teleportEvent.Invoke();
            }
        }
    }
}
