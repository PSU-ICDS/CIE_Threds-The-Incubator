using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_TrsLocationScaler : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] bool matchLocation;
    [SerializeField] bool matchRectSize;
    [SerializeField] GameObject location;
    [SerializeField] GameObject anchor;

    // Start is called before the first frame update
    void Start()
    {
        if (active)
            UpdateLocation();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
            UpdateLocation();
    }

    public void UpdateLocation()
    {
        if (matchRectSize)
            MatchRectSize();

        if (matchLocation)
            MatchLocation();
    }

    public void MatchLocation()
    {
        if (location != null && anchor != null)
        {
            if (location.transform.position != anchor.transform.position)
                location.transform.position = anchor.transform.position;
        }
    }

    public void MatchRectSize()
    {
        if (location != null && anchor != null)
        {
            if (anchor.TryGetComponent<RectTransform>(out RectTransform _anchorRect))
            {
                if (location.TryGetComponent<RectTransform>(out RectTransform _locationRect))
                {
                    if (_locationRect.sizeDelta != _anchorRect.sizeDelta)
                        _locationRect.sizeDelta = _anchorRect.sizeDelta;

                    //_locationRect.rect.width = _anchorRect.rect.width;

                }
            }
        }
    }

}
