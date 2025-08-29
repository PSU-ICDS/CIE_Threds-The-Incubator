using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevicePreview : MonoBehaviour
{
    [SerializeField] bool useLimitsRef;
    [SerializeField] TrackedVariables statsRef;
    [SerializeField] TrackedVariables.DesignedDevice device;
    [SerializeField] string utility_Bin;
    [SerializeField] UtilityCalculation_ThredGame utilityCalculator;
    [Space(10)]
    [SerializeField] Button runButton;
    [Space(10)]
    [SerializeField] PreviewsCount count;
    [Space(10)]
    [SerializeField] UI_Preview preview_UI;
    [Space(10)]
    [SerializeField] GameObject blockingObj;
    [Space(10)]
    [SerializeField] DebugSettings debug;

    [Serializable]
    public class PreviewsCount
    {
        public bool useLimits;
        [Space(10)]
        public int available;
        public int used;

        public bool PreviewsAvailable()
        {
            if (!useLimits)
                return true;

            if (available > 0 && available > used)
                return true;

            return false;
        }

        public void SetAvailable(int _available)
        {
            available = _available;
        }

        public void UsePreview()
        {
            if (PreviewsAvailable())
            {
                used++;
            }
        }

        public int GetRemainingCount()
        {
            return available - used;
        }

    }

    [Serializable]
    public class UI_Preview
    {
        public TMP_Text utilValue_TextObj;
        public TMP_Text part01Lvl_TextObj;
        public TMP_Text part02Lvl_TextObj;
        public TMP_Text part03Lvl_TextObj;
        public TMP_Text part04Lvl_TextObj;
        public TMP_Text part05Lvl_TextObj;
        public TMP_Text previewsCount_TextObj;

        public void UI_SetUtilVal(string _utilVal)
        {
            if (utilValue_TextObj != null)
                utilValue_TextObj.text = _utilVal;
        }

        public void UI_SetPart01Lvl(string _lvl)
        {
            if (part01Lvl_TextObj != null)
                part01Lvl_TextObj.text = _lvl;
        }

        public void UI_SetPart02Lvl(string _lvl)
        {
            if (part02Lvl_TextObj != null)
                part02Lvl_TextObj.text = _lvl;
        }

        public void UI_SetPart03Lvl(string _lvl)
        {
            if (part03Lvl_TextObj != null)
                part03Lvl_TextObj.text = _lvl;
        }
        public void UI_SetPart04Lvl(string _lvl)
        {
            if (part04Lvl_TextObj != null)
                part04Lvl_TextObj.text = _lvl;
        }
        public void UI_SetPart05Lvl(string _lvl)
        {
            if (part05Lvl_TextObj != null)
                part05Lvl_TextObj.text = _lvl;
        }

        public void UI_SetPreviewsCount(string _count)
        {
            if (previewsCount_TextObj != null)
                previewsCount_TextObj.text = _count;
        }
    }


    [Serializable]
    public class DebugSettings
    {
        public bool debug;
        public bool debug_RunUtilityCalc;
        [Space(10)]
        public bool debug_SetPreviewsCount;
        public int debug_PreviewsCount;
        [Space(10)]
        public bool debug_Part01_UpdateLevel;
        public int debug_Part01Level;
        [Space(10)]
        public bool debug_Part02_UpdateLevel;
        public int debug_Part02Level;
        [Space(10)]
        public bool debug_Part03_UpdateLevel;
        public int debug_Part03Level;
        [Space(10)]
        public bool debug_Part04_UpdateLevel;
        public int debug_Part04Level;
        [Space(10)]
        public bool debug_Part05_UpdateLevel;
        public int debug_Part05Level;

    }


    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        Blocker_CheckAvailablePreviewCount();
    }

    public void Setup()
    {
        FindTrackedVariables();
        Limits_GetCountLimitsFromRef();
        FindUtilityCalculator();
        UI_UpdatePreviewInfo();

        if (useLimitsRef)
            count.useLimits = true;

        if (count.useLimits)
        {
            preview_UI.UI_SetPreviewsCount(count.GetRemainingCount().ToString());
            Button_SetActiveState(count.PreviewsAvailable());
        }
        else
            preview_UI.UI_SetPreviewsCount("unlimited");
    }

    public void FindTrackedVariables()
    {
        if (useLimitsRef && statsRef == null)
        {
            if (gameObject.TryGetComponent<TrackedVariables>(out TrackedVariables _stats))
            {
                statsRef = _stats;
            }
        }
    }

    public void FindUtilityCalculator()
    {
        if (utilityCalculator == null)
        {
            if (gameObject.TryGetComponent<UtilityCalculation_ThredGame>(out UtilityCalculation_ThredGame _calc))
            {
                utilityCalculator = _calc;
            }
        }

        if (utilityCalculator != null)
            device.SetUitilityCalculator(utilityCalculator);
    }

    public void Limits_GetCountLimitsFromRef()
    {
        if (statsRef == null)
            FindTrackedVariables();

        if (statsRef != null)
        {
            if (useLimitsRef)
            {
                count.useLimits = true;
                count.SetAvailable(statsRef.FeedbackTotal);
                count.used = statsRef.FeedbackUsed;

            }
            //statsRef.Feedback_UseFeedback();
        }

        UI_UpdatePreviewInfo();
    }

    public void Utility_CalculateDeviceUtility()
    {
        if (utilityCalculator == null)
            FindUtilityCalculator();

        if (utilityCalculator != null)
        {
            Limits_GetCountLimitsFromRef();

            if (count.useLimits)
            {
                if (count.PreviewsAvailable())
                {
                    count.UsePreview();
                    if (useLimitsRef)
                    {
                        Feedback_UseFeedback();
                        Limits_GetCountLimitsFromRef();
                    }

                    utility_Bin = utilityCalculator.GetAccommodation(device.part_01.partLevel, device.part_02.partLevel, device.part_03.partLevel).ToString("00.00");
                    Debug.Log("UtilityCalc => OutputVal: " + utility_Bin + ", Pt01 Lvl: " + device.part_01.partLevel.ToString() + ", Pt02 Lvl: " + device.part_02.partLevel.ToString() + ", Pt03 Lvl: " + device.part_03.partLevel.ToString());
                    //stats.utilityString = utilityCalculator.GetAccommodation(device.part_01.utilityVal.ToString(), device.part_02.utilityVal.ToString(), device.part_03.utilityVal.ToString()).ToString();
                    UI_UpdatePreviewInfo();

                    preview_UI.UI_SetPreviewsCount(count.GetRemainingCount().ToString());
                    Button_SetActiveState(count.PreviewsAvailable());
                }
            }
            else
            {
                utility_Bin = utilityCalculator.GetAccommodation(device.part_01.partLevel, device.part_02.partLevel, device.part_03.partLevel).ToString("00.00");
                Debug.Log("UtilityCalc => OutputVal: " + utility_Bin + ", Pt01 Lvl: " + device.part_01.partLevel.ToString() + ", Pt02 Lvl: " + device.part_02.partLevel.ToString() + ", Pt03 Lvl: " + device.part_03.partLevel.ToString());
                //stats.utilityString = utilityCalculator.GetAccommodation(device.part_01.utilityVal.ToString(), device.part_02.utilityVal.ToString(), device.part_03.utilityVal.ToString()).ToString();
                UI_UpdatePreviewInfo();

                preview_UI.UI_SetPreviewsCount("unlimited");
            }
        }

        Limits_GetCountLimitsFromRef();

    }

    public void Feedback_UseFeedback()
    {
        if (statsRef == null)
            FindTrackedVariables();

        if (statsRef != null)
        {
            if (useLimitsRef)
            {
                statsRef.Feedback_UseFeedback();
            }
            //statsRef.Feedback_UseFeedback();
        }
    }

    public void Preview_SetPart01Lvl(int _lvl)
    {
        device.part_01.partLevel = _lvl;
        UI_UpdatePreviewInfo();
    }

    public void Preview_SetPart02Lvl(int _lvl)
    {
        device.part_02.partLevel = _lvl;
        UI_UpdatePreviewInfo();
    }

    public void Preview_SetPart03Lvl(int _lvl)
    {
        device.part_03.partLevel = _lvl;
        UI_UpdatePreviewInfo();
    }

    public void Preview_SetPart04Lvl(int _lvl)
    {
        device.part_04.partLevel = _lvl;
        UI_UpdatePreviewInfo();
    }

    public void Preview_SetPart05Lvl(int _lvl)
    {
        device.part_05.partLevel = _lvl;
        UI_UpdatePreviewInfo();
    }

    public void UI_UpdatePreviewInfo()
    {
        if (count.useLimits)
        {
            preview_UI.UI_SetPreviewsCount(count.GetRemainingCount().ToString());
            Button_SetActiveState(count.PreviewsAvailable());
        }
        else
            preview_UI.UI_SetPreviewsCount("unlimited");

        preview_UI.UI_SetUtilVal(utility_Bin);
        preview_UI.UI_SetPart01Lvl(device.part_01.partLevel.ToString());
        preview_UI.UI_SetPart02Lvl(device.part_02.partLevel.ToString());
        preview_UI.UI_SetPart03Lvl(device.part_03.partLevel.ToString());
        preview_UI.UI_SetPart04Lvl(device.part_04.partLevel.ToString());
        preview_UI.UI_SetPart05Lvl(device.part_05.partLevel.ToString());
    }

    public void Button_SetActiveState(bool _active)
    {
        if (runButton != null)
            runButton.interactable = _active;
    }

    public void Debug_CheckDebugSettings()
    {
        if (debug.debug)
        {
            if (debug.debug_SetPreviewsCount)
            {
                count.SetAvailable(debug.debug_PreviewsCount);
                debug.debug_SetPreviewsCount = false;
            }

            if (debug.debug_Part01_UpdateLevel)
            {
                Preview_SetPart01Lvl(debug.debug_Part01Level);
                debug.debug_Part01_UpdateLevel = false;
            }

            if (debug.debug_Part02_UpdateLevel)
            {
                Preview_SetPart02Lvl(debug.debug_Part02Level);
                debug.debug_Part02_UpdateLevel = false;
            }

            if (debug.debug_Part03_UpdateLevel)
            {
                Preview_SetPart03Lvl(debug.debug_Part03Level);
                debug.debug_Part03_UpdateLevel = false;
            }

            if (debug.debug_Part04_UpdateLevel)
            {
                Preview_SetPart04Lvl(debug.debug_Part04Level);
                debug.debug_Part04_UpdateLevel = false;
            }

            if (debug.debug_Part05_UpdateLevel)
            {
                Preview_SetPart05Lvl(debug.debug_Part05Level);
                debug.debug_Part05_UpdateLevel = false;
            }

            if (debug.debug_RunUtilityCalc)
            {
                Utility_CalculateDeviceUtility();
                debug.debug_RunUtilityCalc = false;
            }

        }
    }

    public void UI_ResetUtilityVal()
    {
        utility_Bin = "00";
        preview_UI.UI_SetUtilVal(utility_Bin);
    }

    public void Blocker_CheckAvailablePreviewCount()
    {
        if (blockingObj != null)
        {
            if (count.GetRemainingCount() <= 0)
                blockingObj.SetActive(true);
            else blockingObj.SetActive(false);
        }
    }

}
