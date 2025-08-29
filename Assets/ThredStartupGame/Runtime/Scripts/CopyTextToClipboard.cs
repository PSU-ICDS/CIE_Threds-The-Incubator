using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CopyTextToClipboard : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button copyButton;

    // Start is called before the first frame update
    void Start()
    {
        if (active && copyButton != null && input != null)
        {
            copyButton.onClick.AddListener(() =>
            {
                //input.text.CopyToClipboard();
                CopyToClipboard(input.text);
            });
        }
    }

    public void CopyToClipboard(string _str)
    {
        GUIUtility.systemCopyBuffer = _str;
        Debug.Log("Text copied to clipboard");
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
