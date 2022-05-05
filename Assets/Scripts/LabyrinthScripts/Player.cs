using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Аниматор для запуска анимаций
    Animator _anim;
    AnimationClip[] clips;

    [SerializeField] GameObject controller;
    GameController _gameController;

    //Скорость игрока
    readonly float _speed = 0.2f;

    bool isPause;

    public void SetPause(bool p)
    {
        isPause = p;
    }

    //Флаг ожидания инициализации
    bool _start;

    //Переменные для гироскопа
    Vector3 _lastGyro;
    Rigidbody2D _rb;

    public void CreatePlayer(float widthCell, float heightCell, Vector2 firstCell)
    {
        transform.localScale = new Vector2(widthCell + 1.0f, heightCell + 1.0f);
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

        controller = GameObject.FindWithTag("Controller");
        _gameController = controller.GetComponent<GameController>();
    }
    
    void FixedUpdate()
    {
        if (!_start || isPause) return;

        var gyro = Vector3.Lerp(_lastGyro, Input.gyro.rotationRateUnbiased, 2f * Time.deltaTime);
        var move = new Vector2(-_lastGyro.y, _lastGyro.x);

        StaticClass.SetValue(_lastGyro, Input.acceleration.normalized);

        if (gyro.y > 0)
        {
            if (gyro.y > Mathf.Abs(gyro.x))
            {
                _anim.Play(clips[2].name);
            }
            else if (gyro.x > 0)
            {
                _anim.Play(clips[3].name);
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
                _anim.Play(clips[0].name);
            }
            else if (gyro.x > 0)
            {
                _anim.Play(clips[3].name);
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

            _gameController.AddCurrentPoint();
            if (_gameController.GetCurrentPoint() == _gameController.GetMaxPoint())
                _gameController.OpenFinish();
        }

        if (other.CompareTag("Monster"))
        {
            _gameController.LoseLevel();
        }

        if (other.CompareTag("Finish"))
        {
            _gameController.Finish();
        }
        if (other.CompareTag("Wall"))
        {
            _gameController.ClearSpeedMonster();
        }
    }
}