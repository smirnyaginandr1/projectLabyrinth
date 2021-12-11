using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Аниматор для запуска анимаций
    private Animator _anim;

    [SerializeField] private GameObject controller;
    private GameController _gameController;
    private AnimationClip[] clips;
    //Скорость игрока
    private readonly float _speed = 0.2f;

    //Флаг ожидания инициализации
    private bool _start;

    //Переменные для гироскопа
    private Vector2 _lastGyro;
    private Rigidbody2D _rb;


    public void CreatePlayer(float widthCell, float heightCell, Vector2 firstCell)
    {
        transform.localScale = new Vector2(widthCell + 0.5f, heightCell + 0.5f);
        transform.position = new Vector2(firstCell.x, firstCell.y);

        _anim = GetComponent<Animator>();
        clips = _anim.runtimeAnimatorController.animationClips;
        Input.gyro.enabled = true;
        _lastGyro = Input.gyro.rotationRateUnbiased;
        
        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _start = true;

        //_anim.enabled = false;
        _anim.enabled = true;
        _anim.speed = 1f;
        _anim.Play(clips[0].name);
    }

    private Vector2 _lastPosition;
        
    
    private void FixedUpdate()
    {
        if (!_start) return;

        var gyro = Vector2.Lerp(_lastGyro, Input.gyro.rotationRateUnbiased, 2f * Time.deltaTime);
        
        //Последняя позиция игрока
        _lastPosition = transform.position;


        var move = new Vector2(-_lastGyro.y, _lastGyro.x);

        if (gyro.y > 0)
        {
            if (gyro.y > Mathf.Abs(gyro.x))
            {
                _anim.Play(clips[2].name);
            }
            else if (gyro.x > 0)
            {
                _anim.Play(clips[0].name);
            }
            else
            {
                _anim.Play(clips[1].name);
            }
        }
        else if (gyro.y < 0)
        {
            if (Mathf.Abs(gyro.y) > Mathf.Abs(gyro.x))
            {
                _anim.Play(clips[3].name);
            }
            else if (gyro.x > 0)
            {
                _anim.Play(clips[0].name);
            }
            else
            {
                _anim.Play(clips[1].name);
            }
        }

        _lastGyro = gyro;

     //   playerPrefab.transform.position += _speed * move;
        _rb.MovePosition(_rb.position + move * _speed);
        //Если позиция не изменилась, останавливаем анимацию
        if (_lastPosition.x == transform.position.x
        && _lastPosition.y == transform.position.y)
        {
            //EnableAnimation(0);
        }
    }

    private float FindMaxValue(float val1, float val2, float val3, float val4)
    {
        float maxValue = val1;
        if (maxValue > val2)
            maxValue = val2;
        if (maxValue > val3)
            maxValue = val3;
        if (maxValue > val4)
            maxValue = val4;

        return maxValue;
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