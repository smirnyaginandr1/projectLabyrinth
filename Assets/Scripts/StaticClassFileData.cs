using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class StaticClassFileData
{
    public static string fileName = "";

    public static int pointCount = 0;

    public static string path = "";

    public static bool init = false;

    public static bool currentState = false;

    public static Vector3 _lastGyro;

    public static Vector3 _lastAccel;

    public static int time = 0;

    public static string initialJson = "[{\"Count\":\"\",\"AccelXYZ\":\"\",\"GyroXYZ\":\"\"}]";

    public static JArray array;
}
