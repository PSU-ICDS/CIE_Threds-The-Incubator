/*
 * Name: ManageSetPlayerPref
 * Project: Threds Startup Game Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 09/13/2024
 * Description: This script tracks and sets PlayerPrefs variables
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Code created for Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_Utility
{
    public class ManageSetPlayerPref : MonoBehaviour
    {
        public enum PrefVarTypes { NONE, FLOAT, INT, STRING }

        [SerializeField] bool checkOnAwake;
        [SerializeField] bool checkOnStart;
        [SerializeField] bool checkOnUpdate;


        [Space(10)]
        [SerializeField] List<PrefVar> prefs;

        [Serializable]
        public class PrefVar
        {
            public string name;
            public bool active;

            [Space(15)]
            public PrefVarTypes type;
            public string prefKey;
            public bool alreadyExists;

            [Space(15)]
            public float val_Float;
            public int val_Int;
            public string val_String;

            [Space(15)]
            public bool debugPref;
            public bool debugClearPref;

            public void CheckForPlayerPrefsKey()
            {
                alreadyExists = PlayerPrefs.HasKey(prefKey);
                if (alreadyExists)
                {
                    switch (type)
                    {
                        case PrefVarTypes.NONE:
                            break;
                        case PrefVarTypes.FLOAT:
                            val_Float = PlayerPrefs.GetFloat(prefKey);
                            break;
                        case PrefVarTypes.INT:
                            val_Int = PlayerPrefs.GetInt(prefKey);
                            break;
                        case PrefVarTypes.STRING:
                            val_String = PlayerPrefs.GetString(prefKey);
                            break;
                        default:
                            break;
                    }
                }
            }

            public void SaveToPlayerPrefs()
            {
                if (active)
                {
                    switch (type)
                    {
                        case PrefVarTypes.NONE:
                            break;
                        case PrefVarTypes.FLOAT:
                            PlayerPrefs.SetFloat(prefKey, val_Float);
                            alreadyExists = true;
                            break;
                        case PrefVarTypes.INT:
                            PlayerPrefs.SetInt(prefKey, val_Int);
                            alreadyExists = true;
                            break;
                        case PrefVarTypes.STRING:
                            PlayerPrefs.SetString(prefKey, val_String);
                            alreadyExists = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            public void ClearPref()
            {
                PlayerPrefs.DeleteKey(prefKey);
                val_Float = 0.0f;
                val_Int = 0;
                val_String = null;
                alreadyExists = false;
            }

            public void Debug_RunDebugFunctions()
            {
                Debug_PrintInfo();
                Debug_ClearPref();
            }

            public void Debug_PrintInfo()
            {
                if (debugPref)
                    Debug.Log("PrefDebug-> Name: " + name + ", Active: " + active + ", Type: " + type.ToString() +
                        ", Key: " + prefKey + ", AlreadyExists: " + alreadyExists + ", FloatVal: " + val_Float.ToString() +
                        ", IntVal: " + val_Int.ToString() + ", StringVal: " + val_String);
            }

            public void Debug_ClearPref()
            {
                if(debugClearPref)
                {
                    ClearPref();
                    Debug.Log("PrefDebug-> Clear Pref Named: " + name);
                    debugClearPref = false;
                }
            }

        }


        public void Setup()
        {
            Prefs_CheckForExisting_All();
        }

        private void Awake()
        {
            if (checkOnAwake)
                Setup();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (checkOnStart)
                Setup();
        }

        // Update is called once per frame
        void Update()
        {
            if (checkOnUpdate)
                Prefs_CheckForExisting_All();
        }

        public void Prefs_CheckForExisting_All()
        {
            for (int i = 0; i < prefs.Count; i++)
            {
                if (prefs[i] != null)
                {
                    prefs[i].CheckForPlayerPrefsKey();
                    prefs[i].Debug_RunDebugFunctions();
                }
            }
        }

        public void Prefs_UpdateByKey(string _key)
        {
            for (int i = 0; i < prefs.Count; i++)
            {
                if (prefs[i] != null)
                {
                    if (prefs[i].prefKey.ToLower() == _key.ToLower())
                        prefs[i].CheckForPlayerPrefsKey();
                }
            }
        }

        public void Prefs_SetValByKey_Float(string _key, float _val)
        {
            PlayerPrefs.SetFloat(_key, _val);
            Prefs_UpdateByKey(_key);
        }

        public void Prefs_SetValByKey_Int(string _key, int _val)
        {
            PlayerPrefs.SetInt(_key, _val);
            Prefs_UpdateByKey(_key);
        }

        public void Prefs_SetValByKey_String(string _key, string _val)
        {
            PlayerPrefs.SetString(_key, _val);
            Prefs_UpdateByKey(_key);
        }

        public float Prefs_GetValByKey_Float(string _key)
        {
            if (PlayerPrefs.HasKey(_key))
                return PlayerPrefs.GetFloat(_key);

            return 0.0f;
        }

        public int Prefs_GetValByKey_Int(string _key)
        {
            if (PlayerPrefs.HasKey(_key))
                return PlayerPrefs.GetInt(_key);

            return 0;
        }

        public string Prefs_GetValByKey_String(string _key)
        {
            if (PlayerPrefs.HasKey(_key))
                return PlayerPrefs.GetString(_key);

            return "";
        }

        public void Prefs_ClearSavedPrefs_All()
        {
            for (int i = 0; i < prefs.Count; i++)
            {
                if (prefs[i] != null)
                {
                    prefs[i].ClearPref();
                }
            }
        }

        public void Prefs_ClearSavedPrefs_ByKey(string _key)
        {
            for (int i = 0; i < prefs.Count; i++)
            {
                if (prefs[i] != null)
                {
                    if (prefs[i].prefKey.ToLower() == _key.ToLower())
                        prefs[i].ClearPref();
                }
            }
        }

    }
}