using UnityEngine;

/*
 * @brief: Класс создания лабиринта и объектов
 */

public class Constructor : MonoBehaviour
{
    [SerializeField] GameObject pointPrefab;
    
    [SerializeField] GameObject controller;

    [SerializeField] GameObject player;

    int _pointCount = 0;
    int _angle;
    GameObject _obj;
    
    const int Wall0 = 0;
    const int Wall1 = 1;
    const int Wall2 = 2;
    const int Wall2Angle = 3;
    const int Wall3 = 4;
    const int Wall4 = 5;
    
    [SerializeField] GameObject[] allWall;

    GameController _gameController;
    Player _player;

    GameObject _finishWall;

    struct CheckWall
    {
        public bool top;
        public bool bottom;
        public bool left;
        public bool right;
    }


    public int[,] GenerateNewMaze(int sizeRows, int sizeCols)
    {
        _gameController = controller.GetComponent<GameController>();
        _player = player.GetComponent<Player>();

        var helpCheckWall = new CheckWall();

        var dataGenerator = new MazeDataGenerator();
        var maze = dataGenerator.FromDimensions(sizeRows, sizeCols);

        if (Camera.main is null)
            return null;

        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        var heightCell = Singleton.GetHeightDisplay() / (rMax + 1);
        var widthCell = Singleton.GetWidthDisplay() / (cMax + 1);
        
        foreach (var t in allWall)
            t.transform.localScale = new Vector3(widthCell - 0.5f, heightCell - 0.5f , 2);
        
        pointPrefab.transform.localScale = new Vector3(widthCell - 0.8f, heightCell - 0.8f, 2);
        pointPrefab.SetActive(true);
        var firstCell = new Vector2(-Singleton.GetWidthDisplay() / 2 + widthCell / 2, Singleton.GetHeightDisplay() / 2 - heightCell / 2);

        for (var i = rMax; i >= 0; i--)
        {
            for (var j = 0; j <= cMax; j++)
            {
                if (i == 1 && j == 1)
                {
                    _player.CreatePlayer(widthCell, heightCell, firstCell);
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
                
                if (i == 0 && j == 1)
                {
                    _finishWall = Instantiate(_obj, firstCell, Quaternion.identity);
                        _finishWall.transform.rotation =
                    Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + _angle));
                   // _gameController.SetFinishWall(_finishWall);
                    

                }
                else 
                    Instantiate(_obj, firstCell, Quaternion.identity)
                        .transform.rotation =
                    Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + _angle));
                firstCell.x += widthCell;
            }

            firstCell.y -= heightCell;
            firstCell.x = -Singleton.GetWidthDisplay() / 2 + widthCell / 2;
        }
        
        _gameController.maxPoint = _pointCount;
        return maze;
    }

    void SetWallAngle(GameObject wall, int angle)
    {
        _obj = wall;
        _angle = angle;
    }

    public GameObject GetFinishWall()
    {
        return _finishWall;
    }
}