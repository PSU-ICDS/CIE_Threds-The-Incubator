using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImageColor : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Color color_01;
    [SerializeField] Color color_02;

    // Start is called before the first frame update
    void Start()
    {
        FindImage();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    void FindImage()
    {
        if(image == null)
        {
            if(gameObject.TryGetComponent<Image>(out Image _image))
            {
                image = _image;
            }
        }
    }

    public void Color_Set01()
    {
        if(image != null && color_01 != null)
        {
            image.color = color_01;
        }
    }

    public void Color_Set02()
    {
        if (image != null && color_02 != null)
        {
            image.color = color_02;
        }
    }


}
