using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScaleBoxCollider_2D : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] RectTransform rect;
    [SerializeField] BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
            UpdateColliderSize();
    }

    public void Setup()
    {
        FindRectTransform();
        FindBoxCollider2D();
    }

    void FindRectTransform()
    {
        if (rect == null)
        {
            if (gameObject.TryGetComponent<RectTransform>(out RectTransform _rect))
                rect = _rect;
        }
    }

    void FindBoxCollider2D()
    {
        if(col == null)
        {
            if (gameObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D _col))
                col = _col;
        }
    }

    void UpdateColliderSize()
    {
        if (rect != null && col != null)
        {
            if (rect.hasChanged)
            {
                col.size = new Vector2(rect.rect.width, rect.rect.height);
            }
        }
    }

}
