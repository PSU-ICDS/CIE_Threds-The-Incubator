using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DataOutput_Basic : MonoBehaviour
{
    [SerializeField] bool setupFileOnStart;
    [SerializeField] int userID;
    [SerializeField] bool useStringID;
    [SerializeField] string userID_string;
    [SerializeField] string fileLocation;
    [SerializeField] bool useAlternateDelimiter;
    [SerializeField] string altDelimiter;
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

    public enum ExportFormatTypes { NONE, TEXT_FILE, SQL, URL }
    public enum Php_InfoType { NONE, TOTAL, GAMESTATS, RESPONSES }

    public int UserID { get => dataOutput.UserID; set { dataOutput.UserID = value; } }
    public string UserID_String { get=> dataOutput.UserID_String; set { dataOutput.UserID_String = value; } }

    public string FilePath { get => dataOutput.filePath; set { dataOutput.filePath = value; } }
    public string FilePath_PHP { get => dataOutput.php_Filepath; set { dataOutput.php_Filepath = value; } }

    public ExportFormatTypes FileFormat { get => dataOutput.fileFormat; set { dataOutput.fileFormat = value; } }

    public WriteToPHP_ThredGame PHP_Connector { get => dataOutput.phpConnector; set { dataOutput.phpConnector = value; } }

    public Php_InfoType PHP_InfoType { get => dataOutput.php_InfoType; set { dataOutput.php_InfoType = value; } }

    public string DataString { get=>dataOutput.dataString; set {dataOutput.dataString = value ;} }

    /// <summary>
    /// Container class which holds settings for the exported dataset and file
    /// </summary>
    [Serializable]
    public class DataOutputSettings
    {
        [SerializeField] int userID;
        [SerializeField] bool useStringID;
        [SerializeField] string userID_string;
        [SerializeField] public ExportFormatTypes fileFormat;
        public string delimiter;
        [Space(10)]
        /// <summary>
        /// Name of the file to be created and exported
        /// </summary>
        [Tooltip("Name of the file to be created and exported")]
        public string filename;
        [TextArea(2, 20)]
        /// <summary>
        /// Label to add to the begining of the name of the file to be created and exported
        /// </summary>
        [Tooltip("Label to add to the begining of the name of the file to be created and exported")]
        public string fileLabel;
        [TextArea(2, 20)]
        /// <summary>
        /// Suffix to be added to the end of the name of the file to be created and exported
        /// </summary>
        [Tooltip("Suffix to be added to the end of the name of the file to be created and exported")]
        public string fileSuffix;
        [TextArea(5, 20)]
        /// <summary>
        /// Filepath to the location where the exported data file will be created/located
        /// </summary>
        [Tooltip("Filepath to the location where the exported data file will be created/located")]
        public string filePath;
        [TextArea(5, 20)]
        /// <summary>
        /// Info about the current date and time
        /// </summary>
        [Tooltip("Info about the current date and time")]
        public string dateTimeData;
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
        /// String containing all intro setup info which will get written to the data file
        /// </summary>
        [Tooltip("String containing all intro setup info which will get written to the data file")]
        public string introString;
        [TextArea(5, 20)]
        /// <summary>
        /// String containing all output info which will get written to the data file
        /// </summary>
        [Tooltip("String containing all output info which will get written to the data file")]
        public string outputString;
        [TextArea(5, 20)]
        /// <summary>
        /// String containing data which will get written to the data file
        /// </summary>
        [Tooltip("String containing data which will get written to the data file")]
        public string dataString;

        [Space(10)]
        [Header("PHP Settings")]
        [Space(10)]
        public WriteToPHP_ThredGame phpConnector;
        public string php_Filepath;
        public Php_InfoType php_InfoType;

        [Header("Settings")]
        [Space(10)]
        /// <summary>
        /// Determines if the data file gets cleared on start (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if the data file gets cleared on start (TRUE) or not (FALSE)")]
        public bool clearFileOnStart;
        [Space(10)]
        ///// <summary>
        ///// Determines if the data export file gets created as a text file (TRUE) or not (FALSE)
        ///// </summary>
        //[Tooltip("Determines if the data export file gets created as a text file (TRUE) or not (FALSE)")]
        ////public bool createAsTextFile;
        /// <summary>
        /// Determines if the file label gets added to the name of the data file (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if the file label gets added to the name of the data file (TRUE) or not (FALSE)")]
        public bool includeFileLabel;
        /// <summary>
        /// Determines if the file label gets added to the name of the data file (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if the file suffix gets added to the name of the data file (TRUE) or not (FALSE)")]
        public bool includeFileSuffix;
        /// <summary>
        /// Determines if the User ID number gets included in data written into the data file (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if the User ID number gets included in data written into the data file (TRUE) or not (FALSE)")]
        public bool includeUserIdInFile;
        /// <summary>
        /// Determines if the date and time gets included in the data written into the data file (TRUE) or not (FALSE)
        /// </summary>
        [Tooltip("Determines if the date and time gets included in the data written into the data file (TRUE) or not (FALSE)")]
        public bool includeDateAndTime;
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
        /// Debugging Settings
        /// </summary>
        [Space(10)]
        [Tooltip("Debugging settings")]
        public bool debug_data;

        public int UserID { get => userID; set { userID = value; } }
        public string UserID_String { get => userID_string; set { userID_string = value; } }
        public string Delimiter { get => delimiter; set { delimiter = value; } }

        //public ExportFormatTypes FileFormatType { get; set; }

        //public ExportFormatTypes FileFormat { get=>fileFormat; set {fileFormat = value; } }

        /// <summary>
        /// Sets up the data and data file based on the settings variables
        /// </summary>
        public void Setup()
        {
            File_SetupFileName();
            File_SetupFilePath();
            if (clearFileOnStart)
                File_ClearFile();

            File_SetupDateTimeData();
            File_SetupSceneData();
            //File_SetupImageConditionData();
            File_WriteSceneStartString();
        }

        /// <summary>
        /// Sets up the data and data file based on the settings variables
        /// </summary>
        public void Setup(int _userID)
        {
            useStringID = false;
            UserID = _userID;
            File_SetupFileName();
            File_SetupFilePath();
            if (clearFileOnStart)
                File_ClearFile();

            File_SetupDateTimeData();
            File_SetupSceneData();
            //File_SetupImageConditionData();
            File_WriteSceneStartString();
        }

        /// <summary>
        /// Sets up the data and data file based on the settings variables
        /// </summary>
        public void Setup(string _userID)
        {
            useStringID = true;
            UserID_String = _userID;
            File_SetupFileName();
            File_SetupFilePath();
            if (clearFileOnStart)
                File_ClearFile();

            File_SetupDateTimeData();
            File_SetupSceneData();
            //File_SetupImageConditionData();
            File_WriteSceneStartString();
        }

        /// <summary>
        /// Sets up the filename for the exported data file
        /// </summary>
        void File_SetupFileName()
        {

            //  Name the file with the User ID and Event

            if (useStringID)
            {
                if (includeFileLabel)
                    filename = fileLabel + "_" + UserID_String;
                else
                    filename = UserID_String;
            }
            else
            {
                if (includeFileLabel)
                    filename = fileLabel + "_" + UserID.ToString();
                else
                    filename = UserID.ToString();
            }

            if (includeFileSuffix)
                filename = filename + "_" + fileSuffix;

            //filename = fileLabel + "_" + UserID.ToString() + "_" + fileSuffix;
            //filename = globals.userID.ToString() + "_Responses.txt";

            //  Add file format
            filename = filename + File_GetExportType();

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
        /// Returns a string for the file format type to be added to the end of the filename
        /// </summary>
        /// <returns>Returns a string used for the export file format</returns>
        string File_GetExportType()
        {
            switch (fileFormat)
            {
                case ExportFormatTypes.NONE:
                    break;
                case ExportFormatTypes.TEXT_FILE:
                    return ".txt";
                case ExportFormatTypes.SQL:
                    break;
                case ExportFormatTypes.URL:
                    break;
                default:
                    return ".txt";
            }

            return ".txt";
        }

        /// <summary>
        /// Sets up the filepath for the exported data file 
        /// </summary>
        void File_SetupFilePath()
        {
            if (fileFormat == ExportFormatTypes.TEXT_FILE)
            {
                filePath = Application.persistentDataPath + "/" + filename;

                if (debug_data)
                    Debug.Log("Data File Path: " + filePath);
            }

            if (fileFormat == ExportFormatTypes.SQL)
            {
                if (phpConnector != null)
                {
                    phpConnector.SetFilePath(filePath);
                    //phpConnector.SetFilePath(filePath);
                }
            }

            //filePath = Application.persistentDataPath + "/" + filename;

            //if (debug_data)
            //    Debug.Log("Data File Path: " + filePath);
        }

        /// <summary>
        /// Sets up the date and time data for the exported data file
        /// </summary>
        void File_SetupDateTimeData()
        {
            if (includeDateAndTime)
            {
                //sceneData = "\n" + "Loaded Scene : " + SceneManager.GetActiveScene().name + " at " + Time.realtimeSinceStartup.ToString() + " global time on " + System.DateTime.Now + "." + "\n";
                dateTimeData = "Entry Date: " + System.DateTime.Now + "." + "\n";
                //dateTimeData = "Entry Date: " + System.DateTime.Now + "." + "\n" + "Loaded Scene: " + SceneManager.GetActiveScene().name + " at " + Time.realtimeSinceStartup.ToString() + " global time. " + "\n";
                //dateTimeData += "Scene changed at " + Time.realtimeSinceStartup.ToString() + ". Total time in scene was " + Time.timeSinceLevelLoad.ToString() + "\n";
            }
            else
                dateTimeData = "";
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



        /// <summary>
        /// Clears the data file (if any) at the filepath location
        /// </summary>
        void File_ClearFile()
        {
            if (fileFormat == ExportFormatTypes.TEXT_FILE)
            {
                //Overwrite some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, false);
                writer.WriteLine(" ");
                writer.Close();
            }

            if (fileFormat == ExportFormatTypes.SQL)
            {
                if (phpConnector != null)
                {
                    //TODO: Add PHP connected functionality to: Clear File
                }
            }

            ////Overwrite some text to the test.txt file
            //StreamWriter writer = new StreamWriter(filePath, false);
            //writer.WriteLine(" ");
            //writer.Close();
        }

        /// <summary>
        /// Writes an info string to the filepath at the start of the scene
        /// </summary>
        public void File_WriteSceneStartString()
        {
            if (fileFormat == ExportFormatTypes.TEXT_FILE)
            {
                filePath = Application.persistentDataPath + "/" + filename;

                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine("");
                writer.WriteLine(sceneData + dateTimeData);
                writer.Close();
            }

            if (fileFormat == ExportFormatTypes.SQL)
            {
                if (phpConnector != null)
                {
                    //TODO: Add PHP connected functionality to: Write Scene Starting String
                }
            }

            //filePath = Application.persistentDataPath + "/" + filename;

            ////Write some text to the test.txt file
            //StreamWriter writer = new StreamWriter(filePath, true);
            //writer.WriteLine("");
            //writer.WriteLine(sceneData + dateTimeData);
            //writer.Close();
        }

        /// <summary>
        /// Writes the gathered output string to the file at the filepath location
        /// </summary>
        public void File_WriteOutputToFile()
        {
            Debug.Log("DataOutput_Basic => WriteOutputToFile() started for output type: " + php_InfoType.ToString());

            if (fileFormat == ExportFormatTypes.TEXT_FILE)
            {
                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine("");
                writer.WriteLine(outputString);
                writer.Close();

                Debug.Log("DataOutput_Basic => WriteOutputToFile() info written to text file finished for output type: " + php_InfoType.ToString());
            }

            if (fileFormat == ExportFormatTypes.SQL)
            {
                if (phpConnector != null)
                {
                    //TODO: Add PHP connected functionality to: Write output to file
                    switch (php_InfoType)
                    {
                        case Php_InfoType.NONE:
                            break;
                        case Php_InfoType.TOTAL:
                            phpConnector.SetInfo_Total(outputString);
                            break;
                        case Php_InfoType.GAMESTATS:
                            phpConnector.SetInfo_GameStats(outputString);
                            break;
                        case Php_InfoType.RESPONSES:
                            phpConnector.SetInfo_Responses(outputString);
                            break;
                        default:
                            break;
                    }

                    //phpConnector.SetInfo_Total(outputString);
                    phpConnector.Send_TextToFile();

                    Debug.Log("DataOutput_Basic => WriteOutputToFile() info written to php/sql file finished for output type: " + php_InfoType.ToString());

                }
            }

            ////Write some text to the test.txt file
            //StreamWriter writer = new StreamWriter(filePath, true);
            //writer.WriteLine("");
            //writer.WriteLine(outputString);
            //writer.Close();
        }

        /// <summary>
        /// Writes the gathered output string to the file at the filepath location
        /// </summary>
        /// <param name="_append">Determines if the file gets appended (TRUE) or overwritten (FALSE)</param>
        public void File_WriteOutputToFile(bool _append)
        {
            Debug.Log("DataOutput_Basic => WriteOutputToFile() started for output type: " + php_InfoType.ToString());
            // Debug.Log("DataOutput_Basic => WriteOutputToFile() info written to text file finished for output type: " + php_InfoType.ToString());
            //Debug.Log("DataOutput_Basic => WriteOutputToFile() info written to php/sql file finished for output type: " + php_InfoType.ToString());

            if (fileFormat == ExportFormatTypes.TEXT_FILE)
            {
                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, _append);
                writer.WriteLine("");
                writer.WriteLine(outputString);
                writer.Close();

                Debug.Log("DataOutput_Basic => WriteOutputToFile() info written to text file finished for output type: " + php_InfoType.ToString());
            }

            if (fileFormat == ExportFormatTypes.SQL)
            {
                if (phpConnector != null)
                {
                    //TODO: Add PHP connected functionality to: Write output to file
                    switch (php_InfoType)
                    {
                        case Php_InfoType.NONE:
                            break;
                        case Php_InfoType.TOTAL:
                            phpConnector.SetInfo_Total(outputString);
                            break;
                        case Php_InfoType.GAMESTATS:
                            phpConnector.SetInfo_GameStats(outputString);
                            break;
                        case Php_InfoType.RESPONSES:
                            phpConnector.SetInfo_Responses(outputString);
                            break;
                        default:
                            break;
                    }

                    //TODO: Add PHP connected functionality to: Write output to file
                    //phpConnector.SetInfo_Total(outputString);
                    phpConnector.Send_TextToFile();

                    Debug.Log("DataOutput_Basic => WriteOutputToFile() info written to php/sql file finished for output type: " + php_InfoType.ToString());
                }
            }


            ////Write some text to the test.txt file
            //StreamWriter writer = new StreamWriter(filePath, _append);
            //writer.WriteLine("");
            //writer.WriteLine(outputString);
            //writer.Close();
        }


        /// <summary>
        /// Writes the gathered output string to the file at the filepath location
        /// </summary>
        public void File_WriteDataToFile()
        {
            Debug.Log("DataOutput_Basic => WriteDataToFile() started for output type: " + php_InfoType.ToString());
            //Debug.Log("DataOutput_Basic => WriteDataToFile() info written to text file finished for output type: " + php_InfoType.ToString());
            //Debug.Log("DataOutput_Basic => WriteDataToFile() info written to php/sql file finished for output type: " + php_InfoType.ToString());

            if (fileFormat == ExportFormatTypes.TEXT_FILE)
            {
                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine("");
                writer.WriteLine(dataString);
                writer.Close();

                dataString = "";
                Debug.Log("DataOutput_Basic => WriteDataToFile() info written to text file finished for output type: " + php_InfoType.ToString());
            }

            if (fileFormat == ExportFormatTypes.SQL)
            {
                if (phpConnector != null)
                {
                    //TODO: Add PHP connected functionality to: Write output to file
                    switch (php_InfoType)
                    {
                        case Php_InfoType.NONE:
                            break;
                        case Php_InfoType.TOTAL:
                            phpConnector.SetInfo_Total(outputString);
                            break;
                        case Php_InfoType.GAMESTATS:
                            phpConnector.SetInfo_GameStats(outputString);
                            break;
                        case Php_InfoType.RESPONSES:
                            phpConnector.SetInfo_Responses(outputString);
                            break;
                        default:
                            break;
                    }

                    //TODO: Add PHP connected functionality to: Write output to file
                    //phpConnector.SetInfo_Total(dataString);
                    phpConnector.Send_TextToFile();
                    Debug.Log("DataOutput_Basic => WriteDataToFile() info written to php/sql file finished for output type: " + php_InfoType.ToString());
                }
            }


            ////Write some text to the test.txt file
            //StreamWriter writer = new StreamWriter(filePath, true);
            //writer.WriteLine("");
            //writer.WriteLine(dataString);
            //writer.Close();

            //dataString = "";
        }

        /// <summary>
        /// Writes the gathered output string to the file at the filepath location
        /// </summary>
        /// <param name="_append">Determines if the file gets appended (TRUE) or overwritten (FALSE)</param>
        public void File_WriteDataToFile(bool _append)
        {
            Debug.Log("DataOutput_Basic => WriteDataToFile() started for output type: " + php_InfoType.ToString());

            if (fileFormat == ExportFormatTypes.TEXT_FILE)
            {
                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(filePath, _append);
                writer.WriteLine("");
                writer.WriteLine(dataString);
                writer.Close();

                dataString = "";
                Debug.Log("DataOutput_Basic => WriteDataToFile() info written to text file finished for output type: " + php_InfoType.ToString());
            }

            if (fileFormat == ExportFormatTypes.SQL)
            {
                if (phpConnector != null)
                {
                    //TODO: Add PHP connected functionality to: Write output to file
                    switch (php_InfoType)
                    {
                        case Php_InfoType.NONE:
                            break;
                        case Php_InfoType.TOTAL:
                            phpConnector.SetInfo_Total(outputString);
                            break;
                        case Php_InfoType.GAMESTATS:
                            phpConnector.SetInfo_GameStats(outputString);
                            break;
                        case Php_InfoType.RESPONSES:
                            phpConnector.SetInfo_Responses(outputString);
                            break;
                        default:
                            break;
                    }

                    Debug.Log("DataOutput_Basic => WriteDataToFile() info written to php/sql file finished for output type: " + php_InfoType.ToString());
                    //TODO: Add PHP connected functionality to: Write output to file
                    //phpConnector.SetInfo_Total(dataString);
                    phpConnector.Send_TextToFile();
                }
            }

            ////Write some text to the test.txt file
            //StreamWriter writer = new StreamWriter(filePath, _append);
            //writer.WriteLine("");
            //writer.WriteLine(dataString);
            //writer.Close();

            //dataString = "";
        }

        /// <summary>
        /// Sets up the output string by gathering the response data for all questionnaires.
        /// Also includes any additional info as determined by the data set variables
        /// </summary>
        public void File_SetupOutputString()
        {
            outputString = "";
            introString = "";

            if (useStringID)
                introString = "User ID " + delimiter + userID_string + delimiter + "\n";
            else
                introString = "User ID " + delimiter + userID.ToString() + delimiter + "\n";

            if (includeUserIdInFile)
            {
                outputString = outputString + introString;

                //if (useStringID)
                //    outputString = outputString + "User ID " + delimiter + userID_string + delimiter + "\n";
                //else
                //    outputString = outputString + "User ID " + delimiter + userID.ToString() + delimiter + "\n";
            }

            //if (includeUserIdInFile)
            //    outputString = outputString + "User ID ," + userID.ToString() + ",\n";
        }

        public void File_AddData(string _data)
        {
            dataString = dataString + _data;
        }


        public void File_AddData(string _data, bool _newLineBefore = false, bool _newLineAfter = false, bool _commaAfter = false)
        {
            if (_newLineBefore)
                dataString = dataString + "\n" + _data;
            else
                dataString = dataString + _data;

            //if (_commaAfter)
            //    dataString = dataString + ",";
            if (_commaAfter)
                dataString = dataString + delimiter;

            if (_newLineAfter)
                dataString = dataString + "\n";

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
        if (setupFileOnStart)
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
        dataOutput.Delimiter = GetDelimiterString();
        if (useStringID)
            dataOutput.Setup(userID_string);
        else
            dataOutput.Setup(userID);
        //dataOutput.Setup();
    }

    public void SaveFileLocation()
    {
        fileLocation = Application.persistentDataPath + "/";
    }

    public void Data_BuildDataSetString()
    {
        dataOutput.File_SetupOutputString();
        dataOutput.File_SetupSceneData_EndOfExperience();
        dataOutput.outputString = dataOutput.outputString + "\n" + dataOutput.dataString + "\n" + dataOutput.sceneData_End;
        //dataOutput.outputString = dataOutput.outputString + "\n" + dataOutput.sceneData_End;

        dataOutput.outputString = dataOutput.outputString + "\n\n\n";

    }

    public void Data_AddDataToOutput(string _data)
    {
        dataOutput.File_AddData(_data);
    }

    public void Data_AddDataToOutput(string _data, bool _newLineBefore = false, bool _newLineAfter = false, bool _commaAfter = false)
    {
        dataOutput.File_AddData(_data, _newLineBefore, _newLineAfter, _commaAfter);
    }

    public void Data_WriteDataToFile()
    {
        dataOutput.File_WriteDataToFile(true);
    }

    public void Data_WriteDataToFile(bool _append = false)
    {
        dataOutput.File_WriteDataToFile(_append);
    }

    public void File_WriteOutputToFile()
    {
        dataOutput.File_WriteOutputToFile();
    }

    public void File_WriteOutputToFile(bool _append)
    {
        dataOutput.File_WriteOutputToFile(_append);
    }

    public void SetUserID(int _id)
    {
        userID = _id;
        dataOutput.UserID = _id;
    }

    public string GetDelimiterString()
    {
        if (useAlternateDelimiter)
            return altDelimiter;
        else
            return ",";
    }

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
