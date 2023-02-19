using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Text;
using UnityEngine;

public class LogManager : MonoBehaviour
{

    string path;

    StreamWriter sw;

    public int roundNumber = 1;

    public static LogManager instance;

    void Awake()
    {
        // singleton LogManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // already have LogManager in scene, don't need another one
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);  // LogManager persists between scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        string now = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
        string filename = now + "_round-" + roundNumber.ToString();

        path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RobotRampage_RoundLogs\" + filename + ".txt";

        if (!File.Exists(path))
        {
            //Debug.Log("file at  " + path + "  does not exist");
            path = @".\RobotRampage_RoundLogs\" + filename + ".txt";
        }

        sw = File.CreateText(path);

        Debug.Log("LogManager Start() called");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void writeLog(string log)
    {
        File.AppendAllText(path, log);
    }
}
