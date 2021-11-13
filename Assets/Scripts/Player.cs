using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefabBottom;
    [SerializeField] private GameObject playerPrefabTop;
    [SerializeField] private GameObject playerPrefabLeft;
    [SerializeField] private GameObject playerPrefabRight;

    public void CreatePlayer(int[,] maze)
    {
        var height = Camera.main.orthographicSize * 1.95f;
        var width = height / Screen.height * Screen.width;
    
        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        var heightCell = height / (rMax + 1);
        var widthCell = width / (cMax + 1);

        playerPrefabBottom.transform.localScale = new Vector3(widthCell + 0.5f, heightCell + 0.5f, 2);
        playerPrefabLeft.transform.localScale = new Vector3(widthCell + 0.5f, heightCell + 0.5f, 2);
        playerPrefabRight.transform.localScale = new Vector3(widthCell + 0.5f, heightCell + 0.5f, 2);
        playerPrefabTop.transform.localScale = new Vector3(widthCell + 0.5f, heightCell + 0.5f, 2);
        
        var firstCell = new Vector2(-width / 2 + widthCell / 2, height / 2 - heightCell / 2);

        
        for (var i = rMax; i >= 0; i--)
        {
            for (var j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    Instantiate(playerPrefabBottom, firstCell, Quaternion.identity);
                    return;
                }
                firstCell.x += widthCell;
            }

            firstCell.y -= heightCell;
            firstCell.x = -width / 2 + widthCell / 2;
        }
    }

    public void SetTopPlayer()
    {
        
    }

    public void SetBottomPlayer()
    {
        
    }

    public void SetLeftPlayer()
    {
        
    }

    public void SetRightPlayer()
    {
        
    }
}
