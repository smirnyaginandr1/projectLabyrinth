using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 *@brief: Класс игрового контроллера. Запускает создание игровых элементов
 */
[RequireComponent(typeof(Constructor))]

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _text;

    [SerializeField] private GameObject _button;
    
    private Constructor _generator;

    private int _currentPoint;

    private int _maxPoint;
    
    private int[,] _maze;

    private float _height, _width;
    
    [SerializeField] private GameObject background;

    private void Start()
    {
        
        _text.SetActive(false);
        _button.SetActive(false);
        _height = Camera.main.orthographicSize * 1.95f;
        _width = _height / Screen.height * Screen.width;

        var percent = _height / _width;

        var widthCount = (int)(_width);
        var heightCount = (int)(widthCount * percent);
        
        _generator = GetComponent<Constructor>();
        _maze = _generator.GenerateNewMaze(heightCount, widthCount);



        background.transform.localScale = new Vector3(widthCount, heightCount, 1);
    }

    public void AddCurrentPoint()
    {
        _currentPoint++;
    }
    
    public void SetMaxPoint(int point)
    {
        _maxPoint = point;
    }
    
    public int GetCurrentPoint()
    {
        return _currentPoint;
    }

    public int GetMaxPoint()
    {
        return _maxPoint;
    }

    public void Finish()
    {
        _text.SetActive(true);
        _button.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}