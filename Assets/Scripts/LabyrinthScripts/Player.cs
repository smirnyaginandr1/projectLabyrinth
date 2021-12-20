using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Аниматор для запуска анимаций
    Animator _anim;

    [SerializeField] GameObject controller;
    GameController _gameController;

    AnimationClip[] clips;
    //Скорость игрока
    readonly float _speed = 0.2f;

    public bool isPause
    {
        get
        {
            return isPause;
        }
        set
        {
            isPause = value;
        }
    }

    //Флаг ожидания инициализации
    bool _start;

    //Переменные для гироскопа
    Vector2 _lastGyro;
    Rigidbody2D _rb;

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
    
    void FixedUpdate()
    {
        if (!_start && isPause) return;

        var gyro = Vector2.Lerp(_lastGyro, Input.gyro.rotationRateUnbiased, 2f * Time.deltaTime);
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
      
        _rb.MovePosition(_rb.position + move * _speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Point"))
        {
            var enteredObject = other.gameObject;
            Destroy(enteredObject);
            controller = GameObject.FindWithTag("Controller");
            _gameController = controller.GetComponent<GameController>();
            _gameController.AddCurrentPoint();
            if (_gameController.GetCurrentPoint() == _gameController.maxPoint)
                _gameController.OpenFinish();
        }

        if (other.CompareTag("Finish"))
        {
            _gameController.FinishLevel();
        }
    }
}