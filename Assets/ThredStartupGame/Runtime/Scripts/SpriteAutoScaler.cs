using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAutoScaler : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] RectTransform parentCanvas;
    [SerializeField] float savedWidth;
    [SerializeField] float savedHeight;
    [SerializeField] GameObject spriteObj;
    [Space(15)]
    [SerializeField] Vector3 relativeSpriteScaling;

    // Start is called before the first frame update
    void Start()
    {
        ScreenSpace_SaveCurrentRatioValues();
        ScreenSpace_UpdateSpriteScale();
    }

    // Update is called once per frame
    void Update()
    {
        ScreenSpace_CheckRatioValuesChange();
    }

    private void OnEnable()
    {
        ScreenSpace_SaveCurrentRatioValues();
        ScreenSpace_UpdateSpriteScale();
    }

    public void ScreenSpace_SaveCurrentRatioValues()
    {
        if (active && parentCanvas != null)
        {
            savedWidth = parentCanvas.rect.width;
            savedHeight = parentCanvas.rect.height;
        }
    }


    public void ScreenSpace_CheckRatioValuesChange()
    {
        if (active && parentCanvas != null)
        {
            if (savedWidth != parentCanvas.rect.width || savedHeight != parentCanvas.rect.height)
                ScreenSpace_UpdateSpriteScale();

            ScreenSpace_SaveCurrentRatioValues();
        }
    }

    public void ScreenSpace_UpdateSpriteScale()
    {
        if (active && spriteObj != null)
        {
            float _scalarX = savedWidth / 1469.0f;
            spriteObj.transform.localScale = Vector3.one * _scalarX;
            relativeSpriteScaling = Vector3.one * _scalarX;
        }
    }




}
