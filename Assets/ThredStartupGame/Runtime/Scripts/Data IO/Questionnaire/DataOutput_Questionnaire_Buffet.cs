/*
 * Name: DataOutput_Questionnaire
 * Project: Nutrition - Buffet Project (v2)
 * Author(s): Bart Masters - btm5024@psu.edu
 * Date: 10/25/2023
 * Description: This script manages sets of questionnaire responses to collect and output to a data file
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// Code created for XR Utility functionality used in projects for the Center for Immersive Experiences (CIE) at Penn State University (PSU).
/// </summary>
namespace CIE_XR_Utility
{
    public class DataOutput_Questionnaire_Buffet : MonoBehaviour
    {
        [SerializeField] int userID;
        /// <summary>
        /// List of Response Sets each containing info and responses for a specific questionnaire
        /// </summary>
        [Tooltip("List of Response Sets each containing info and responses for a specific questionnaire")]
        [SerializeField] List<ResponseSet> sets;
        [Space(10)]
        /// <summary>
        /// Output settings for the gathered dataset
        /// </summary>
        [Tooltip("Output settings for the gathered dataset")]
        [SerializeField] DataOutputSettings dataOutput;
        [Space(10)]
        /// <summary>
        /// Debugging settings for use in debug testing inside the editor
        /// </summary>
        [Tooltip("Debugging settings for use in debug testing inside the editor")]
        [SerializeField] DebugSettings debugSettings;

        public int UserID { get=>userID; set {userID = value; } }
        //public bool ManagerOverride { get { return managerOverride; } set { managerOverride = value; } }

        //public enum SortTypes { ID, NAME, RESPONSES }
        //[SerializeField] SortTypes sortType;



        /// <summary>
        /// Container class containing info for a questionnaire and its responses
        /// </summary>
        [Serializable]
        public class ResponseSet
        {
            /// <summary>
            /// Name of this questionnaire
            /// </summary>
            [Tooltip("Name of this questionnaire")]
            public string name;
            /// <summary>
            /// ID number used to identify this questionnaire
            /// </summary>
            [Tooltip("ID number used to identify this questionnaire")]
            public int id;
            /// <summary>
            /// Total number of responses expected for this questionnaire
            /// </summary>
            [Tooltip("Total number of responses expected for this questionnaire")]
            public int totalResponses;
            /// <summary>
            /// List of saved responses for this questionnaire
            /// </summary>
            [Tooltip("List of saved responses for this questionnaire")]
            public List<string> responses;

            /// <summary>
            /// Sets up this questionnaire
            /// </summary>
            public void Setup()
            {
                responses = new List<string>();

                for (int i = 0; i < totalResponses; i++)
                {
                    responses.Add("Null Response");
                }
            }

            /// <summary>
            /// Updates the saved response info for a given response
            /// </summary>
            /// <param name="_index">Index of the response to update in the list or saved responses</param>
            /// <param name="_response">Content of the response to update</param>
            public void Response_UpdateInfo(int _index, string _response)
            {
                if (_index >= 0 && _index < responses.Count)
                {
                    if (responses[_index] != null)
                        responses[_index] = _response;
                }
            }

            /// <summary>
            /// Compiles and returns a string containing all of the responses in comma seperated format
            /// </summary>
            /// <param name="_includeSetName">Determines if the name of the questionnaire gets added to the beginning of the string (TRUE) or not (FALSE) </param>
            /// <returns></returns>
            public string GetCompleteOutput(bool _includeSetName)
            {
                string output = "";
                if (_includeSetName)
                    output = name + ",";

                if (responses != null)
                {
                    for (int i = 0; i < responses.Count; i++)
                    {
                        if (responses[i] != null)
                        {
                            output = output + responses[i] + ",";
                        }
                    }
                }

                return output;
            }
        }


        /// <summary>
        /// Container class which holds settings for the exported dataset and file
        /// </summary>
        [Serializable]
        public class DataOutputSettings
        {
            [SerializeField] int userID;
            /// <summary>
            /// Name of the file to be created and exported
            /// </summary>
            [Tooltip("Name of the file to be created and exported")]
            public string filename;
            [TextArea(5, 20)]
            /// <summary>
            /// Filepath to the location where the exported data file will be created/located
            /// </summary>
            [Tooltip("Filepath to the location where the exported data file will be created/located")]
            public string filePath;
            [TextArea(5, 20)]
            /// <summary>
            /// Info about the current scene of the program, duration of time since the program started, duration spent in current scene, etc
            /// </summary>
            [Tooltip("Info about the current scene of the program, duration of time since the program started, duration spent in current scene, etc")]
            public string sceneData;
            [TextArea(5, 20)]
            /// <summary>
            /// Info about the current scene of the program, duration of time since the program started, duration spent in current scene, etc. Added at the end of an experience
            /// </summary>
            [Tooltip("Info about the current scene of the program, duration of time since the program started, duration spent in current scene, etc. Added at the end of an experience")]
            public string sceneData_End;
            [TextArea(5, 20)]            
            /// <summary>
            /// Contains the Visual Prompt Condition state used to determine which set of cultural priming images the user is shown during the slideshow portion of the experience
            /// </summary>
            [Tooltip("Contains the Visual Prompt Condition state used to determine which set of cultural priming images the user is shown during the slideshow portion of the experience")]
            public string imageConditionData;
            [TextArea(5, 20)]
            /// <summary>
            /// String containing all output info which will get written to the data file
            /// </summary>
            [Tooltip("String containing all output info which will get written to the data file")]
            public string outputString;
            [Header("Settings")]
            [Space(10)]
            /// <summary>
            /// Determines if the data file gets cleared on start (TRUE) or not (FALSE)
            /// </summary>
            [Tooltip("Determines if the data file gets cleared on start (TRUE) or not (FALSE)")]
            public bool clearFileOnStart;
            ///// <summary>
            ///// Determines if the date gets included in the file name (TRUE) or not (FALSE)
            ///// </summary>
            //[Tooltip("Determines if the date gets included in the file name (TRUE) or not (FALSE)")]
            //public bool addDateToFileName;
            ///// <summary>
            ///// Determines if the User ID number gets included in the file name (TRUE) or not (FALSE)
            ///// </summary>
            //[Tooltip("Determines if the User ID number gets included in the file name (TRUE) or not (FALSE)")]
            //public bool addUserIdToFileName;
            /// <summary>
            /// Determines if the data export file gets created as a text file (TRUE) or not (FALSE)
            /// </summary>
            [Tooltip("Determines if the data export file gets created as a text file (TRUE) or not (FALSE)")]
            public bool createAsTextFile;
            /// <summary>
            /// Determines if the User ID number gets included in data written into the data file (TRUE) or not (FALSE)
            /// </summary>
            [Tooltip("Determines if the User ID number gets included in data written into the data file (TRUE) or not (FALSE)")]
            public bool includeUserIdInFile;
            /// <summary>
            /// Determines if the scene data gets included in the data written into the data file (TRUE) or not (FALSE)
            /// </summary>
            [Tooltip("Determines if the scene data gets included in the data written into the data file (TRUE) or not (FALSE)")]
            public bool includeSceneData;
            /// <summary>
            /// Determines if the ending scene data gets included in the data written into the data file (TRUE) or not (FALSE)
            /// </summary>
            [Tooltip("Determines if the ending scene data gets included in the data written into the data file (TRUE) or not (FALSE)")]
            public bool includeSceneData_End;
            /// <summary>
            /// Determines if the name of each questionnaire gets included in the data file before the responses are listed (TRUE)
            /// or if only the responses are added (FALSE)
            /// </summary>
            [Tooltip("Determines if the name of each questionnaire gets included in the data file before the responses are listed (TRUE) or if only the responses are added (FALSE)")]
            public bool includeSetNames;
            /// <summary>
            /// Determines if the Visual Prompt Condition used during the Cultural Priming Condition phase of the experience gets included (TRUE) or not (FALSE)
            /// </summary>
            [Tooltip("Determines if the Visual Prompt Condition used during the Cultural Priming Condition phase of the experience gets included (TRUE) or not (FALSE)")]
            public bool includeImageCondition;
            /// <summary>
            /// Debugging Settings
            /// </summary>
            [Space(10)]
            [Tooltip("Debugging settings")]
            public bool debug_data;

            public int UserID { get=>userID; set { userID = value; } }

            /// <summary>
            /// Sets up the data and data file based on the settings variables
            /// </summary>
            public void Setup()
            {
                File_SetupFileName();
                File_SetupFilePath();
                if (clearFileOnStart)
                    File_ClearFile();

                File_SetupSceneData();
                File_SetupImageConditionData();
                File_WriteSceneStartString();
            }

            /// <summary>
            /// Sets up the data and data file based on the settings variables
            /// </summary>
            public void Setup(int _userID)
            {
                UserID = _userID;
                File_SetupFileName();
                File_SetupFilePath();
                if (clearFileOnStart)
                    File_ClearFile();

                File_SetupSceneData();
                File_SetupImageConditionData();
                File_WriteSceneStartString();
            }

            //public void Setup_ManagerOverride(string _filename, string _filePath, bool _clearFileOnStart,
            //                                  bool _includeUserID, bool _includeSceneData, bool _includeSetNames)
            //{
            //    filename = _filename;
            //    filePath = _filePath;
            //    clearFileOnStart = _clearFileOnStart;
            //    includeUserIdInFile = _includeUserID;
            //    includeSceneData = _includeSceneData;
            //    includeSetNames = _includeSetNames;

            //    File_SetupSceneData();
            //}

            /// <summary>
            /// Sets up the filename for the exported data file
            /// </summary>
            void File_SetupFileName()
            {

                //Name the file with the User ID and Event
                filename = UserID.ToString() + "_Responses.txt";
                //filename = globals.userID.ToString() + "_Responses.txt";



                //if (addUserIdToFileName)
                //    filename = filename + "_ID" + globals.userID.ToString() + "_";

                //if (addDateToFileName)
                //{
                //    string date = System.DateTime.Now.ToString();
                //    date = date.Replace("/", "_");
                //    date = date.Replace(":", "_");
                //    date = date.Replace(" ", "_");
                //    filename = filename + date;
                //}

                //if (createAsTextFile)
                //    filename = filename + ".txt";
            }

            /// <summary>
            /// Sets up the filepath for the exported data file 
            /// </summary>
            void File_SetupFilePath()
            {
                filePath = Application.persistentDataPath + "/" + filename;

                if (debug_data)
                    Debug.Log("Data File Path: " + filePath);
            }

            /// <summary>
            /// Sets up the scene data for the exported data file
            /// </summary>
            void File_SetupSceneData()
            {
                if (includeSceneData)
                {
                    //sceneData = "\n" + "Loaded Scene : " + SceneManager.GetActiveScene().name + " at " + Time.realtimeSinceStartup.ToString() + " global time on " + System.DateTime.Now + "." + "\n";
                    sceneData = "Entry Date: " + System.DateTime.Now + "." + "\n" + "Loaded Scene: " + SceneManager.GetActiveScene().name + " at " + Time.realtimeSinceStartup.ToString() + " global time. " + "\n";
                    sceneData += "Scene changed at " + Time.realtimeSinceStartup.ToString() + ". Total time in scene was " + Time.timeSinceLevelLoad.ToString() + "\n";
                }
                else
                    sceneData = "";
            }

            /// <summary>
            /// Sets up the scene data for the exported data file
            /// </summary>
            public void File_SetupSceneData_EndOfExperience()
            {
                if (includeSceneData_End)
                {
                    sceneData_End = "Experience concluded at " + Time.realtimeSinceStartup.ToString() + ". Total time in scene was " + Time.timeSinceLevelLoad.ToString() + "\n";
                }
                else
                    sceneData_End = "";
            }

            void File_SetupImageConditionData()
            {
                    imageConditionData = "";

                //if (includeImageCondition)
                //    imageConditionData = "Visual Prompt Condition: " + globals.promptCondition.ToString() + ".\n";
                //else
                //    imageConditionData = "";
            }

            /// <summary>
            /// Clears the data file (if any) at the filepath location
            /// </summary>
            void File_ClearFile()
            {
                //Overwrite some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, false);
                writer.WriteLine(" ");
                writer.Close();
            }

            /// <summary>
            /// Writes an info string to the filepath at the start of the scene
            /// </summary>
            public void File_WriteSceneStartString()
            {
                filePath = Application.persistentDataPath + "/" + filename;

                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine("");
                writer.WriteLine(sceneData);
                writer.Close();
            }

            /// <summary>
            /// Writes the gathered output string to the file at the filepath location
            /// </summary>
            public void File_WriteOutputToFile()
            {
                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine("");
                writer.WriteLine(outputString);
                writer.Close();
            }

            /// <summary>
            /// Writes the gathered output string to the file at the filepath location
            /// </summary>
            /// <param name="_append">Determines if the file gets appended (TRUE) or overwritten (FALSE)</param>
            public void File_WriteOutputToFile(bool _append)
            {
                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, _append);
                writer.WriteLine("");
                writer.WriteLine(outputString);
                writer.Close();
            }

            /// <summary>
            /// Sets up the output string by gathering the response data for all questionnaires.
            /// Also includes any additional info as determined by the data set variables
            /// </summary>
            public void File_SetupOutputString()
            {
                outputString = "";

                if (includeUserIdInFile)
                    outputString = outputString + "User ID ," + userID.ToString() + ",\n";

                //if (includeUserIdInFile)
                //    outputString = outputString + "User ID ," + globals.userID.ToString() + ",\n";

                if (includeImageCondition)
                    outputString = outputString + "\n" + imageConditionData + "\n";

                //if (includeSceneData)
                //    outputString = outputString + sceneData + "\n";

                    //outputString = outputString + sceneData + ",";
            }

        }

        /// <summary>
        /// Container class which holds debugging variables for testing and debugging through the editor at runtime
        /// </summary>
        [Serializable]
        public class DebugSettings
        {
            /// <summary>
            /// Determines if debugging is active (TRUE) or not (FALSE)
            /// </summary>
            [Tooltip("Determines if debugging is active (TRUE) or not (FALSE)")]
            public bool debug;
            /// <summary>
            /// Builds the output string when set to TRUE while debug is also set to TRUE
            /// </summary>
            [Tooltip("Builds the output string when set to TRUE while debug is also set to TRUE")]
            public bool debug_Output_BuildString;
            /// <summary>
            /// Prints the output string to the data file when set to TRUE while debug is also set to TRUE
            /// </summary>
            [Tooltip("Prints the output string to the data file when set to TRUE while debug is also set to TRUE")]
            public bool debug_Output_PrintToFile;
            /// <summary>
            /// Determines if the debug print to file functionality appends the file (TRUE) or overwrites it (FALSE)
            /// </summary>
            [Tooltip("Determines if the debug print to file functionality appends the file (TRUE) or overwrites it (FALSE)")]
            public bool debug_Output_AppendFileOnPrint;
        }

        /// <summary>
        ///  Start is called before the first frame update
        /// </summary>
        void Start()
        {
            Setup();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update()
        {
            CheckDebugBools();
        }

        /// <summary>
        /// Sets up the data output and the questionnaire/response sets
        /// </summary>
        public void Setup()
        {
            dataOutput.Setup(userID);
            //dataOutput.Setup();

            for (int i = 0; i < sets.Count; i++)
            {
                if (sets[i] != null)
                {
                    sets[i].Setup();
                    Data_SortSetsByID();
                }
            }
        }

        //public void Setup_ManagerOverride(string _filename, string _filePath, bool _clearFileOnStart,
        //                                      bool _includeUserID, bool _includeSceneData, bool _includeSetNames)
        //{
        //    dataOutput.Setup_ManagerOverride(_filename, _filePath, _clearFileOnStart, _includeUserID, _includeSceneData, _includeSetNames);

        //    for (int i = 0; i < sets.Count; i++)
        //    {
        //        if (sets[i] != null)
        //        {
        //            sets[i].Setup();
        //            Data_SortSetsByID();
        //        }
        //    }
        //}

        //  Data Set Functions      -------------------------------------------------------------------------------------
        #region Data Set Functions

        /// <summary>
        /// Updates questionnaire set info for a given questionnaire set
        /// </summary>
        /// <param name="_setID">ID of the questionnaire set to be updated</param>
        /// <param name="_setName">Name to apply to the questionnaire set</param>
        /// <param name="_totalResponses">Total number of responses to apply to the questionnaire set</param>
        public void SetInfo_UpdateSet(int _setID, string _setName, int _totalResponses)
        {
            for (int i = 0; i < sets.Count; i++)
            {
                if (sets[i] != null && sets[i].id == _setID)
                {
                    sets[i].name = _setName;
                    if (sets[i].responses != null && sets[i].responses.Count != _totalResponses)
                    {
                        sets[i].totalResponses = _totalResponses;
                        sets[i].Setup();
                    }
                }
            }
        }

        public void Data_SetResponse(int _setID, int _questionNum, string _response)
        {
            for (int i = 0; i < sets.Count; i++)
            {
                if (sets[i] != null && sets[i].id == _setID)
                {
                    sets[i].Response_UpdateInfo(_questionNum, _response);
                }
            }
        }

        public void Data_AddNewSet(int _setID, string _setName)
        {
            if (!Data_ContainsSetByID(_setID))
            {
                ResponseSet newSet = new ResponseSet();
                newSet.id = _setID;
                newSet.name = _setName;
                newSet.Setup();
                sets.Add(newSet);

                Data_SortSetsByID();
            }
        }

        public bool Data_ContainsSetByID(int _setID)
        {
            for (int i = 0; i < sets.Count; i++)
            {
                if (sets[i] != null && sets[i].id == _setID)
                {
                    return true;
                }
            }

            return false;
        }

        public void Data_SortSetsByID()
        {
            if (sets != null)
                sets.Sort(CompareID);
        }

        private int CompareID(ResponseSet a, ResponseSet b)
        {
            int a_ID = a.id;
            int b_ID = b.id;

            if (a_ID >= b_ID)
                return 1;
            else
                return -1;
        }

        public void Data_RemoveSetByID(int _id)
        {
            for (int i = sets.Count - 1; i >= 0; i--)
            {
                if (sets[i] != null && sets[i].id == _id)
                    sets.RemoveAt(i);
            }
        }

        public void Data_BuildDataSetString()
        {
            dataOutput.File_SetupOutputString();

            //dataOutput.outputString = "";

            //if (dataOutput.includeUserIdInFile)
            //    dataOutput.outputString = dataOutput.outputString + "User ID" + globals.userID.ToString() + ",";

            //if (dataOutput.includeSceneData)
            //    dataOutput.outputString = dataOutput.outputString + dataOutput.sceneData;

            if (sets != null)
            {
                for (int i = 0; i < sets.Count; i++)
                {
                    if (sets[i] != null)
                    {
                        dataOutput.outputString = dataOutput.outputString + sets[i].GetCompleteOutput(dataOutput.includeSetNames) + "\n";
                    }
                }
            }

            dataOutput.File_SetupSceneData_EndOfExperience();
            dataOutput.outputString = dataOutput.outputString + "\n" + dataOutput.sceneData_End;

            dataOutput.outputString = dataOutput.outputString + "\n\n\n";

        }

        public void Data_WriteDataToFile(bool _appendFile)
        {
            dataOutput.File_WriteOutputToFile(_appendFile);
        }

        public void Data_BuildAndOutputToFile(bool _append)
        {
            Data_BuildDataSetString();
            Data_WriteDataToFile(_append);
        }

        #endregion

        //  Debugging Functions     -------------------------------------------------------------------------------------
        #region Debugging Functions

        void CheckDebugBools()
        {
            if (debugSettings.debug)
            {
                if (debugSettings.debug_Output_BuildString)
                {
                    Data_BuildDataSetString();
                    Debug.Log("DataOutput_Questionnaire - > Debug: Build Output String Complete.");
                    debugSettings.debug_Output_BuildString = false;
                }

                if (debugSettings.debug_Output_PrintToFile)
                {
                    dataOutput.File_WriteOutputToFile(debugSettings.debug_Output_AppendFileOnPrint);
                    Debug.Log("DataOutput_Questionnaire - > Debug: Write Output To File Complete. Append File State: " + debugSettings.debug_Output_AppendFileOnPrint);
                    debugSettings.debug_Output_PrintToFile = false;
                }
            }
        }

        #endregion

    }
}