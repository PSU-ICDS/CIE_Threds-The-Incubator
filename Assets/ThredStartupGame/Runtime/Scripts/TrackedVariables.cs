/*
 * Name: TrackedVariables
 * Project: Threds Startup Game Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 05/02/2024
 * Description: This script holds the primary stats/variables for the game
 */

using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;

public class TrackedVariables : MonoBehaviour
{
    private static TrackedVariables _trackedVariables;

    public static TrackedVariables TrackedInfo { get { return _trackedVariables; } }


    //  Public Enums          ----------------------------------------------------------------------------------------------------------------
    #region Enums

    public enum CategoryType { NONE, TEAM, NETWORKING, FUNDING, CUSTOMER_DISCOVERY, DEVICE }
    public enum ResourceCostType { NONE, BUDGET, TIME, OTHER }
    public enum TeamMemberType { NONE, TYPE_01, TYPE_02, TYPE_03, TYPE_04, TYPE_05, TYPE_06 }
    public enum FundingType { NONE, BANK, INVESTMENT, GRANT, OTHER }
    public enum PartTypes { NONE, PART_01, PART_02, PART_03, PART_04, PART_05 }
    public enum TimeIntervals { NONE, DAY, WEEK, MONTH, MINUTES }
    public enum EffectType { NONE, UNLOCK, AWARD, DISCOUNT, INTEREST, EQUITY, FEEDBACK, OTHER }
    public enum UnlockType { NONE, TYPE_01, TYPE_02, TYPE_03, TYPE_04, TYPE_05, TYPE_06 }
    //public enum AwardType { NONE, BUDGET, TIME, PART, TEAM }

    #endregion

    //  Variables          ----------------------------------------------------------------------------------------------------------------
    #region Variables

    [SerializeField] PrimaryStats stats;
    [Space(10)]
    [SerializeField] TimeSpanBreakdowns timeUnits;
    [Space(10)]
    [SerializeField] UtilityCalculation_ThredGame utilityCalculator;
    [Space(10)]
    [SerializeField] CollectedData data;
    [Space(10)]
    [SerializeField] DesignedDevice device;
    [Space(10)]
    [SerializeField] GameRooms rooms;
    [Space(10)]
    [SerializeField] TimestampTracker timeTracker;
    [Space(10)]
    [SerializeField] GamePlayTimer gamePlayTimer;
    [Space(10)]
    [Header("Value Settings")]
    [SerializeField] CostValues costValues;
    [Space(10)]
    [SerializeField] TeamValues teamValues;
    [Space(10)]
    [SerializeField] PartValues partsValues;
    [Space(10)]
    [SerializeField] UnlockEvents unlockEvents;
    [Space(10)]
    [Header("Team Settings")]
    [SerializeField] List<TeamMember> team;
    [Space(10)]
    [Header("Checkpoint Settings")]
    [SerializeField] bool useCheckpoints;
    [SerializeField] UnityEvent decisionAddedEvent;
    [Space(10)]
    [SerializeField] List<DecisionEntry> decisions;
    [Space(10)]
    public List<CharacterController_2D> players;

    [Space(10)]
    [SerializeField] string delimiter;
    [SerializeField] bool useAlternateDelimiter;
    [SerializeField] string altDelimiter;
    [Space(10)]
    [SerializeField] bool debug;
    [SerializeField] bool debug_SelectEveryDecision;
    [SerializeField] bool isDebugData;

    #endregion

    //  Properties          ----------------------------------------------------------------------------------------------------------------
    #region Properties

    public PrimaryStats PrimaryStat { get => stats; }
    public CollectedData Data { get => data; }
    public DesignedDevice Device { get => device; }
    public TimestampTracker TimeTracker { get => timeTracker; }
    public CostValues CostsValues { get => costValues; }
    public TeamValues TeamsValues { get => teamValues; }
    public PartValues PartsValues { get => partsValues; }
    public List<TeamMember> TeamMembers { get => team; }
    public List<DecisionEntry> Decisions { get => decisions; }

    public float Progress_Days { get => timeUnits.Progress_GetDaysPassed(); }
    public float Progress_Weeks { get => timeUnits.Progress_GetWeeksPassed(); }
    public float Progress_Months { get => timeUnits.Progress_GetMonthsPassed(); }

    public string ProgressAndTotal_Days { get => timeUnits.Progress_GetDays_PassedAndTotal(); }
    public string ProgressAndTotal_Weeks { get => timeUnits.Progress_GetWeeks_PassedAndTotal(); }
    public string ProgressAndTotal_Months { get => timeUnits.Progress_GetMonths_PassedAndTotal(); }

    public int FeedbackTotal { get => stats.feedbackLevel; }
    public int FeedbackUsed { get => stats.feedback_previewsUsed; }
    public int GlobalMarketLevel { get => stats.globalMarketLevel; }

    public bool IsDebugData { get => isDebugData; }

    #endregion

    //  Container Classes   -------------------------------------------------------------------------------------------------
    #region Container Classes

    [Serializable]
    public class PrimaryStats
    {
        //  Primary Tracked Variables/Stats
        [Header("Primary Tracked Variables/Stats")]
        public float budget;
        public float time;
        public float utility;
        public string utility_Bin;
        //public string utilityString;

        //  Secondary Tracked Variables/Stats
        [Header("Secondary Tracked Variables/Stats")]
        public float fundingModifier;
        public float utilityModifier;
        public float designDiscountModifier;
        public int partsUnlockLevel = 1;
        public float equity;
        public float interestPerMonth;
        public int feedbackLevel;
        public int feedback_previewsUsed;
        public bool globalMarketUnlocked = false;
        public int globalMarketLevel;

        [Space(10)]
        public float reset_budget;
        public float reset_time;

        public void AddFunding(float _fundingAmount)
        {
            budget += _fundingAmount;

            //  TODO
            //Check budget threshhold values and trigger events accordingly
        }

        public void Spend_Budget(float _costAmount)
        {
            budget -= _costAmount;

            //  TODO
            //Check budget threshhold values and trigger events accordingly
        }

        public void Spend_Time(float _costAmount)
        {
            time -= _costAmount;

            //  TODO
            //Check time threshhold values and trigger events accordingly
        }

        public void UpdateUtilityValue(float _newUtility)
        {
            utility = _newUtility + utilityModifier;

            //  TODO
            //Check utility threshhold values and trigger events accordingly
        }

        public void UpdateEquityValue(float _equity)
        {
            equity += _equity;
        }

        public void UpdateInterestPerMonth(float _interestAmount)
        {
            interestPerMonth += _interestAmount;
        }

        public void UpdateFeedbackLevel(int _newLevel)
        {
            if (_newLevel > feedbackLevel)
                feedbackLevel = _newLevel;
        }

        //public void SpendFeedback()
        //{
        //    if(feedbackLevel > 0 && feedback_previewsUsed < feedbackLevel)
        //    {
        //        feedback_previewsUsed++;
        //    }
        //}

        public void UnlockGlobalMarket()
        {
            globalMarketUnlocked = true;
        }

        public void UpdateGlobalMarketLevel(int _newLevel)
        {
            if (_newLevel > globalMarketLevel)
                globalMarketLevel = _newLevel;
        }

        public void UpdatePartsUnlockLevel(int _newLevel)
        {
            if (_newLevel > partsUnlockLevel)
                partsUnlockLevel = _newLevel;
        }

        public void UpdateDesignDiscountModifier(float _discountAmount)
        {
            designDiscountModifier += _discountAmount;
        }

        public float GetDiscountedDesignCost(float _originalCost)
        {
            float _val = _originalCost;

            if (designDiscountModifier > 0.0f)
            {
                float discount = _originalCost / 100.0f;
                discount = discount * designDiscountModifier;
                _val = _originalCost - discount;
            }

            return _val;
        }

        public void ResourceValues_SaveStarting()
        {
            reset_budget = budget;
            reset_time = time;
        }

        public void ResourceValues_Reset()
        {
            budget = reset_budget;
            time = reset_time;
            utility = 0.0f;
            utility_Bin = "00.00";
            fundingModifier = 0.0f;
            utilityModifier = 0.0f;
            designDiscountModifier = 0.0f;
            partsUnlockLevel = 1;
            equity = 0.0f;
            interestPerMonth = 0.0f;
            feedbackLevel = 0;
            feedback_previewsUsed = 0;
            globalMarketUnlocked = false;
            globalMarketLevel = 0;

        }

        public void Interest_ReduceBudget()
        {
            Spend_Budget(interestPerMonth);
        }

    }

    [Serializable]
    public class CollectedData
    {
        public bool info_addOutputKey;
        public bool info_addLabels;
        public string delimiter;

        //  Number of Decisions Made
        public int decisionsCount;

        [Space(10)]
        //  Budget Info
        public float budget_RemainingAtGameOver;
        public float budget_TotalSpent;

        [Space(10)]
        //  Time Info
        public float time_RemainingAtGameOver;
        public float time_TotalSpent;

        [Space(10)]
        //  Team Info
        public int team_NumMembersHired;
        public string team_TypesHired;

        [Space(10)]
        //  Funding Info
        public float funding_TotalGained;
        public int funding_TotalAttempts;
        public int funding_TotalAttempts_Success;
        public int funding_TotalAttempts_Fail;
        public string funding_TotalTypesAndLevels;
        public float funding_InterestPerMonth;
        public float funding_InterestPaidTotal;
        public float funding_Equity;

        [Space(10)]
        //  Design Device
        public float designDevice_Utility;
        public string designDevice_UtilityBinary;
        public int designDevice_Part01_Level;
        public int designDevice_Part02_Level;
        public int designDevice_Part03_Level;

        [Space(10)]
        //  Customer Discovery
        public int customerDiscovery_FeedbackLevel;
        public int customerDiscovery_GlobalMarketLevel;

        [Space(10)]
        //  Game Rooms
        public float roomTime_Main;
        public float roomTime_ResearchDev;
        public float roomTime_CustomerDisc;
        public float roomTime_Hiring;
        public float roomTime_Funding;
        public float roomTime_Networking;

        [Space(10)]
        //  Output Info Strings
        public string decisionsInfo;
        public string budgetInfo;
        public string timeInfo;
        public string teamInfo;
        public string fundingInfo;
        public string designDeviceInfo;
        public string customerDiscoveryInfo;
        public string gameRoomsInfo;
        public string outputInfo;

        [Space(10)]
        //  Full Sets - Output Info Strings
        public string fullPrintout_Decisions;
        public string fullPrintout_Team;
        public string fullPrintout_Device;

        [Space(10)]
        public int checkPointNumber;
        //public int info_LastDecisionIndexPrinted;



        //  Decisions Functions
        public void Decisions_SetCount(int _decisionCount)
        {
            decisionsCount = _decisionCount;
        }

        //  Budget Functions
        public void Budget_SetEndTotal(float _endBudget)
        {
            budget_RemainingAtGameOver = _endBudget;
        }

        public void Budget_AddToTotalSpent(float _spent)
        {
            budget_TotalSpent += _spent;
        }


        //  Time Functions
        public void Time_SetEndTotal(float _endTime)
        {
            time_RemainingAtGameOver = _endTime;
        }

        public void Time_AddToTotalSpent(float _spent)
        {
            time_TotalSpent += _spent;
        }

        //  Team Functions
        public void Team_SetNumberHired(int _hiredNumber)
        {
            team_NumMembersHired = _hiredNumber;
        }

        public void Team_AddTypesHired(string _hiredType)
        {
            team_TypesHired = team_TypesHired + _hiredType + delimiter;
            //team_TypesHired = team_TypesHired + _hiredType + ", ";
        }

        public void Team_SetTypesHired_Full(string _hiredTypesFull)
        {
            team_TypesHired = _hiredTypesFull;
        }

        //  Funding Functions
        public void Funding_AddToTotalGained(float _newFunds)
        {
            funding_TotalGained += _newFunds;
        }

        public void Funding_AddFundingAttempt(bool _success)
        {
            funding_TotalAttempts++;

            if (_success)
                funding_TotalAttempts_Success++;
            else
                funding_TotalAttempts_Fail++;
        }

        public void Funding_AddTypeAndLevel(string _type, string _level)
        {
            funding_TotalTypesAndLevels = funding_TotalTypesAndLevels + _type + delimiter + " " + _level + delimiter + " ";
            //funding_TotalTypesAndLevels = funding_TotalTypesAndLevels + _type + ", " + _level + ", ";
        }

        public void Funding_SetInterestPerMonthAtEndgame(float _interestPerMonth)
        {
            funding_InterestPerMonth = _interestPerMonth;
        }

        public void Funding_AddInterestPayment(float _interestPayment)
        {
            funding_InterestPaidTotal += _interestPayment;
        }

        public void Funding_SetEquity(float _equity)
        {
            funding_Equity = _equity;
        }


        //  Design Device Functions
        public void DesignDevice_SetUtility(float _utility)
        {
            designDevice_Utility = _utility;
        }

        public void DesignDevice_SetUtility_Binary(string _utilityBin)
        {
            designDevice_UtilityBinary = _utilityBin;
        }

        public void DesignDevice_SetPartLevel(int _partNum, int _partLevel)
        {
            if (_partNum == 1)
                designDevice_Part01_Level = _partLevel;
            if (_partNum == 2)
                designDevice_Part02_Level = _partLevel;
            if (_partNum == 3)
                designDevice_Part03_Level = _partLevel;
        }

        public void DesignDevice_SetPartLevel(PartTypes _partNum, int _partLevel)
        {
            switch (_partNum)
            {
                case PartTypes.NONE:
                    break;
                case PartTypes.PART_01:
                    designDevice_Part01_Level = _partLevel;
                    break;
                case PartTypes.PART_02:
                    designDevice_Part02_Level = _partLevel;
                    break;
                case PartTypes.PART_03:
                    designDevice_Part03_Level = _partLevel;
                    break;
                case PartTypes.PART_04:
                    break;
                case PartTypes.PART_05:
                    break;
                default:
                    break;
            }

            //if (_partNum == 1)
            //    designDevice_Part01_Level = _partLevel;
            //if (_partNum == 2)
            //    designDevice_Part02_Level = _partLevel;
            //if (_partNum == 3)
            //    designDevice_Part03_Level = _partLevel;
        }


        //  Customer Discovery Functions
        public void SetFeedbackLevel(int _feedbaclLevel)
        {
            customerDiscovery_FeedbackLevel = _feedbaclLevel;
        }

        public void SetGlobalMarketLevel(int _globalMarketLevel)
        {
            customerDiscovery_GlobalMarketLevel = _globalMarketLevel;
        }

        public void SetGameRoomTimesInfo(GameRooms _rooms)
        {
            roomTime_Main = _rooms.GameRoom_GetRoomTime_Main();
            roomTime_ResearchDev = _rooms.GameRoom_GetRoomTime_Research();
            roomTime_CustomerDisc = _rooms.GameRoom_GetRoomTime_CustomerDiscovery();
            roomTime_Hiring = _rooms.GameRoom_GetRoomTime_Hiring();
            roomTime_Funding = _rooms.GameRoom_GetRoomTime_Funding();
            roomTime_Networking = _rooms.GameRoom_GetRoomTime_Networking();
        }

        //  Info String Update Functions

        public void InfoUpdate_All(bool _addLabels)
        {
            InfoUpdate_Decisions(_addLabels);
            InfoUpdate_Budget(_addLabels);
            InfoUpdate_Time(_addLabels);
            InfoUpdate_Team(_addLabels);
            InfoUpdate_Funding(_addLabels);
            InfoUpdate_Device(_addLabels);
            InfoUpdate_CustomerDiscovery(_addLabels);
            InfoUpdate_GameRoomTimes(_addLabels);
        }

        public void InfoUpdate_Decisions(bool _addLabels)
        {
            if (_addLabels)
                decisionsInfo = "Number of Decisions: " + decisionsCount.ToString();
            else
                decisionsInfo = decisionsCount.ToString();
        }

