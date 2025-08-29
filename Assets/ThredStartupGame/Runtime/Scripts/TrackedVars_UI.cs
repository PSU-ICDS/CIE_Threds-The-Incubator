using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackedVars_UI : MonoBehaviour
{

    //  Variables                   -----------------------------------------------------------------------------------------------------------------
    #region Variables

    [SerializeField] bool active;
    [SerializeField] TrackedVariables dataTracker;
    [SerializeField] DataPrint_Connector dataConnector;
    [SerializeField] string ui_ConnectorName;

    [Space(10)]
    [Header("Primary Stat Text")]
    [SerializeField] TMP_Text budgetText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text utilityTotalText;
    [SerializeField] TMP_Text utilityBinText;

    [Space(10)]
    [Header("Secondary Stat Text")]
    [SerializeField] TMP_Text decisionCountText;
    [SerializeField] TMP_Text interestPerMonthText;
    [SerializeField] TMP_Text partsDiscountText;
    [SerializeField] TMP_Text partsUnlockLevelText;
    [SerializeField] TMP_Text equityText;
    [SerializeField] TMP_Text feedbackLevelText;
    [SerializeField] TMP_Text globalMarketLevelText;

    [Space(10)]
    [SerializeField] TMP_Text fundingModifierText;
    [SerializeField] TMP_Text utilityModifierText;

    [Space(10)]
    [Header("Device Parts Text")]
    [SerializeField] TMP_Text partText_01_Level;
    [SerializeField] TMP_Text partText_01_Utility;
    [SerializeField] TMP_Text partText_02_Level;
    [SerializeField] TMP_Text partText_02_Utility;
    [SerializeField] TMP_Text partText_03_Level;
    [SerializeField] TMP_Text partText_03_Utility;
    [Space(10)]
    [Header("Values Text")]
    [SerializeField] ValuesInfo valuesInfo;
    [Space(10)]
    [Header("Game Timer Text")]
    [SerializeField] GamePlayTimer gameTimer;
    [SerializeField] TMP_Text timer_GameDuration;
    [SerializeField] TMP_Text timer_GameRunningState;
    [Space(10)]
    [Header("Progress Text")]
    [SerializeField] TMP_Text progressText_Months;
    [SerializeField] TMP_Text progressText_Weeks;
    [SerializeField] TMP_Text progressText_Days;

    [Space(10)]
    [Header("HUD Text")]
    [SerializeField] TMP_Text hudText_Budget;
    [SerializeField] TMP_Text hudText_Months;
    [SerializeField] TMP_Text hudText_Weeks;
    [SerializeField] TMP_Text hudText_Days;
    [Space(10)]
    [SerializeField] TMP_Text hudText_TeamName01;
    [SerializeField] TMP_Text hudText_TeamName02;
    [SerializeField] TMP_Text hudText_TeamName03;
    [SerializeField] TMP_Text hudText_TeamName04;
    [Space(10)]
    [SerializeField] bool usePartName;
    [SerializeField] TMP_Text hudText_Part01;
    [SerializeField] TMP_Text hudText_Part02;
    [SerializeField] TMP_Text hudText_Part03;
    [SerializeField] TMP_Text hudText_Part04;
    [SerializeField] TMP_Text hudText_Part05;
    [Space(10)]
    [SerializeField] TMP_Text hudText_Feedback;
    [SerializeField] TMP_Text hudText_FeedbackUsed;
    [SerializeField] TMP_Text hudText_GlobalMarket;

    [Space(10)]
    [Header("Endgame Printout Info")]
    [SerializeField] TMP_Text endGameInfoText;
    [SerializeField] TMP_Text fullPrintout_DecisionsText;
    [SerializeField] TMP_Text fullPrintout_TeamText;
    [SerializeField] TMP_Text fullPrintout_DeviceText;
    [SerializeField] TMP_Text fullPrintout_GameRoomText;
    [Space(10)]
    [SerializeField] TMP_Text dataFileLocation_Text;
    [SerializeField] TMP_Text dataFileLocationText_Text;
    [SerializeField] TMP_Text dataFileLocationPHP_Text;
    [Space(10)]
    [SerializeField] TMP_Text endGameInfo_TimeSpent;
    [SerializeField] TMP_Text endGameInfo_BudgetSpent;
    [SerializeField] TMP_Text endGameInfo_FinalUtilityVal;

    [Space(10)]
    [Header("Debug")]
    [SerializeField] bool debug;
    [SerializeField] bool debug_PrintEndGameInfo;


    #endregion

    //  Container Classes           -----------------------------------------------------------------------------------------------------------------
    #region Container Classes

    [Serializable]
    public class ValuesInfo
    {
        public TMP_Text budgetCostText_Lvl_01;
        public TMP_Text budgetCostText_Lvl_02;
        public TMP_Text budgetCostText_Lvl_03;
        [Space(10)]
        public TMP_Text timeCostText_Lvl_01;
        public TMP_Text timeCostText_Lvl_02;
        public TMP_Text timeCostText_Lvl_03;
        public TMP_Text timeCostText_Lvl_04;
        public TMP_Text timeCostText_Lvl_05;
        [Space(10)]
        [Space(10)]
        public TMP_Text partUtilText_Display_Lvl_01;
        public TMP_Text partUtilText_Display_Lvl_02;
        public TMP_Text partUtilText_Display_Lvl_03;
        [Space(10)]
        public TMP_Text partUtilText_Alarm_Lvl_01;
        public TMP_Text partUtilText_Alarm_Lvl_02;
        public TMP_Text partUtilText_Alarm_Lvl_03;
        [Space(10)]
        public TMP_Text partUtilText_Energy_Lvl_01;
        public TMP_Text partUtilText_Energy_Lvl_02;
        public TMP_Text partUtilText_Energy_Lvl_03;
        [Space(10)]
        public TMP_Text teamEffectValText_EngStudent;
        public TMP_Text teamEffectValText_EngExpert;
        public TMP_Text teamEffectValText_BusStudent;
        public TMP_Text teamEffectValText_BusExpert;


    }

    [Serializable]
    public class TeamInfo
    {
        //  TODO: Fill In Team Member Info Connectivity
    }

    [Serializable]
    public class SelectedDecisionsInfo
    {
        //  TODO: Fill In Selected Decisions Info Connectivity
    }

    #endregion

    //  Runtime Functions           -----------------------------------------------------------------------------------------------------------------
    #region Runtime Functions

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void LateUpdate()
    {
        UpdateUI();
        Debug_CheckDebugSettings();
    }

    public void UpdateUI()
    {
        if (dataTracker == null)
            FindDataTracker();

        if (gameTimer == null)
            FindGamePlayTimer();

        if (active && dataTracker != null)
        {
            UpdateUI_PrimaryStats();
            UpdateUI_DevicePartStats();
            //UpdateUI_CostValues();
            UpdateUI_ProgressStats();
            UpdateUI_HUDStats();

            if (gameTimer != null)
                UpdateUI_GamePlayTimerStats();

            UpdateUI_EndGameInfo_All();
        }

        UpdateUI_FilePathInfo();
    }

    #endregion

    //  Setup Functions             -----------------------------------------------------------------------------------------------------------------
    #region Setup Functions


    public void Setup()
    {
        FindDataTracker();
        FindGamePlayTimer();
        SaveFileLocation();
        UpdateUI();
    }

    public void FindDataTracker()
    {
        if (active && dataTracker == null)
        {
            if (GameObject.FindObjectOfType<TrackedVariables>() != null)
                dataTracker = GameObject.FindObjectOfType<TrackedVariables>();
        }
    }

    public void FindGamePlayTimer()
    {
        if (active && gameTimer == null)
        {
            if (GameObject.FindObjectOfType<GamePlayTimer>() != null)
                gameTimer = GameObject.FindObjectOfType<GamePlayTimer>();
        }
    }

    public void SaveFileLocation()
    {
        if (dataFileLocation_Text != null)
            dataFileLocation_Text.text = Application.persistentDataPath + "/";
    }

    #endregion

    //  UI Game Stats Functions     -----------------------------------------------------------------------------------------------------------------
    #region UI Game Stats Functions

    void UpdateUI_PrimaryStats()
    {
        if (budgetText != null)
            budgetText.text = dataTracker.PrimaryStat.budget.ToString();

        if (timeText != null)
            timeText.text = dataTracker.PrimaryStat.time.ToString("F0");
        //timeText.text = dataTracker.PrimaryStat.time.ToString("F2");

        if (utilityTotalText != null)
            utilityTotalText.text = dataTracker.PrimaryStat.utility.ToString();

        //decisionCountText
        if (decisionCountText != null)
            decisionCountText.text = dataTracker.Data.decisionsCount.ToString();


        //  Secondary Stats (Old)

        //fundingModifierText
        if (fundingModifierText != null)
            fundingModifierText.text = dataTracker.PrimaryStat.fundingModifier.ToString();

        //utilityModifierText
        if (utilityModifierText != null)
            utilityModifierText.text = dataTracker.PrimaryStat.utilityModifier.ToString();

        //  Secondary Stats (new - 06/13)

        //  Interest Per Month
        if (interestPerMonthText != null)
            interestPerMonthText.text = dataTracker.PrimaryStat.interestPerMonth.ToString();

        //  Parts Design Discount
        if (partsDiscountText != null)
            partsDiscountText.text = dataTracker.PrimaryStat.designDiscountModifier.ToString();

        //  Parts Unlock Level
        if (partsUnlockLevelText != null)
            partsUnlockLevelText.text = dataTracker.PrimaryStat.partsUnlockLevel.ToString();

        //  Equity
        if (equityText != null)
            equityText.text = dataTracker.PrimaryStat.equity.ToString();

        //  Feedback Level
        if (feedbackLevelText != null)
            feedbackLevelText.text = dataTracker.PrimaryStat.feedbackLevel.ToString();

        //  Global Market Level
        if (globalMarketLevelText != null)
            globalMarketLevelText.text = dataTracker.PrimaryStat.globalMarketLevel.ToString();

        //  Utility Bin
        if (utilityBinText != null)
            utilityBinText.text = dataTracker.PrimaryStat.utility_Bin.ToString();

    }

    void UpdateUI_DevicePartStats()
    {
        if (partText_01_Level != null)
            partText_01_Level.text = dataTracker.Device.part_01.partLevel.ToString();

        if (partText_01_Utility != null)
            partText_01_Utility.text = dataTracker.Device.part_01.utilityVal.ToString();

        if (partText_02_Level != null)
            partText_02_Level.text = dataTracker.Device.part_02.partLevel.ToString();

        if (partText_02_Utility != null)
            partText_02_Utility.text = dataTracker.Device.part_02.utilityVal.ToString();

        if (partText_03_Level != null)
            partText_03_Level.text = dataTracker.Device.part_03.partLevel.ToString();

        if (partText_03_Utility != null)
            partText_03_Utility.text = dataTracker.Device.part_03.utilityVal.ToString();
    }



    void UpdateUI_GamePlayTimerStats()
    {
        if (timer_GameDuration != null)
            timer_GameDuration.text = gameTimer.GameDuration.ToString() + " sec";

        if (timer_GameRunningState != null)
        {
            if (gameTimer.IsRunning)
                timer_GameRunningState.text = "Running";
            else
                timer_GameRunningState.text = "Stopped";
        }
    }

    void UpdateUI_ProgressStats()
    {
        if (progressText_Months != null)
        {
            progressText_Months.text = dataTracker.ProgressAndTotal_Months;
        }

        if (progressText_Weeks != null)
        {
            progressText_Weeks.text = dataTracker.ProgressAndTotal_Weeks;
        }

        if (progressText_Days != null)
        {
            progressText_Days.text = dataTracker.ProgressAndTotal_Days;
        }
    }

    void UpdateUI_FilePathInfo()
    {
        if (dataConnector == null)
        {
            if (GameObject.FindObjectOfType<DataPrint_Connector>() != null)
            {
                dataConnector = GameObject.FindObjectOfType<DataPrint_Connector>();
            }
        }

        if (dataConnector != null)
        {
            if (dataFileLocation_Text != null)
            {
                dataFileLocation_Text.text = dataConnector.FilePath_Applied;
            }

            if (dataFileLocationText_Text != null)
            {
                dataFileLocationText_Text.text = dataConnector.FilePath_DefaultText;
            }

            if (dataFileLocationPHP_Text != null)
            {
                dataFileLocationPHP_Text.text = dataConnector.FilePath_DefaultPHP;
            }
        }
    }

    #endregion

    //  UI HUD Functions            -----------------------------------------------------------------------------------------------------------------
    #region UI HUD Functions

    void UpdateUI_HUDStats()
    {
        UpdateUI_HUDStats_Resources();
        UpdateUI_HUDStats_Team();
        UpdateUI_HUDStats_Device();
        UpdateUI_HUDStats_CustomerDiscovery();
    }

    void UpdateUI_HUDStats_Resources()
    {
        if (hudText_Budget != null)
            hudText_Budget.text = dataTracker.PrimaryStat.budget.ToString();

        if (hudText_Months != null)
            hudText_Months.text = dataTracker.ProgressAndTotal_Months;

        if (hudText_Weeks != null)
            hudText_Weeks.text = dataTracker.ProgressAndTotal_Weeks;

        if (hudText_Days != null)
            hudText_Days.text = dataTracker.ProgressAndTotal_Days;
    }

    void UpdateUI_HUDStats_Team()
    {
        if (hudText_TeamName01 != null)
            hudText_TeamName01.text = dataTracker.Team_GetMemberNameByIndex(0);

        if (hudText_TeamName02 != null)
            hudText_TeamName02.text = dataTracker.Team_GetMemberNameByIndex(1);

        if (hudText_TeamName03 != null)
            hudText_TeamName03.text = dataTracker.Team_GetMemberNameByIndex(2);

        if (hudText_TeamName04 != null)
            hudText_TeamName04.text = dataTracker.Team_GetMemberNameByIndex(3);
    }

    void UpdateUI_HUDStats_Device()
    {
        if (usePartName)
        {
            if (hudText_Part01 != null)
                hudText_Part01.text = dataTracker.Device_GetPartNameByPartNum(1);

            if (hudText_Part02 != null)
                hudText_Part02.text = dataTracker.Device_GetPartNameByPartNum(2);

            if (hudText_Part03 != null)
                hudText_Part03.text = dataTracker.Device_GetPartNameByPartNum(3);

            if (hudText_Part04 != null)
                hudText_Part04.text = dataTracker.Device_GetPartNameByPartNum(4);

            if (hudText_Part05 != null)
                hudText_Part05.text = dataTracker.Device_GetPartNameByPartNum(5);
        }
        else
        {
            if (hudText_Part01 != null)
                hudText_Part01.text = dataTracker.Device_GetPartLevelByPartNum(1);

            if (hudText_Part02 != null)
                hudText_Part02.text = dataTracker.Device_GetPartLevelByPartNum(2);

            if (hudText_Part03 != null)
                hudText_Part03.text = dataTracker.Device_GetPartLevelByPartNum(3);

            if (hudText_Part04 != null)
                hudText_Part04.text = dataTracker.Device_GetPartLevelByPartNum(4);

            if (hudText_Part05 != null)
                hudText_Part05.text = dataTracker.Device_GetPartLevelByPartNum(5);
        }
    }

    void UpdateUI_HUDStats_CustomerDiscovery()
    {
        if (hudText_Feedback != null)
            hudText_Feedback.text = dataTracker.FeedbackTotal.ToString();

        if (hudText_FeedbackUsed != null)
            hudText_FeedbackUsed.text = dataTracker.FeedbackUsed.ToString();

        if (hudText_GlobalMarket != null)
            hudText_GlobalMarket.text = dataTracker.GlobalMarketLevel.ToString();
    }

    #endregion

    //  UI Print Info Functions     -----------------------------------------------------------------------------------------------------------------
    #region UI Print Info Functions

    public void UpdateUI_PrintEndgameInfo()
    {
        if (endGameInfoText != null && dataTracker != null)
        {
            endGameInfoText.text = dataTracker.Info_GetEndgameStats();
            //endGameInfoText.text = dataTracker.Data.Info_GetOutputInfo(dataTracker.Data.info_addLabels);
        }
    }

    public void UpdateUI_PrintAllGameInfo()
    {
        UpdateUI_PrintEndgameInfo();
        UpdateUI_PrintFullInfo_All();
    }

    public void UpdateUI_PrintFullInfo_All()
    {
        UpdateUI_PrintFullInfo_Decisions();
        UpdateUI_PrintFullInfo_Team();
        UpdateUI_PrintFullInfo_Device();
        UpdateUI_PrintFullInfo_GameRooms();
    }

    public void UpdateUI_PrintFullInfo_Decisions()
    {
        if (fullPrintout_DecisionsText != null && dataTracker != null)
        {
            fullPrintout_DecisionsText.text = dataTracker.Info_GetFullPrintout_Decisions();
        }
    }

    public void UpdateUI_PrintFullInfo_Team()
    {
        if (fullPrintout_TeamText != null && dataTracker != null)
        {
            fullPrintout_TeamText.text = dataTracker.Info_GetFullPrintout_Team();
        }
    }

    public void UpdateUI_PrintFullInfo_Device()
    {
        if (fullPrintout_DeviceText != null && dataTracker != null)
        {
            fullPrintout_DeviceText.text = dataTracker.Info_GetFullPrintout_Device();
        }
    }

    public void UpdateUI_PrintFullInfo_GameRooms()
    {
        if (fullPrintout_GameRoomText != null && dataTracker != null)
        {
            fullPrintout_GameRoomText.text = dataTracker.Info_GetFullPrintout_GameRooms();
        }
    }

    #endregion

    //  End Game Info Functions     -----------------------------------------------------------------------------------------------------------------
    #region End Game Info Functions

    public void UpdateUI_EndGameInfo_All()
    {
        UpdateUI_EndGameInfo_TimeSpent();
        UpdateUI_EndGameInfo_BudgetSpent();
        UpdateUI_EndGameInfo_FinalUtilityVal();
    }

    public void UpdateUI_EndGameInfo_TimeSpent()
    {
        if (endGameInfo_TimeSpent != null && dataTracker != null)
        {
            endGameInfo_TimeSpent.text = dataTracker.Info_EndGame_GetPrint_TimeSpent();
        }
    }

    public void UpdateUI_EndGameInfo_BudgetSpent()
    {
        if (endGameInfo_BudgetSpent != null && dataTracker != null)
        {
            endGameInfo_BudgetSpent.text = dataTracker.Info_EndGame_GetPrint_BudgetSpent();
        }
    }

    public void UpdateUI_EndGameInfo_FinalUtilityVal()
    {
        if (endGameInfo_FinalUtilityVal != null && dataTracker != null)
        {
            endGameInfo_FinalUtilityVal.text = dataTracker.Info_EndGame_GetPrint_FinalAccommodation();
            //endGameInfo_FinalUtilityVal.text = dataTracker.Info_EndGame_GetPrint_FinalAccommodation();
        }
    }

    #endregion

    //  Utility Functions           -----------------------------------------------------------------------------------------------------------------
    #region Utility Functions

    public float ConvertStringToFloat(string _stringValue)
    {
        if (float.TryParse(_stringValue, out float result))
        {
            return result;
        }

        return -2000.0f;
    }

    public float GetPercentageFromFloat(float _value)
    {
        if (_value <= 1.0f)
        {
            return _value * 100.0f;
        }

        return _value;
    }

    public void Debug_CheckDebugSettings()
    {
        if (debug)
        {
            if (debug_PrintEndGameInfo)
            {
                UpdateUI_PrintAllGameInfo();
                debug_PrintEndGameInfo = false;
            }
        }
    }

    #endregion

}
