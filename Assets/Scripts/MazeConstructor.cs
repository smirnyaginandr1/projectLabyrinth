using System;
using UnityEngine;

/*
 * @brief: Класс создания лабиринта
 */

public class MazeConstructor : MonoBehaviour
{
    [SerializeField] private GameObject wall0Prefab;
    [SerializeField] private GameObject wall1Prefab;
    [SerializeField] private GameObject wall2Prefab;
    [SerializeField] private GameObject wall2AnglePrefab;
    [SerializeField] private GameObject wall3Prefab;
    [SerializeField] private GameObject wall4Prefab;
    [SerializeField] private GameObject pointPrefab;


    private const int WALL_0 = 0;

    private const int WALL_1 = 1;
    private const int WALL_2 = 2;
    private const int WALL_2_ANGLE = 3;
    private const int WALL_3 = 4;
    private const int WALL_4 = 5;
    
    private GameObject[] allWall;

    private struct CheckWall
    {
        public bool top;
        public bool bottom;
        public bool left;
        public bool right;
    }


    public int[,] GenerateNewMaze(int sizeRows, int sizeCols)
    {
        CheckWall helpCheckWall = new CheckWall();
        HelpCheckWallInit(helpCheckWall);
        
        var dataGenerator = new MazeDataGenerator();
        var maze = dataGenerator.FromDimensions(sizeRows, sizeCols);

        if (Camera.main is null)
            return null;
        
        var height = Camera.main.orthographicSize * 1.95f;
        var width = height / Screen.height * Screen.width;
        
        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        var heightCell = height / (rMax + 1);
        var widthCell = width / (cMax + 1);

        InitWall(widthCell, heightCell);
        
        pointPrefab.transform.localScale = new Vector3(widthCell - 0.5f, heightCell - 0.5f, 2);
        
        var firstCell = new Vector2(-width / 2 + widthCell / 2, height / 2 - heightCell / 2);
        
        for (var i = rMax; i >= 0; i--)
        {
            for (var j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    Instantiate(pointPrefab, firstCell, Quaternion.identity);
                }
                else
                {
                    if (i == 0 || i == rMax || j == 0 || j == cMax)
                    {
                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                Instantiate(allWall[WALL_2_ANGLE], firstCell, Quaternion.identity);
                            }
                            else if (j == cMax)
                            {
                                Instantiate(allWall[WALL_2_ANGLE], firstCell, Quaternion.identity)
                                    .transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90));

                            }
                            else
                            {
                                if (maze[i + 1, j] != 0)
                                    Instantiate(allWall[WALL_3], firstCell, Quaternion.identity);
                                else
                                    Instantiate(allWall[WALL_2], firstCell, Quaternion.identity)
                                            .transform.rotation =
                                        Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90));
                            }
                        }
                        else if (i == rMax)
                        {
                            if (j == 0)
                            {
                                Instantiate(allWall[WALL_2_ANGLE], firstCell, Quaternion.identity)
                                        .transform.rotation =
                                    Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z - 90));
                            }
                            else if (j == cMax)
                            {
                                Instantiate(allWall[WALL_2_ANGLE], firstCell, Quaternion.identity)
                                        .transform.rotation =
                                    Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 180));

                            }
                            else
                            {
                                if (maze[i - 1, j] != 0)
                                    Instantiate(allWall[WALL_3], firstCell, Quaternion.identity)
                                            .transform.rotation =
                                        Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 180));
                                else
                                    Instantiate(allWall[WALL_2], firstCell, Quaternion.identity)
                                        .transform.rotation =
                                    Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90));
                            }
                        }
                        else
                        {
                            if (j == 0)
                            {
                                if (maze[i, j + 1] != 0)
                                    Instantiate(allWall[WALL_3], firstCell, Quaternion.identity)
                                            .transform.rotation =
                                        Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z - 90));
                                else
                                    Instantiate(allWall[WALL_2], firstCell, Quaternion.identity);
                            }

                            if (j == cMax)
                            {
                                if (maze[i, j - 1] != 0)
                                    Instantiate(allWall[WALL_3], firstCell, Quaternion.identity)
                                            .transform.rotation =
                                        Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90));
                                else 
                                    Instantiate(allWall[WALL_2], firstCell, Quaternion.identity);
                                
                            }
                            else
                                Instantiate(allWall[WALL_2], firstCell, Quaternion.identity);

                        }
                            
                    }
                    else
                    {
                        HelpCheckWallInit(helpCheckWall);
                        int state = 0;
                        if (maze[i - 1, j] != 0)
                        {
                            helpCheckWall.bottom = true;
                            state++;
                        }

                        if (maze[i + 1, j] != 0)
                        {
                            helpCheckWall.top = true;
                            state++;
                        }
                        
                        if (maze[i, j + 1] != 0)
                        {
                            helpCheckWall.right = true;
                            state++;
                        }
                        
                        if (maze[i, j - 1] != 0)
                        {
                            helpCheckWall.left = true;
                            state++;
                        }
                        
                        var angle = 0;
                        
                        switch (state)
                        {
                            case 0:
                                Instantiate(allWall[WALL_0], firstCell, Quaternion.identity);
                                break;
                            
                            case 1:
                                if (helpCheckWall.left)
                                    angle = 90;
                                else if (helpCheckWall.right)
                                    angle = -90;
                                else if (helpCheckWall.bottom)
                                    angle = 180;
                                
                                Instantiate(allWall[WALL_1], firstCell, Quaternion.identity)
                                        .transform.rotation =
                                    Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + angle));
                                break;
                            
                            case 2:
                                
                                break;
                                
                            case 3:
                                if (!helpCheckWall.top)
                                    angle = 90;
                                else if (!helpCheckWall.bottom)
                                    angle = 180;
                                else if (!helpCheckWall.right)
                                    angle = -90;
                                
                                Instantiate(allWall[WALL_3], firstCell, Quaternion.identity)
                                        .transform.rotation =
                                    Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + angle));
                                break;
                                
                            case 4:
                                Instantiate(allWall[WALL_4], firstCell, Quaternion.identity);
                                break;
                        }

                    }
                    
                }
                // Instantiate(maze[i, j] == 0 ? pointPrefab : wallPrefab, firstCell, Quaternion.identity);
                firstCell.x += widthCell;
            }

            firstCell.y -= heightCell;
            firstCell.x = -width / 2 + widthCell / 2;
        }
        return maze;
    }

    private void InitWall(float widthCell, float heightCell)
    {
        allWall = new GameObject[6];
        allWall[0] = wall0Prefab;
        allWall[1] = wall1Prefab;
        allWall[2] = wall2Prefab;
        allWall[3] = wall2AnglePrefab;
        allWall[4] = wall3Prefab;
        allWall[5] = wall4Prefab;
        
        foreach (var t in allWall)
            t.transform.localScale = new Vector3(widthCell - 0.5f, heightCell - 0.5f , 2);
    }

    private void HelpCheckWallInit(CheckWall check)
    {
        check.bottom = false;
        check.top = false;
        check.right = false;
        check.left = false;
    }
    
    private void CheckFigure(int posX, int posY, int[,] maze, Vector3 firstCell)
    {
        if (maze[posX, posY] == 0)
        {
            Instantiate(pointPrefab, firstCell, Quaternion.identity);
            return;
        }
    }
    
}