using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteToPHP_ThredGame : MonoBehaviour
{
    [SerializeField] bool useFilePath;
    [SerializeField] string filePath;
    string path;
    [Space(10)]
    [SerializeField] bool includeInfo_Total;
    [SerializeField] string info_Total;
    [Space(10)]
    [SerializeField] bool includeInfo_Responses;
    [SerializeField] string info_Responses;
    [Space(10)]
    [SerializeField] bool includeInfo_GameStats;
    [SerializeField] string info_GameStats;
    //[Space(10)]
    //[SerializeField] bool includeInfo_Decisions;
    //[SerializeField] string info_Decisions;
    //[Space(10)]
    //[SerializeField] bool includeInfo_Team;
    //[SerializeField] string info_Team;
    //[Space(10)]
    //[SerializeField] bool includeInfo_Device;
    //[SerializeField] string info_Device;

    public string FilePath { get=>filePath; set { SetFilePath(value); } }

    private void Awake()
    {
        SetFilePath();
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void SetFilePath()
    {
        if (useFilePath)
            path = filePath;
        else
            path = "http://localhost:9000/fromunity.php";
    }

    public void SetFilePath(string _path)
    {
        filePath = _path;

        if (useFilePath)
            path = filePath;
        else
            path = "http://localhost:9000/fromunity.php";
    }

    public void SetInfo_Total(string _info)
    {
        if (includeInfo_Total)
            info_Total = _info;
        else
            info_Total = "";
    }

    public void SetInfo_Responses(string _info)
    {
        if (includeInfo_Responses)
            info_Responses = _info;
        else
            info_Responses = "";
    }

    public void SetInfo_GameStats(string _info)
    {
        if (includeInfo_GameStats)
            info_GameStats = _info;
        else
            info_GameStats = "";
    }

    //public void SetInfo_Decisions(string _info)
    //{
    //    if (includeInfo_Decisions)
    //        info_Decisions = _info;
    //    else
    //        info_Decisions = "";
    //}

    //public void SetInfo_Team(string _info)
    //{
    //    if (includeInfo_Team)
    //        info_Team = _info;
    //    else
    //        info_Team = "";
    //}

    //public void SetInfo_Device(string _info)
    //{
    //    if (includeInfo_Device)
    //        info_Device = _info;
    //    else
    //        info_Device = "";
    //}

    public void Send_TextToFile()
    {
        StartCoroutine(sendTextToFile());
    }

    IEnumerator sendTextToFile()
    {
        Debug.Log("SQL Data Coroutine Started! Output to filepath: " + path);

        bool successful = true;

        WWWForm form = new WWWForm();
        form.AddField("info_Total", info_Total);
        form.AddField("info_Responses", info_Responses);
        form.AddField("info_GameStats", info_GameStats);
        //form.AddField("info_Decisions", info_Decisions);
        //form.AddField("info_Team", info_Team);
        //form.AddField("info_Device", info_Device);
        WWW www = new WWW(path, form);
        //WWW www = new WWW("http://localhost:9000/fromunity.php", form);

        yield return www;
        if (www.error != null)
        {
            Debug.Log("SQL=> No return error yet!");
            successful = false;
        }
        else
        {
            Debug.Log(www.text);
            successful = true;
        }
    }

}
