using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
//using HP.Omnicept.Unity;
//using HP.Omnicept.Messaging.Messages;


/// <summary>
/// Script that many other scrips call to to write data to the exported data files.
/// </summary>
public class dataGatherer : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static dataGatherer data;
    /// <summary>
    /// The filename for the exported data
    /// </summary>
    public string filename;
    /// <summary>
    /// Data about the scene information, included in the top level of the Event Data document
    /// </summary>
    [TextArea(5, 20)]
    public string sceneData;
    /// <summary>
    /// Data about actions during the program, included in the Event Data document
    /// </summary>
    [TextArea(5, 20)]
    public string actionData;
    /// <summary>
    /// Data about what was looked at during the program, included in the Event Data document
    /// </summary>
    [TextArea(5, 20)]
    public string lookData;
    //Physio Data split into two Strings for data streaming
    /// <summary>
    /// Data from the Omnicept "Glia Behavior script's eye and heart tracker, included in the Physio Data document
    /// </summary>
    [TextArea(5, 20)]
    public string physioLog1;
    [TextArea(5, 20)]
    public string physioLog2;
    /// <summary>
    /// Switches the current Physiolog being used
    /// </summary>
    private bool dataSwap = true;
    /// <summary>
    /// The transform of the Player Rig head
    /// </summary>
    public Transform playerHead;
    /// <summary>
    /// Amount of time before swapping physio data streams
    /// </summary>
    public float logTime;
    /// <summary>
    /// Timer counting up till swapping physio data streams
    /// </summary>
    private float logTimer;

    /// <summary>
    /// Heart Rate data
    /// </summary>
    private uint heartRate;

    //At the start of each scene, include the initial physio data heading and the scene data update.
    private void Start()
    {
        if (data == null)
            data = this;
        sceneData = "Entry Date: " + System.DateTime.Now + "." + "\n" + "Loaded Scene: " + SceneManager.GetActiveScene().name + " at " + Time.realtimeSinceStartup.ToString() + " global time. " + "\n";
        //sceneData = "\n" + "Loaded Scene " + SceneManager.GetActiveScene().name + " at " + Time.realtimeSinceStartup.ToString() + " global time." + "\n";
        physioLog1 = "\n" + "TS,HR,LGX,LGY,LGZ,LGC,LPD,LPDC,RGX,RGY,RGZ,RGC,RPD,RPDC,CGX,CGY,CGZ,CGC,PPX,PPY,PPZ,PRX,PRY,PRZ" + "\n";
    }

    ///// <summary>
    ///// Heart Rate recording sent from Omnicept's Glia Behavior script
    ///// </summary>
    ///// <param name="hr"></param>
    //public void updateHeartRate(HeartRate hr)
    //{
    //    heartRate = hr.Rate;
    //}

    ///// <summary>
    ///// String update for the Physio Data taken from the Omnicept's Glia Behavior script. Happens like every frame so it has two data streams to swap between constnatly.
    ///// </summary>
    ///// <param name="et"></param>
    //public void updatePhysioData(EyeTracking et)
    //{
    //    //Add the current data
    //    if (dataSwap)
    //        physioLog1 += Time.realtimeSinceStartup.ToString() + "," + heartRate.ToString() + ","
    //            + et.LeftEye.Gaze.X.ToString() + "," + et.LeftEye.Gaze.Y.ToString() + "," + et.LeftEye.Gaze.Z.ToString() + "," + et.LeftEye.Gaze.Confidence.ToString() + ","
    //            + et.LeftEye.PupilDilation.ToString() + "," + et.LeftEye.PupilDilationConfidence.ToString() + ","
    //            + et.RightEye.Gaze.X.ToString() + "," + et.RightEye.Gaze.Y.ToString() + "," + et.RightEye.Gaze.Z.ToString() + "," + et.RightEye.Gaze.Confidence.ToString() + ","
    //            + et.RightEye.PupilDilation.ToString() + "," + et.RightEye.PupilDilationConfidence.ToString() + ","
    //            + et.CombinedGaze.X.ToString() + "," + et.CombinedGaze.Y.ToString() + "," + et.CombinedGaze.Z.ToString() + "," + et.CombinedGaze.Confidence.ToString() + ","
    //            + playerHead.position.x.ToString() + "," + playerHead.position.y.ToString() + "," + playerHead.position.z.ToString() + ","
    //            + playerHead.eulerAngles.x.ToString() + "," + playerHead.eulerAngles.y.ToString() + "," + playerHead.eulerAngles.z.ToString() + "\n";
    //    else
    //        physioLog2 += Time.realtimeSinceStartup.ToString() + "," + heartRate.ToString() + ","
    //            + et.LeftEye.Gaze.X.ToString() + "," + et.LeftEye.Gaze.Y.ToString() + "," + et.LeftEye.Gaze.Z.ToString() + "," + et.LeftEye.Gaze.Confidence.ToString() + ","
    //            + et.LeftEye.PupilDilation.ToString() + "," + et.LeftEye.PupilDilationConfidence.ToString() + ","
    //            + et.RightEye.Gaze.X.ToString() + "," + et.RightEye.Gaze.Y.ToString() + "," + et.RightEye.Gaze.Z.ToString() + "," + et.RightEye.Gaze.Confidence.ToString() + ","
    //            + et.RightEye.PupilDilation.ToString() + "," + et.RightEye.PupilDilationConfidence.ToString() + ","
    //            + et.CombinedGaze.X.ToString() + "," + et.CombinedGaze.Y.ToString() + "," + et.CombinedGaze.Z.ToString() + "," + et.CombinedGaze.Confidence.ToString() + ","
    //            + playerHead.position.x.ToString() + "," + playerHead.position.y.ToString() + "," + playerHead.position.z.ToString() + ","
    //            + playerHead.eulerAngles.x.ToString() + "," + playerHead.eulerAngles.y.ToString() + "," + playerHead.eulerAngles.z.ToString() + "\n";
    //    //Add to the logTimer. If it passes the Log Time, begin to save the current Physio Data string and swap to the other one while the save occurs.
    //    logTimer += Time.deltaTime;
    //    if (logTimer >= logTime)
    //    {
    //        WritePhysString();
    //        logTimer = 0;
    //    }
    //}

    //When the scene changes or the application closes, write the data to the text files.
    private void OnDestroy()
    {
        WritePhysString();
        WriteEventString();
    }


    /// <summary>
    /// Function to save the physio data to an external text file
    /// </summary>
    public void WritePhysString()
    {
        //Name the file with the User ID and Physio
        filename = "_Physio.txt";
        //filename = globals.userID.ToString() + "_Physio.txt";

        //Write file path
        string path = Application.persistentDataPath + "/" + filename;

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        if (dataSwap)
        {
            writer.WriteLine(physioLog1);
            dataSwap = false;
            physioLog2 = "";
        } else
        {
            writer.WriteLine(physioLog2);
            dataSwap = true;
            physioLog1 = "";
        }
        writer.Close();
    }

    /// <summary>
    /// Function to save the event data to an external text file
    /// </summary>
    public void WriteEventString()
    {
        //Name the file with the User ID and Event
        filename = "_Event.txt";
        //filename = globals.userID.ToString() + "_Event.txt";
        sceneData += "Scene changed at " + Time.realtimeSinceStartup.ToString() + ". Total time in scene was " + Time.timeSinceLevelLoad.ToString() + "\n";

        //Write file path
        string path = Application.persistentDataPath + "/" + filename;

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("");
        writer.WriteLine(sceneData);
        writer.WriteLine(actionData);
        writer.WriteLine(lookData);
        writer.Close();
    }

}
