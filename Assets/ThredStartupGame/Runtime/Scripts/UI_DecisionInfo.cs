using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DecisionInfo : MonoBehaviour
{
    [SerializeField] DecisionNode node;
    [SerializeField] string decisionLabel;
    [SerializeField] UI_Objs ui;
    public bool infoUpdated = false;
    public bool displayPercentageSymbol;
    public bool displayAsPercentage;
    public bool displayTimeUnit;
    [SerializeField] TrackedVariables.TimeIntervals timeUnits;

    [Serializable]
    public class UI_Objs
    {
        public bool addIdentifiers;
        public TMP_Text categoryObj;
        public TMP_Text labelObj_primary;
        public TMP_Text costObj_01_Type;
        public TMP_Text costObj_01_Amount;
        public TMP_Text costObj_02_Type;
        public TMP_Text costObj_02_Amount;
        public TMP_Text effectObj_01;
        public TMP_Text effectObj_02;
        public TMP_Text hitRateObj;
        public TMP_Text interestObj;
        public TMP_Text equityObj;

        public void UI_UpdateText_All(string _category, string _label, string _cost01Type, string _cost01Amount,
            string _cost02Type, string _cost02Amount, string _effect01, string _effect02,
            string _hitRate, string _interest, string _equity)
        {
            UI_UpdateText_Category(_category);
            UI_UpdateText_LabelPrimary(_label);
            UI_UpdateText_Cost_01_Type(_cost01Type);
            UI_UpdateText_Cost_01_Amount(_cost01Amount);
            UI_UpdateText_Cost_02_Type(_cost02Type);
            UI_UpdateText_Cost_02_Amount(_cost02Amount);
            UI_UpdateText_Effect_01(_effect01);
            UI_UpdateText_Effect_02(_effect02);
            UI_UpdateText_HitRate(_hitRate);
            UI_UpdateText_Interest(_interest);
            UI_UpdateText_Equity(_equity);
        }

        public void UI_UpdateText_Category(string _text)
        {
            if (categoryObj != null)
            {
                if (addIdentifiers)
                    categoryObj.text = "Category: " + _text;
                else
                    categoryObj.text = _text;
            }
        }

        public void UI_UpdateText_LabelPrimary(string _text)
        {
            if (labelObj_primary != null)
            {
                if (addIdentifiers)
                    labelObj_primary.text = "Decision: " + _text;
                else
                    labelObj_primary.text = _text;
            }
        }

        public void UI_UpdateText_Cost_01_Type(string _text)
        {
            if (costObj_01_Type != null)
            {
                if (addIdentifiers)
                    costObj_01_Type.text = "Cost Type: " + _text;
                else
                    costObj_01_Type.text = _text;
            }
        }

        public void UI_UpdateText_Cost_01_Amount(string _text)
        {
            if (costObj_01_Amount != null)
            {
                if (addIdentifiers)
                    costObj_01_Amount.text = "Cost Amount: " + _text;
                else
                    costObj_01_Amount.text = _text;
            }
        }

        public void UI_UpdateText_Cost_02_Type(string _text)
        {
            if (costObj_02_Type != null)
            {
                if (addIdentifiers)
                    costObj_02_Type.text = "Cost Type: " + _text;
                else
                    costObj_02_Type.text = _text;
            }
        }

        public void UI_UpdateText_Cost_02_Amount(string _text)
        {
            if (costObj_02_Amount != null)
            {
                //_text = _text.ToString("D");
                //_text = String.Format("{0:N0}", _text);
                //String.Format("N0", _text);
                //String.Format("F0", _text);
               //_text = String.Format("F0");
               //_text = String.Format("00", _text);

                if (addIdentifiers)
                    costObj_02_Amount.text = "Cost Amount: " + _text;
                else
                    costObj_02_Amount.text = _text;
            }
        }

        public void UI_UpdateText_Effect_01(string _text)
        {
            if (effectObj_01 != null)
            {
                if (addIdentifiers)
                    effectObj_01.text = "Effect 01: " + _text;
                else
                    effectObj_01.text = _text;
            }
        }

        public void UI_UpdateText_Effect_02(string _text)
        {
            if (effectObj_02 != null)
            {
                if (addIdentifiers)
                    effectObj_02.text = "Effect 02: " + _text;
                else
                    effectObj_02.text = _text;
            }
        }

        public void UI_UpdateText_HitRate(string _text)
        {
            if (hitRateObj != null)
            {
                if (addIdentifiers)
                    hitRateObj.text = "Hit Rate: " + _text;
                else
                    hitRateObj.text = _text;
            }
        }

        public void UI_UpdateText_Interest(string _text)
        {
            if (interestObj != null)
            {
                if (addIdentifiers)
                    interestObj.text = "Interest Per Month: " + _text;
                else
                    interestObj.text = _text;
            }
        }

        public void UI_UpdateText_Equity(string _text)
        {
            if (equityObj != null)
            {
                if (addIdentifiers)
                    equityObj.text = "Equity Amount: " + _text;
                else
                    equityObj.text = _text;
            }
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        FindDecisionNode();
    }

    void FindDecisionNode()
    {
        if (node == null)
        {
            if (gameObject.TryGetComponent<DecisionNode>(out DecisionNode _node))
            {
                node = _node;
            }
        }
    }

    public float GetPercentageFromFloat(float _value)
    {
        if (_value <= 1.0f)
        {
            return _value * 100.0f;
        }

        return _value;
    }

    // Update is called once per frame
    void Update()
    {
        if (!infoUpdated)
            UI_CheckInfoUpdated();
    }

    void UI_CheckInfoUpdated()
    {
        if (node != null && node.InfoUpdated)
        {
            string effectType = "";

            switch (node.Info.categoryType)
            {
                case TrackedVariables.CategoryType.NONE:
                    break;
                case TrackedVariables.CategoryType.TEAM:
                    effectType = node.Info.team_EffectType.ToString();
                    break;
                case TrackedVariables.CategoryType.NETWORKING:
                    break;
                case TrackedVariables.CategoryType.FUNDING:
                    effectType = node.Info.fundingType.ToString();
                    break;
                case TrackedVariables.CategoryType.CUSTOMER_DISCOVERY:
                    if (node.Info.globalMarket_Level > 0)
                        effectType = "Global Market";
                    else
                        effectType = "Feedback";
                    break;
                case TrackedVariables.CategoryType.DEVICE:
                    effectType = "New Part";
                    break;
                default:
                    break;
            }

            string effectAmount = "";

            switch (node.Info.categoryType)
            {
                case TrackedVariables.CategoryType.NONE:
                    break;
                case TrackedVariables.CategoryType.TEAM:
                    if (node.Info.team_EffectType == TrackedVariables.EffectType.DISCOUNT)
                        effectAmount = node.Info.team_DiscountAmmount.ToString();

                    if (node.Info.team_EffectType == TrackedVariables.EffectType.UNLOCK)
                        effectAmount = node.Info.team_unlockType.ToString();
                    break;
                case TrackedVariables.CategoryType.NETWORKING:
                    break;
                case TrackedVariables.CategoryType.FUNDING:
                    effectAmount = node.Info.funding_AwardAmount.ToString();
                    break;
                case TrackedVariables.CategoryType.CUSTOMER_DISCOVERY:
                    if (node.Info.globalMarket_Level > 0)
                        effectAmount = "Lvl: " + node.Info.globalMarket_Level.ToString();
                    else
                        effectAmount = "Lvl: " + node.Info.feedback_Level.ToString();
                    break;
                case TrackedVariables.CategoryType.DEVICE:
                    effectAmount = "Lvl: " + node.Info.partLevel.ToString();
                    break;
                default:
                    break;
            }

            string _label = "";

            switch (node.Info.categoryType)
            {
                case TrackedVariables.CategoryType.NONE:
                    _label = decisionLabel;
                    break;
                case TrackedVariables.CategoryType.TEAM:
                    _label = "Menber: " + node.Info.team_MemberType.ToString();
                    break;
                case TrackedVariables.CategoryType.NETWORKING:
                    _label = "Networking";
                    break;
                case TrackedVariables.CategoryType.FUNDING:
                    _label = "Level: " + node.Info.funding_Level.ToString();
                    break;
                case TrackedVariables.CategoryType.CUSTOMER_DISCOVERY:
                    if (node.Info.feedback_Level == 0)
                        _label = "None";
                    if (node.Info.feedback_Level == 1)
                        _label = "Basic";
                    if (node.Info.feedback_Level == 2)
                        _label = "Advanced";
                    break;
                case TrackedVariables.CategoryType.DEVICE:
                    _label = "Part: " + node.Info.partType.ToString();
                    break;
                default:
                    _label = decisionLabel;
                    break;
            }

            string hitRate = "";
            if (displayAsPercentage)
            {
                if (displayPercentageSymbol)
                    hitRate = "% " + GetPercentageFromFloat(node.Info.funding_HitRate).ToString();
                else
                    hitRate = GetPercentageFromFloat(node.Info.funding_HitRate).ToString();
            }
            else
                hitRate = node.Info.funding_HitRate.ToString();

            string interest = "";
            if (displayAsPercentage)
            {
                if (displayPercentageSymbol)
                    interest = "% " + GetPercentageFromFloat(node.Info.funding_InterestPercentage).ToString();
                else
                    interest = GetPercentageFromFloat(node.Info.funding_InterestPercentage).ToString();
            }
            else
                interest = node.Info.funding_InterestPercentage.ToString();

            string equity = "";
            if (displayAsPercentage)
            {
                if (displayPercentageSymbol)
                    equity = "% " + GetPercentageFromFloat(node.Info.funding_EquityAmount).ToString();
                else
                    equity = GetPercentageFromFloat(node.Info.funding_EquityAmount).ToString();
            }
            else
                equity = node.Info.funding_EquityAmount.ToString();

            string timeUnitString = "";
            if (displayTimeUnit)
            {
                timeUnitString = node.Time_GetTimeByUnits(node.Info.time_CostAmount, timeUnits);
            }
            else
                timeUnitString = node.Info.time_CostAmount.ToString();

            ui.UI_UpdateText_All(node.Info.categoryType.ToString(), _label, "Budget", node.Info.budget_CostAmount.ToString(),
                    "Time", timeUnitString, effectType, effectAmount, hitRate, interest,
                    equity);

            infoUpdated = true;
        }

    }
}
