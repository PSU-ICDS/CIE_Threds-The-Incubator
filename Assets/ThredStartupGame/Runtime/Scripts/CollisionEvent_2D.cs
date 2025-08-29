using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent_2D : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] bool track2D;
    [SerializeField] bool track3D;
    [Space(15)]
    [SerializeField] bool ignoreCollisionEvents;
    [SerializeField] UnityEvent collisionEnterEvent;
    [SerializeField] UnityEvent collisionExitEvent;
    [Space(15)]
    [SerializeField] bool ignoreTriggerEvents;
    [SerializeField] UnityEvent triggerEnterEvent;
    [SerializeField] UnityEvent triggerExitEvent;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(active && track2D && !ignoreCollisionEvents)
        {          
            collisionEnterEvent.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && track2D && !ignoreTriggerEvents)
            triggerEnterEvent.Invoke();
    }

}
