using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteToFile_Test : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Send_TextToFile()
    {
        StartCoroutine(sendTextToFile());
    }

    IEnumerator sendTextToFile()
    {
        bool successful = true;

        WWWForm form = new WWWForm();
        form.AddField("name", "Lorem Ipsum");
        form.AddField("age", "32");
        form.AddField("score", "125");
        WWW www = new WWW("http://localhost:9000/fromunity.php", form);

        yield return www;
        if(www.error != null)
        {
            successful = false;
        }
        else
        {
            Debug.Log(www.text);
            successful = true;
        }
    }

}
