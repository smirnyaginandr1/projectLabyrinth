using UnityEngine;
/*
 *@brief: Класс игрового контроллера. Запускает создание игровых элементов
 */
[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    private MazeConstructor generator;

    [SerializeField] private GameObject controller;
    private Player _player;
    
    private int[,] maze;

    private float height, width;
    
    [SerializeField] private GameObject background;

    private void Start()
    {
        _player = controller.GetComponent<Player>();
        
        height = Camera.main.orthographicSize * 1.95f;
        width = height / Screen.height * Screen.width;

        var percent = height / width;

        var widthCount = (int)(width);
        var heightCount = (int)(widthCount * percent);
        
        generator = GetComponent<MazeConstructor>();
        maze = generator.GenerateNewMaze(heightCount, widthCount);

        _player.CreatePlayer(maze);

        background.transform.localScale = new Vector3(widthCount, heightCount, 1);
    }
}