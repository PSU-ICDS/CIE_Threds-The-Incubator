/*
 * Name: MenuPanelNavigator
 * Project: XR Template Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 06/12/2024
 * Description: This script manages and navigates through a list of menu panel gameObjects
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Code created for XR Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_XR_Utility
{
    public class MenuPanelNavigator : MonoBehaviour
    {
        [SerializeField] bool closeAllOnStart;
        [SerializeField] bool looping;
        [Space(15)]
        [SerializeField] List<UI_MenuItem> menus;
        [Space(15)]
        [SerializeField] DebugSettings debug;
        int current;

        //[Serializable]
        //public class MenuItem
        //{
        //    public string name;
        //    public GameObject panel;
        //    [Space(15)]
        //    public UnityEvent startEvent;
        //    public UnityEvent endEvent;

        //    public void OpenMenu(bool _runEvent)
        //    {
        //        if (panel != null)
        //        {
        //            panel.SetActive(true);

        //            if (_runEvent)
        //                startEvent.Invoke();
        //        }
        //    }

        //    public void CloseMenu(bool _runEvent)
        //    {
        //        if (panel != null)
        //        {
        //            panel.SetActive(false);

        //            if (_runEvent)
        //                endEvent.Invoke();
        //        }
        //    }
        //}

        [Serializable]
        public class DebugSettings
        {
            public bool debug;
            public bool debug_Previous;
            public bool debug_Next;
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

        public void FindMenuItems()
        {
            UI_MenuItem[] _items = gameObject.transform.GetComponentsInChildren<UI_MenuItem>(true);

            for (int i = 0; i < _items.Length; i++)
            {
                if(_items[i] != null)
                {
                    if (!menus.Contains(_items[i]))
                        menus.Add(_items[i]);
                }
            }
        }

        void ResetCurrent()
        {
            current = 0;
        }

        public void Setup()
        {
            ResetCurrent();

            FindMenuItems();

            if (closeAllOnStart)
                CloseAllMenus();

            OpenCurrentIndexMenu();
        }

        public void CloseAllMenus()
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if (menus[i] != null)
                    menus[i].CloseMenu(false);
            }
        }

        public void CloseMenu(int _index)
        {
            if (_index >= 0 && _index < menus.Count)
            {
                if (menus[_index] != null)
                    menus[_index].CloseMenu(true);
            }
        }

        public void OpenCurrentIndexMenu()
        {
            if (current >= 0 && current < menus.Count)
            {
                if (menus[current] != null)
                    menus[current].OpenMenu(true);
            }
        }

        public void NextMenu()
        {
            int prevIndex = current;
            int newIndex = current + 1;

            if (newIndex < menus.Count)
            {
                current = newIndex;
            }
            else if (looping)
            {
                current = 0;
            }

            CloseMenu(prevIndex);
            OpenCurrentIndexMenu();
        }

        public void PreviousMenu()
        {
            int prevIndex = current;
            int newIndex = current - 1;

            if (newIndex > -1)
            {
                current = newIndex;
            }
            else if (looping)
            {
                current = menus.Count - 1;
            }

            CloseMenu(prevIndex);
            OpenCurrentIndexMenu();
        }

        public void OpenMenuByIndex(int _index)
        {
            if(_index > -1 && _index < menus.Count)
            {
                if (menus[_index] != null)
                {
                    CloseMenu(current);
                    current = _index;
                    OpenCurrentIndexMenu();
                }
            }
        }

        public void OpenMenuByID(int _id)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if(menus[i] != null)
                {
                    if(menus[i].menuID == _id)
                    {
                        OpenMenuByIndex(i);
                        break;
                    }
                }
            }
        }

        public void OpenMenuByName(string _menuName)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if (menus[i] != null)
                {
                    if (menus[i].menuName.ToLower() == _menuName.ToLower())
                    {
                        OpenMenuByIndex(i);
                        break;
                    }
                }
            }
        }

        void CheckDebugSettings()
        {
            if (debug.debug)
            {
                if (debug.debug_Previous)
                {
                    PreviousMenu();
                    debug.debug_Previous = false;
                }

                if (debug.debug_Next)
                {
                    NextMenu();
                    debug.debug_Next = false;
                }
            }
        }


        // Update is called once per frame
        void Update()
        {
            CheckDebugSettings();
        }
    }
}