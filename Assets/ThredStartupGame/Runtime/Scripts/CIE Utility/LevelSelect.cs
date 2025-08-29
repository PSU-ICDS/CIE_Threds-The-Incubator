/*
 * Name: LevelSelect
 * Project: XR Template Project
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 02/13/2023
 * Description: This script manages level select functionality for loading differant levels
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Code created for XR Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_XR_Utility
{

    /// <summary>
    /// Manages level select functionality for loading differant levels
    /// </summary>
    public class LevelSelect : MonoBehaviour
    {
        /// <summary>
        /// Determines which version of load scene will be used by DynamicLoadScene()
        /// </summary>
        public enum state { BUILD, URL, NEWWINDOW };

        /// <summary>
        /// Holds the load level state type
        /// </summary>
        [Tooltip("Holds the load level state type for this script")]
        //[HideInInspector]
        public state loadState;

        //[SerializeField] SceneGroups sceneGroups;

        /// <summary>
        /// Holds the set of external links to load using the LoadExternalLink() func
        /// </summary>
        [Tooltip("Holds the set of external links to load using the LoadExternalLink() func")]
        [SerializeField]
        string[] externalLinks = null;

        /// <summary>
        /// Get/Set the load level state type
        /// </summary>
        public state LoadState
        {
            get { return loadState; }
            set { loadState = value; }
        }

        /// <summary>
        /// Get/Set the set of external links to load using the LoadExternalLink() func
        /// </summary>
        public string[] ExternalURLLinks
        {
            get { return externalLinks; }
            set { externalLinks = value; }
        }

        /// <summary>
        /// Sets the load scene state
        /// </summary>
        /// <param name="_state">The state to set the loadState to</param>
        public void SetLoadState(string _state)
        {
            if (_state == "BUILD")
                loadState = state.BUILD;
            if (_state == "URL")
                loadState = state.URL;
            if (_state == "NEWWINDOW")
                loadState = state.NEWWINDOW;
        }

        /// <summary>
        /// Loads the scene by build index based on "_index"
        /// </summary>
        /// <param name="_index">Build scene index of the scene to be loaded</param>
        public void LoadSceneNumber(int _index)
        {
            if (_index > -1 && _index < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(_index);
        }

        /// <summary>
        /// Loads a URL from externalLinks at index # "_index"
        /// </summary>
        /// <param name="_index">Index of the URL to load from externalLinks[]</param>
        public void LoadExternalLink(int _index)
        {
            if (_index < externalLinks.Length)
                Application.OpenURL(externalLinks[_index]);
        }

        ///// <summary>
        ///// Opens a new window and loads a URL from externalLinks at index # "_index"
        ///// </summary>
        ///// <param name="_index">Index of the URL to load from externalLinks[]</param>
        //public void LoadNewWindowLink(int _index)
        //{
        //    if (_index < externalLinks.Length)
        //    {
        //        string link = "window.open(" + '\"' + externalLinks[_index] + '"' + ",\"_blank\")";
        //        Application.ExternalEval(link);
        //    }

        //}

        /// <summary>
        /// Dynamicly loads new scenes determined by the loadState. Buildindex = BUILD, External URL = URL, New Window = NEWWINDOW
        /// </summary>
        /// <param name="_index">Index of the new scene to load</param>
        public void DynamicLoadScene(int _index)
        {
            if (loadState == state.BUILD)
                LoadSceneNumber(_index);
            if (loadState == state.URL)
                LoadExternalLink(_index);
            //if (loadState == state.NEWWINDOW)
                //LoadNewWindowLink(_index);
        }

        /// <summary>
        /// Loads the next scene in the build.
        /// Loads the first scene in the build if the current scene is the last scene in the build
        /// </summary>
        public void LoadNextBuildScene()
        {
            int current = SceneManager.GetActiveScene().buildIndex + 1;
            if (current >= SceneManager.sceneCountInBuildSettings)
                current = 0;

            LoadSceneNumber(current);
        }

        /// <summary>
        /// Loads the previous scene in the build.
        /// </summary>
        public void LoadPreviousBuildScene()
        {
            int current = SceneManager.GetActiveScene().buildIndex - 1;
            if (current < 0)
                current = 0;

            LoadSceneNumber(current);
        }

        public void LoadSceneByName(string _name)
        {
            if (SceneManager.GetSceneByName(_name).IsValid())
            {
                if (SceneManager.GetSceneByName(_name) != SceneManager.GetActiveScene())
                    SceneManager.LoadScene(_name);
            }

        }

        //public void LoadNextSceneFromGroup()
        //{
        //    if(sceneGroups != null)
        //    {
        //        Scene next = sceneGroups.GetNextScene(SceneManager.GetActiveScene());
        //        if (next.IsValid() && next != SceneManager.GetActiveScene())
        //            SceneManager.LoadScene(next.name);
        //    }
        //}

        //public void LoadFirstInGroup(string _groupName)
        //{
        //    if(sceneGroups != null)
        //    {
        //        Scene next = sceneGroups.GetFirstInGroup(_groupName);
        //        if (next.IsValid() && next != SceneManager.GetActiveScene())
        //            SceneManager.LoadScene(next.name);
        //    }
        //}

        //public void LoadGroupLobby()
        //{
        //    if(sceneGroups != null)
        //    {
        //        if (sceneGroups.Lobby.IsValid() && sceneGroups.Lobby != SceneManager.GetActiveScene())
        //            SceneManager.LoadScene(sceneGroups.Lobby.name);
        //    }
        //}

        /// <summary>
        /// Quits the game
        /// </summary>
        public void QuitProgram()
        {
            Application.Quit();
        }

    }
}