using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountEvent : MonoBehaviour
{
    [SerializeField] int count;
    [SerializeField] int limit;
    [SerializeField] bool resetWhenInvoked;
    [SerializeField] UnityEvent countEvent;

   
    public void Count_Increase(int _increaseAmount)
    {
        count += _increaseAmount;
        Count_CheckLimit();
    }

    public void Count_CheckLimit()
    {
        if(count >= limit)
        {
            countEvent.Invoke();
            if (resetWhenInvoked)
                count = 0;
        }
    }

}