        public void InfoUpdate_Budget(bool _addLabels)
        {
            if (_addLabels)
                budgetInfo = "Budget remaining at endgame: " + budget_RemainingAtGameOver.ToString() + delimiter + " Total budget spend: " + budget_TotalSpent.ToString();
            else
                budgetInfo = budget_RemainingAtGameOver.ToString() + delimiter + " " + budget_TotalSpent.ToString();

            //if (_addLabels)
            //    budgetInfo = "Budget remaining at endgame: " + budget_RemainingAtGameOver.ToString() + ", Total budget spend: " + budget_TotalSpent.ToString();
            //else
            //    budgetInfo = budget_RemainingAtGameOver.ToString() + ", " + budget_TotalSpent.ToString();
        }

        public void InfoUpdate_Time(bool _addLabels)
        {
            if (_addLabels)
                timeInfo = "Time remaining at endgame: " + time_RemainingAtGameOver.ToString() + delimiter + " Total time spend: " + time_TotalSpent.ToString();
            else
                timeInfo = time_RemainingAtGameOver.ToString() + delimiter + " " + time_TotalSpent.ToString();

            //if (_addLabels)
            //    timeInfo = "Time remaining at endgame: " + time_RemainingAtGameOver.ToString() + ", Total time spend: " + time_TotalSpent.ToString();
            //else
            //    timeInfo = time_RemainingAtGameOver.ToString() + ", " + time_TotalSpent.ToString();
        }

        public void InfoUpdate_Team(bool _addLabels)
        {
            if (_addLabels)
                teamInfo = "Number of hired team members: " + team_NumMembersHired.ToString() + delimiter + " Team members hired by type: " + team_TypesHired;
            else
                teamInfo = team_NumMembersHired.ToString() + delimiter + " " + team_TypesHired;

            //if (_addLabels)
            //    teamInfo = "Number of hired team members: " + team_NumMembersHired.ToString() + ", Team members hired by type: " + team_TypesHired;
            //else
            //    teamInfo = team_NumMembersHired.ToString() + ", " + team_TypesHired;
        }

        public void InfoUpdate_Funding(bool _addLabels)
        {
            if (_addLabels)
            {

                fundingInfo =
                "Total funding gained: " + funding_TotalGained.ToString() + delimiter + " " +
                "Funding attempts total: " + funding_TotalAttempts.ToString() + delimiter + " " +
                "Funding attepmts successful: " + funding_TotalAttempts_Success.ToString() + delimiter + " " +
                "Funding attempts failed: " + funding_TotalAttempts_Fail.ToString() + delimiter + " " +
                "Funding types and levels: " + funding_TotalTypesAndLevels + delimiter + " " +
                "Interest per month: " + funding_InterestPerMonth.ToString() + delimiter + " " +
                "Total interest paid: " + funding_InterestPaidTotal.ToString() + delimiter + " " +
                "Equity: " + funding_Equity.ToString();
            }
            else
            {
                fundingInfo = funding_TotalGained.ToString() + delimiter + " " + funding_TotalAttempts.ToString() + delimiter + " " + funding_TotalAttempts_Success.ToString() + delimiter + " " + funding_TotalAttempts_Fail.ToString() +
                     delimiter + " " + funding_TotalTypesAndLevels + delimiter + " " + funding_InterestPerMonth.ToString() + delimiter + " " + funding_InterestPaidTotal.ToString() + delimiter + " " + funding_Equity.ToString();
            }

            //if (_addLabels)
            //{

            //    fundingInfo =
            //    "Total funding gained: " + funding_TotalGained.ToString() + ", " +
            //    "Funding attempts total: " + funding_TotalAttempts.ToString() + ", " +
            //    "Funding attepmts successful: " + funding_TotalAttempts_Success.ToString() + ", " +
            //    "Funding attempts failed: " + funding_TotalAttempts_Fail.ToString() + ", " +
            //    "Funding types and levels: " + funding_TotalTypesAndLevels + ", " +
            //    "Interest per month: " + funding_InterestPerMonth.ToString() + ", " +
            //    "Total interest paid: " + funding_InterestPaidTotal.ToString() + ", " +
            //    "Equity: " + funding_Equity.ToString();
            //}
            //else
            //{
            //    fundingInfo = funding_TotalGained.ToString() + ", " + funding_TotalAttempts.ToString() + ", " + funding_TotalAttempts_Success.ToString() + ", " + funding_TotalAttempts_Fail.ToString() +
            //         ", " + funding_TotalTypesAndLevels + ", " + funding_InterestPerMonth.ToString() + ", " + funding_InterestPaidTotal.ToString() + ", " + funding_Equity.ToString();
            //}
        }

        public void InfoUpdate_Device(bool _addLabels)
        {
            if (_addLabels)
            {
                designDeviceInfo =
                    "Device Utility: " + designDevice_Utility.ToString() + delimiter + " " +
                    "Device Utility (Binary): " + designDevice_UtilityBinary.ToString() + delimiter + " " +
                    "Part 01 level: " + designDevice_Part01_Level.ToString() + delimiter + " " +
                    "Part 02 level: " + designDevice_Part02_Level.ToString() + delimiter + " " +
                    "Part 03 level: " + designDevice_Part03_Level.ToString();
            }
            else
            {
                designDeviceInfo =
                    designDevice_Utility.ToString() + delimiter + " " +
                    designDevice_UtilityBinary.ToString() + delimiter + " " +
                    designDevice_Part01_Level.ToString() + delimiter + " " +
                    designDevice_Part02_Level.ToString() + delimiter + " " +
                    designDevice_Part03_Level.ToString();
            }

            //if (_addLabels)
            //{
            //    designDeviceInfo =
            //        "Device Utility: " + designDevice_Utility.ToString() + ", " +
            //        "Device Utility (Binary): " + designDevice_UtilityBinary.ToString() + ", " +
            //        "Part 01 level: " + designDevice_Part01_Level.ToString() + ", " +
            //        "Part 02 level: " + designDevice_Part02_Level.ToString() + ", " +
            //        "Part 03 level: " + designDevice_Part03_Level.ToString();
            //}
            //else
            //{
            //    designDeviceInfo =
            //        designDevice_Utility.ToString() + ", " +
            //        designDevice_UtilityBinary.ToString() + ", " +
            //        designDevice_Part01_Level.ToString() + ", " +
            //        designDevice_Part02_Level.ToString() + ", " +
            //        designDevice_Part03_Level.ToString();
            //}
        }

        public void InfoUpdate_CustomerDiscovery(bool _addLabels)
        {
            if (_addLabels)
            {
                customerDiscoveryInfo =
                   "Feedback level: " + customerDiscovery_FeedbackLevel.ToString() + delimiter + " " +
                   "Global market level: " + customerDiscovery_GlobalMarketLevel.ToString();
            }
            else
            {
                customerDiscoveryInfo = customerDiscovery_FeedbackLevel.ToString() + delimiter + " " + customerDiscovery_GlobalMarketLevel.ToString();
            }

            //if (_addLabels)
            //{
            //    customerDiscoveryInfo =
            //       "Feedback level: " + customerDiscovery_FeedbackLevel.ToString() + ", " +
            //       "Global market level: " + customerDiscovery_GlobalMarketLevel.ToString();
            //}
            //else
            //{
            //    customerDiscoveryInfo = customerDiscovery_FeedbackLevel.ToString() + ", " + customerDiscovery_GlobalMarketLevel.ToString();
            //}
        }

        public void InfoUpdate_GameRoomTimes(bool _addLabels)
        {
            if (_addLabels)
            {
                gameRoomsInfo =
                   "Time in Game Room - Main: " + roomTime_Main.ToString() + delimiter + " " +
                   "Time in Game Room - R&D: " + roomTime_ResearchDev.ToString() + delimiter + " " +
                   "Time in Game Room - Customer Discovery: " + roomTime_CustomerDisc.ToString() + delimiter + " " +
                   "Time in Game Room - Hiring: " + roomTime_Hiring.ToString() + delimiter + " " +
                   "Time in Game Room - Funding: " + roomTime_Funding.ToString() + delimiter + " " +
                   "Time in Game Room - Networking: " + roomTime_Networking.ToString();
            }
            else
            {
                gameRoomsInfo =
                  roomTime_Main.ToString() + delimiter + " " +
                  roomTime_ResearchDev.ToString() + delimiter + " " +
                  roomTime_CustomerDisc.ToString() + delimiter + " " +
                  roomTime_Hiring.ToString() + delimiter + " " +
                  roomTime_Funding.ToString() + delimiter + " " +
                  roomTime_Networking.ToString();
            }
        }

        public void InfoUpdate_OutputTotal(bool _addLabels)
        {
            InfoUpdate_All(_addLabels);

            outputInfo =
               decisionsInfo + delimiter + " " +
               budgetInfo + delimiter + " " +
               timeInfo + delimiter + " " +
               teamInfo + delimiter + " " +
               fundingInfo + delimiter + " " +
               designDeviceInfo + delimiter + " " +
               customerDiscoveryInfo + delimiter + " " +
               gameRoomsInfo;

            //outputInfo =
            //    decisionsInfo + ", " +
            //    budgetInfo + ", " +
            //    timeInfo + ", " +
            //    teamInfo + ", " +
            //    fundingInfo + ", " +
            //    designDeviceInfo + ", " +
            //    customerDiscoveryInfo;
        }

        public string Info_GetOutputInfo(bool _addLabels)
        {
            InfoUpdate_OutputTotal(_addLabels);

            return outputInfo;
        }

        public void Info_ResetAllInfo()
        {
            outputInfo = "";
            decisionsInfo = "";
            budgetInfo = "";
            timeInfo = "";
            teamInfo = "";
            fundingInfo = "";
            designDeviceInfo = "";
            customerDiscoveryInfo = "";
            gameRoomsInfo = "";
        }
    }

    [Serializable]
    public class TimestampTracker
    {
        public float decisionTime;
        public float decisionTime_Previous;
        public float timeInterval;
        public float timeInterval_prev;

        public void CalculateTimeInterval()
        {
            timeInterval_prev = timeInterval;
            timeInterval = decisionTime - decisionTime_Previous;
        }

        public float GetTimeInterval(float _decisionTime)
        {
            return (_decisionTime - decisionTime_Previous);
        }

        public void NewDecisionMade()
        {
            decisionTime = Time.time;
            timeInterval = decisionTime - decisionTime_Previous;

            decisionTime_Previous = decisionTime;
            timeInterval_prev = timeInterval;
        }
    }


    [Serializable]
    public class DecisionEntry
    {
        [Tooltip("")]
        public int decisionNum;
        [Tooltip("")]
        public float decisionTime;
        [Tooltip("")]
        public float decisionInterval;
        [Tooltip("Primary Category Type for this decision:" +
            "\nNONE, TEAM, NETWORKING, FUNDING, CUSTOMER_DISCOVERY, DEVICE")]
        public CategoryType categoryType;

        [Space(10)]
        [Header("Cost Vars")]
        public int budget_CostLevel;
        public float budget_CostAmount;

        public TimeIntervals time_CostUnits;
        public int time_CostLevel;
        public float time_CostAmount;


        [Space(10)]
        [Header("Parts Vars")]
        public PartTypes partType;
        public int partLevel;
        public float partUtility;

        [Space(10)]
        [Header("Team Vars")]
        public TeamMemberType team_MemberType;
        public EffectType team_EffectType;
        public float team_DiscountAmmount;
        public UnlockType team_unlockType;

        [Space(10)]
        [Header("Loan Vars")]
        public FundingType fundingType;
        public int funding_Level;
        public float funding_AwardAmount;
        [Range(0.0f, 1.0f)]
        public float funding_InterestPercentage;
        public float funding_InterestAmount;
        public float funding_EquityAmount;
        public float funding_HitRate;
        public bool funding_Successful;

        [Space(10)]
        [Header("Customer Discovery Vars")]
        public int feedback_Level;
        public int globalMarket_Level;

        [Space(10)]
        [Header("Networking Vars")]
        public int networkingType;
        public int networkingLevel;

        [Space(10)]
        public string info_Total;
        public string delimiter;

        //public void Budget_GetCostAmountByLevel(CostValues _values)
        //{
        //    if (_values != null)
        //    {
        //        budget_CostAmount = _values.GetValue_Budget(budget_CostLevel);
        //    }
        //}

        //public void Time_GetCostAmountByLevel(CostValues _values)
        //{
        //    if (_values != null)
        //    {
        //        time_CostAmount = _values.GetValue_Time(time_CostLevel);
        //    }
        //}

        public void Part_GetCost(CostValues _values)
        {
            if (_values != null)
            {
                budget_CostAmount = _values.GetPartCost_ByTypeAndLevel(partType, partLevel);
            }
        }

        public void Part_GetUtilityValueByLevel(PartValues _values)
        {
            if (_values != null)
            {
                partUtility = _values.GetValue_PartUtility(partType, partLevel);
            }
        }

        //  Team Functions

        public void Team_GetCostLevelByType(TeamValues _values)
        {
            if (_values != null)
                budget_CostLevel = _values.Team_GetBudgetCostLevelByType(team_MemberType);
        }

        public void Team_GetCostAmountByCostLevel(CostValues _values)
        {
            if (_values != null)
            {
                budget_CostAmount = _values.GetBudgetCost_ByTeamMemberCostLevel(budget_CostLevel);
            }
        }

        public void Team_GetEffectTypeByMemberType(TeamValues _values)
        {
            if (_values != null)
            {
                team_EffectType = _values.Team_GetEffectType(team_MemberType);
            }
        }

        public void Team_GetEffectAmountByMemberType(TeamValues _values)
        {
            if (_values != null)
            {
                team_DiscountAmmount = _values.Team_GetEffectAmount(team_MemberType);
            }
        }

        public void Team_GetUnlockTypeByMemeberType(TeamValues _values)
        {
            if (_values != null)
            {
                team_unlockType = _values.Team_GetUnlockType(team_MemberType);
            }
        }

        //  Funding Functions

        public void Funding_GetFundTimeUnitByLevel(CostValues _values)
        {
            if (_values != null)
            {
                time_CostUnits = _values.GetTimeUnit_Funding(fundingType, funding_Level);
            }
        }

        public void Funding_GetFundTimeCostLevel(CostValues _values)
        {
            if (_values != null)
            {
                time_CostLevel = _values.GetTimeCostLevel_ByFundingTypeAndLevel(fundingType, funding_Level);
            }
        }

        public void Funding_GetFundTimeCostAmount(CostValues _values)
        {
            if (_values != null)
            {
                time_CostAmount = _values.GetTimeCostAmount_Funding(time_CostUnits, time_CostLevel);
            }
        }

        public void Funding_GetFundAmountByLevel(CostValues _values)
        {
            if (_values != null)
            {
                funding_AwardAmount = _values.GetAwardAmount_FundingTypeAndLevel(fundingType, funding_Level);
                //funding_AwardAmount = _values.GetValue_Funding(funding_Level);
            }
        }

        public void Funding_GetFundHitRateByTypeAndLevel(CostValues _values)
        {
            if (_values != null)
            {
                funding_HitRate = _values.GetHitRate_FundingTypeAndLevel(fundingType, funding_Level);
            }
        }

        //public void Funding_GetFundHitRateByLevel(CostValues _values)
        //{
        //    if (_values != null)
        //    {
        //        funding_HitRate = _values.GetHitRate_Funding(funding_Level);
        //    }
        //}

        public void Funding_GetInterestRateByTypeAndLevel(CostValues _values)
        {
            if (_values != null)
            {
                funding_InterestPercentage = _values.GetInterestAmount_FundingTypeAndLevel(fundingType, funding_Level);
            }
        }

        public void Funding_CalculateInterest()
        {
            if (fundingType != FundingType.NONE && funding_AwardAmount > 0.0f)
            {
                funding_InterestAmount = funding_AwardAmount * funding_InterestPercentage;
            }
        }

