using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;

public static class StaticClass
{
    private static  WriteCoroutine writeScript;

    public enum GameScene{
        SceneLabyrinth,
        SceneTarget,
        ScenePingPong
    };
    public static void InitJSON(string fileName, GameObject obj)
    {
        writeScript = obj.AddComponent<WriteCoroutine>();

        writeScript.InitializeJSON(fileName);
    }

    public static void FinalCreateFileJSON()
    {
        writeScript.FinalCreateJSON();
    }

    public static void StartOrStopWriteInFile(bool start)
    {
        writeScript.StartOrStopWrite(start);
    }

    public static int GetTime()
    {
        return writeScript.GetTime() / 10;
    }

    public static void SetValue(Vector3 gyro, Vector3 accel)
    {
        writeScript.SetValue(gyro, accel);
    }

    static bool firstWaitScene = false;

    public static void SetFirstWaitScene(bool t)
    {
        firstWaitScene = t;
    }

    public static bool GetFirstWaitScene()
    {
        return firstWaitScene;
    }



    static GameScene currentScene = GameScene.SceneLabyrinth;

    

    public static void SetCurrentScene(GameScene value)
    {
        switch (value)
        {
            case GameScene.SceneLabyrinth:
            case GameScene.ScenePingPong:
            case GameScene.SceneTarget:
                currentScene = value;
                break;
        }
    }

    public static GameScene GetCurrentScene()
    {
        return currentScene;
    }

    //Поля и методы для работы с лабиринтом


    static bool firstRun = true;

    public static bool GetFirstRun()
    {
        if (firstRun)
        {
            firstRun = false;
            return true;
        }
        else
            return firstRun;
    }

    public static void InitFirstRun()
    {
        firstRun = true;
    }

    static int timerValue = 0;

    public static void SetTimerValue(int val)
    {
        timerValue = val;
    }

    public static int GetTimerValue()
    {
        return timerValue;
    }

    static int pointCount = 0;
    public static void SetPointCount(int val)
    {
        pointCount = val;
    }
    
    public static int GetPointCount()
    {
        return pointCount;
    }
}
