using UnityEngine;

/*
 * @brief: Класс создания лабиринта
 */

public class MazeConstructor : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;

    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        var dataGenerator = new MazeDataGenerator();
        var maze = dataGenerator.FromDimensions(sizeRows, sizeCols);

        if (Camera.main is null)
            return;
        
        var height = Camera.main.orthographicSize * 1.95f;
        var width = height / Screen.height * Screen.width;
        
        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        var heightCell = height / (rMax + 1);
        var widthCell = width / (cMax + 1);

        wallPrefab.transform.localScale = new Vector3(widthCell - 0.1f, heightCell - 0.1f, 2);
        
        var firstCell = new Vector2(-width / 2 + widthCell / 2, height / 2 - heightCell / 2);
        
        for (var i = rMax; i >= 0; i--)
        {
            for (var j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    //TODO: Добавить генерацию точек, которые надо собрать
                }
                else
                {
                    Instantiate(wallPrefab, firstCell, Quaternion.identity);
                }
                firstCell.x += widthCell;       
            }

            firstCell.y -= heightCell;
            firstCell.x = -width / 2 + widthCell / 2;
        }
    }

}