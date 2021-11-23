using UnityEngine;

public class Player : MonoBehaviour
{
    //Аниматор для запуска анимаций
    private Animator _anim;
    
    //Флаг запущенной анимации
    private bool _animationRun;
    
    //Скорость игрока
    private const float Speed = 0.01f;
    
    //Текущее направление игрока
    private int _currentDirection = 1;
    
    //Флаг ожидания инициализации
    private bool _start;
    
    //Префабы игрока
    [SerializeField] private GameObject[] playerPrefab;

    //Текущий префаб
    private GameObject _currentObject;
    public void CreatePlayer(int[,] maze)
    {
        if (Camera.main is null) return;
        
        //Получение длины и ширины экрана
        var height = Camera.main.orthographicSize * 1.95f;
        var width = height / Screen.height * Screen.width;
    
        //Получение количества строк и столбцов лабиринта
        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        //Вычисление длины и ширины одной ячейки
        var heightCell = height / (rMax + 1);
        var widthCell = width / (cMax + 1);

        //Смена размеров префабов игрока
        foreach (var t in playerPrefab)
        {
            t.transform.localScale = new Vector3(widthCell + 0.5f, heightCell + 0.5f, 2);
        }
        
        //Ячейка спавна игрока
        var firstCell = new Vector2(-width / 2 + widthCell / 2, height / 2 - heightCell / 2);

        
        for (var i = rMax; i >= 0; i--)
        {
            for (var j = 0; j <= cMax; j++)
            {
                //Проверка на стену. Если не стена, то игрок спавнится тут
                if (maze[i, j] == 0)
                {
                    _currentObject = Instantiate(playerPrefab[1], firstCell, Quaternion.identity);
                    _anim = _currentObject.GetComponent<Animator>();
                    _start = true;
                    return;
                }
                firstCell.x += widthCell;
            }

            firstCell.y -= heightCell;
            firstCell.x = -width / 2 + widthCell / 2;
        }
    }

    private void Update()
    {
        if (!_start) return;
        
        //Последняя позиция игрока
        var lastPosition = _currentObject.transform.position;
        
        //Сдвиг по X и Y (нажатие на кнопку)
        var xDirection = Input.GetAxis("Horizontal");
        var yDirection = Input.GetAxis("Vertical");
        
        //Смещённая позиция игрока
        var move = new Vector3(xDirection, yDirection, playerPrefab[0].transform.position.z);
        
        //Проверка на нажатую кнопку (в Android будет гироскоп)
        if (Input.GetKey ("w"))
        {
            //Проверка на текущее направление
            if (_currentDirection != 0)
            {
                //Смена направления 
                _currentDirection = 0;
                
                //Уничтожение и создание нового объекта
                Destroy(_currentObject);
                _currentObject = Instantiate(playerPrefab[0], lastPosition, Quaternion.identity);
                
                //Присваивание нового аниматора
                _anim = _currentObject.GetComponent<Animator>();
            }
            //Проверка на запущенную анимацию
            if (!_animationRun)
            {
                //Включение анимации
                _anim.enabled = true;
                _anim.speed = 1f;
                _animationRun = true;
            }
            
        }
        else if (Input.GetKey ("s"))
        {
            if (_currentDirection != 1)
            {
                _currentDirection = 1;
                Destroy(_currentObject);
                _currentObject = Instantiate(playerPrefab[1], lastPosition, Quaternion.identity);
                _anim = _currentObject.GetComponent<Animator>();
            }
            if (!_animationRun)
            {
                _anim.enabled = true;
                _anim.speed = 1f;
                _animationRun = true;
            }
        }
        else if (Input.GetKey ("d"))
        {
            if (_currentDirection != 3)
            {
                _currentDirection = 3;
                Destroy(_currentObject);
                _currentObject = Instantiate(playerPrefab[3], lastPosition, Quaternion.identity);
                _anim = _currentObject.GetComponent<Animator>();
            }

            if (!_animationRun)
            {
                _anim.enabled = true;
                _anim.speed = 1f;
                _animationRun = true;
            }
        }
        else if (Input.GetKey ("a"))
        {
            if (_currentDirection != 2)
            {
                _currentDirection = 2;
                Destroy(_currentObject);
                _currentObject = Instantiate(playerPrefab[2], lastPosition, Quaternion.identity);
                _anim = _currentObject.GetComponent<Animator>();
            }
            if (!_animationRun)
            {
                _anim.enabled = true;
                _anim.speed = 1f;
                _animationRun = true;
            }
        }
        
        //Смена положения игрока
        _currentObject.transform.position += Speed * move;
        
        //Если позиция не изменилась, останавливаем анимацию
        if (lastPosition == _currentObject.transform.position)
        {
            _anim.enabled = false;
            _anim.speed = 0;
            _animationRun = false;
        }
        
        //Запрет на сальто
        _currentObject.transform.rotation = playerPrefab[0].transform.rotation;
    }
}
