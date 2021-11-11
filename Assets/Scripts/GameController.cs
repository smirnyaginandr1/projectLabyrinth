using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *@brief: Класс игрового контроллера. Запускает создание игровых элементов
 */
[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    private MazeConstructor generator;

    private int[,] maze;

    private float height, width;
    
    [SerializeField] private GameObject background;
    
    void Start()
    {
        height = Camera.main.orthographicSize * 1.95f;
        width = height / Screen.height * Screen.width;

        var percent = height / width;

        int widthCount = (int)(width);
        var heightCount = (int)(widthCount * percent);
        
        generator = GetComponent<MazeConstructor>();
        maze = generator.GenerateNewMaze(heightCount, widthCount);

        background.transform.localScale = new Vector3(widthCount, heightCount, 1);
    }
}