        public void Funding_GetEquityByTypeAndLevel(CostValues _values)
        {
            if (_values != null)
                funding_EquityAmount = _values.GetEquityAmountByFundingType(fundingType, funding_Level);
        }

        public void CustomerDiscovery_GetCostByFeedbackLevel(CostValues _values)
        {
            budget_CostAmount = _values.GetCost_Budget_CustomerDiscoveryByLevel(feedback_Level);
            time_CostAmount = _values.GetCost_Time_CustomerDiscoveryByLevel(feedback_Level);
        }

        public void CustomerDiscovery_GlobalMarket_GetCostByFeedbackLevel(CostValues _values)
        {
            budget_CostAmount = _values.GetCost_Budget_GlobalMarketByLevel(globalMarket_Level);
            //budget_CostAmount = _values.GetCost_Budget_CustomerDiscoveryByLevel(feedback_Level);
            //time_CostAmount = _values.GetCost_Time_CustomerDiscoveryByLevel(feedback_Level);
        }

        public void Values_GetAllValues(CostValues _cost, PartValues _part, TeamValues _team)
        {
            //Budget_GetCostAmountByLevel(_cost);
            //Time_GetCostAmountByLevel(_cost);
            if (categoryType == CategoryType.DEVICE)
            {
                Part_GetCost(_cost);
                Part_GetUtilityValueByLevel(_part);
            }

            if (categoryType == CategoryType.TEAM)
            {
                Team_GetCostLevelByType(_team);
                Team_GetCostAmountByCostLevel(_cost);
                Team_GetEffectTypeByMemberType(_team);
                Team_GetEffectAmountByMemberType(_team);
                if (team_EffectType == EffectType.UNLOCK)
                    Team_GetUnlockTypeByMemeberType(_team);
            }


            if (fundingType != FundingType.NONE)
            {
                Funding_GetFundTimeUnitByLevel(_cost);
                Funding_GetFundTimeCostLevel(_cost);
                Funding_GetFundTimeCostAmount(_cost);
                Funding_GetFundAmountByLevel(_cost);
                //Funding_GetFundHitRateByLevel(_cost);
                Funding_GetInterestRateByTypeAndLevel(_cost);
                Funding_CalculateInterest();
                Funding_GetFundHitRateByTypeAndLevel(_cost);
                Funding_GetEquityByTypeAndLevel(_cost);
            }

            if (categoryType == CategoryType.CUSTOMER_DISCOVERY)
            {
                if (globalMarket_Level > 0)
                    CustomerDiscovery_GlobalMarket_GetCostByFeedbackLevel(_cost);
                else
                    CustomerDiscovery_GetCostByFeedbackLevel(_cost);

            }

            Info_UpdateInfoDecisionString();
            //info_Total = Info_GetFullDecisionString();
        }

        public string Info_GetFullDecisionString()
        {
            string info = "";

            string catagoryString = categoryType.ToString();
            if (categoryType == CategoryType.CUSTOMER_DISCOVERY && globalMarket_Level > 0)
                catagoryString = "GLOBAL_MARKET";

            info = "" +
                decisionNum + delimiter +
                decisionTime + delimiter +
                decisionInterval + delimiter +
                catagoryString + delimiter +
                //categoryType.ToString() + delimiter +
                budget_CostLevel + delimiter +
                budget_CostAmount + delimiter +
                time_CostUnits + delimiter +
                time_CostLevel + delimiter +
                time_CostAmount + delimiter +
                partType.ToString() + delimiter +
                partLevel + delimiter +
                partUtility + delimiter +
                team_MemberType.ToString() + delimiter +
                team_EffectType.ToString() + delimiter +
                team_DiscountAmmount + delimiter +
                team_unlockType.ToString() + delimiter +
                fundingType + delimiter +
                funding_Level + delimiter +
                funding_AwardAmount + delimiter +
                funding_InterestPercentage + delimiter +
                funding_InterestAmount + delimiter +
                funding_EquityAmount + delimiter +
                funding_HitRate + delimiter +
                funding_Successful + delimiter +
                feedback_Level + delimiter +
                globalMarket_Level + delimiter +
                networkingType + delimiter +
                networkingLevel;

            //info = "" + decisionNum + "," + decisionTime + "," + decisionInterval + "," + categoryType.ToString() + "," +
            //    budget_CostLevel + "," + budget_CostAmount + "," + time_CostUnits + "," + time_CostLevel + "," + time_CostAmount + "," +
            //    partType.ToString() + "," + partLevel + "," + partUtility + "," + team_MemberType.ToString() + "," + team_EffectType.ToString() + "," + team_DiscountAmmount + "," +
            //    team_unlockType.ToString() + "," + fundingType + "," + funding_Level + "," + funding_AwardAmount + "," + funding_InterestPercentage + "," +
            //    funding_InterestAmount + "," + funding_EquityAmount + "," + funding_HitRate + "," + funding_Successful + "," + feedback_Level + "," +
            //    globalMarket_Level;

            return info;
        }

        public void Info_UpdateInfoDecisionString()
        {
            info_Total = Info_GetFullDecisionString();
        }
    }

    //  Device Container Classes

    [Serializable]
    public class DesignedDevice
    {
        public string utility_Binary;
        public float utilityTotal;
        public bool allPartsChosen;
        public int numberOfPartsUsed;
        public DevicePart part_01;
        public DevicePart part_02;
        public DevicePart part_03;
        public DevicePart part_04;
        public DevicePart part_05;
        UtilityCalculation_ThredGame utilCalculator;
        public UnityEvent partsMissingEvent;
        public UnityEvent partsAllChosenEvent;
        public string delimiter;

        public void Parts_CheckChosenPartsCount()
        {
            bool missingPart = false;

            for (int i = 0; i < numberOfPartsUsed; i++)
            {
                if (i == 0 && part_01.type == PartTypes.NONE)
                    missingPart = true;

                if (i == 1 && part_02.type == PartTypes.NONE)
                    missingPart = true;

                if (i == 2 && part_03.type == PartTypes.NONE)
                    missingPart = true;

                if (i == 3 && part_04.type == PartTypes.NONE)
                    missingPart = true;

                if (i == 4 && part_05.type == PartTypes.NONE)
                    missingPart = true;

                if (missingPart)
                {
                    allPartsChosen = false;
                    break;
                }
            }

            if (missingPart == false)
            {
                allPartsChosen = true;
                partsAllChosenEvent.Invoke();
            }


        }

        public void Parts_RunMissingOrChosenEvents()
        {
            if (!allPartsChosen)
                partsMissingEvent.Invoke();
            else
                partsAllChosenEvent.Invoke();
        }

        public void Parts_RunAllChosenEvents()
        {
            if (allPartsChosen)
                partsAllChosenEvent.Invoke();
        }

        public void SetUitilityCalculator(UtilityCalculation_ThredGame _calc)
        {
            utilCalculator = _calc;
        }

        public void SetNewPart(DevicePart _part)
        {
            if (_part != null)
            {
                switch (_part.type)
                {
                    case PartTypes.NONE:
                        break;
                    case PartTypes.PART_01:
                        part_01 = _part;
                        break;
                    case PartTypes.PART_02:
                        part_02 = _part;
                        break;
                    case PartTypes.PART_03:
                        part_03 = _part;
                        break;
                    case PartTypes.PART_04:
                        part_04 = _part;
                        break;
                    case PartTypes.PART_05:
                        part_05 = _part;
                        break;
                    default:
                        break;
                }

                UpdateUtilityValueTotal();
                UpdateUtilityValueByTiers();
                Parts_CheckChosenPartsCount();
            }
        }

        public void SetNewPart(PartTypes _partType, int _partLevel)
        {
            DevicePart newPart = new DevicePart();
            newPart.type = _partType;
            newPart.partLevel = _partLevel;
            newPart.SetDelimiter(delimiter);

            switch (newPart.type)
            {
                case PartTypes.NONE:
                    break;
                case PartTypes.PART_01:
                    part_01 = newPart;
                    break;
                case PartTypes.PART_02:
                    part_02 = newPart;
                    break;
                case PartTypes.PART_03:
                    part_03 = newPart;
                    break;
                case PartTypes.PART_04:
                    part_04 = newPart;
                    break;
                case PartTypes.PART_05:
                    part_05 = newPart;
                    break;
                default:
                    break;
            }

            UpdateUtilityValueTotal();
            UpdateUtilityValueByTiers();
            Parts_CheckChosenPartsCount();
        }

        public void GetUtilityValueByPart(PartValues _values)
        {
            part_01.utilityVal = _values.GetValue_PartUtility(part_01.type, part_01.partLevel);
            part_02.utilityVal = _values.GetValue_PartUtility(part_02.type, part_02.partLevel);
            part_03.utilityVal = _values.GetValue_PartUtility(part_03.type, part_03.partLevel);
            part_04.utilityVal = _values.GetValue_PartUtility(part_04.type, part_04.partLevel);
            part_05.utilityVal = _values.GetValue_PartUtility(part_05.type, part_05.partLevel);

            UpdateUtilityValueTotal();
            UpdateUtilityValueByTiers();
            Parts_CheckChosenPartsCount();
        }

        public void UpdateUtilityValueTotal()
        {
            utilityTotal = part_01.utilityVal + part_02.utilityVal + part_03.utilityVal + part_04.utilityVal + part_05.utilityVal;
        }

        public void UpdateUtilityValueByTiers()
        {
            if (utilCalculator != null)
                utility_Binary = utilCalculator.GetAccommodation(part_01.partLevel, part_02.partLevel, part_03.partLevel).ToString("00.00");
            else
                utility_Binary = "00.00";
            //utility_Binary = (part_01.partLevel * 9) + (part_02.partLevel * 3) + (part_03.partLevel);
        }

        public void Reset()
        {
            part_01.Reset();
            part_02.Reset();
            part_03.Reset();
            part_04.Reset();
            part_05.Reset();
            utilityTotal = 0.0f;
            utility_Binary = "0";
            allPartsChosen = false;
        }

        public string Info_GetFullPartsInfoString()
        {
            string _info = "";
            _info =
                //utilityTotal.ToString() + ", " +
                utility_Binary + delimiter + " " +
                part_01.Info_GetPartInfoString() + delimiter + " " +
                part_02.Info_GetPartInfoString() + delimiter + " " +
                part_03.Info_GetPartInfoString();

            //_info =
            //    //utilityTotal.ToString() + ", " +
            //    utility_Binary + ", " +
            //    part_01.Info_GetPartInfoString() + ", " +
            //    part_02.Info_GetPartInfoString() + ", " +
            //    part_03.Info_GetPartInfoString();

            return _info;
        }

    }

    [Serializable]
    public class DevicePart
    {
        public PartTypes type;
        //[Range(1, 3)]
        public int partLevel;
        public float utilityVal;
        string delimiter;

        public void SetDelimiter(string _delimiter)
        {
            delimiter = _delimiter;
        }

        public void Reset()
        {
            type = PartTypes.NONE;
            partLevel = 0;
            utilityVal = 0.0f;
        }

        public string Info_GetPartInfoString()
        {
            string _info = "";
            _info = type.ToString() + delimiter + " " + partLevel.ToString() + delimiter + " " + utilityVal.ToString();

            return _info;
        }
    }

    //  Team Member Classes

    [Serializable]
    public class TeamMember
    {
        public TeamMemberType memberType;
        public EffectType effectType;
        public float effectAmount;
        public UnlockType unlockType;
        string delimiter;

        public void SetDelimiter(string _delimiter)
        {
            delimiter = _delimiter;
        }

        public void FindValuesByType(TeamValues _values, TeamMemberType _type)
        {
            if (_values != null)
            {
                memberType = _type;
                effectType = _values.Team_GetEffectType(memberType);
                effectAmount = _values.Team_GetEffectAmount(memberType);
                unlockType = _values.Team_GetUnlockType(memberType);
            }
        }

        public void FindValuesByType(TeamValues _values)
        {
            if (_values != null)
            {
                effectType = _values.Team_GetEffectType(memberType);
                effectAmount = _values.Team_GetEffectAmount(memberType);
                unlockType = _values.Team_GetUnlockType(memberType);
            }
        }

        public string Info_GetTeamMemberInfoString()
        {
            string _info = "";

            _info =
                memberType.ToString() + delimiter + " " +
                effectType.ToString() + delimiter + " " +
                effectAmount.ToString() + delimiter + " " +
                unlockType.ToString();


            //_info =
            //    memberType.ToString() + ", " +
            //    effectType.ToString() + ", " +
            //    effectAmount.ToString() + ", " +
            //    unlockType.ToString();
            return _info;
        }
    }

    [Serializable]
    public class TeamValues
    {
        public float effectValue_Type01;
        public float effectValue_Type02;
        public float effectValue_Type03;
        public float effectValue_Type04;
        public float effectValue_Type05;
        public float effectValue_Type06;
        [Space(10)]
        public List<string> teamNames;

        public int Team_GetBudgetCostLevelByType(TeamMemberType _type)
        {
            switch (_type)
            {
                case TeamMemberType.NONE:
                    return 0;
                case TeamMemberType.TYPE_01:
                    return 1;
                case TeamMemberType.TYPE_02:
                    return 2;
                case TeamMemberType.TYPE_03:
                    return 3;
                case TeamMemberType.TYPE_04:
                    return 4;
                case TeamMemberType.TYPE_05:
                    return 5;
                case TeamMemberType.TYPE_06:
                    return 6;
                default:
                    return 0;
            }
        }

        public EffectType Team_GetEffectType(TeamMemberType _type)
        {
            switch (_type)
            {
                case TeamMemberType.NONE:
                    return EffectType.NONE;
                case TeamMemberType.TYPE_01:
                    return EffectType.UNLOCK;
                case TeamMemberType.TYPE_02:
                    return EffectType.UNLOCK;
                case TeamMemberType.TYPE_03:
                    return EffectType.DISCOUNT;
                case TeamMemberType.TYPE_04:
                    return EffectType.UNLOCK;
                case TeamMemberType.TYPE_05:
                    return EffectType.NONE;
                case TeamMemberType.TYPE_06:
                    return EffectType.NONE;
                default:
                    return EffectType.NONE;
            }
        }

        public float Team_GetEffectAmount(TeamMemberType _type)
        {
            float returnVal = 0.0f;

            switch (_type)
            {
                case TeamMemberType.NONE:
                    break;
                case TeamMemberType.TYPE_01:
                    returnVal = effectValue_Type01;
                    break;
                case TeamMemberType.TYPE_02:
                    returnVal = effectValue_Type02;
                    break;
                case TeamMemberType.TYPE_03:
                    returnVal = effectValue_Type03;
                    break;
                case TeamMemberType.TYPE_04:
                    returnVal = effectValue_Type04;
                    break;
                case TeamMemberType.TYPE_05:
                    returnVal = effectValue_Type05;
                    break;
                case TeamMemberType.TYPE_06:
                    returnVal = effectValue_Type06;
                    break;
                default:
                    break;
            }

            return returnVal;
        }

        public UnlockType Team_GetUnlockType(TeamMemberType _type)
        {
            switch (_type)
            {
                case TeamMemberType.NONE:
                    return UnlockType.NONE;
                case TeamMemberType.TYPE_01:
                    return UnlockType.TYPE_01;
                case TeamMemberType.TYPE_02:
                    return UnlockType.TYPE_02;
                case TeamMemberType.TYPE_03:
                    return UnlockType.TYPE_03;
                case TeamMemberType.TYPE_04:
                    return UnlockType.TYPE_04;
                case TeamMemberType.TYPE_05:
                    return UnlockType.TYPE_05;
                case TeamMemberType.TYPE_06:
                    return UnlockType.TYPE_06;
                default:
                    return UnlockType.NONE;
            }
        }

