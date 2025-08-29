using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceSelectionManager : MonoBehaviour
{
    [SerializeField] float budget;
    [SerializeField] int unlockLevel;
    [SerializeField] float costTotal;
    [SerializeField] bool hasFinalized = false;
    [SerializeField] TrackedVariables trackedVariables;

    [Space(10)]
    [SerializeField] List<Part> parts_Type1;
    [SerializeField] List<Part> parts_Type2;
    [SerializeField] List<Part> parts_Type3;

    [Space(10)]
    [SerializeField] List<Part> selectedParts;

    [Space(10)]
    [SerializeField] Button finalizeButton;
    [SerializeField] GameObject blocker;


    [Serializable]
    public class Part
    {
        public TrackedVariables.PartTypes type;
        public int partLevel;
        public float cost;
        //public int unlockLevel;
        public bool hasBeenPurchased;
        public DecisionNode node;
        public Button selectionButton;


        public void Setup()
        {
            if (node != null)
            {
                cost = node.Info.budget_CostAmount;
                partLevel = node.Info.partLevel;

                hasBeenPurchased = node.HasBeenSelected;
                selectionButton = node.SelectionButton;
                if (selectionButton != null)
                    selectionButton.interactable = node.Availability;

                //unlockLevel = node.Info.partLevel;
            }
        }

        public void SetSelectedColor(bool _selected)
        {
            if (node != null)
            {
                if (node.gameObject.TryGetComponent<ToggleImageColor>(out ToggleImageColor _toggle))
                {
                    if (_selected)
                        _toggle.Color_Set02();
                    else
                        _toggle.Color_Set01();
                }
            }
        }

        public void FinalizeSelectedPart()
        {
            if (node != null)
            {
                node.DecisionSelected();
                node.Button_CheckBlockingVars();
            }
        }
    }

    public void FindTrackedVars()
    {
        if (trackedVariables == null)
        {
            if (GameObject.FindObjectOfType<TrackedVariables>() != null)
                trackedVariables = GameObject.FindObjectOfType<TrackedVariables>();
        }
    }

    public void UpdateTrackedVars()
    {
        if (trackedVariables == null)
            FindTrackedVars();

        if (trackedVariables != null)
        {
            budget = trackedVariables.PrimaryStat.budget;
            unlockLevel = trackedVariables.PrimaryStat.partsUnlockLevel;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void OnEnable()
    {
        Setup();
    }


    public void Setup()
    {
        UpdateUI();
        //FindTrackedVars();
        //UpdateTrackedVars();
        //Parts_SetupAll();
    }

    public void Parts_SetupAll()
    {
        for (int i = 0; i < parts_Type1.Count; i++)
        {
            if (parts_Type1[i] != null)
                parts_Type1[i].Setup();
        }

        for (int j = 0; j < parts_Type2.Count; j++)
        {
            if (parts_Type2[j] != null)
                parts_Type2[j].Setup();
        }

        for (int k = 0; k < parts_Type3.Count; k++)
        {
            if (parts_Type3[k] != null)
                parts_Type3[k].Setup();
        }
    }

    public void Part01_Select(int _partLevel)
    {
        if (unlockLevel >= _partLevel)
        {
            for (int i = 0; i < parts_Type1.Count; i++)
            {
                if (parts_Type1[i] != null)
                {
                    if (parts_Type1[i].partLevel == _partLevel)
                    {
                        if (selectedParts.Contains(parts_Type1[i]))
                            Selected_RemovePartByType(TrackedVariables.PartTypes.PART_01);
                        else
                        {
                            Selected_RemovePartByType(TrackedVariables.PartTypes.PART_01);
                            selectedParts.Add(parts_Type1[i]);
                            break;
                        }
                    }
                }
            }
        }

        UpdateUI();
        //Selected_FindCostTotal();
    }

    public void Part02_Select(int _partLevel)
    {
        if (unlockLevel >= _partLevel)
        {
            for (int i = 0; i < parts_Type2.Count; i++)
            {
                if (parts_Type2[i] != null)
                {
                    if (parts_Type2[i].partLevel == _partLevel)
                    {
                        if (selectedParts.Contains(parts_Type2[i]))
                            Selected_RemovePartByType(TrackedVariables.PartTypes.PART_02);
                        else
                        {
                            Selected_RemovePartByType(TrackedVariables.PartTypes.PART_02);
                            selectedParts.Add(parts_Type2[i]);
                            break;
                        }
                    }
                }
            }
        }

        UpdateUI();
        //Selected_FindCostTotal();
    }

    public void Part03_Select(int _partLevel)
    {
        if (unlockLevel >= _partLevel)
        {
            for (int i = 0; i < parts_Type3.Count; i++)
            {
                if (parts_Type3[i] != null)
                {
                    if (parts_Type3[i].partLevel == _partLevel)
                    {
                        if (selectedParts.Contains(parts_Type3[i]))
                            Selected_RemovePartByType(TrackedVariables.PartTypes.PART_03);
                        else
                        {
                            Selected_RemovePartByType(TrackedVariables.PartTypes.PART_03);
                            selectedParts.Add(parts_Type3[i]);
                            break;
                        }
                    }
                }
            }
        }

        UpdateUI();
        //Selected_FindCostTotal();
    }

    public void Selected_RemovePartByType(TrackedVariables.PartTypes _type)
    {
        for (int i = 0; i < selectedParts.Count; i++)
        {
            if (selectedParts[i] != null)
            {
                if (selectedParts[i].type == _type)
                {
                    selectedParts[i].SetSelectedColor(false);
                    selectedParts.RemoveAt(i);
                    break;
                }
            }
        }

        selectedParts.TrimExcess();
    }

    public void Selected_FindCostTotal()
    {
        costTotal = 0.0f;

        for (int i = 0; i < selectedParts.Count; i++)
        {
            if (selectedParts[i] != null)
            {
                if (!selectedParts[i].hasBeenPurchased)
                {
                    costTotal += selectedParts[i].cost;
                }
            }
        }

        Button_SetFinalizeInteractableState((costTotal <= budget));

        //if (costTotal > budget) { }
    }

    public void Selected_ToggleSelectedColors()
    {
        for (int i = 0; i < parts_Type1.Count; i++)
        {
            if (parts_Type1[i] != null)
                parts_Type1[i].SetSelectedColor(selectedParts.Contains(parts_Type1[i]));
        }

        for (int j = 0; j < parts_Type2.Count; j++)
        {
            if (parts_Type2[j] != null)
                parts_Type2[j].SetSelectedColor(selectedParts.Contains(parts_Type2[j]));
        }

        for (int k = 0; k < parts_Type3.Count; k++)
        {
            if (parts_Type3[k] != null)
                parts_Type3[k].SetSelectedColor(selectedParts.Contains(parts_Type3[k]));
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void UpdateUI()
    {
        FindTrackedVars();
        UpdateTrackedVars();
        Parts_SetupAll();
        Selected_FindCostTotal();
        Selected_ToggleSelectedColors();
        Button_FinalizeButtonActiveState();
        Blocker_SetActiveState(hasFinalized);
    }

    public void Button_FinalizeButtonActiveState()
    {
        if (finalizeButton != null)
        {
            if(selectedParts.Count == 3)
                finalizeButton.gameObject.SetActive(true);
            else
                finalizeButton.gameObject.SetActive(false);
                //finalizeButton.gameObject.SetActive(selectedParts.Count == 3);
        }
    }

    public void Button_SetFinalizeInteractableState(bool _available)
    {
        if (finalizeButton != null)
        {
            if (hasFinalized)
                finalizeButton.interactable = false;
            else
                finalizeButton.interactable = _available;

            //if (finalizeButton.gameObject.TryGetComponent<ToggleImageColor>(out ToggleImageColor _toggle))
            //{
            //    if (_available)
            //        _toggle.Color_Set01();
            //    else
            //        _toggle.Color_Set02();
            //}
        }
    }

    public void Blocker_SetActiveState(bool _active)
    {
        if (blocker != null)
            blocker.SetActive(_active);
    }

    public void FinalizeSelections()
    {
        for (int i = 0; i < selectedParts.Count; i++)
        {
            if (selectedParts[i] != null)
                selectedParts[i].FinalizeSelectedPart();
        }

        Button_SetFinalizeInteractableState(false);
        hasFinalized = true;
        Blocker_SetActiveState(true);
    }

}
