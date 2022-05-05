using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class WriteCoroutine : MonoBehaviour
{
    void Start() { }
    private void Update()
    {
        
    }

    public void InitializeJSON(string name)
    {
        if (StaticClassFileData.init)
            return;

        Debug.Log("—“¿–“ «¿œ»—»");

        StaticClassFileData.array = JArray.Parse(StaticClassFileData.initialJson);

        StaticClassFileData.fileName = name;

        StaticClassFileData.pointCount = 0;

        StaticClassFileData.init = true;
    }
    
    public void StartOrStopWrite(bool start)
    {
        if (!StaticClassFileData.init)
            return;
        StaticClassFileData.currentState = start;
        StaticClassFileData.time = 0;
        if (start)
            StartCoroutine(WriteInFileCoroutine());
        else
            StopCoroutine(WriteInFileCoroutine());
    }

    public int GetTime()
    {
        return StaticClassFileData.time;
    }

    public void FinalCreateJSON()
    {
        Debug.Log(" ŒÕ≈÷ «¿œ»—»");
        if (!StaticClassFileData.init)
            return;
        StaticClassFileData.init = false;
        string date = DateTime.Now.ToString();
        date = date.Replace(' ', '_');
        date = date.Replace('.', '_');
        date = date.Replace(':', '_');
        date = date.Replace('/', '_');
        StaticClassFileData.fileName = date + "_" + StaticClassFileData.fileName;
        StaticClassFileData.path = Path.Combine(Application.persistentDataPath, StaticClassFileData.fileName + ".json");
        var jsonToOutput = JsonConvert.SerializeObject(StaticClassFileData.array, Formatting.Indented);
        File.WriteAllText(StaticClassFileData.path, jsonToOutput);

        Debug.Log(StaticClassFileData.path);
        Debug.Log(jsonToOutput);

        StaticClassFileData.path = "";
        StaticClassFileData.array.Clear();
    }

    public void SetValue(Vector3 gyro, Vector3 accel)
    {
        if (!StaticClassFileData.init)
            return;
        StaticClassFileData._lastGyro = gyro;
        StaticClassFileData._lastAccel = accel;
    }

    IEnumerator WriteInFileCoroutine()
    {
        StaticClassFileData.time++;
        var itemToAdd = new JObject();
        string strGyro = (StaticClassFileData._lastGyro.x).ToString() + " " + 
                         (StaticClassFileData._lastGyro.y).ToString() + " " + 
                         (StaticClassFileData._lastGyro.z).ToString();
        
        string strAccel = (StaticClassFileData._lastAccel.x).ToString() + " " + 
                          (StaticClassFileData._lastAccel.y).ToString() + " " + 
                          (StaticClassFileData._lastAccel.z).ToString();

        itemToAdd["Count"] = StaticClassFileData.pointCount++;
        itemToAdd["AccelXYZ"] = strAccel;
        itemToAdd["GyroXYZ"] = strGyro;

        StaticClassFileData.array.Add(itemToAdd);

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(WriteInFileCoroutine());
    }
}
