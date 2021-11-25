using UnityEngine;

/*
 * @brief: Класс создания лабиринта и объектов
 */

public class Constructor : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    
    [SerializeField] private GameObject controller;

    private int _pointCount = 0;
    private int _angle;
    private GameObject _obj;
    
    private const int Wall0 = 0;
    private const int Wall1 = 1;
    private const int Wall2 = 2;
    private const int Wall2Angle = 3;
    private const int Wall3 = 4;
    private const int Wall4 = 5;
    
    [SerializeField] private GameObject[] allWall;

    private GameController _gameController;
    private Player _player;
    private struct CheckWall
    {
        public bool top;
        public bool bottom;
        public bool left;
        public bool right;
    }


    public int[,] GenerateNewMaze(int sizeRows, int sizeCols)
    {
        _gameController = controller.GetComponent<GameController>();
        _player = controller.GetComponent<Player>();

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
        
        foreach (var t in allWall)
            t.transform.localScale = new Vector3(widthCell - 0.5f, heightCell - 0.5f , 2);
        
        pointPrefab.transform.localScale = new Vector3(widthCell - 0.8f, heightCell - 0.8f, 2);
        pointPrefab.SetActive(true);
        var firstCell = new Vector2(-width / 2 + widthCell / 2, height / 2 - heightCell / 2);

        for (var i = rMax; i >= 0; i--)
        {
            for (var j = 0; j <= cMax; j++)
            {
                if (i == 1 && j == 1)
                {
                    _player.CreatePlayer(maze, widthCell, heightCell, firstCell);
                    firstCell.x += widthCell;
                    continue;
                }
                    
                if (maze[i, j] == 0)
                {
                    _pointCount++;
                    firstCell.x += widthCell;
                    Instantiate(pointPrefab, new Vector2(firstCell.x - widthCell, firstCell.y), Quaternion.identity);
                    continue;
                }

                if (i == 0 || i == rMax || j == 0 || j == cMax)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                            SetWallAngle(allWall[Wall2Angle], 0);
                        else if (j == cMax)
                            SetWallAngle(allWall[Wall2Angle], 90);
                        else
                        {
                            if (maze[i + 1, j] != 0)
                                SetWallAngle(allWall[Wall3], 0);
                            else
                                SetWallAngle(allWall[Wall2], 90);
                        }
                    }
                    else if (i == rMax)
                    {
                        if (j == 0)
                            SetWallAngle(allWall[Wall2Angle], 270);
                        else if (j == cMax)
                            SetWallAngle(allWall[Wall2Angle], 180);
                        else
                        {
                            if (maze[i - 1, j] != 0)
                                SetWallAngle(allWall[Wall3], 180);
                            else
                                SetWallAngle(allWall[Wall2], 90);
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            if (maze[i, j + 1] != 0)
                                SetWallAngle(allWall[Wall3], 270);
                            else 
                                SetWallAngle(allWall[Wall2], 0);
                        }
                        else if (j == cMax)
                        {
                            if (maze[i, j - 1] != 0)
                                SetWallAngle(allWall[Wall3], 90);
                            else
                                SetWallAngle(allWall[Wall2], 0);
                        }
                        else
                            SetWallAngle(allWall[Wall2], 0);
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
                            SetWallAngle(allWall[Wall0], 0);
                            break;

                        case 1:
                            if (!helpCheckWall.left)
                                SetWallAngle(allWall[Wall1], 90);
                            else if (!helpCheckWall.right)
                                SetWallAngle(allWall[Wall1], 270);
                            else if (!helpCheckWall.bottom)
                                SetWallAngle(allWall[Wall1], 180);
                            else
                                SetWallAngle(allWall[Wall1], 0);
                            break;

                        case 2:
                            if (!helpCheckWall.left)
                            {
                                if (!helpCheckWall.right)
                                    SetWallAngle(allWall[Wall2], 90);
                                else if (!helpCheckWall.top)
                                    SetWallAngle(allWall[Wall2Angle], 90);
                                else
                                    SetWallAngle(allWall[Wall2Angle], 180);
                            }
                            else if (!helpCheckWall.top)
                            {
                                SetWallAngle(!helpCheckWall.bottom ? allWall[Wall2] : allWall[Wall2Angle], 0);
                            }
                            else
                                SetWallAngle(allWall[Wall2Angle], 270);

                            break;

                        case 3:
                            if (helpCheckWall.top)
                                SetWallAngle(allWall[Wall3], 180);
                            else if (helpCheckWall.bottom)
                                SetWallAngle(allWall[Wall3], 0);
                            else if (helpCheckWall.right)
                                SetWallAngle(allWall[Wall3], 90);
                            else
                                SetWallAngle(allWall[Wall3], 270);
                            break;

                        case 4:
                            SetWallAngle(allWall[Wall4], 0);
                            break;
                        default:
                            SetWallAngle(allWall[Wall0], 0);
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
        
        _gameController.SetMaxPoint(_pointCount);
        return maze;
    }

    private void SetWallAngle(GameObject wall, int angle)
    {
        _obj = wall;
        _angle = angle;
    }
}