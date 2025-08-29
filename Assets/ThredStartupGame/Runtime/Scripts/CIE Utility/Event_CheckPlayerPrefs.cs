/*
 * Name: Event_CheckPlayerPrefs
 * Project: Threds Startup Game Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 09/13/2024
 * Description: This script checks PlayerPrefs for existing variables, then runs events if found or not.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Code created for Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_Utility
{

    public class Event_CheckPlayerPrefs : MonoBehaviour
    {
        public enum PrefVarTypes { NONE, FLOAT, INT, STRING }

        [SerializeField] PrefVarTypes type;
        [SerializeField] string prefKey;
        [SerializeField] bool runOnStart;
        [SerializeField] bool runOnAwake;
        [SerializeField] bool runOnUpdate;
        [SerializeField] private bool found = false;

        [Space(15)]
        [SerializeField] private float foundFloat = 0.0f;
        [SerializeField] private int foundInt = 0;
        [SerializeField] private string foundString = "";

        [Space(15)]
        [SerializeField] bool eventsActive = true;
        [SerializeField] UnityEvent keyFound;
        [SerializeField] UnityEvent keyNotFound;

        private void Awake()
        {
            if (runOnAwake)
                CheckForPlayerPrefsKey();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (runOnStart)
                CheckForPlayerPrefsKey();
        }

        // Update is called once per frame
        void Update()
        {
            if (runOnUpdate)
                CheckForPlayerPrefsKey();
        }

        public void Prefs_SetValByKey_Float(float _val)
        {
            PlayerPrefs.SetFloat(prefKey, _val);
            CheckForPlayerPrefsKey();
        }

        public void Prefs_SetValByKey_Int(int _val)
        {
            PlayerPrefs.SetInt(prefKey, _val);
            CheckForPlayerPrefsKey();
        }

        public void Prefs_SetValByKey_String(string _val)
        {
            PlayerPrefs.SetString(prefKey, _val);
            CheckForPlayerPrefsKey();
        }

        public void Prefs_DeleteSavedPref()
        {
                PlayerPrefs.DeleteKey(prefKey);            
        }

        public void CheckForPlayerPrefsKey()
        {
            found = PlayerPrefs.HasKey(prefKey);
            if (found)
            {
                switch (type)
                {
                    case PrefVarTypes.NONE:
                        break;
                    case PrefVarTypes.FLOAT:
                        foundFloat = PlayerPrefs.GetFloat(prefKey);
                        break;
                    case PrefVarTypes.INT:
                        foundInt = PlayerPrefs.GetInt(prefKey);
                        break;
                    case PrefVarTypes.STRING:
                        foundString = PlayerPrefs.GetString(prefKey);
                        break;
                    default:
                        break;
                }
            }

            if (eventsActive)
                RunKeyEvents(found);
        }

        public void RunKeyEvents(bool _found)
        {
            if (_found)
                keyFound.Invoke();
            else
                keyNotFound.Invoke();
        }

    }
}