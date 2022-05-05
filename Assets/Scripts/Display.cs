using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Display
{
    static float widthDisplay;
    static float heightDisplay;

    public static bool debug = true;

    static Vector3 coordinateCenter;
    static Vector3 coordinateLB;
    static Vector3 coordinateLT;
    static Vector3 coordinateRT;
    static Vector3 coordinateRB;

    static Vector3 coordinateCurrent;

    public static void SetWidthDisplay(float w)
    {
        widthDisplay = w / 2;
    }


    public static float GetWidthDisplay()
    {
        if (debug)
            return Camera.main.orthographicSize * 1.95f / Screen.height * Screen.width / 2;
        return widthDisplay;
    }


    public static void SetHeightDisplay(float w)
    {
        heightDisplay = w;
    }


    public static float GetHeightDisplay()
    {
        if (debug)
            return Camera.main.orthographicSize * 1.95f;
        return heightDisplay;
    }


    public static void SetCoordinateLB(Vector3 coord)
    {
        coordinateLB = coord;
    }

    public static float GetWidthDisplay2()
    {
        if (debug)
            return (Camera.main.orthographicSize * 1.95f / Screen.height * Screen.width) / 2;
        return widthDisplay;
    }

    public static Vector3 GetCoordinateLB()
    {
        return coordinateLB;
    }


    public static void SetCoordinateRB(Vector3 coord)
    {
        coordinateRB = coord;
    }


    public static Vector3 GetCoordinateRB()
    {
        return coordinateRB;
    }


    public static void SetCoordinateLT(Vector3 coord)
    {
        coordinateLT = coord;
    }


    public static Vector3 GetCoordinateLT()
    {
        return coordinateLT;
    }

    public static void SetCoordinateRT(Vector3 coord)
    {
        coordinateRT = coord;
    }


    public static Vector3 GetCoordinateRT()
    {
        return coordinateRT;
    }


    public static void SetCoordinateCenter(Vector3 coord)
    {
        coordinateCenter = coord;
    }
    

    public static Vector3 GetCoordinateCenter()
    {
        return coordinateCenter;
    }


    public static void SetCoordinateCurrent(Vector3 coord)
    {
        coordinateCurrent = coord;
    }
    
    
    public static Vector3 GetCoordinateCurrent()
    {
        return coordinateCurrent;
    }
}
