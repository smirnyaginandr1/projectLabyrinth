using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Singleton
{
    static float widthDisplay;
    static float heightDisplay;

    static Vector3 coordinateCenter;
    static Vector3 coordinateLB;
    static Vector3 coordinateLT;
    static Vector3 coordinateRT;
    static Vector3 coordinateRB;

    static Vector3 coordinateCurrent;

    public static void SetWidthDisplay(float w)
    {
        widthDisplay = w;
    }
    public static float GetWidthDisplay()
    {
        return widthDisplay;
    }



    public static void SetHeightDisplay(float w)
    {
        heightDisplay = w;
    }
    public static float GetHeightDisplay()
    {
        return heightDisplay;
    }



    public static void SetCoordinateLB(Vector3 coord)
    {
        coordinateLB = coord;
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
    //public static float widthDisplay
    //{
    //    get
    //    {
    //        return widthDisplay;
    //    }
    //    set
    //    {
    //        widthDisplay = value;
    //    }
    //}

    //public static float heightDisplay
    //{
    //    get
    //    {
    //        return heightDisplay;
    //    }
    //    set
    //    {
    //        widthDisplay = value;
    //    }
    //}

    //public static Vector3 coordinateRB
    //{
    //    get
    //    {
    //        return coordinateRB;
    //    }
    //    set
    //    {
    //        coordinateRB = value;
    //    }
    //}

    //public static Vector3 coordinateLT
    //{
    //    get
    //    {
    //        return coordinateLT;
    //    }
    //    set
    //    {
    //        coordinateLT = value;
    //    }
    //}

    //public static Vector3 coordinateRT
    //{
    //    get
    //    {
    //        return coordinateRT;
    //    }
    //    set
    //    {
    //        coordinateRT = value;
    //    }
    //}

    //public static Vector3 coordinateLB
    //{
    //    get
    //    {
    //        return coordinateLB;
    //    }
    //    set
    //    {
    //        coordinateLB = value;
    //    }
    //}

    //public static Vector3 current
    //{
    //    get
    //    {
    //        return current;
    //    }
    //    set
    //    {
    //        current = value;
    //    }
    //}

    //public static Vector3 coordinateCenter
    //{
    //    get
    //    {
    //        return coordinateCenter;
    //    }
    //    set
    //    {
    //        coordinateCenter = value;
    //    }
    //}
}
