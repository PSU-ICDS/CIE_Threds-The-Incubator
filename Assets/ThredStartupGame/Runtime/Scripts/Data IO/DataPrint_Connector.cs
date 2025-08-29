using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPrint_Connector : MonoBehaviour
{
    [SerializeField] TrackedVariables trackedData;
    [SerializeField] DataOutput_Basic data_Total;
    [SerializeField] DataOutput_Basic data_GameStats;
    [SerializeField] DataOutput_Basic data_Decisions;
    [SerializeField] DataOutput_Basic data_Team;
    [SerializeField] DataOutput_Basic data_Device;
    [Space(10)]
    [SerializeField] DataOutput_Questionnaire data_Questionnaire;
    [Space(10)]
    [SerializeField] PrintSettings settings;
    [Space(10)]
    [SerializeField] ExportFormatTypes fileFormat;
    [Space(10)]
    [SerializeField] string fileLocation;
    [Space(10)]
    [SerializeField] WriteToPHP_ThredGame php_Connector;
    [SerializeField] string filePath_PHP;
    [Space(10)]
    [SerializeField] string filePath_Default_Text;
    [SerializeField] string filePath_Default_PHP;
    [Space(10)]
    [SerializeField] DebugSettings debug;


    public enum ExportFormatTypes { NONE, TEXT_FILE, SQL, URL }


    public string FilePath_Applied { get => GetFilePathByFormatType(); }
    public string FilePath_DefaultText { get => filePath_Default_Text; }
    public string FilePath_DefaultPHP { get => filePath_Default_PHP; }

    [Serializable]
    public class PrintSettings
    {
        public string idKey;
        public bool applyIdFromPrefs;
        public bool printTotal;
        public bool printGameStats;
        public bool printDecisions;
        public bool printTeam;
        public bool printDevice;
        public bool printQuestionnaire;
    }

    [Serializable]
    public class DebugSettings
    {
        public bool debug;
        [Space(10)]
        public bool debug_ApplyUserID;
        public int debug_UserID;
        [Space(10)]
        public bool debug_printAll;
        public bool debug_printTotal;
        public bool debug_printGameStats;
        public bool debug_printDecisions;
        public bool debug_printTeam;
        public bool debug_printDevice;
        public bool debug_printQuestionnaire;
    }

    private void Awake()
    {
        if (settings.applyIdFromPrefs)
        {
            if (PlayerPrefs.HasKey(settings.idKey))
                SetUserID(PlayerPrefs.GetString(settings.idKey));
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        fileLocation = Application.persistentDataPath + "/";
        SaveFileLocation();
    }

    // Update is called once per frame
    void Update()
    {
        Debug_CheckSettings();
    }

    public void SetFileFormat_TextFile()
    {
        fileFormat = ExportFormatTypes.TEXT_FILE;
        OverrideFileFormat();

        Debug.Log("Text File Format Applied. FileFormat: " + fileFormat.ToString() + ", Text Path: " + fileLocation + ", PHP Path: " + filePath_PHP);

        //  TODO: Update all data outputs of new file format
    }

    public void SetFileFormat_PHP()
    {
        fileFormat = ExportFormatTypes.SQL;
        OverrideFileFormat();

        Debug.Log("PHP/SQL File Format Applied. FileFormat: " + fileFormat.ToString() + ", Text Path: " + fileLocation + ", PHP Path: " + filePath_PHP);

        //  TODO: Update all data outputs of new file format
    }

    public void OverrideFilePath_ByFileFormat(string _path)
    {

        switch (fileFormat)
        {
            case ExportFormatTypes.NONE:
                break;
            case ExportFormatTypes.TEXT_FILE:
                OverrideFilePath_Text(_path);
                break;
            case ExportFormatTypes.SQL:
                OverrideFilePath_PHP(_path);
                break;
            case ExportFormatTypes.URL:
                break;
            default:
                break;
        }
    }

    public void OverrideFilePath_Text(string _path)
    {
        fileLocation = _path;
        SetFilePath_TextFile();

        Debug.Log("Text File Path Applied. FileFormat: " + fileFormat.ToString() + ", Text Path: " + fileLocation + ", PHP Path: " + filePath_PHP);
    }

    public void OverrideFilePath_PHP(string _path)
    {
        filePath_PHP = _path;
        Set_PhpConnector();
        SetFilePath_PHP();

        Debug.Log("PHP/SQL File Path Applied. FileFormat: " + fileFormat.ToString() + ", Text Path: " + fileLocation + ", PHP Path: " + filePath_PHP);
    }

    public void SaveFileLocation()
    {
        filePath_Default_Text = Application.persistentDataPath + "/";
        filePath_Default_PHP = filePath_PHP;

        switch (fileFormat)
        {
            case ExportFormatTypes.NONE:
                break;
            case ExportFormatTypes.TEXT_FILE:
                fileLocation = Application.persistentDataPath + "/";
                filePath_Default_Text = fileLocation;
                break;
            case ExportFormatTypes.SQL:
                SetFilePath_PHP();
                filePath_Default_PHP = filePath_PHP;
                break;
            case ExportFormatTypes.URL:
                break;
            default:
                break;
        }

        //fileLocation = Application.persistentDataPath + "/";

    }

    public string GetFilePathByFormatType()
    {
        string _path = "";

        switch (fileFormat)
        {
            case ExportFormatTypes.NONE:
                break;
            case ExportFormatTypes.TEXT_FILE:
                _path = fileLocation;
                break;
            case ExportFormatTypes.SQL:
                _path = filePath_PHP;
                break;
            case ExportFormatTypes.URL:
                break;
            default:
                break;
        }

        return _path;
    }

    public void FilePath_ApplyDefaultPath()
    {

        filePath_PHP = filePath_Default_PHP;
        fileLocation = Application.persistentDataPath + "/";

        //switch (fileFormat)
        //{
        //    case ExportFormatTypes.NONE:
        //        break;
        //    case ExportFormatTypes.TEXT_FILE:
        //        fileLocation = Application.persistentDataPath + "/";
        //        //filePath_Default = fileLocation;
        //        break;
        //    case ExportFormatTypes.SQL:
        //        filePath_PHP = filePath_Default_PHP;
        //        SetFilePath_PHP();
        //        break;
        //    case ExportFormatTypes.URL:
        //        break;
        //    default:
        //        break;
        //}

        Debug.Log("Default File Path Applied. FileFormat: " + fileFormat.ToString() + ", Text Path: " + fileLocation + ", PHP Path: " + filePath_PHP);

    }

    public void SetFilePath_TextFile()
    {
        if (data_Total != null)
        {
            data_Total.FilePath = fileLocation;
            //data_Total.UserID = _id;
            data_Total.Setup();
        }

        if (data_GameStats != null)
        {
            data_GameStats.FilePath = fileLocation;
            //data_GameStats.UserID = _id;
            data_GameStats.Setup();
        }

        if (data_Decisions != null)
        {
            data_Decisions.FilePath = fileLocation;
            //data_Decisions.UserID = _id;
            data_Decisions.Setup();
        }

        if (data_Team != null)
        {
            data_Team.FilePath = fileLocation;
            //data_Team.UserID = _id;
            data_Team.Setup();
        }

        if (data_Device != null)
        {
            data_Device.FilePath = fileLocation;
            //data_Device.UserID = _id;
            data_Device.Setup();
        }

        if (data_Questionnaire != null)
        {
            data_Questionnaire.FilePath = fileLocation;
            data_Questionnaire.Setup();
        }
    }

    public void SetFilePath_PHP()
    {
        if (data_Total != null)
        {
            data_Total.FilePath = filePath_PHP;
            //data_Total.UserID = _id;
            data_Total.Setup();
        }

        if (data_GameStats != null)
        {
            data_GameStats.FilePath = filePath_PHP;
            //data_GameStats.UserID = _id;
            data_GameStats.Setup();
        }

        if (data_Decisions != null)
        {
            data_Decisions.FilePath = filePath_PHP;
            //data_Decisions.UserID = _id;
            data_Decisions.Setup();
        }

        if (data_Team != null)
        {
            data_Team.FilePath = filePath_PHP;
            //data_Team.UserID = _id;
            data_Team.Setup();
        }

        if (data_Device != null)
        {
            data_Device.FilePath = filePath_PHP;
            //data_Device.UserID = _id;
            data_Device.Setup();
        }

        if (data_Questionnaire != null)
        {
            data_Questionnaire.FilePath = filePath_PHP;
            data_Questionnaire.Setup();
        }
    }

    public void Set_PhpConnector()
    {
        if (php_Connector != null)
        {
            if (data_Total != null)
            {
                data_Total.PHP_Connector = php_Connector;
                data_Total.PHP_InfoType = DataOutput_Basic.Php_InfoType.TOTAL;
            }

            if (data_GameStats != null)
            {
                data_GameStats.PHP_Connector = php_Connector;
                data_GameStats.PHP_InfoType = DataOutput_Basic.Php_InfoType.GAMESTATS;
            }

            if (data_Decisions != null)
            {
                data_Decisions.PHP_Connector = php_Connector;
            }

            if (data_Team != null)
            {
                data_Team.PHP_Connector = php_Connector;
            }

            if (data_Device != null)
            {
                data_Device.PHP_Connector = php_Connector;
            }

            if (data_Questionnaire != null)
            {
                data_Questionnaire.PHP_Connector = php_Connector;
                data_Questionnaire.PHP_InfoType = DataOutput_Questionnaire.Php_InfoType.RESPONSES;
            }
        }
    }

    public void OverrideFileFormat()
    {
        if (data_Total != null)
        {
            if (fileFormat == ExportFormatTypes.SQL)
                data_Total.FileFormat = DataOutput_Basic.ExportFormatTypes.SQL;
            if (fileFormat == ExportFormatTypes.TEXT_FILE)
                data_Total.FileFormat = DataOutput_Basic.ExportFormatTypes.TEXT_FILE;
            //data_Total.UserID = _id;
            data_Total.Setup();
        }

        if (data_GameStats != null)
        {
            if (fileFormat == ExportFormatTypes.SQL)
                data_GameStats.FileFormat = DataOutput_Basic.ExportFormatTypes.SQL;
            if (fileFormat == ExportFormatTypes.TEXT_FILE)
                data_GameStats.FileFormat = DataOutput_Basic.ExportFormatTypes.TEXT_FILE;

            data_GameStats.Setup();
        }

        if (data_Decisions != null)
        {
            if (fileFormat == ExportFormatTypes.SQL)
                data_Decisions.FileFormat = DataOutput_Basic.ExportFormatTypes.SQL;
            if (fileFormat == ExportFormatTypes.TEXT_FILE)
                data_Decisions.FileFormat = DataOutput_Basic.ExportFormatTypes.TEXT_FILE;

            data_Decisions.Setup();
        }

        if (data_Team != null)
        {
            if (fileFormat == ExportFormatTypes.SQL)
                data_Team.FileFormat = DataOutput_Basic.ExportFormatTypes.SQL;
            if (fileFormat == ExportFormatTypes.TEXT_FILE)
                data_Team.FileFormat = DataOutput_Basic.ExportFormatTypes.TEXT_FILE;

            data_Team.Setup();
        }

        if (data_Device != null)
        {
            if (fileFormat == ExportFormatTypes.SQL)
                data_Device.FileFormat = DataOutput_Basic.ExportFormatTypes.SQL;
            if (fileFormat == ExportFormatTypes.TEXT_FILE)
                data_Device.FileFormat = DataOutput_Basic.ExportFormatTypes.TEXT_FILE;

            data_Device.Setup();
        }

        if (data_Questionnaire != null)
        {
            if (fileFormat == ExportFormatTypes.SQL)
                data_Questionnaire.FileFormat = DataOutput_Questionnaire.ExportFormatTypes.SQL;
            if (fileFormat == ExportFormatTypes.TEXT_FILE)
                data_Questionnaire.FileFormat = DataOutput_Questionnaire.ExportFormatTypes.TEXT_FILE;

            data_Questionnaire.Setup();
        }
    }

    public void SetUserID(int _id)
    {
        if (data_Total != null)
        {
            data_Total.SetUserID(_id);
            //data_Total.UserID = _id;
            data_Total.Setup();
        }

        if (data_GameStats != null)
        {
            data_GameStats.SetUserID(_id);
            //data_GameStats.UserID = _id;
            data_GameStats.Setup();
        }

        if (data_Decisions != null)
        {
            data_Decisions.SetUserID(_id);
            //data_Decisions.UserID = _id;
            data_Decisions.Setup();
        }

        if (data_Team != null)
        {
            data_Team.SetUserID(_id);
            //data_Team.UserID = _id;
            data_Team.Setup();
        }

        if (data_Device != null)
        {
            data_Device.SetUserID(_id);
            //data_Device.UserID = _id;
            data_Device.Setup();
        }

        if (data_Questionnaire != null)
        {
            data_Questionnaire.SetUserID(_id);
            data_Questionnaire.Setup();
        }
    }

    public void SetUserID(string _id)
    {
        if (settings.applyIdFromPrefs)
        {
            if (data_Total != null)
                data_Total.UserID_String = _id;

            if (data_GameStats != null)
                data_GameStats.UserID_String = _id;

            if (data_Decisions != null)
                data_Decisions.UserID_String = _id;

            if (data_Team != null)
                data_Team.UserID_String = _id;

            if (data_Device != null)
                data_Device.UserID_String = _id;
        }
        else
        {


            int _userID;
            if (int.TryParse(_id, out _userID))
            {

                if (data_Total != null)
                    data_Total.UserID = _userID;

                if (data_GameStats != null)
                    data_GameStats.UserID = _userID;

                if (data_Decisions != null)
                    data_Decisions.UserID = _userID;

                if (data_Team != null)
                    data_Team.UserID = _userID;

                if (data_Device != null)
                    data_Device.UserID = _userID;
            }
        }
    }

    public void Print_AllData_Checkpoint(bool _append)
    {
        Debug.Log("DataConnector=> Print Data All at Checkpoint started. Append State: " + _append);

        if (trackedData != null && data_Total != null && settings.printTotal)
        {
            data_Total.DataString = "";
            data_Total.Data_BuildDataSetString();

            string _checkpoint = "\n -------------------------------------------- \n" +
                                 " ---------  Checkpoint Number: " + trackedData.Data.checkPointNumber.ToString() + " --------- " +
                                 "\n -------------------------------------------- \n\n";
            data_Total.Data_AddDataToOutput(_checkpoint);
        }

        PrintData_Total(_append);
        PrintData_GameStats(_append);
        PrintData_Decisions(_append);
        PrintData_Team(_append);
        PrintData_Device(_append);
    }

    public void Print_AllData_Endgame(bool _append)
    {
        Debug.Log("DataConnector=> Print Data All at Game Over. Append State: " + _append);

        if (trackedData != null && data_Total != null && settings.printTotal)
        {
            data_Total.DataString = "";
            data_Total.Data_BuildDataSetString();

            string _checkpoint = "\n -------------------------------------------- \n" +
                                 "\n ----------------- Game Over ----------------- \n" +
                                 " ---------  Checkpoint Number: " + trackedData.Data.checkPointNumber.ToString() + " --------- " +
                                 "\n -------------------------------------------- \n\n";
            data_Total.Data_AddDataToOutput(_checkpoint);
        }

        PrintData_Total(_append);
        PrintData_GameStats(_append);
        PrintData_Decisions(_append);
        PrintData_Team(_append);
        PrintData_Device(_append);
    }

    public void Print_AllData()
    {
        Debug.Log("DataConnector=> Print Data All started.");

        PrintData_Total();
        PrintData_GameStats();
        PrintData_Decisions();
        PrintData_Team();
        PrintData_Device();
    }

    public void Print_AllData(bool _append)
    {
        Debug.Log("DataConnector=> Print Data All started. Append State: " + _append);

        PrintData_Total(_append);
        PrintData_GameStats(_append);
        PrintData_Decisions(_append);
        PrintData_Team(_append);
        PrintData_Device(_append);
    }

    public void PrintData_Total()
    {
        if (trackedData != null && data_Total != null && settings.printTotal)
        {
            Debug.Log("DataConnector=> Print Data Total started.");

            data_Total.Data_AddDataToOutput(trackedData.Info_GetEndgameStats());
            data_Total.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Decisions());
            data_Total.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Team());
            data_Total.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Device());
            data_Total.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_GameRooms());
            data_Total.Data_BuildDataSetString();
            data_Total.File_WriteOutputToFile();
        }
    }

    public void PrintData_Total(bool _append)
    {
        if (trackedData != null && data_Total != null && settings.printTotal)
        {
            Debug.Log("DataConnector=> Print Data Total started.");

            data_Total.Data_AddDataToOutput(trackedData.Info_GetEndgameStats());
            data_Total.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Decisions());
            data_Total.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Team());
            data_Total.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Device());
            data_Total.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_GameRooms());
            data_Total.Data_BuildDataSetString();
            data_Total.File_WriteOutputToFile(_append);
        }
    }

    public void PrintData_GameStats()
    {
        if (trackedData != null && data_GameStats != null && settings.printGameStats)
        {
            Debug.Log("DataConnector=> Print Data GameStats started.");

            data_GameStats.Data_AddDataToOutput(trackedData.Info_GetEndgameStats());
            data_GameStats.Data_BuildDataSetString();
            data_GameStats.File_WriteOutputToFile();
        }
    }

    public void PrintData_GameStats(bool _append)
    {
        if (trackedData != null && data_GameStats != null && settings.printGameStats)
        {
            Debug.Log("DataConnector=> Print Data GameStats started.");

            data_GameStats.Data_AddDataToOutput(trackedData.Info_GetEndgameStats());
            data_GameStats.Data_BuildDataSetString();
            data_GameStats.File_WriteOutputToFile(_append);
        }
    }

    public void PrintData_Decisions()
    {
        if (trackedData != null && data_Decisions != null && settings.printDecisions)
        {
            Debug.Log("DataConnector=> Print Data Decisions started.");

            data_Decisions.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Decisions());
            data_Decisions.Data_BuildDataSetString();
            data_Decisions.File_WriteOutputToFile();
        }
    }

    public void PrintData_Decisions(bool _append)
    {
        if (trackedData != null && data_Decisions != null && settings.printDecisions)
        {
            Debug.Log("DataConnector=> Print Data Decisions started.");

            data_Decisions.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Decisions());
            data_Decisions.Data_BuildDataSetString();
            data_Decisions.File_WriteOutputToFile(_append);
        }
    }

    public void PrintData_Team()
    {
        if (trackedData != null && data_Team != null && settings.printTeam)
        {
            Debug.Log("DataConnector=> Print Data Team started.");

            data_Team.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Team());
            data_Team.Data_BuildDataSetString();
            data_Team.File_WriteOutputToFile();
        }
    }

    public void PrintData_Team(bool _append)
    {
        if (trackedData != null && data_Team != null && settings.printTeam)
        {
            Debug.Log("DataConnector=> Print Data Team started.");

            data_Team.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Team());
            data_Team.Data_BuildDataSetString();
            data_Team.File_WriteOutputToFile(_append);
        }
    }

    public void PrintData_Device()
    {
        if (trackedData != null && data_Device != null && settings.printDevice)
        {
            Debug.Log("DataConnector=> Print Data Device started.");

            data_Device.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Device());
            data_Device.Data_BuildDataSetString();
            data_Device.File_WriteOutputToFile();
        }
    }

    public void PrintData_Device(bool _append)
    {
        if (trackedData != null && data_Device != null && settings.printDevice)
        {
            Debug.Log("DataConnector=> Print Data Device started.");

            data_Device.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Device());
            data_Device.Data_BuildDataSetString();
            data_Device.File_WriteOutputToFile(_append);
        }
    }

    public void PrintData_Questionnaire()
    {
        if (data_Questionnaire != null && settings.printQuestionnaire)
        {
            Debug.Log("DataConnector=> Print Data Questionnaire started.");

            data_Questionnaire.Data_BuildAndOutputToFile(true);

            //data_Device.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Device());
            //data_Device.Data_BuildDataSetString();
            //data_Device.File_WriteOutputToFile();
        }
    }

    public void PrintData_Questionnaire(bool _append)
    {
        if (data_Questionnaire != null && settings.printQuestionnaire)
        {
            Debug.Log("DataConnector=> Print Data Questionnaire started.");

            data_Questionnaire.Data_BuildAndOutputToFile(_append);

            //data_Device.Data_AddDataToOutput(trackedData.Info_GetFullPrintout_Device());
            //data_Device.Data_BuildDataSetString();
            //data_Device.File_WriteOutputToFile();
        }
    }

    public void Debug_CheckSettings()
    {
        if (debug.debug)
        {
            if (debug.debug_ApplyUserID)
            {
                if (settings.applyIdFromPrefs)
                {
                    if (PlayerPrefs.HasKey(settings.idKey))
                        SetUserID(PlayerPrefs.GetString(settings.idKey));
                }
                else
                    SetUserID(debug.debug_UserID);
                debug.debug_ApplyUserID = false;
            }

            if (debug.debug_printAll)
            {
                Print_AllData();
                debug.debug_printAll = false;
            }

            if (debug.debug_printTotal)
            {
                PrintData_Total();
                debug.debug_printTotal = false;
            }

            if (debug.debug_printGameStats)
            {
                PrintData_GameStats();
                debug.debug_printGameStats = false;
            }

            if (debug.debug_printDecisions)
            {
                PrintData_Decisions();
                debug.debug_printDecisions = false;
            }

            if (debug.debug_printTeam)
            {
                PrintData_Team();
                debug.debug_printTeam = false;
            }

            if (debug.debug_printDevice)
            {
                PrintData_Device();
                debug.debug_printDevice = false;
            }

            if (debug.debug_printQuestionnaire)
            {
                PrintData_Questionnaire();
                debug.debug_printQuestionnaire = false;
            }
        }
    }

}
