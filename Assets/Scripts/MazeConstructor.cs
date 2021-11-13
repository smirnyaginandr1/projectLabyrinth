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


    private int _angle = 0;
    private GameObject _obj;
    
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
        var helpCheckWall = new CheckWall();

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
                    firstCell.x += widthCell;
                    continue;
                    //SetWallAngle(pointPrefab, 0);
                }

                if (i == 0 || i == rMax || j == 0 || j == cMax)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                            SetWallAngle(allWall[WALL_2_ANGLE], 0);
                        else if (j == cMax)
                            SetWallAngle(allWall[WALL_2_ANGLE], 90);
                        else
                        {
                            if (maze[i + 1, j] != 0)
                                SetWallAngle(allWall[WALL_3], 0);
                            else
                                SetWallAngle(allWall[WALL_2], 90);
                        }
                    }
                    else if (i == rMax)
                    {
                        if (j == 0)
                            SetWallAngle(allWall[WALL_2_ANGLE], 270);
                        else if (j == cMax)
                            SetWallAngle(allWall[WALL_2_ANGLE], 180);
                        else
                        {
                            if (maze[i - 1, j] != 0)
                                SetWallAngle(allWall[WALL_3], 180);
                            else
                                SetWallAngle(allWall[WALL_2], 90);
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            if (maze[i, j + 1] != 0)
                                SetWallAngle(allWall[WALL_3], 270);
                            else 
                                SetWallAngle(allWall[WALL_2], 0);
                        }
                        else if (j == cMax)
                        {
                            if (maze[i, j - 1] != 0)
                                SetWallAngle(allWall[WALL_3], 90);
                            else
                                SetWallAngle(allWall[WALL_2], 0);
                        }
                        else
                            SetWallAngle(allWall[WALL_2], 0);
                    }
                }
                else
                {
                    helpCheckWall.bottom = false;
                    helpCheckWall.top = false;
                    helpCheckWall.left = false;
                    helpCheckWall.right = false;
                    var state = 4;
                    if (maze[i - 1, j] == 0)
                    {
                        helpCheckWall.bottom = true;
                        state--;
                    }

                    if (maze[i + 1, j] == 0)
                    {
                        helpCheckWall.top = true;
                        state--;
                    }
                        
                    if (maze[i, j + 1] == 0)
                    {
                        helpCheckWall.right = true;
                        state--;
                    }
                        
                    if (maze[i, j - 1] == 0)
                    {
                        helpCheckWall.left = true;
                        state--;
                    }
                        
                    switch (state)
                    {
                        case 0:
                            SetWallAngle(allWall[WALL_0], 0);
                            break;
                            
                        case 1:
                            if (!helpCheckWall.left)
                                SetWallAngle(allWall[WALL_1], 90);
                            else if (!helpCheckWall.right)
                                SetWallAngle(allWall[WALL_1], 270);
                            else if (!helpCheckWall.bottom)
                                SetWallAngle(allWall[WALL_1], 180);
                            else 
                                SetWallAngle(allWall[WALL_1], 0);
                            break;
                            
                        case 2:
                            if (!helpCheckWall.left)
                            {
                                if (!helpCheckWall.right)
                                    SetWallAngle(allWall[WALL_2], 90);
                                else if (!helpCheckWall.top)
                                    SetWallAngle(allWall[WALL_2_ANGLE], 90);
                                else
                                    SetWallAngle(allWall[WALL_2_ANGLE], 180);
                            }
                            else if(!helpCheckWall.top)
                            {
                                SetWallAngle(!helpCheckWall.bottom ? allWall[WALL_2] : allWall[WALL_2_ANGLE], 0);
                            }
                            else
                                SetWallAngle(allWall[WALL_2_ANGLE], 270);
                            break;
                                
                        case 3:
                            if (helpCheckWall.top)
                                SetWallAngle(allWall[WALL_3], 180);
                            else if (helpCheckWall.bottom)
                                SetWallAngle(allWall[WALL_3], 0);
                            else if (helpCheckWall.right)
                                SetWallAngle(allWall[WALL_3], 90);
                            else
                                SetWallAngle(allWall[WALL_3], 270);
                            break;
                                
                        case 4:
                            SetWallAngle(allWall[WALL_4], 0);
                            break;
                        default:
                            SetWallAngle(allWall[WALL_0], 0);
                            break;
                    }

                }
                Instantiate(_obj, firstCell, Quaternion.identity)
                        .transform.rotation =
                    Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + _angle));
                firstCell.x += widthCell;
            }

            firstCell.y -= heightCell;
            firstCell.x = -width / 2 + widthCell / 2;
        }
        return maze;
    }

    private void SetWallAngle(GameObject wall, int angle)
    {
        _obj = wall;
        _angle = angle;
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
}