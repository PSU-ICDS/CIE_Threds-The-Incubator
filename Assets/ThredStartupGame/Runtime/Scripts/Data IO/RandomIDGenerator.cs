/*
 * Name: RandomIDGenerator
 * Project: Threds Startup Game Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 09/13/2024
 * Description: This script generates randomized IDs and can save/pull them to/from PlayerPrefs
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Code created for Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_Utility
{
    public class RandomIDGenerator : MonoBehaviour
    {
        /// <summary>
        /// Public enum containing the options for ID types
        /// NONE = no ID will be generated
        /// LETTERS = Random ID with only letters will be generated
        /// NUMBERS = Random ID with only numbers will be generated
        /// BOTH = Random ID with letters and numbers will be generated (all digits are randomized, so a BOTH ID may result in an all letters or all numbers output)
        /// </summary>
        public enum ID_Type { NONE, LETTERS, NUMBERS, BOTH }
        /// <summary>
        /// Determines the type of randomized ID that will be generated
        /// NONE = no ID will be generated
        /// LETTERS = Random ID with only letters will be generated
        /// NUMBERS = Random ID with only numbers will be generated
        /// BOTH = Random ID with letters and numbers will be generated (all digits are randomized, so a BOTH ID may result in an all letters or all numbers output)
        /// </summary>
        [Tooltip("Determines the type of randomized ID that will be generated \n" +
            "NONE = no ID will be generated \n" +
            "LETTERS = Random ID with only letters will be generated \n" +
            "NUMBERS = Random ID with only numbers will be generated \n" +
            "BOTH = Random ID with letters and numbers will be generated (all digits are randomized, so a BOTH ID may result in an all letters or all numbers output).")]
        [SerializeField] ID_Type idType;
        /// <summary>
        /// Determines the number of digits included in the randomly generated ID 
        /// </summary>
        [Range(0, 3000)]
        [Tooltip("Determines the number of digits included in the randomly generated ID")]
        [SerializeField] int length;
        /// <summary>
        /// Holds the randomly generated ID
        /// </summary>
        [Tooltip("Holds the randomly generated ID")]
        [SerializeField] string iD;

        [Space(15)]
        /// <summary>
        /// Determines if the ID gets generated on Awake/Start (TRUE) or only generates when called (FALSE)
        /// </summary>
        [Tooltip("Determines if the ID gets generated on Awake/Start (TRUE) or only generates when called (FALSE)")]
        [SerializeField] bool generateOnStart;
        /// <summary>
        /// Determines if a new ID is generated every time the game is started (TRUE) or saved IDs can be reused (FALSE)
        /// </summary>
        [Tooltip("Determines if a new ID is generated every time the game is started (TRUE) or saved IDs can be reused (FALSE)")]
        [SerializeField] bool alwaysGenerateNew;
        /// <summary>
        /// Determines if the ID uses PlayerPrefs to save, store, and retrieve generated IDs (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if the ID uses PlayerPrefs to save, store, and retrieve generated IDs (TRUE) or not (FALSE)")]
        [SerializeField] bool usePlayerPrefs;
        [Space(15)]
        /// <summary>
        /// TextMeshPro Text object used to display the ID
        /// </summary>
        [Tooltip("TextMeshPro Text object used to display the ID")]
        [SerializeField] TMP_Text idDisplayText;
        /// <summary>
        /// TextMeshPro InputField Text object used to display the ID (and make it copyable by the user)
        /// </summary>
        [Tooltip("TextMeshPro InputField Text object used to display the ID (and make it copyable by the user)")]
        [SerializeField]
        TMP_InputField idDisplayInputFieldText;
        [SerializeField]
        UI_OverrideInputField inputFieldOverride;

        [Space(15)]
        /// <summary>
        /// Debugging bool used to generate new randomized IDs when set to TRUE in the inspector
        /// </summary>
        [Tooltip("Debugging bool used to generate new randomized IDs when set to TRUE in the inspector")]
        [SerializeField] bool debug_generateID;
        [SerializeField] bool debug_clearPrefsID;
        [SerializeField] bool debug_PrintInfo;

        /// <summary>
        /// Determines the type of randomized ID that will be generated
        /// NONE = no ID will be generated
        /// LETTERS = Random ID with only letters will be generated
        /// NUMBERS = Random ID with only numbers will be generated
        /// BOTH = Random ID with letters and numbers will be generated (all digits are randomized, so a BOTH ID may result in an all letters or all numbers output)
        /// </summary>
        public ID_Type Type { get => idType; set { idType = value; } }
        /// <summary>
        /// Determines the number of digits included in the randomly generated ID 
        /// </summary>
        public int Length { get => length; set { length = value; } }
        /// <summary>
        /// Holds the randomly generated ID
        /// </summary>
        public string ID { get => iD; set { iD = value; } }
        /// <summary>
        /// Determines if the ID uses PlayerPrefs to save, store, and retrieve generated IDs (TRUE) or not (FALSE)
        /// </summary>
        public bool UsePlayerPrefs { get => usePlayerPrefs; set { usePlayerPrefs = value; } }

        /// <summary>
        /// Awake is called when the script instance is being loaded
        /// </summary>
        private void Awake()
        {
            Setup();
        }

        ///// <summary>
        ///// Start is called before the first frame update
        ///// </summary>
        //void Start()
        //{
        //    Setup();
        //}

        private void Update()
        {
            Debug_CheckSettings();
        }

        /// <summary>
        /// Sets up the ID by pulling from PlayerPrefs or generating a new ID
        /// </summary>
        public void Setup()
        {
            if (alwaysGenerateNew)
                ID_ClearPlayerPrefs();

            if (generateOnStart)
                ID_GetSavedOrGenerateNewID();
            else
            {
                if (usePlayerPrefs && ID_CheckPlayerPrefs())
                {
                    ID_GetStoredPlayerPrefs();
                    ID_PrintToDisplayText();
                }
            }
        }

        /// <summary>
        /// Pulls ID stored in PlayerPrefs or generates a new ID
        /// </summary>
        public void ID_GetSavedOrGenerateNewID()
        {
            if (usePlayerPrefs)
            {
                if (ID_CheckPlayerPrefs())
                    ID_GetStoredPlayerPrefs();
                else
                    GenerateID();
            }
            else
                GenerateID();

            ID_PrintToDisplayText();
        }

        /// <summary>
        /// Generates a new randomized ID based on the 'IdType' and 'length' variables
        /// </summary>
        public void ID_GenerateNewID()
        {
            GenerateID();
            ID_PrintToDisplayText();
        }

        /// <summary>
        /// Generates a randomized ID based on the 'IdType' and 'length' variables
        /// </summary>
        public void GenerateID()
        {
            switch (idType)
            {
                case ID_Type.NONE:
                    break;
                case ID_Type.LETTERS:
                    GenerateID_Letters();
                    break;
                case ID_Type.NUMBERS:
                    GenerateID_Numbers();
                    break;
                case ID_Type.BOTH:
                    GenerateID_Both();
                    break;
                default:
                    break;
            }

            ID_SaveToPlayerPrefs();
        }

        /// <summary>
        /// Generates a randomized ID containing only letters
        /// </summary>
        void GenerateID_Letters()
        {
            string rndID = "";
            int rndVal = 0;

            //  Unicode Alphabet range = 65 (A) to 90 (Z)
            for (int i = 0; i < length; i++)
            {
                rndVal = UnityEngine.Random.Range(65, 90);
                rndID = rndID + (char)rndVal;
            }

            iD = rndID;
        }

        /// <summary>
        /// Generates a randomized ID containing only numbers
        /// </summary>
        void GenerateID_Numbers()
        {
            string rndID = "";
            int rndVal = 0;

            //  Unicode Alphabet range = 65 (A) to 90 (Z)
            for (int i = 0; i < length; i++)
            {
                rndVal = UnityEngine.Random.Range(0, 9);
                rndID = rndID + rndVal.ToString();
            }

            iD = rndID;
        }

        /// <summary>
        /// Generates a randomized ID which can contain letters and/or numbers
        /// </summary>
        void GenerateID_Both()
        {
            string rndID = "";
            int rndVal = 0;
            int rndToggleVal = 0;

            //  Unicode Alphabet range = 65 (A) to 90 (Z)
            for (int i = 0; i < length; i++)
            {
                rndToggleVal = UnityEngine.Random.Range(0, 100);

                if (rndToggleVal % 2 == 0)
                {
                    //  Adds a Letter
                    rndVal = UnityEngine.Random.Range(65, 90);
                    rndID = rndID + (char)rndVal;
                }
                else
                {
                    //  Adds a Number
                    rndVal = UnityEngine.Random.Range(0, 9);
                    rndID = rndID + rndVal.ToString();
                }
            }

            iD = rndID;
        }

        /// <summary>
        /// Checks PlayerPrefs for a saved ID
        /// </summary>
        /// <returns>Returns TRUE if a saved ID is found in PlayerPrefs.
        ///          Returns FALSE if no saved ID is found.</returns>
        public bool ID_CheckPlayerPrefs()
        {
            return PlayerPrefs.HasKey("ID");
        }

        /// <summary>
        /// Pulls the saved ID from PlayerPrefs
        /// </summary>
        public void ID_GetStoredPlayerPrefs()
        {
            if (usePlayerPrefs)
                iD = PlayerPrefs.GetString("ID");
        }

        /// <summary>
        /// Saves the ID to PlayerPrefs
        /// </summary>
        public void ID_SaveToPlayerPrefs()
        {
            if (usePlayerPrefs)
            {
                PlayerPrefs.SetString("ID", iD);

                if (debug_PrintInfo)
                    Debug.Log("ID Generated and saved to player prefs. Saved Check: " + ID_CheckPlayerPrefs() + ", Saved ID: " + PlayerPrefs.GetString("ID"));
            }
        }

        /// <summary>
        /// Deletes the ID saved in PlayerPrefs
        /// </summary>
        public void ID_ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey("ID");
        }

        /// <summary>
        /// Prints the ID to the TextMeshPro Text object (if not null)
        /// </summary>
        public void ID_PrintToDisplayText()
        {
            if (idDisplayText != null)
                idDisplayText.text = iD;

            if (idDisplayInputFieldText != null)
                idDisplayInputFieldText.text = iD;

            if (inputFieldOverride != null)
            {
                inputFieldOverride.SetDisplayText(iD);
            }
        }

        public void Debug_CheckSettings()
        {
            if (debug_generateID)
            {
                ID_GenerateNewID();
                debug_generateID = false;
            }

            if (debug_clearPrefsID)
            {
                ID_ClearPlayerPrefs();
                debug_clearPrefsID = false;
            }
        }
    }
}