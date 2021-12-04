using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Аниматор для запуска анимаций
    private Animator _anim;

    private Text textFinish;

    private Button buttonRestart;
    
    [SerializeField] private GameObject controller;
    private GameController _gameController;
    
    //Скорость игрока
    private float _speed = 0.2f;
    
    //Текущее направление игрока
    private int _currentDirection = 1;
    
    //Флаг ожидания инициализации
    private bool _start;
    
    //Префабы игрока
    [SerializeField] private GameObject[] playerPrefab;

    //Переменные для гироскопа
    private Vector3 lastGyro;
    
    
    
    //Текущий префаб
    private GameObject _currentObject;
    public void CreatePlayer(int[,] maze, float widthCell, float heightCell, Vector2 firstCell)
    {
        
        //Смена размеров префабов игрока
        foreach (var t in playerPrefab)
            t.transform.localScale = new Vector3(widthCell + 0.5f, heightCell + 0.5f, 2);

        _currentObject = Instantiate(playerPrefab[1], firstCell, Quaternion.identity);
        _anim = _currentObject.GetComponent<Animator>();
        _start = true;

        Input.gyro.enabled = true;
        lastGyro = Input.gyro.rotationRateUnbiased;
        
    }

    private float _floatGyroX = 0;
    private float _floatGyroY = 0;
    private Vector2 lastPosition;
        
    
    private void FixedUpdate()
    {
        if (!_start) return;

        var gyro = Vector3.Lerp(lastGyro, Input.gyro.rotationRateUnbiased, 2f * Time.deltaTime);
        
        //Последняя позиция игрока
        lastPosition = _currentObject.transform.position;


        var move = new Vector3(
            -lastGyro.y,
            lastGyro.x,
            playerPrefab[0].transform.position.z);
        
        //Проверка на нажатую кнопку (в Android будет гироскоп)
        if (gyro.y > lastGyro.y/*Input.GetKey ("w")*/)
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
            if (!_anim.enabled)
            {
                //Включение анимации
                EnableAnimation(1);
            }
            
        }
        else if (gyro.y < lastGyro.y/*Input.GetKey ("s")*/)
        {
            if (_currentDirection != 1)
            {
                _currentDirection = 1;
                Destroy(_currentObject);
                _currentObject = Instantiate(playerPrefab[1], lastPosition, Quaternion.identity);
                _anim = _currentObject.GetComponent<Animator>();
            }
            if (!_anim.enabled)
            {
                EnableAnimation(1);
            }
        }
        else if (/*Input.GetKey ("d")*/gyro.x > lastGyro.x)
        {
            if (_currentDirection != 3)
            {
                _currentDirection = 3;
                Destroy(_currentObject);
                _currentObject = Instantiate(playerPrefab[3], lastPosition, Quaternion.identity);
                _anim = _currentObject.GetComponent<Animator>();
            }

            if (!_anim.enabled)
            {
                EnableAnimation(1);
            }
        }
        else if (/*Input.GetKey ("a")*/gyro.x < lastGyro.x)
        {
            if (_currentDirection != 2)
            {
                _currentDirection = 2;
                Destroy(_currentObject);
                _currentObject = Instantiate(playerPrefab[2], lastPosition, Quaternion.identity);
                _anim = _currentObject.GetComponent<Animator>();
            }
            if (!_anim.enabled)
            {
                EnableAnimation(1);
            }
        }

        
        lastGyro = gyro;


        //Смена положения игрока
        _currentObject.transform.position += _speed * move;
        
        //Если позиция не изменилась, останавливаем анимацию
        if (lastPosition.x == _currentObject.transform.position.x
        && lastPosition.y == _currentObject.transform.position.y)
        {
            EnableAnimation(0);
        }
        
        //Запрет на сальто
        _currentObject.transform.rotation = playerPrefab[0].transform.rotation;
    }
    private void EnableAnimation(int value)
    {
        switch (value)
        {
            case 1:
                _anim.enabled = true;
                _anim.speed = 1f;
                break;
            case 0:
                _anim.enabled = false;
                _anim.speed = 0;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Point")) return;
        var enteredObject = other.gameObject;
        Destroy(enteredObject);
        controller = GameObject.FindWithTag("Controller");
        _gameController = controller.GetComponent<GameController>();
        _gameController.AddCurrentPoint();
        if (_gameController.GetCurrentPoint() == _gameController.GetMaxPoint())
            _gameController.Finish();
    }
}
