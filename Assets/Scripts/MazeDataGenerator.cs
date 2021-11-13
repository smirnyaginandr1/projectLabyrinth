using UnityEngine;
/*
 *  @brief: Класс генерации массива лабиринта
 */
public class MazeDataGenerator
{
    public float placementThreshold;

    public MazeDataGenerator()
    {
        placementThreshold = .1f;
    }

    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        var maze = new int[sizeRows, sizeCols];
        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        for (var i = 0; i <= rMax; i++)
        {
            for (var j = 0; j <= cMax; j++)
            {
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    maze[i, j] = 1;
                }

                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (!(Random.value > placementThreshold)) continue;
                    maze[i, j] = 1;

                    var a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                    var b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                    maze[i + a, j + b] = 1;
                }
            }
        }
        return maze;
    }
}