using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_OverrideInputField : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] bool autoCopyToClipboard;
    [SerializeField] bool hasDisplayText;
    [SerializeField] string displayText;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text inputFieldText;
    [SerializeField] TMP_Text inputFieldPlaceholder;

    // Start is called before the first frame update
    void Start()
    {
        if (active) ApplyTextToInputField();
    }

    private void OnEnable()
    {
        if (active) ApplyTextToInputField();
    }

    public void SetDisplayText(string _text)
    {
        displayText = _text;
        hasDisplayText = true;
        ApplyTextToInputField();
    }

    public void ApplyTextToInputField()
    {
        if (hasDisplayText)
        {
            if (inputField != null)
                inputField.text = displayText;

            if (inputFieldText != null)
                inputFieldText.text = displayText;

            if (inputFieldPlaceholder != null)
                inputFieldPlaceholder.text = displayText;

            if (autoCopyToClipboard)
                CopyToClipboard_DisplayText();
        }
    }

    public void CopyToClipboard(string _str)
    {
        GUIUtility.systemCopyBuffer = _str;
        Debug.Log("Text copied to clipboard");
    }

    public void CopyToClipboard_DisplayText()
    {
        GUIUtility.systemCopyBuffer = displayText;
        Debug.Log("Display text copied to clipboard");
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