        public string Team_GetNameByType(TeamMemberType _type)
        {
            switch (_type)
            {
                case TeamMemberType.NONE:
                    return Team_GetNameFromList(0);
                case TeamMemberType.TYPE_01:
                    return Team_GetNameFromList(1);
                case TeamMemberType.TYPE_02:
                    return Team_GetNameFromList(2);
                case TeamMemberType.TYPE_03:
                    return Team_GetNameFromList(3);
                case TeamMemberType.TYPE_04:
                    return Team_GetNameFromList(4);
                case TeamMemberType.TYPE_05:
                    return Team_GetNameFromList(5);
                case TeamMemberType.TYPE_06:
                    return Team_GetNameFromList(6);
                default:
                    return "None";
            }

        }

        public string Team_GetNameFromList(int _index)
        {
            if (_index >= 0 && _index < teamNames.Count)
            {
                if (teamNames[_index] != null)
                    return teamNames[_index];
            }

            return "None";
        }

    }

    [Serializable]
    public class GameRooms
    {
        public GameRoom main;
        public GameRoom research;
        public GameRoom customerDisc;
        public GameRoom hiring;
        public GameRoom funding;
        public GameRoom networking;
        public string delimiter;

        public float GameRoom_GetRoomTime_Main()
        {
            if (main != null)
                return main.TimeInRoom_Total;

            return 0.0f;
        }

        public float GameRoom_GetRoomTime_Research()
        {
            if (research != null)
                return research.TimeInRoom_Total;

            return 0.0f;
        }

        public float GameRoom_GetRoomTime_CustomerDiscovery()
        {
            if (customerDisc != null)
                return customerDisc.TimeInRoom_Total;

            return 0.0f;
        }

        public float GameRoom_GetRoomTime_Hiring()
        {
            if (hiring != null)
                return hiring.TimeInRoom_Total;

            return 0.0f;
        }

        public float GameRoom_GetRoomTime_Funding()
        {
            if (funding != null)
                return funding.TimeInRoom_Total;

            return 0.0f;
        }

        public float GameRoom_GetRoomTime_Networking()
        {
            if (networking != null)
                return networking.TimeInRoom_Total;

            return 0.0f;
        }

        public string Info_GetFullRoomTimesInfoString()
        {
            string _info = "";

            _info =
                "Main Lobby" + delimiter + " " +
                GameRoom_GetRoomTime_Main().ToString() + delimiter + " " + "\n" +
                "R&D" + delimiter + " " +
                GameRoom_GetRoomTime_Research().ToString() + delimiter + " " + "\n" +
                "Customer Discovery Room" + delimiter + " " +
                GameRoom_GetRoomTime_CustomerDiscovery().ToString() + delimiter + " " + "\n" +
                "Hiring Office" + delimiter + " " +
                GameRoom_GetRoomTime_Hiring().ToString() + delimiter + " " + "\n" +
                "Funding Office" + delimiter + " " +
                GameRoom_GetRoomTime_Funding().ToString() + delimiter + " " + "\n" +
                "Networking Room" + delimiter + " " +
                GameRoom_GetRoomTime_Networking().ToString();


            //_info =
            //    GameRoom_GetRoomTime_Main().ToString() + delimiter + " " +
            //    GameRoom_GetRoomTime_Research().ToString() + delimiter + " " +
            //    GameRoom_GetRoomTime_CustomerDiscovery().ToString() + delimiter + " " +
            //    GameRoom_GetRoomTime_Hiring().ToString() + delimiter + " " +
            //    GameRoom_GetRoomTime_Funding().ToString() + delimiter + " " +
            //    GameRoom_GetRoomTime_Networking().ToString();


            return _info;
        }

    }


    [Serializable]
    public class CostValues
    {
        //[Space(10)]
        //[Header("Budget Cost Values")]
        //public List<float> costLevels_Budget;

        //[Header("Time Cost Values")]
        //public List<float> costLevels_Time;

        //[Header("Funding Cost Values")]
        //public List<float> costLevels_Funding;

        [Space(10)]
        [Header("Team Cost Values")]
        public List<float> costLevels_Budget_Team;


        [Space(30)]
        [Header("Funding -Hit Rate Values")]
        //[Header("Funding - Bank Hit Rate Values")]
        public List<float> funding_BankHitRates;

        // [Header("Funding - Investment Hit Rate Values")]
        public List<float> funding_InvestmentHitRates;

        // [Header("Funding - Grant Hit Rate Values")]
        public List<float> funding_GrantHitRates;


        [Space(30)]
        [Header("Funding - Award Values")]
        // [Header("Funding - Bank Award Values")]
        public List<float> funding_BankAwardAmounts;

        //[Header("Funding - Investment Award Values")]
        public List<float> funding_InvestmentAwardAmounts;

        //[Header("Funding - Grant Award Values")]
        public List<float> funding_GrantAwardAmounts;


        [Space(30)]
        [Header("Funding - Interest Values")]
        //[Header("Funding - Bank Interest Values")]
        public List<float> funding_BankInterest;

        //[Header("Funding - Investment Interest Values")]
        public List<float> funding_InvestmentInterest;

        //[Header("Funding - Grant Interest Values")]
        public List<float> funding_GrantInterest;

        [Space(30)]
        [Header("Parts - Budget Cost Values")]
        public List<float> parts_costs_Budget;

        [Space(30)]
        [Header("Customer Discovery - Budget Cost Values")]
        public List<float> customerDiscovery_Costs_Budget;

        //[Space(30)]
        //[Header("Customer Discovery - Time Cost Values")]
        //public List<float> customerDiscovery_Costs_Time;

        [Space(30)]
        [Header("GlobalMarket - Budget Cost Values")]
        public List<float> globalMarket_Costs_Budget;

        //[Header("Funding Time Units")]
        //public List<TimeIntervals> funding_HitRates;

        [Space(30)]
        [Header("Time Unit Cost Values")]
        public float costUnit_Day;
        public float costUnit_Week;
        public float costUnit_Month;

        //public float level_01_Time;
        //public float level_02_Time;
        //public float level_03_Time;
        //public float level_04_Time;
        //public float level_05_Time;

        public void SetTimeCostUnitValues(float _day, float _week, float _month)
        {
            costUnit_Day = _day;
            costUnit_Week = _week;
            costUnit_Month = _month;
        }

        //public float GetValue_Budget(int _level)
        //{
        //    float val = 0.0f;

        //    if (_level >= 0 && _level < costLevels_Budget.Count)
        //        val = costLevels_Budget[_level];

        //    return val;
        //}

        //public float GetValue_Funding(int _level)
        //{
        //    float val = 0.0f;

        //    if (_level >= 0 && _level < costLevels_Funding.Count)
        //        val = costLevels_Funding[_level];

        //    return val;
        //}

        public float GetBudgetCost_ByTeamMemberCostLevel(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < costLevels_Budget_Team.Count)
                val = costLevels_Budget_Team[_level];

            return val;
        }

        public float GetPartCost_ByTypeAndLevel(PartTypes _type, int _level)
        {
            return GetPartCost_ByIndex(GetPartCostIndex_ByPartTypeAndLevel(_type, _level));
        }

        public int GetPartCostIndex_ByPartTypeAndLevel(PartTypes _type, int _level)
        {
            switch (_type)
            {
                case PartTypes.NONE:
                    return -1;
                case PartTypes.PART_01:
                    {
                        switch (_level)
                        {
                            case 1:
                                return 0;
                            case 2:
                                return 1;
                            case 3:
                                return 2;
                            default:
                                return -1;
                        }
                    }
                case PartTypes.PART_02:
                    {
                        switch (_level)
                        {
                            case 1:
                                return 3;
                            case 2:
                                return 4;
                            case 3:
                                return 5;
                            default:
                                return -1;
                        }
                    }
                case PartTypes.PART_03:
                    {
                        switch (_level)
                        {
                            case 1:
                                return 6;
                            case 2:
                                return 7;
                            case 3:
                                return 8;
                            default:
                                return -1;
                        }
                    }
                case PartTypes.PART_04:
                    {
                        switch (_level)
                        {
                            case 1:
                                return 9;
                            case 2:
                                return 10;
                            case 3:
                                return 11;
                            default:
                                return -1;
                        }
                    }
                case PartTypes.PART_05:
                    {
                        switch (_level)
                        {
                            case 1:
                                return 12;
                            case 2:
                                return 13;
                            case 3:
                                return 14;
                            default:
                                return -1;
                        }
                    }
                default:
                    return -1;
            }

        }

        public float GetPartCost_ByIndex(int _index)
        {
            float val = 0.0f;

            if (_index >= 0 && _index < parts_costs_Budget.Count)
                val = parts_costs_Budget[_index];

            return val;
        }

        //  Funding Hit Rates

        public float GetHitRate_FundingTypeAndLevel(FundingType _type, int _level)
        {
            switch (_type)
            {
                case FundingType.NONE:
                    return 0.0f;
                case FundingType.BANK:
                    return GetHitRate_BankFunding(_level);
                case FundingType.INVESTMENT:
                    return GetHitRate_InvestmentFunding(_level);
                case FundingType.GRANT:
                    return GetHitRate_GrantFunding(_level);
                case FundingType.OTHER:
                    return 0.0f;
                default:
                    return 0.0f;
            }
        }

        public float GetHitRate_BankFunding(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < funding_BankHitRates.Count)
                val = funding_BankHitRates[_level];

