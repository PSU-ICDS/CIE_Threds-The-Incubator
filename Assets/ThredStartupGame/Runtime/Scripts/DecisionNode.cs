using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DecisionNode : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] bool locked;
    [Space(10)]
    [SerializeField] bool singleRun;
    [SerializeField] bool hasBeenSelected;
    [Space(10)]
    [SerializeField] TrackedVariables dataTracker;
    [SerializeField] TrackedVariables.DecisionEntry decisionInfo;
    [SerializeField] AvailabilitySettings availabilitySettings;
    [Space(10)]
    [SerializeField] UnityEvent fundingEvent_Success;
    [SerializeField] UnityEvent fundingEvent_Fail;
    [Space(10)]
    [SerializeField] Button button;
    [SerializeField] bool debug_MakeDecision;
    [SerializeField] bool isDebugDecision;
    bool infoUpdated = false;
    bool lockedAtStart;

    public TrackedVariables.DecisionEntry Info { get => decisionInfo; }
    public bool Availability { get => GetAvailability(); set => SetAvailabilityState(value); }
    public bool HasBeenSelected { get => hasBeenSelected; }
    public bool InfoUpdated { get => infoUpdated; }
    public bool IsDebugDecision { get => isDebugDecision; }

    public Button SelectionButton { get => button; }


    [Serializable]
    public class AvailabilitySettings
    {
        public bool available;

        public bool CheckAvailability()
        {
            if (available)
                return true;

            return false;
        }

        public void SetAvailability(bool _available)
        {
            available = _available;
        }
    }

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        UpdateDecisionInfo();

        //FindDataTracker();
        //if (dataTracker != null)
        //{
        //    decisionInfo.Values_GetAllValues(dataTracker.CostsValues, dataTracker.PartsValues, dataTracker.TeamsValues);
        //    infoUpdated = true;
        //    //decisionInfo.GetCostAmountByLevel(dataTracker.CostSettings);
        //    //decisionInfo.GetPartUtilityByLevel(dataTracker.PartsValues);
        //}

        lockedAtStart = locked;
        Button_CheckBlockingVars();
        //Locked_StateChanged();
    }

    void UpdateDecisionInfo()
    {
        FindDataTracker();
        if (dataTracker != null)
        {
            decisionInfo.Values_GetAllValues(dataTracker.CostsValues, dataTracker.PartsValues, dataTracker.TeamsValues);
            decisionInfo.delimiter = dataTracker.GetDelimiterString();
            infoUpdated = true;
            //decisionInfo.GetCostAmountByLevel(dataTracker.CostSettings);
            //decisionInfo.GetPartUtilityByLevel(dataTracker.PartsValues);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }
    private void OnEnable()
    {
        CheckAvailabilityByTypeAndLevel();
        Button_CheckBlockingVars();
        //Locked_StateChanged();
    }

    // Update is called once per frame
    void Update()
    {
        if (debug_MakeDecision)
        {
            DecisionSelected();
            debug_MakeDecision = false;
        }
    }

    public void FindDataTracker()
    {
        if (active && dataTracker == null)
        {
            if (GameObject.FindObjectOfType<TrackedVariables>() != null)
                dataTracker = GameObject.FindObjectOfType<TrackedVariables>();
        }
    }

    public void DecisionSelected()
    {
        if (dataTracker == null)
            FindDataTracker();

        if (active && !locked && Availability && !hasBeenSelected && dataTracker != null)
        {
            Debug.Log("DecisionNode: DecisionSelected()");
            dataTracker.DecisionMade(decisionInfo);
            if (singleRun)
                hasBeenSelected = true;
        }
    }

    public void DecisionSelected_DebugOverride()
    {
        if (!isDebugDecision && active)
        {
            if (dataTracker == null)
                FindDataTracker();

            if (dataTracker != null)
            {
                Setup();

                Debug.Log("DecisionNode: DecisionSelected() for gameObject:" + gameObject.name);
                dataTracker.DecisionMade(decisionInfo);
                if (singleRun)
                    hasBeenSelected = true;
            }
        }
    }

    public void Locked_SetLockedState(bool _state)
    {
        if (locked != _state)
        {
            locked = _state;
            //Locked_StateChanged();
        }

        Button_CheckBlockingVars();
    }

    //public void Locked_StateChanged()
    //{
    //    if (locked)
    //        Button_SetInteractable(false);
    //    else
    //        Button_SetInteractable(Availability);
    //}

    public void Button_CheckBlockingVars()
    {
        if (button != null)
        {
            if (locked || hasBeenSelected || !Availability)
                Button_SetInteractable(false);
            else
                Button_SetInteractable(Availability);
        }
    }

    public void Button_SetInteractable(bool _interactable)
    {
        if (button != null)
            button.interactable = _interactable;
    }

    public void SetAvailabilityState(bool _available)
    {
        if (Availability != _available)
        {
            availabilitySettings.SetAvailability(_available);
            Button_SetInteractable(Availability);
        }

        Button_CheckBlockingVars();

        //if (locked)
        //    Button_SetInteractable(!locked);

    }



    public bool GetAvailability()
    {
        if (!locked && availabilitySettings.CheckAvailability() && CheckAvailabilityByCost())
            return true;

        return false;

    }

    public void CheckAvailabilityByTypeAndLevel()
    {
        if (dataTracker == null)
            FindDataTracker();

        if (active && dataTracker != null)
        {
            switch (decisionInfo.categoryType)
            {
                case TrackedVariables.CategoryType.NONE:
                    break;
                case TrackedVariables.CategoryType.TEAM:
                    Availability = dataTracker.Team_HireAvailableByType(decisionInfo.team_MemberType);
                    break;
                case TrackedVariables.CategoryType.NETWORKING:
                    break;
                case TrackedVariables.CategoryType.FUNDING:
                    break;
                case TrackedVariables.CategoryType.CUSTOMER_DISCOVERY:
                    if (decisionInfo.globalMarket_Level != 0)
                    {
                        if (!dataTracker.PrimaryStat.globalMarketUnlocked)
                            Availability = false;
                        else
                        {
                            if (decisionInfo.globalMarket_Level > 0)
                            {
                                if (decisionInfo.globalMarket_Level > dataTracker.PrimaryStat.globalMarketLevel)
                                    Availability = true;
                                else
                                    Availability = false;
                            }
                        }
                    }
                    else
                    {
                        if (dataTracker.PrimaryStat.feedbackLevel > 0 && decisionInfo.feedback_Level <= dataTracker.PrimaryStat.feedbackLevel)
                            Availability = false;

                    }

                    //if (decisionInfo.feedback_Level != 0 || decisionInfo.globalMarket_Level != 0)
                    //{
                    //    if (decisionInfo.feedback_Level > 0)
                    //    {
                    //        if (decisionInfo.feedback_Level <= dataTracker.PrimaryStat.feedbackLevel)
                    //            Availability = true;
                    //        else
                    //            Availability = false;
                    //    }

                    //    if (decisionInfo.globalMarket_Level > 0)
                    //    {
                    //        if (decisionInfo.globalMarket_Level <= dataTracker.PrimaryStat.globalMarketLevel)
                    //            Availability = true;
                    //        else
                    //            Availability = false;
                    //    }
                    //}

                    break;
                case TrackedVariables.CategoryType.DEVICE:
                    if (decisionInfo.partLevel <= dataTracker.PrimaryStat.partsUnlockLevel)
                        Availability = true;
                    else
                        Availability = false;

                    break;
                default:
                    break;
            }
        }
    }

    public bool CheckAvailabilityByCost()
    {
        if (decisionInfo.budget_CostAmount > 0.0f)
        {
            if (dataTracker.PrimaryStat.budget >= decisionInfo.budget_CostAmount)
                return true;

            Debug.Log("User does not have enough budget resource for the selected decision to be made!!");
        }

        if (decisionInfo.time_CostAmount > 0.0f)
        {
            if (dataTracker.PrimaryStat.time >= decisionInfo.time_CostAmount)
                return true;

            Debug.Log("User does not have enough time resource for the selected decision to be made!!");
        }

        if (decisionInfo.time_CostAmount == 0.0f && decisionInfo.budget_CostAmount == 0.0f)
            return true;

        return false;
    }

    public void FundingEvent_RunBySuccessState(bool _success)
    {
        decisionInfo.funding_Successful = _success;
        decisionInfo.Info_UpdateInfoDecisionString();

        if (_success)
        {
            fundingEvent_Success.Invoke();
            hasBeenSelected = true;
            Availability = false;
            Button_CheckBlockingVars();
        }
        else
        {
            fundingEvent_Fail.Invoke();
        }

    }

    public string Time_GetTimeByUnits(float _time, TrackedVariables.TimeIntervals _type)
    {
        return dataTracker.Time_GetTimeByUnits(_time, _type);
    }


    public void ResetDecisionSettings()
    {
        locked = lockedAtStart;
        hasBeenSelected = false;
        infoUpdated = false;
        UpdateDecisionInfo();
        Button_CheckBlockingVars();
    }
}