            return val;
        }

        public float GetHitRate_InvestmentFunding(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < funding_InvestmentHitRates.Count)
                val = funding_InvestmentHitRates[_level];

            return val;
        }

        public float GetHitRate_GrantFunding(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < funding_GrantHitRates.Count)
                val = funding_GrantHitRates[_level];

            return val;
        }


        //  Funding Award Amounts

        public float GetAwardAmount_FundingTypeAndLevel(FundingType _type, int _level)
        {
            switch (_type)
            {
                case FundingType.NONE:
                    return 0.0f;
                case FundingType.BANK:
                    return GetAwardAmount_BankFunding(_level);
                case FundingType.INVESTMENT:
                    return GetAwardAmount_InvestmentFunding(_level);
                case FundingType.GRANT:
                    return GetAwardAmount_GrantFunding(_level);
                case FundingType.OTHER:
                    return 0.0f;
                default:
                    return 0.0f;
            }
        }

        public float GetAwardAmount_BankFunding(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < funding_BankAwardAmounts.Count)
                val = funding_BankAwardAmounts[_level];

            return val;
        }

        public float GetAwardAmount_InvestmentFunding(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < funding_InvestmentAwardAmounts.Count)
                val = funding_InvestmentAwardAmounts[_level];

            return val;
        }

        public float GetAwardAmount_GrantFunding(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < funding_GrantAwardAmounts.Count)
                val = funding_GrantAwardAmounts[_level];

            return val;
        }


        //  Funding Interest Amounts

        public float GetInterestAmount_FundingTypeAndLevel(FundingType _type, int _level)
        {
            switch (_type)
            {
                case FundingType.NONE:
                    return 0.0f;
                case FundingType.BANK:
                    return GetInterestAmount_BankFunding(_level);
                case FundingType.INVESTMENT:
                    return GetInterestAmount_InvestmentFunding(_level);
                case FundingType.GRANT:
                    return GetInterestAmount_GrantFunding(_level);
                case FundingType.OTHER:
                    return 0.0f;
                default:
                    return 0.0f;
            }
        }

        public float GetInterestAmount_BankFunding(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < funding_BankInterest.Count)
                val = funding_BankInterest[_level];

            return val;
        }

        public float GetInterestAmount_InvestmentFunding(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < funding_InvestmentInterest.Count)
                val = funding_InvestmentInterest[_level];

            return val;
        }

        public float GetInterestAmount_GrantFunding(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < funding_GrantInterest.Count)
                val = funding_GrantInterest[_level];

            return val;
        }


        public TimeIntervals GetTimeUnit_Funding(FundingType _type, int _level)
        {
            switch (_type)
            {
                case FundingType.NONE:
                    return TimeIntervals.NONE;
                case FundingType.BANK:
                    return GetFundingTimeUnit_BankingByLevel(_level);
                case FundingType.INVESTMENT:
                    return GetFundingTimeUnit_InvestmentByLevel(_level);
                case FundingType.GRANT:
                    return GetFundingTimeUnit_GrantByLevel(_level);
                case FundingType.OTHER:
                    return TimeIntervals.NONE;
                default:
                    return TimeIntervals.NONE;
            }
        }

        public TimeIntervals GetFundingTimeUnit_BankingByLevel(int _level)
        {
            switch (_level)
            {
                case 1:
                    return TimeIntervals.NONE;
                case 2:
                    return TimeIntervals.NONE;
                case 3:
                    return TimeIntervals.NONE;
                case 4:
                    return TimeIntervals.NONE;
                default:
                    return TimeIntervals.NONE;
            }
        }

        public TimeIntervals GetFundingTimeUnit_InvestmentByLevel(int _level)
        {
            switch (_level)
            {
                case 1:
                    return TimeIntervals.WEEK;
                case 2:
                    return TimeIntervals.WEEK;
                case 3:
                    return TimeIntervals.WEEK;
                case 4:
                    return TimeIntervals.NONE;
                default:
                    return TimeIntervals.NONE;
            }
        }

        public TimeIntervals GetFundingTimeUnit_GrantByLevel(int _level)
        {
            switch (_level)
            {
                case 1:
                    return TimeIntervals.WEEK;
                case 2:
                    return TimeIntervals.WEEK;
                case 3:
                    return TimeIntervals.MONTH;
                case 4:
                    return TimeIntervals.NONE;
                default:
                    return TimeIntervals.NONE;
            }
        }

        public int GetTimeCostLevel_ByFundingTypeAndLevel(FundingType _type, int _level)
        {
            switch (_type)
            {
                case FundingType.NONE:
                    return 0;
                case FundingType.BANK:
                    return 0;
                case FundingType.INVESTMENT:
                    return 1;
                case FundingType.GRANT:
                    return GetTimeCostLevel_Funding_GrantLevel(_level);
                case FundingType.OTHER:
                    return 0;
                default:
                    return 0;
            }
        }

        //public int GetTimeCostLevel_Funding_BankLevel(int _level)
        //{
        //    switch (_level)
        //    {
        //        case 1:
        //            return 0;
        //        case 2:
        //            return TimeIntervals.WEEK;
        //        case 3:
        //            return TimeIntervals.MONTH;
        //        case 4:
        //            return TimeIntervals.NONE;
        //        default:
        //            return TimeIntervals.NONE;
        //    }
        //}

        //public int GetTimeCostLevel_Funding_InvestmentLevel(int _level)
        //{
        //    switch (_level)
        //    {
        //        case 1:
        //            return 0;
        //        case 2:
        //            return TimeIntervals.WEEK;
        //        case 3:
        //            return TimeIntervals.MONTH;
        //        case 4:
        //            return TimeIntervals.NONE;
        //        default:
        //            return TimeIntervals.NONE;
        //    }
        //}

        public int GetTimeCostLevel_Funding_GrantLevel(int _level)
        {
            switch (_level)
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 1;
                case 4:
                    return 0;
                default:
                    return 0;
            }
        }

        public float GetTimeCostAmount_Funding(TimeIntervals _timeUnits, int _timeCostLevel)
        {
            switch (_timeUnits)
            {
                case TimeIntervals.NONE:
                    return 0.0f;
                case TimeIntervals.DAY:
                    return GetValue_TimeByNumOfDays(_timeCostLevel);
                case TimeIntervals.WEEK:
                    return GetValue_TimeByNumOfWeeks(_timeCostLevel);
                case TimeIntervals.MONTH:
                    return GetValue_TimeByNumOfMonths(_timeCostLevel);
                default:
                    return 0.0f;
            }
        }

        public float GetEquityAmountByFundingType(FundingType _type, int _level)
        {
            if (_type == FundingType.INVESTMENT && _level == 3)
                return 0.3f;

            return 0.0f;
        }

        //public float GetTimeCostAmount_FundingByType(TimeIntervals _timeUnit, int _timeCostLevel)
        //{

        //}

        //public float GetValue_Time(int _level)
        //{
        //    float val = 0.0f;

        //    if (_level >= 0 && _level < costLevels_Time.Count)
        //        val = costLevels_Time[_level];

        //    return val;
        //}

        public float GetValue_TimeByNumOfDays(int _days)
        {
            return _days * costUnit_Day;
        }

        public float GetValue_TimeByNumOfWeeks(int _weeks)
        {
            return _weeks * costUnit_Week;
        }

        public float GetValue_TimeByNumOfMonths(int _months)
        {
            return _months * costUnit_Month;
        }

        public float GetCost_Budget_CustomerDiscoveryByLevel(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < customerDiscovery_Costs_Budget.Count)
                val = customerDiscovery_Costs_Budget[_level];

            return val;
        }

        public float GetCost_Budget_GlobalMarketByLevel(int _level)
        {
            float val = 0.0f;

            if (_level >= 0 && _level < globalMarket_Costs_Budget.Count)
                val = globalMarket_Costs_Budget[_level];

            return val;
        }

        public float GetCost_Time_CustomerDiscoveryByLevel(int _level)
        {
            switch (_level)
            {
                case 0:
                    return 0.0f;
                case 1:
                    return (costUnit_Week * 2.0f);
                case 2:
                    return costUnit_Month;
                case 3:
                    return costUnit_Month;
                default:
                    return 0.0f;

                    //case 0:
                    //    return 0.0f;
                    //case 1:
                    //    return 0.0f;
                    //case 2:
                    //    return (costUnit_Week * 2.0f);
                    //case 3:
                    //    return costUnit_Month;
                    //default:
                    //    return 0.0f;
            }
        }

        //public float GetValue_Budget(int _level)
        //{
        //    switch (_level)
        //    {
        //        case 1:
        //            return level_01_Budget;
        //        case 2:
        //            return level_02_Budget;
        //        case 3:
        //            return level_03_Budget;
        //        default:
        //            break;
        //    }

        //    return 0.0f;
        //}

        //public float GetValue_Time(int _level)
        //{
        //    switch (_level)
        //    {
        //        case 1:
        //            return level_01_Time;
        //        case 2:
        //            return level_02_Time;
        //        case 3:
        //            return level_03_Time;
        //        case 4:
        //            return level_04_Time;
        //        case 5:
        //            return level_05_Time;
        //        default:
        //            break;
        //    }

        //    return 0.0f;
        //}
    }

    [Serializable]
    public class PartValues
    {
        public bool useLinearValues;

        [Space(10)]
        [Header("Part Utility Values")]
        //[Header("Part 01 Values")]
        public List<float> partVals_01;

        [Space(10)]
        //[Header("Part 02 Values")]
        public List<float> partVals_02;

        [Space(10)]
        //[Header("Part 03 Values")]
        public List<float> partVals_03;

        [Space(10)]
        //[Header("Part 04 Values")]
        public List<float> partVals_04;

        [Space(10)]
        //[Header("Part 05 Values")]
        public List<float> partVals_05;

        [Header("Part Names")]
        [Space(10)]
        public List<string> partNames_01;

        [Space(10)]
        public List<string> partNames_02;

        [Space(10)]
        public List<string> partNames_03;

        [Space(10)]
        public List<string> partNames_04;

        [Space(10)]
        public List<string> partNames_05;



        public float GetValue_PartUtility(PartTypes _type, int _level)
        {
            if (useLinearValues)
                return _level;


            switch (_type)
            {
                case PartTypes.NONE:
                    break;
                case PartTypes.PART_01:
                    return GetValue_Part01(_level);
                case PartTypes.PART_02:
                    return GetValue_Part02(_level);
                case PartTypes.PART_03:
                    return GetValue_Part03(_level);
                case PartTypes.PART_04:
                    return GetValue_Part04(_level);
                case PartTypes.PART_05:
                    return GetValue_Part05(_level);
                default:
                    break;
            }

            return 0.0f;
        }

        public float GetValue_Part01(int _level)
        {
            if (_level >= 0 && _level < partVals_01.Count)
                return partVals_01[_level];

            return 0.0f;
        }

        public float GetValue_Part02(int _level)
        {
            if (_level >= 0 && _level < partVals_02.Count)
                return partVals_02[_level];

            return 0.0f;
        }

        public float GetValue_Part03(int _level)
        {
            if (_level >= 0 && _level < partVals_03.Count)
                return partVals_03[_level];

            return 0.0f;
        }

        public float GetValue_Part04(int _level)
        {
            if (_level >= 0 && _level < partVals_04.Count)
                return partVals_04[_level];

            return 0.0f;
        }

        public float GetValue_Part05(int _level)
        {
            if (_level >= 0 && _level < partVals_05.Count)
                return partVals_05[_level];

            return 0.0f;
        }

        public string GetPartName(PartTypes _type, int _level)
        {
            switch (_type)
            {
                case PartTypes.NONE:
                    return "None";
                case PartTypes.PART_01:
                    if (_level >= 0 && _level < partNames_01.Count)
                        return partNames_01[_level];
                    break;
                case PartTypes.PART_02:
                    if (_level >= 0 && _level < partNames_02.Count)
                        return partNames_02[_level];
                    break;
                case PartTypes.PART_03:
                    if (_level >= 0 && _level < partNames_03.Count)
                        return partNames_03[_level];
                    break;
                case PartTypes.PART_04:
                    if (_level >= 0 && _level < partNames_04.Count)
                        return partNames_04[_level];
                    break;
                case PartTypes.PART_05:
                    if (_level >= 0 && _level < partNames_05.Count)
                        return partNames_05[_level];
                    break;
                default:
                    return "None";
            }

            return "None";
        }

    }

    [Serializable]
    public class UnlockEvents
    {
        [Header("Team Unlock Events")]
        public UnityEvent unlockEvent_01;
        public UnityEvent unlockEvent_02;
        public UnityEvent unlockEvent_03;
        public UnityEvent unlockEvent_04;
        public UnityEvent unlockEvent_05;
        public UnityEvent unlockEvent_06;

        [Space(15)]
        [Header("Global Market Unlock Events")]
        public UnityEvent unlockEvent_GlobalMarket;
        public UnityEvent unlockEvent_GlobalMarket_Lvl01;
        public UnityEvent unlockEvent_GlobalMarket_Lvl02;
        public UnityEvent unlockEvent_GlobalMarket_Lvl03;

        [Space(15)]
        [Header("Reset Locks Events")]
        public UnityEvent resetLocksEvent;

        public void Unlock_RunEventByType(UnlockType _type)
        {
            Debug.Log("UnlockEvent=> Run Event: " + _type.ToString());

            switch (_type)
            {
                case UnlockType.NONE:
                    break;
                case UnlockType.TYPE_01:
                    unlockEvent_01.Invoke();
                    break;
                case UnlockType.TYPE_02:
                    unlockEvent_02.Invoke();
                    break;
                case UnlockType.TYPE_03:
                    unlockEvent_03.Invoke();
                    break;
                case UnlockType.TYPE_04:
                    unlockEvent_04.Invoke();
                    break;
                case UnlockType.TYPE_05:
                    unlockEvent_05.Invoke();
                    break;
                case UnlockType.TYPE_06:
                    unlockEvent_06.Invoke();
                    break;
                default:
                    break;
            }
        }

        public void Unlock_GlobalMarketUnlocked()
        {
            unlockEvent_GlobalMarket.Invoke();
        }

        public void Unlock_GlobalMarketByLevel(int _level)
        {
            switch (_level)
            {
                case 1:
                    unlockEvent_GlobalMarket_Lvl01.Invoke();
                    break;
                case 2:
                    unlockEvent_GlobalMarket_Lvl02.Invoke();
                    break;
                case 3:
                    unlockEvent_GlobalMarket_Lvl03.Invoke();
                    break;
                default:
                    break;
            }
        }

        public void Unlock_ResetLocks()
        {
            resetLocksEvent.Invoke();
        }

    }

    [Serializable]
    public class TimeSpanBreakdowns
    {
        public bool overrideTotalGametime;
        public float totalUnits_GameTime;
        public TimeIntervals minutesToUnits;
        //public float realMinutesTotal;
        public float realSecondsTotal;
        [Space(10)]
        public float dayUnitSeconds;
        public float weekUnitSeconds;
        public float monthUnitSeconds;
        [Space(10)]
        public float dayUnit;
        public float weekUnit;
        public float monthUnit;
        [Space(10)]
        public float daysTotal;
        public float weeksTotal;
        public float monthsTotal;
        [Space(10)]
        public Vector3 progress = Vector3.zero;
        public float interest_MonthOfLastPayment;

        public void SetupTimeSpanBreakdowns(float _timeTotal = 0.0f)
        {
            if (overrideTotalGametime)
            {
                realSecondsTotal = totalUnits_GameTime * 60.0f;
                Time_CalculateTimeFromUnits();
            }
            else
                Time_CalculateUnitsFromTime(_timeTotal);

        }

        public void Time_CalculateTimeFromUnits()
        {
            Time_CalculateUnits();
        }

        public void Time_CalculateUnitsFromTime(float _timeTotal)
        {
            if (_timeTotal > 0.0f)
            {
                realSecondsTotal = _timeTotal;
                totalUnits_GameTime = _timeTotal / 60.0f;
                Time_CalculateUnits();
            }
        }

        public void Time_CalculateUnits()
        {
            switch (minutesToUnits)
            {
                case TimeIntervals.NONE:
                    break;
                case TimeIntervals.DAY:
                    Time_BreakdownByDays();
                    break;
                case TimeIntervals.WEEK:
                    Time_BreakdownByWeeks();
                    break;
                case TimeIntervals.MONTH:
                    Time_BreakdownByMonths();
                    break;
                case TimeIntervals.MINUTES:
                    Time_BreakdownByMinutes();
                    break;
                default:
                    break;
            }

            //Time_CalculateUnitTotals();

        }

        public void Time_BreakdownByDays()
        {
            //  Totals
            daysTotal = totalUnits_GameTime;
            weeksTotal = daysTotal / 7.0f;
            monthsTotal = weeksTotal / 4.0f;

            //  Seconds
            weekUnitSeconds = realSecondsTotal / weeksTotal;
            monthUnitSeconds = realSecondsTotal / monthsTotal;
            dayUnitSeconds = realSecondsTotal / daysTotal;

            //  Units
            dayUnit = totalUnits_GameTime;
            weekUnit = dayUnit * 7.0f;
            monthUnit = weekUnit * 4.0f;

        }

        public void Time_BreakdownByWeeks()
        {
            //  Totals
            weeksTotal = totalUnits_GameTime;
            monthsTotal = weeksTotal / 4.0f;
            daysTotal = weeksTotal * 7.0f;

            //  Seconds
            //weekUnitSeconds = realSecondsTotal / totalUnits_GameTime;
            weekUnitSeconds = realSecondsTotal / weeksTotal;
            monthUnitSeconds = realSecondsTotal / monthsTotal;
            dayUnitSeconds = realSecondsTotal / daysTotal;

            //  Units
            weekUnit = totalUnits_GameTime;
            dayUnit = weekUnit / 7.0f;
            monthUnit = weekUnit * 4.0f;

        }

        public void Time_BreakdownByMonths()
        {
            //  Totals
            monthsTotal = totalUnits_GameTime;
            weeksTotal = monthsTotal * 4.0f;
            daysTotal = weeksTotal * 7.0f;

            //  Seconds
            //monthUnitSeconds = realSecondsTotal / totalUnits_GameTime;
            monthUnitSeconds = realSecondsTotal / monthsTotal;
            weekUnitSeconds = realSecondsTotal / weeksTotal;
            dayUnitSeconds = realSecondsTotal / daysTotal;

            //  Units
            monthUnit = totalUnits_GameTime;
            weekUnit = monthUnit / 4.0f;
            dayUnit = weekUnit / 7.0f;

            ////  Units
            //monthUnit = 1.0f;
            //weekUnit = monthUnit / 4.0f;
            //dayUnit = weekUnit / 7.0f;
        }

        public void Time_BreakdownByMinutes()
        {
            //  Totals
            //monthsTotal = totalUnits_GameTime;
            //weeksTotal = monthsTotal * 4.0f;
            //daysTotal = weeksTotal * 7.0f;

            //  Seconds
            //monthUnitSeconds = realSecondsTotal / totalUnits_GameTime;
            monthUnitSeconds = realSecondsTotal / monthsTotal;
            weekUnitSeconds = realSecondsTotal / weeksTotal;
            dayUnitSeconds = realSecondsTotal / daysTotal;

            //  Units
            monthUnit = monthsTotal;
            //monthUnit = totalUnits_GameTime;
            weekUnit = weeksTotal;
            //weekUnit = monthUnit / 4.0f;
            dayUnit = daysTotal;
            //dayUnit = weekUnit / 7.0f;

            ////  Units
            //monthUnit = 1.0f;
            //weekUnit = monthUnit / 4.0f;
            //dayUnit = weekUnit / 7.0f;
        }

        //public void Time_CalculateUnitTotals()
        //{
        //      daysTotal = totalUnits_GameTime * dayUnit;
        //     weeksTotal = totalUnits_GameTime * weekUnit;
        //    monthsTotal = totalUnits_GameTime * monthUnit;

        //    //float _timeTotal = totalUnits_GameTime * 60.0f;
        //    //  daysTotal = totalUnits_GameTime / dayUnit;
        //    // weeksTotal = totalUnits_GameTime / weekUnit;
        //    //monthsTotal = totalUnits_GameTime / monthUnit;
        //}

        public float Time_GetGameTimeFromUnits()
        {
            if (overrideTotalGametime)
                return (totalUnits_GameTime * 60.0f);
            else
                return -1.0f;
        }

        public Vector3 Time_GetCurrentProgressUnits(float _remainingGametime)
        {
            Vector3 progressUnits = Vector3.zero;

            if (_remainingGametime > 0.0f)
            {
                //  float _days = (_remainingGametime / dayUnitSeconds);
                // float _weeks = (_remainingGametime / weekUnitSeconds);
                //float _months = (_remainingGametime / monthUnitSeconds);

                float _progressVal = realSecondsTotal - _remainingGametime;

                float _days = (_progressVal / dayUnitSeconds);
                float _weeks = (_progressVal / weekUnitSeconds);
                float _months = (_progressVal / monthUnitSeconds);


                int _daysVal = Mathf.FloorToInt(_days);
                int _weeksVal = Mathf.FloorToInt(_weeks);
                int _monthsVal = Mathf.FloorToInt(_months);
                progressUnits = new Vector3(_daysVal, _weeksVal, _monthsVal);

                //progressUnits = new Vector3(_days, _weeks, _months);

                //Debug.Log("Time Units Remaining (days / weeks / months) : " + progressUnits + ", Progress Val: " + _progressVal + ", RemainingTime: " + _remainingGametime);
                //Debug.Log("Time Units Remaining (days / weeks / months) : " + progressUnits + ", RemainingTime: " + (_remainingGametime / 60.0f));

                progress = progressUnits;
            }

            return progressUnits;
        }

        public float Progress_GetDaysPassed()
        {
            return progress.x;
        }

        public float Progress_GetWeeksPassed()
        {
            return progress.y;
        }

        public float Progress_GetMonthsPassed()
        {
            return progress.z;
        }

        public string Progress_GetDays_PassedAndTotal()
        {
            return "" + progress.x.ToString("F0") + " / " + daysTotal.ToString("F0");
        }

        public string Progress_GetWeeks_PassedAndTotal()
        {
            return "" + progress.y.ToString("F0") + " / " + weeksTotal.ToString("F0");
        }

        public string Progress_GetMonths_PassedAndTotal()
        {
            return "" + progress.z.ToString("F0") + " / " + monthsTotal.ToString("F0");
        }

        public string TimeUnit_GetDaysCount(float _seconds)
        {
            float val = (_seconds / dayUnitSeconds);
            if (val == 1.0f)
                return "" + val.ToString("F0") + " Day";
            else
                return "" + val.ToString("F0") + " Days";
        }

        public string TimeUnit_GetWeeksCount(float _seconds)
        {
            float val = (_seconds / weekUnitSeconds);
            if (val == 1.0f)
                return "" + val.ToString("F0") + " Week";
            else
                return "" + val.ToString("F0") + " Weeks";
        }

        public string TimeUnit_GetMonthsCount(float _seconds)
        {
            float val = (_seconds / monthUnitSeconds);
            if (val == 1.0f)
                return "" + val.ToString("F0") + " Month";
            else
                return "" + val.ToString("F0") + " Months";
        }

    }

    [Serializable]
    public class PlayerPrefInfo
    {
        public bool usePlayerPrefs;

        //  Variables - Initial Values
        //Starting Budget
        public float startingBudget;
        //Starting Time
        public float startingTime;

        ////  Values - Device Parts
        ////Display_01, 02, 03
        //public float partUtil_Display_01;
        //public float partUtil_Display_02;
        //public float partUtil_Display_03;

        ////Alarm_01, 02, 03
        //public float partUtil_Alarm_01;
        //public float partUtil_Alarm_02;
        //public float partUtil_Alarm_03;

        ////EnergySource_01, 02, 03
        //public float partUtil_EnergySource_01;
        //public float partUtil_EnergySource_02;
        //public float partUtil_EnergySource_03;

        ////  Values - Budget Costs
        ////CostBudget_01, 02, 03
        //public float cost_Budget_01;
        //public float cost_Budget_02;
        //public float cost_Budget_03;

        ////  Values - Time Costs
        ////CostTime_01, 02, 03, 04, 05
        //public float cost_Time_01;
        //public float cost_Time_02;
        //public float cost_Time_03;
        //public float cost_Time_04;
        //public float cost_Time_05;

        ////  Values - Team Effects
        ////TeamEffect_Eng_01, 02
        //public float teamEffect_Eng_01;
        //public float teamEffect_Eng_02;

        ////TeamEffect_Bus_01, 02
        //public float teamEffect_Bus_01;
        //public float teamEffect_Bus_02;


        public void SavePrefs_StartingResources(float _budget, float _time)
        {
            PlayerPrefs.SetFloat("StartingBudget", _budget);
            PlayerPrefs.SetFloat("StartingTime", _time);
        }

    }

    #endregion

    //  Setup Functions     ----------------------------------------------------------------------------------------------------
    #region Setup Functions

    private void Awake()
    {
        if (_trackedVariables != null && _trackedVariables != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _trackedVariables = this;
            Setup();
        }
    }

    private void Start()
    {
        //Setup();
        stats.ResourceValues_SaveStarting();
    }

    public void Setup()
    {
        delimiter = GetDelimiterString();
        data.delimiter = delimiter;
        device.delimiter = delimiter;
        rooms.delimiter = delimiter;
        timeUnits.SetupTimeSpanBreakdowns(stats.time);
        if (timeUnits.Time_GetGameTimeFromUnits() > 0.0f)
        {
            stats.time = timeUnits.Time_GetGameTimeFromUnits();
            stats.reset_time = stats.time;
        }

        costValues.SetTimeCostUnitValues(timeUnits.dayUnitSeconds, timeUnits.weekUnitSeconds, timeUnits.monthUnitSeconds);

        FindPlayers();
        FindGamePlayTimer();
        FindUtilityCalculator();

        if (gamePlayTimer != null)
            gamePlayTimer.Game_SetGameDuration(stats.time);

        stats.partsUnlockLevel = 1;
    }

    public void FindPlayers()
    {
        CharacterController_2D[] _players = GameObject.FindObjectsOfType<CharacterController_2D>(true);

        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i] != null && !players.Contains(_players[i]))
                players.Add(_players[i]);
        }
    }

    public void FindGamePlayTimer()
    {
        if (gamePlayTimer == null)
        {
            if (gameObject.TryGetComponent<GamePlayTimer>(out GamePlayTimer _gameTimer))
            {
                gamePlayTimer = _gameTimer;
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

    #endregion

    //  Player Functions     ----------------------------------------------------------------------------------------------------
    #region Player Functions

    public void Player_AddSelf(CharacterController_2D _player)
    {
        if (!players.Contains(_player))
            players.Add(_player);
    }

    public void Player_RemoveSelf(CharacterController_2D _player)
    {
        if (players.Contains(_player))
            players.Remove(_player);

        if (players.Count > 0)
            players.TrimExcess();
    }

    public void Players_SetPauseInputsState(bool _pause)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
                players[i].PauseInputs = _pause;
        }
    }

    #endregion

    //  Decision Functions      -----------------------------------------------------------------------------------------------
    #region Decision Functions

    public void DecisionMade(DecisionEntry _decisionInfo)
    {

        if (_decisionInfo != null)
        {
            DecisionEntry newEntry = new DecisionEntry();
            data.decisionsCount++;

            //  Timestamp
            newEntry.decisionNum = data.decisionsCount;
            newEntry.decisionTime = Time.time;
            timeTracker.NewDecisionMade();
            newEntry.decisionInterval = timeTracker.timeInterval;

            newEntry.categoryType = _decisionInfo.categoryType;

            //  Budget Cost
            newEntry.budget_CostLevel = _decisionInfo.budget_CostLevel;
            newEntry.budget_CostAmount = _decisionInfo.budget_CostAmount;

            //  Time Cost
            newEntry.time_CostUnits = _decisionInfo.time_CostUnits;
            newEntry.time_CostLevel = _decisionInfo.time_CostLevel;
            newEntry.time_CostAmount = _decisionInfo.time_CostAmount;

            //  Parts
            newEntry.partType = _decisionInfo.partType;
            newEntry.partLevel = _decisionInfo.partLevel;
            newEntry.partUtility = _decisionInfo.partUtility;

            //  Team
            newEntry.team_MemberType = _decisionInfo.team_MemberType;
            newEntry.team_DiscountAmmount = _decisionInfo.team_DiscountAmmount;
            newEntry.team_EffectType = _decisionInfo.team_EffectType;
            newEntry.team_unlockType = _decisionInfo.team_unlockType;

            //  Loan
            newEntry.fundingType = _decisionInfo.fundingType;
            newEntry.funding_Level = _decisionInfo.funding_Level;
            newEntry.funding_AwardAmount = _decisionInfo.funding_AwardAmount;
            newEntry.funding_InterestPercentage = _decisionInfo.funding_InterestPercentage;
            newEntry.funding_InterestAmount = _decisionInfo.funding_InterestAmount;
            newEntry.funding_EquityAmount = _decisionInfo.funding_EquityAmount;
            newEntry.funding_HitRate = _decisionInfo.funding_HitRate;
            newEntry.funding_Successful = _decisionInfo.funding_Successful;

            //  Customer Discovery
            newEntry.feedback_Level = _decisionInfo.feedback_Level;
            newEntry.globalMarket_Level = _decisionInfo.globalMarket_Level;

            //  Networking
            newEntry.networkingType = _decisionInfo.networkingType;
            newEntry.networkingLevel = _decisionInfo.networkingLevel;

            newEntry.delimiter = GetDelimiterString();

            //TODO: Apply Decision Vars
            ApplyDecisionVariables(newEntry);
            decisions.Add(newEntry);

            Decision_UpdateAllNodes();

            Checkpoint_RunCheckpointEvent();

            ////Original Version Code
            //{
            ///
            //// Decision and Resource Types
            //newEntry.categoryType = _decisionInfo.categoryType;
            //newEntry.resourceCostType = _decisionInfo.resourceCostType;
            //newEntry.teamMemberType = _decisionInfo.teamMemberType;
            //newEntry.fundingType = _decisionInfo.fundingType;
            ////  Device Part Settings
            //newEntry.devicePart = new DevicePart();
            //newEntry.devicePart.type = _decisionInfo.devicePart.type;
            //newEntry.devicePart.partLevel = _decisionInfo.devicePart.partLevel;
            //newEntry.devicePart.utilityVal = _decisionInfo.devicePart.utilityVal;
            ////newEntry.devicePart = _decisionInfo.devicePart;
            //newEntry.costLevel = _decisionInfo.costLevel;
            //newEntry.costAmount = _decisionInfo.costAmount;
            //
            //if (_decisionInfo.network)
            //{
            //    newEntry.network = _decisionInfo.network;
            //    newEntry.networkCost = _decisionInfo.networkCost;
            //}
            //
            //if (_decisionInfo.customerDiscovery)
            //{
            //    newEntry.customerDiscovery = _decisionInfo.customerDiscovery;
            //    newEntry.discoveryCost = _decisionInfo.discoveryCost;
            //}

            ////TODO: Apply Decision Vars
            //ApplyDecisionVariables(newEntry);
            //decisions.Add(newEntry);
            //}
        }
    }

    public void ApplyDecisionVariables(DecisionEntry _decisionInfo)
    {
        //Decision Vars
        {


            //        public int decisionNum;

            //        public float decisionTime;
            //        public float decisionInterval;
            //        public CategoryType categoryType;

            //        public int budget_CostLevel;
            //        public float budget_CostAmount;

            //        public TimeIntervals time_CostUnits;
            //        public int time_CostLevel;
            //        public float time_CostAmount;

            //        public PartTypes partType;
            //        public int partLevel;
            //        public float partUtility;

            //        public TeamMemberType team_MemberType;
            //        public float team_DiscountAmmount;
            //        public UnlockType team_unlockType;

            //        public FundingType fundingType;
            //        public int funding_Level;
            //        public float funding_AwardAmount;
            //        public float funding_InterestPercentage;
            //        public float funding_InterestAmount;
            //        public float funding_EquityAmount;
            //        public float funding_HitRate;
            //        public bool funding_Successful;

            //        public int feedback_Level;
            //        public int globalMarket_Level;

            //        public string info_Total;
        }

        if (_decisionInfo != null)
        {
            switch (_decisionInfo.categoryType)
            {
                case CategoryType.NONE:
                    break;
                case CategoryType.TEAM:
                    Apply_DecisionSpecs_Team(_decisionInfo.team_MemberType, _decisionInfo.team_DiscountAmmount, _decisionInfo.team_EffectType, _decisionInfo.team_unlockType, _decisionInfo.budget_CostAmount);
                    break;
                case CategoryType.NETWORKING:
                    break;
                case CategoryType.FUNDING:
                    Apply_DecisionSpecs_Funding(_decisionInfo);
                    break;
                case CategoryType.CUSTOMER_DISCOVERY:
                    Apply_DecisionSpecs_CustomerDiscovery(_decisionInfo.feedback_Level, _decisionInfo.globalMarket_Level);
                    break;
                case CategoryType.DEVICE:
                    Apply_DecisionSpecs_Device(_decisionInfo.partType, _decisionInfo.partLevel, _decisionInfo.budget_CostAmount);
                    break;
                default:
                    break;
            }

        }

    }

    public void Apply_DecisionSpecs_Team(TeamMemberType _type, float _discount, EffectType _effect, UnlockType _unlock, float _costAmount_Budget)
    {
        //  Debug Message Printout
        Debug.Log("ApplyDecision_Team=> MemberType: " + _type.ToString() + ", Discount: " + _discount.ToString() +
            ", Effect: " + _effect.ToString() + ", Unlock: " + _unlock.ToString() + ", Cost: " + _costAmount_Budget.ToString());

        //  Update Tracked/Collected Data
        data.Budget_AddToTotalSpent(_costAmount_Budget);
        data.Team_AddTypesHired(_type.ToString());
        data.Team_SetNumberHired(data.team_NumMembersHired + 1);

        //  Apply costs, discounts, unlocks, and effects to primary stats
        stats.Spend_Budget(_costAmount_Budget);
        stats.UpdateDesignDiscountModifier(_discount);
        if (_effect == EffectType.UNLOCK)
        {
            unlockEvents.Unlock_RunEventByType(_unlock);
            switch (_type)
            {
                case TeamMemberType.NONE:
                    break;
                case TeamMemberType.TYPE_01:
                    stats.UpdatePartsUnlockLevel(2);
                    break;
                case TeamMemberType.TYPE_02:
                    stats.UpdatePartsUnlockLevel(3);
                    break;
                case TeamMemberType.TYPE_03:
                    break;
                case TeamMemberType.TYPE_04:
                    stats.UnlockGlobalMarket();
                    unlockEvents.Unlock_GlobalMarketUnlocked();
                    break;
                case TeamMemberType.TYPE_05:
                    break;
                case TeamMemberType.TYPE_06:
                    break;
                default:
                    break;
            }

        }

        //  Add hired Team Member to the team
        Team_AddMember(_type);
    }

    public void Team_AddMember(TeamMemberType _type)
    {
        TeamMember newMember = new TeamMember();
        newMember.memberType = _type;
        newMember.FindValuesByType(teamValues);
        newMember.SetDelimiter(GetDelimiterString());

        team.Add(newMember);
    }

    public void Apply_DecisionSpecs_Funding(DecisionEntry _info)
    {
        // Apply Time Cost
        stats.Spend_Time(_info.time_CostAmount);

        //Check Hit Rate
        bool success = Funding_CheckHitSuccess(_info.funding_HitRate);
        //Apply hit rate success/fail outcome
        _info.funding_Successful = success;
        _info.Info_UpdateInfoDecisionString();

        //Broadcast Success/Fail state to decision nodes
        Broadcast_FundingSuccessEvent(_info, success);

        //  Update Tracked/Collected Data
        data.Time_AddToTotalSpent(_info.time_CostAmount);
        data.Funding_AddFundingAttempt(success);

        // Success (hitRate)
        if (success)
        {
            //success->AddBudgetAmount
            stats.AddFunding(_info.funding_AwardAmount);
            //success->AddInterestAmount
            stats.UpdateInterestPerMonth(_info.funding_InterestAmount);
            //success->AddEquityAmount
            stats.UpdateEquityValue(_info.funding_EquityAmount);

            //  Update collected data
            data.Funding_AddToTotalGained(_info.funding_AwardAmount);
            data.Funding_AddTypeAndLevel(_info.fundingType.ToString(), _info.funding_Level.ToString());

        }

        //// Fail (hitRate)
        //else
        //{

        //}
    }

    public void Apply_DecisionSpecs_CustomerDiscovery(int _feedbackLevel, int _globalMarketLevel)
    {
        if (_feedbackLevel > 0)
        {
            float _budgetCost = costValues.GetCost_Budget_CustomerDiscoveryByLevel(_feedbackLevel);
            float _timeCost = costValues.GetCost_Time_CustomerDiscoveryByLevel(_feedbackLevel);

            stats.Spend_Budget(_budgetCost);
            stats.Spend_Time(_timeCost);
            stats.UpdateFeedbackLevel(_feedbackLevel);

            data.Budget_AddToTotalSpent(_budgetCost);
            data.Time_AddToTotalSpent(_timeCost);

            //stats.Spend_Budget(costValues.GetCost_Budget_CustomerDiscoveryByLevel(_feedbackLevel));
            //stats.Spend_Time(costValues.GetCost_Time_CustomerDiscoveryByLevel(_feedbackLevel));
            //stats.UpdateFeedbackLevel(_feedbackLevel);


        }

        if (_globalMarketLevel > 0)
        {
            float _budgetCost = costValues.GetCost_Budget_GlobalMarketByLevel(_globalMarketLevel);
            //float _budgetCost = costValues.GetCost_Budget_CustomerDiscoveryByLevel(_feedbackLevel);

            stats.Spend_Budget(_budgetCost);
            stats.UpdateGlobalMarketLevel(_globalMarketLevel);

            data.Budget_AddToTotalSpent(_budgetCost);

            Debug.Log("Global Market -> Unlock Level: " + _globalMarketLevel.ToString());


            //stats.Spend_Budget(costValues.GetCost_Budget_GlobalMArketByLevel(_globalMarketLevel));
            //stats.UpdateGlobalMarketLevel(_globalMarketLevel);
        }
    }

    public void Apply_DecisionSpecs_Device(PartTypes _partType, int _partLevel, float _costAmount_Budget)
    {
        float _budgetCost = stats.GetDiscountedDesignCost(_costAmount_Budget);

        stats.Spend_Budget(_budgetCost);
        //stats.Spend_Budget(stats.GetDiscountedDesignCost(_costAmount_Budget));
        //stats.Spend_Budget(_costAmount_Budget);
        device.SetNewPart(_partType, _partLevel);
        device.GetUtilityValueByPart(partsValues);
        //device.SetNewPart(_part);
        stats.utility_Bin = device.utility_Binary;
        Utility_CalculateDeviceUtility();


        data.Budget_AddToTotalSpent(_budgetCost);
        data.DesignDevice_SetPartLevel(_partType, _partLevel);
    }

    public void Decision_UpdateAllNodes()
    {
        DecisionNode[] _nodes = GameObject.FindObjectsOfType<DecisionNode>(true);

        for (int i = 0; i < _nodes.Length; i++)
        {
            if (_nodes[i] != null)
            {
                _nodes[i].Button_CheckBlockingVars();
            }
        }
    }


    #endregion

    //  Funding Functions       -------------------------------------------------------------------------------------------
    #region Funding Functions

    public bool Funding_CheckHitSuccess(float _hitRate)
    {
        float failRate = 1.0f - _hitRate;
        //float failRate = 0.0f;

        float hitVal = UnityEngine.Random.Range(0.0f, 1.0f);
        float hitVal_Modified = hitVal;
        if (stats.fundingModifier > 0.0f)
            hitVal_Modified = hitVal * stats.fundingModifier;

        if (debug)
            Debug.Log("FundingHitSuccess-> HitVal: " + hitVal + ", FundingModifier: " + stats.fundingModifier + ", ModifiedHitVal: " + hitVal_Modified);

        if (hitVal_Modified >= failRate)
            return true;

        return false;
    }

    public void Broadcast_FundingSuccessEvent(DecisionEntry _info, bool _success)
    {
        DecisionNode[] _nodes = GameObject.FindObjectsOfType<DecisionNode>(true);

        for (int i = 0; i < _nodes.Length; i++)
        {
            if (_nodes[i] != null)
            {
                if (_nodes[i].Info.categoryType == CategoryType.FUNDING && _nodes[i].Info.fundingType == _info.fundingType && _nodes[i].Info.funding_Level == _info.funding_Level)
                    _nodes[i].FundingEvent_RunBySuccessState(_success);
            }
        }
    }

    public void Interest_CheckForPaymentDue()
    {
        if (stats.interestPerMonth > 0.0f)
        {
            if (timeUnits.interest_MonthOfLastPayment < timeUnits.Progress_GetMonthsPassed())
            {
                stats.Interest_ReduceBudget();
                data.Funding_AddInterestPayment(stats.interestPerMonth);
                timeUnits.interest_MonthOfLastPayment = timeUnits.Progress_GetMonthsPassed();
                Debug.Log("Interest: Payment made for month #" + timeUnits.interest_MonthOfLastPayment.ToString() + ", Interest amount: " + stats.interestPerMonth);
            }
        }
    }

    #endregion

    //  Customer Discovery Functions       -------------------------------------------------------------------------------------------
    #region Customer Discovery Functions

    public void Unlock_GlobalMarketEvent_Unlock()
    {
        unlockEvents.Unlock_GlobalMarketUnlocked();
    }

    public void Unlock_GlobalMarketEvent_UnlockByLevel(int _level)
    {
        unlockEvents.Unlock_GlobalMarketByLevel(_level);
    }

    public bool Feedback_IsAvailable()
    {
        if (stats.feedback_previewsUsed < stats.feedbackLevel)
            return true;

        return false;
    }

    public void Feedback_UseFeedback()
    {
        if (Feedback_IsAvailable())
        {
            stats.feedback_previewsUsed += 1;
        }
    }

    #endregion

    //  Utility Functions       -------------------------------------------------------------------------------------------
    #region Utility Functions

    public void Utility_CalculateDeviceUtility()
    {
        if (utilityCalculator == null)
            FindUtilityCalculator();

        if (utilityCalculator != null)
        {
            stats.utility_Bin = utilityCalculator.GetAccommodation(device.part_01.partLevel, device.part_02.partLevel, device.part_03.partLevel).ToString("00.00");
            Debug.Log("UtilityCalc => OutputVal: " + stats.utility_Bin + ", Pt01 Lvl: " + device.part_01.partLevel.ToString() + ", Pt02 Lvl: " + device.part_02.partLevel.ToString() + ", Pt03 Lvl: " + device.part_03.partLevel.ToString());
            //stats.utilityString = utilityCalculator.GetAccommodation(device.part_01.utilityVal.ToString(), device.part_02.utilityVal.ToString(), device.part_03.utilityVal.ToString()).ToString();
        }
    }

    public void Utility_CalculateDeviceUtility(string _pt01, string _pt02, string _pt03)
    {
        if (utilityCalculator != null)
        {
            stats.utility_Bin = utilityCalculator.GetAccommodation(_pt01, _pt02, _pt03).ToString("00.00");
            //stats.utilityString = utilityCalculator.GetAccommodation(device.part_01.utilityVal.ToString(), device.part_02.utilityVal.ToString(), device.part_03.utilityVal.ToString()).ToString();
        }
    }

    public bool Team_HireAvailableByType(TeamMemberType _type)
    {
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i] != null)
            {
                if (team[i].memberType == _type)
                    return false;
            }
        }

        return true;
    }



    public void Game_Reset()
    {
        stats.ResourceValues_Reset();
        data.checkPointNumber = 0;
        data.decisionsCount = 0;
        device.Reset();
        team.Clear();
        team.TrimExcess();
        decisions.Clear();
        decisions.TrimExcess();
        unlockEvents.Unlock_ResetLocks();
    }

    public string Team_GetMemberNameByIndex(int _index)
    {
        if (_index >= 0 && _index < team.Count)
        {
            if (team[_index] != null)
            {
                return teamValues.Team_GetNameByType(team[_index].memberType);
            }
        }

        return "None";
    }

    public string Device_GetPartNameByPartNum(int _partNum)
    {
        switch (_partNum)
        {
            case 1:
                if (device.part_01 != null)
                    return partsValues.GetPartName(device.part_01.type, device.part_01.partLevel);
                break;
            case 2:
                if (device.part_02 != null)
                    return partsValues.GetPartName(device.part_02.type, device.part_02.partLevel);
                break;
            case 3:
                if (device.part_03 != null)
                    return partsValues.GetPartName(device.part_03.type, device.part_03.partLevel);
                break;
            case 4:
                if (device.part_04 != null)
                    return partsValues.GetPartName(device.part_04.type, device.part_04.partLevel);
                break;
            case 5:
                if (device.part_05 != null)
                    return partsValues.GetPartName(device.part_05.type, device.part_05.partLevel);
                break;
            default:
                break;
        }


        return "None";
    }

    public string Device_GetPartLevelByPartNum(int _partNum)
    {
        switch (_partNum)
        {
            case 1:
                if (device.part_01 != null)
                    return device.part_01.partLevel.ToString();
                break;
            case 2:
                if (device.part_02 != null)
                    return device.part_02.partLevel.ToString();
                break;
            case 3:
                if (device.part_03 != null)
                    return device.part_03.partLevel.ToString();
                break;
            case 4:
                if (device.part_04 != null)
                    return device.part_04.partLevel.ToString();
                break;
            case 5:
                if (device.part_05 != null)
                    return device.part_05.partLevel.ToString();
                break;
            default:
                break;
        }


        return "00";
    }

    public void Device_CheckMissingPartsEvents()
    {
        device.Parts_RunMissingOrChosenEvents();
    }

    public string Time_GetTimeByUnits(float _time, TimeIntervals _type)
    {
        switch (_type)
        {
            case TimeIntervals.NONE:
                break;
            case TimeIntervals.DAY:
                return timeUnits.TimeUnit_GetDaysCount(_time);
            case TimeIntervals.WEEK:
                return timeUnits.TimeUnit_GetWeeksCount(_time);
            case TimeIntervals.MONTH:
                return timeUnits.TimeUnit_GetMonthsCount(_time);
            default:
                break;
        }

        return "";
    }

    public void GameRooms_UpdateTimeData()
    {
        data.SetGameRoomTimesInfo(rooms);
        //data.InfoUpdate_GameRoomTimes(true);
    }

    public string GetDelimiterString()
    {
        if (useAlternateDelimiter)
            return altDelimiter;
        else
            return ",";
    }

    public void Debug_CheckDebugVars()
    {
        if (debug_SelectEveryDecision)
        {
            Decisions_SelectEveryDecisionInGame();
            debug_SelectEveryDecision = false;
        }
    }

    public void Decisions_SelectEveryDecisionInGame()
    {
        stats.time = 999999999999.0f;
        stats.budget = 999999999999.0f;

        DecisionNode[] _nodes = GameObject.FindObjectsOfType<DecisionNode>(true);

        //TEAM, NETWORKING, FUNDING, CUSTOMER_DISCOVERY, DEVICE

        //Team
        for (int i = 0; i < _nodes.Length; i++)
        {
            if (_nodes[i] != null)
            {
                if (_nodes[i].Info.categoryType == CategoryType.TEAM)
                    _nodes[i].DecisionSelected_DebugOverride();
            }
        }

        //Networking
        for (int j = 0; j < _nodes.Length; j++)
        {
            if (_nodes[j] != null)
            {
                if (_nodes[j].Info.categoryType == CategoryType.NETWORKING)
                    _nodes[j].DecisionSelected_DebugOverride();
            }
        }

        //Funding
        for (int k = 0; k < _nodes.Length; k++)
        {
            if (_nodes[k] != null)
            {
                if (_nodes[k].Info.categoryType == CategoryType.FUNDING)
                    _nodes[k].DecisionSelected_DebugOverride();
            }
        }

        //Customer Discovery
        for (int l = 0; l < _nodes.Length; l++)
        {
            if (_nodes[l] != null)
            {
                if (_nodes[l].Info.categoryType == CategoryType.CUSTOMER_DISCOVERY)
                    _nodes[l].DecisionSelected_DebugOverride();
            }
        }

        //Device
        for (int m = 0; m < _nodes.Length; m++)
        {
            if (_nodes[m] != null)
            {
                if (_nodes[m].Info.categoryType == CategoryType.DEVICE)
                    _nodes[m].DecisionSelected_DebugOverride();
            }
        }

        ////All (no specified order)
        //for (int n = 0; n < _nodes.Length; n++)
        //{
        //    if (_nodes[n] != null)
        //    {
        //        _nodes[n].DecisionSelected_DebugOverride();
        //    }
        //}

        isDebugData = true;
        gamePlayTimer.GameOver_SetEndGameNowState(true);
    }


    public void Checkpoint_RunCheckpointEvent()
    {
        if (useCheckpoints)
        {
            data.checkPointNumber++;
            decisionAddedEvent.Invoke();
            Debug.Log("TrackedVars-> Checkpoint Event Called");
        }
    }

    #endregion

    //  Info Printout Functions       -------------------------------------------------------------------------------------------
    #region Info Printout Functions

    public void Info_UpdateEndgameStats()
    {
        //  Add Number of Decisions Made
        data.Decisions_SetCount(decisions.Count);

        //  Add Ending Budget (Resource)
        data.Budget_SetEndTotal(stats.budget);

        //  Add Ending Time (Resource)
        data.Time_SetEndTotal(stats.time);

        //  Add Ending Utility
        data.DesignDevice_SetUtility(stats.utility);

        //  Add Ending Utility (Binary)
        data.DesignDevice_SetUtility_Binary(stats.utility_Bin);

        //  Add Ending Number Team Members Hired
        data.Team_SetNumberHired(team.Count);

        //  Add Ending Team Member Types
        data.Team_SetTypesHired_Full(Info_GetPrintout_TeamTypes());

        //  Add Ending Part 01 Level
        data.DesignDevice_SetPartLevel(1, device.part_01.partLevel);
        //  Add Ending Part 02 Level
        data.DesignDevice_SetPartLevel(2, device.part_02.partLevel);
        //  Add Ending Part 03 Level
        data.DesignDevice_SetPartLevel(3, device.part_03.partLevel);

        //  Add Ending Interest Per Month
        data.Funding_SetInterestPerMonthAtEndgame(stats.interestPerMonth);

        //  Add Ending Equity Amount
        data.Funding_SetEquity(stats.equity);

        //  Add Ending Customer Feedback Level
        data.SetFeedbackLevel(stats.feedbackLevel);

        //  Add Ending Global Market Level
        data.SetGlobalMarketLevel(stats.globalMarketLevel);

        //  Add Game Room Times
        data.SetGameRoomTimesInfo(rooms);

    }

    public string Info_GetEndgameStats()
    {
        string _info = "";

        Info_UpdateEndgameStats();
        data.InfoUpdate_OutputTotal(data.info_addLabels);

        if (isDebugData)
        {
            _info = "\n" + 
                "-----------------------------------------" + "\n" +
                "--  !!THIS IS A DEBUGGING DATA SET!!  --" + "\n" +
                "-----------------------------------------" + "\n\n";
        }

        if (data.info_addOutputKey)
            _info = _info + "\n" +
                "Decisions Count" + delimiter +
                "Remaining Budget" + delimiter +
                "Remaining Time" + delimiter +
                "Device Utility" + delimiter +
                //"Team Members Hired" + delimiter +
                //"Team Member Types" + delimiter +
                "Part01 Level" + delimiter +
                "Part 02 Level" + delimiter +
                "Part 03 Level" + delimiter +
                "Interest Per Month" + delimiter +
                "Equity Amount" + delimiter +
                "Customer Feedback Level" + delimiter +
                "Global Market Level" + delimiter + "\n";

        _info = _info +
            data.decisionsCount.ToString() + delimiter + " " +
            data.budget_RemainingAtGameOver.ToString() + delimiter + " " +
            data.time_RemainingAtGameOver.ToString() + delimiter + " " +
            //data.designDevice_Utility.ToString() + ", " +
            data.designDevice_UtilityBinary.ToString() + delimiter + " " +
            //data.team_NumMembersHired.ToString() + delimiter + " " +
            //data.team_TypesHired +
            data.designDevice_Part01_Level.ToString() + delimiter + " " +
            data.designDevice_Part02_Level.ToString() + delimiter + " " +
            data.designDevice_Part03_Level.ToString() + delimiter + " " +
            data.funding_InterestPerMonth.ToString() + delimiter + " " +
            data.funding_Equity.ToString() + delimiter + " " +
            data.customerDiscovery_FeedbackLevel.ToString() + delimiter + " " +
            data.customerDiscovery_GlobalMarketLevel.ToString() + delimiter + "\n";

        //if (data.info_addOutputKey)
        //    _info = "\n" +
        //        "Decisions Count, Remaining Budget, Remaining Time, Device Utility, " +
        //        "Team Members Hired, Team Member Types, Part01 Level, Part 02 Level, Part 03 Level," +
        //        "Interest Per Month, Equity Amount, Customer Feedback Level, Global Market Level," + "\n";

        //_info = _info +
        //    data.decisionsCount.ToString() + ", " +
        //    data.budget_RemainingAtGameOver.ToString() + ", " +
        //    data.time_RemainingAtGameOver.ToString() + ", " +
        //    //data.designDevice_Utility.ToString() + ", " +
        //    data.designDevice_UtilityBinary.ToString() + ", " +
        //    data.team_NumMembersHired.ToString() + ", " +
        //    data.team_TypesHired + 
        //    //data.team_TypesHired + ", " +
        //    data.designDevice_Part01_Level.ToString() + ", " +
        //    data.designDevice_Part02_Level.ToString() + ", " +
        //    data.designDevice_Part03_Level.ToString() + ", " +
        //    data.funding_InterestPerMonth.ToString() + ", " +
        //    data.funding_Equity.ToString() + ", " +
        //    data.customerDiscovery_FeedbackLevel.ToString() + ", " +
        //    data.customerDiscovery_GlobalMarketLevel.ToString() + ",\n";

        return _info;
    }

    public string Info_GetEndgameStats(bool _addOutputKey)
    {
        string _info = "";

        Info_UpdateEndgameStats();
        data.InfoUpdate_OutputTotal(_addOutputKey);

        if (isDebugData)
        {
            _info = "\n" +
                "-----------------------------------------" + "\n" +
                "--  !!THIS IS A DEBUGGING DATA SET!!  --" + "\n" +
                "-----------------------------------------" + "\n\n";
        }

        if (_addOutputKey)
            _info = _info + "\n" +
                "Decisions Count" + delimiter +
                "Remaining Budget" + delimiter +
                "Remaining Time" + delimiter +
                "Device Utility" + delimiter +
                //"Team Members Hired" + delimiter +
                //"Team Member Types" + delimiter +
                "Part01 Level" + delimiter +
                "Part 02 Level" + delimiter +
                "Part 03 Level" + delimiter +
                "Interest Per Month" + delimiter +
                "Equity Amount" + delimiter +
                "Customer Feedback Level" + delimiter +
                "Global Market Level" + delimiter + "\n";

        _info = _info +
            data.decisionsCount.ToString() + delimiter + " " +
            data.budget_RemainingAtGameOver.ToString() + delimiter + " " +
            data.time_RemainingAtGameOver.ToString() + delimiter + " " +
            //data.designDevice_Utility.ToString() + ", " +
            data.designDevice_UtilityBinary + delimiter + " " +
            //data.team_NumMembersHired.ToString() + delimiter + " " +
            //data.team_TypesHired +
            data.designDevice_Part01_Level.ToString() + delimiter + " " +
            data.designDevice_Part02_Level.ToString() + delimiter + " " +
            data.designDevice_Part03_Level.ToString() + delimiter + " " +
            data.funding_InterestPerMonth.ToString() + delimiter + " " +
            data.funding_Equity.ToString() + delimiter + " " +
            data.customerDiscovery_FeedbackLevel.ToString() + delimiter + " " +
            data.customerDiscovery_GlobalMarketLevel.ToString() + delimiter + "\n";

        //Info_UpdateEndgameStats();
        //data.InfoUpdate_OutputTotal(_addOutputKey);

        //if (_addOutputKey)
        //    _info = "\n" +
        //        "Decisions Count, Remaining Budget, Remaining Time, Device Utility," +
        //        "Team Members Hired, Team Member Types, Part01 Level, Part 02 Level, Part 03 Level," +
        //        "Interest Per Month, Equity Amount, Customer Feedback Level, Global Market Level," + "\n";

        //_info = _info +
        //    data.decisionsCount.ToString() + ", " +
        //    data.budget_RemainingAtGameOver.ToString() + ", " +
        //    data.time_RemainingAtGameOver.ToString() + ", " +
        //    //data.designDevice_Utility.ToString() + ", " +
        //    data.designDevice_UtilityBinary + ", " +
        //    data.team_NumMembersHired.ToString() + ", " +
        //    data.team_TypesHired + 
        //    //data.team_TypesHired + ", " +
        //    data.designDevice_Part01_Level.ToString() + ", " +
        //    data.designDevice_Part02_Level.ToString() + ", " +
        //    data.designDevice_Part03_Level.ToString() + ", " +
        //    data.funding_InterestPerMonth.ToString() + ", " +
        //    data.funding_Equity.ToString() + ", " +
        //    data.customerDiscovery_FeedbackLevel.ToString() + ", " +
        //    data.customerDiscovery_GlobalMarketLevel.ToString() + ",\n";

        return _info;
    }

    public string Info_GetFullPrintout_Decisions()
    {
        string _printout = "";

        if (data.info_addOutputKey)
            _printout = "\n" +
                "Decision Number" + delimiter +
                "Timestamp" + delimiter +
                "Time Interval" + delimiter +
                "Category" + delimiter +
                "Budget Cost Level" + delimiter +
                "Budget Cost Amount" + delimiter +
                "Time Cost Units" + delimiter +
                "Time Cost Level" + delimiter +
                "Time Cost Amount" + delimiter +
                "Part Type" + delimiter +
                "Part Level" + delimiter +
                "Part Utility" + delimiter +
                "Team Member Type" + delimiter +
                "Team Effect Type" + delimiter +
                "Team Discount Amount" + delimiter +
                "Team Unlock Type" + delimiter +
                "Funding Type" + delimiter +
                "Funding Level" + delimiter +
                "Funding Award Amount" + delimiter +
                "Interest Percentage" + delimiter +
                "Interest Amount" + delimiter +
                "Equity Amount" + delimiter +
                "Funding Hit Rate" + delimiter +
                "Funding Successful" + delimiter +
                "Feedback Level" + delimiter +
                "Global Market Level" + delimiter +
                "Networking Type" + delimiter +
                "Networking Level" + delimiter + "\n";

        for (int i = 0; i < decisions.Count; i++)
        {
            if (decisions[i] != null)
                _printout = _printout + decisions[i].Info_GetFullDecisionString() + delimiter + "\n";
            //_printout = _printout + decisions[i].Info_GetFullDecisionString() + ",\n";
        }

        //if (data.info_addOutputKey)
        //    _printout = "\n" +
        //        "Decision Number, Timestamp, Time Interval, Category," +
        //        "Budget Cost Level, Budget Cost Amount, Time Cost Units, Time Cost Level, Time Cost Amount," +
        //        "Part Type, Part Level, Part Utility, Team Member Type, Team Effect Type, Team Discount Amount, Team Unlock Type," +
        //        "Funding Type, Funding Level, Funding Award Amount, Interest Percentage, Interest Amount, Equity Amount, Funding Hit Rate, Funding Successful," +
        //        "Feedback Level, Global Market Level, \n";

        //for (int i = 0; i < decisions.Count; i++)
        //{
        //    if (decisions[i] != null)
        //        _printout = _printout + decisions[i].Info_GetFullDecisionString() + ",\n";
        //}

        return _printout;
    }

    public string Info_GetFullPrintout_Decisions(bool _addOutputKey)
    {
        string _printout = "";

        if (_addOutputKey)
            _printout = "\n" +
                "Decision Number" + delimiter +
                "Timestamp" + delimiter +
                "Time Interval" + delimiter +
                "Category" + delimiter +
                "Budget Cost Level" + delimiter +
                "Budget Cost Amount" + delimiter +
                "Time Cost Units" + delimiter +
                "Time Cost Level" + delimiter +
                "Time Cost Amount" + delimiter +
                "Part Type" + delimiter +
                "Part Level" + delimiter +
                "Part Utility" + delimiter +
                "Team Member Type" + delimiter +
                "Team Effect Type" + delimiter +
                "Team Discount Amount" + delimiter +
                "Team Unlock Type" + delimiter +
                "Funding Type" + delimiter +
                "Funding Level" + delimiter +
                "Funding Award Amount" + delimiter +
                "Interest Percentage" + delimiter +
                "Interest Amount" + delimiter +
                "Equity Amount" + delimiter +
                "Funding Hit Rate" + delimiter +
                "Funding Successful" + delimiter +
                "Feedback Level" + delimiter +
                "Global Market Level" + delimiter +
                "Networking Type" + delimiter +
                "Networking Level" + delimiter + "\n";

        for (int i = 0; i < decisions.Count; i++)
        {
            if (decisions[i] != null)
                _printout = _printout + decisions[i].Info_GetFullDecisionString() + delimiter + "\n";
            // _printout = _printout + decisions[i].Info_GetFullDecisionString() + ",\n";
        }

        //if (_addOutputKey)
        //    _printout = "\n" +
        //        "Decision Number, Timestamp, Time Interval, Category," +
        //        "Budget Cost Level, Budget Cost Amount, Time Cost Units, Time Cost Level, Time Cost Amount," +
        //        "Part Type, Part Level, Part Utility, Team Member Type, Team Effect Type, Team Discount Amount, Team Unlock Type," +
        //        "Funding Type, Funding Level, Funding Award Amount, Interest Percentage, Interest Amount, Equity Amount, Funding Hit Rate, Funding Successful," +
        //        "Feedback Level, Global Market Level, \n";

        //for (int i = 0; i < decisions.Count; i++)
        //{
        //    if (decisions[i] != null)
        //        _printout = _printout + decisions[i].Info_GetFullDecisionString() + ",\n";
        //}

        return _printout;
    }

    public string Info_GetFullPrintout_Team()
    {
        string _printout = "";

        if (data.info_addOutputKey)
            _printout =
                "\n" + "Team Member Type" + delimiter +
                "Effect Type" + delimiter +
                "Effect Amount" + delimiter +
                "Unlock Type" + delimiter + "\n";
        //_printout = "\n" + "Team Member Type, Effect Type, Effect Amount, Unlock Type,\n";

        for (int i = 0; i < team.Count; i++)
        {
            if (team[i] != null)
                _printout = _printout + team[i].Info_GetTeamMemberInfoString() + delimiter + "\n";
            //_printout = _printout + team[i].Info_GetTeamMemberInfoString() + ",\n";
        }

        //if (data.info_addOutputKey)
        //    _printout = "\n" + "Team Member Type, Effect Type, Effect Amount, Unlock Type,\n";

        //for (int i = 0; i < team.Count; i++)
        //{
        //    if (team[i] != null)
        //        _printout = _printout + team[i].Info_GetTeamMemberInfoString() + ",\n";
        //}

        return _printout;
    }

    public string Info_GetFullPrintout_Team(bool _addOutputKey)
    {
        string _printout = "";

        if (_addOutputKey)
            _printout =
                "\n" + "Team Member Type" + delimiter +
                "Effect Type" + delimiter +
                "Effect Amount" + delimiter +
                "Unlock Type" + delimiter + "\n";

        for (int i = 0; i < team.Count; i++)
        {
            if (team[i] != null)
                _printout = _printout + team[i].Info_GetTeamMemberInfoString() + delimiter + "\n";
            // _printout = _printout + team[i].Info_GetTeamMemberInfoString() + ",\n";
        }

        //if (_addOutputKey)
        //    _printout = "\n" + "Team Member Type, Effect Type, Effect Amount, Unlock Type,\n";

        //for (int i = 0; i < team.Count; i++)
        //{
        //    if (team[i] != null)
        //        _printout = _printout + team[i].Info_GetTeamMemberInfoString() + ",\n";
        //}

        return _printout;
    }

    public string Info_GetFullPrintout_Device()
    {
        string _printout = "";


        if (data.info_addOutputKey)
        {
            _printout = "\n" +
                "Utility" + delimiter +
                "Part_01: Part Type" + delimiter + "Part Level" + delimiter + "Part Utility" + delimiter +
                "Part_02: Part Type" + delimiter + "Part Level" + delimiter + "Part Utility" + delimiter +
                "Part_03: Part Type" + delimiter + "Part Level" + delimiter + "Part Utility" + delimiter + "\n" +
                device.Info_GetFullPartsInfoString() + delimiter + "\n";
        }
        else
            _printout = device.Info_GetFullPartsInfoString() + delimiter + "\n";

        //if (data.info_addOutputKey)
        //{
        //    _printout = "\n" +
        //        "Utility, " +
        //        "Part_01: Part Type, Part Level, Part Utility, " +
        //        "Part_02: Part Type, Part Level, Part Utility, " +
        //        "Part_03: Part Type, Part Level, Part Utility,\n" +
        //        device.Info_GetFullPartsInfoString();
        //}
        //else
        //    _printout = device.Info_GetFullPartsInfoString();

        return _printout;
    }

    public string Info_GetFullPrintout_Device(bool _addOutputKey)
    {
        string _printout = "";

        if (_addOutputKey)
        {
            _printout = "\n" +
                 "Utility" + delimiter +
                 "Part_01: Part Type" + delimiter + "Part Level" + delimiter + "Part Utility" + delimiter +
                 "Part_02: Part Type" + delimiter + "Part Level" + delimiter + "Part Utility" + delimiter +
                 "Part_03: Part Type" + delimiter + "Part Level" + delimiter + "Part Utility" + delimiter + "\n" +
                 device.Info_GetFullPartsInfoString() + delimiter + "\n";
        }
        else
            _printout = device.Info_GetFullPartsInfoString() + delimiter + "\n";

        //if (_addOutputKey)
        //{
        //    _printout = "\n" +
        //        "Utility, " +
        //        "Part_01: Part Type, Part Level, Part Utility, " +
        //        "Part_02: Part Type, Part Level, Part Utility, " +
        //        "Part_03: Part Type, Part Level, Part Utility,\n" +
        //        device.Info_GetFullPartsInfoString();
        //}
        //else
        //    _printout = device.Info_GetFullPartsInfoString();

        return _printout;
    }

    public string Info_GetFullPrintout_GameRooms()
    {
        string _printout = "";


        if (data.info_addOutputKey)
        {
            _printout = "\n" +
                 "Room Name" + delimiter +
                 "Time In Room" + delimiter + "\n" +
                 rooms.Info_GetFullRoomTimesInfoString() + delimiter + "\n";
        }
        else
            _printout = rooms.Info_GetFullRoomTimesInfoString() + delimiter + "\n";

        return _printout;
    }

    public string Info_GetFullPrintout_GameRooms(bool _addOutputKey)
    {
        string _printout = "";

        if (_addOutputKey)
        {
            _printout = "\n" +
                 "Room Name" + delimiter +
                 "Time In Room" + delimiter + "\n" +
                 rooms.Info_GetFullRoomTimesInfoString() + delimiter + "\n";
        }
        else
            _printout = rooms.Info_GetFullRoomTimesInfoString() + delimiter + "\n";

        return _printout;
    }

    public string Info_GetPrintout_TeamTypes()
    {
        string _printout = "";

        //if (data.info_addOutputKey)
        //    _printout = "\n" + "Team Member Type, Effect Type, Effect Amount, Unlock Type,\n";

        for (int i = 0; i < team.Count; i++)
        {
            if (team[i] != null)
                _printout = _printout + team[i].memberType.ToString() + delimiter;
            // _printout = _printout + team[i].memberType.ToString() + delimiter + " ";
        }

        //for (int i = 0; i < team.Count; i++)
        //{
        //    if (team[i] != null)
        //        _printout = _printout + team[i].memberType.ToString() + ", ";
        //}

        return _printout;
    }

    public string Info_EndGame_GetPrint_TimeSpent()
    {
        string _info = "";

        //stats.reset_time - data.time_RemainingAtGameOver

        _info = timeUnits.TimeUnit_GetWeeksCount(stats.reset_time - data.time_RemainingAtGameOver);
        //_info = timeUnits.TimeUnit_GetWeeksCount(data.time_TotalSpent);
        //_info = data.time_TotalSpent.ToString();
        //_info = (stats.reset_time - stats.time).ToString();

        return _info;
    }

    public string Info_EndGame_GetPrint_BudgetSpent()
    {
        return data.budget_TotalSpent.ToString("F0");
    }

    public string Info_EndGame_GetPrint_FinalAccommodation()
    {
        return data.designDevice_UtilityBinary;
    }

    
    #endregion

    //  Unity Functions       -------------------------------------------------------------------------------------------
    #region Unity Functions

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        timeUnits.Time_GetCurrentProgressUnits(stats.time);
        Interest_CheckForPaymentDue();

        Debug_CheckDebugVars();

    }

    #endregion

}
