using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Animator _anim;
    AnimationClip[] clips;

    int[,] currentMaze;

    [SerializeField] GameObject controller;
    Rigidbody2D _rb;

    const int RUN_BOTTOM = 0;
    const int RUN_LEFT = 1;
    const int RUN_TOP = 2;
    const int RUN_RIGHT = 3;

    int currentRotate = 0;

    float widthCell;
    float heightCell;

    int[] currentPoint = { 0, 0};

    float _speedTime = 0.001f;
    
    //Флаг ожидания инициализации
    bool _start;
    bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (check)
        {
            check = false;
            StopCoroutine(RunCoroutine());

        }
    }

    bool isPause;

    public void SetPause(bool p)
    {
        isPause = p;
        if (isPause)
            check = true;
        else
            StartCoroutine(RunCoroutine());
    }


    public void CreateMonster(Vector3 firstCell, float w, float h, int[,] maze, Vector3 parent)
    {
        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        currentPoint[1] = cMax - 1;
        currentPoint[0] = rMax - 1;

        widthCell = w;
        heightCell = h;
        currentMaze = maze;

        transform.localScale = parent;
        transform.position = new Vector2(firstCell.x, firstCell.y);

        _anim = GetComponent<Animator>();
        clips = _anim.runtimeAnimatorController.animationClips;

        /*TODO ЗАМЕНИТЬ, ПЛОХО ТАК*/
        var ploho = clips[RUN_RIGHT];
        clips[RUN_RIGHT] = clips[RUN_TOP];
        clips[RUN_TOP] = ploho;

        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _start = true;

        //_anim.enabled = false;
        _anim.enabled = true;
        _anim.speed = 1f;
        _anim.Play(clips[2].name);

        //Запуск движений
        StartCoroutine(RunCoroutine());
    }

    void RotateRight()
    {
        if (currentRotate == RUN_RIGHT)
            currentRotate = RUN_BOTTOM;
        else
            currentRotate++;
    }

    void RotateLeft()
    {
        if (currentRotate == RUN_BOTTOM)
            currentRotate = RUN_RIGHT;
        else
            currentRotate--;
    }

    IEnumerator RunCoroutine()
    {
        CheckPosition();
        float temp;

        temp = (currentRotate == RUN_BOTTOM || currentRotate == RUN_TOP) ? heightCell : widthCell;

        Vector2 vect = transform.position;
        float offset = temp / 50;
        for (float i = 0; i < temp; i += offset)
        {
            switch (currentRotate)
            {
                case RUN_BOTTOM:
                    vect.y -= offset;
                    break;

                case RUN_TOP:
                    vect.y += offset;
                    break;

                case RUN_RIGHT:
                    vect.x += offset;
                    break;

                case RUN_LEFT:
                    vect.x -= offset;
                    break;
                default:
                    break;
            }

        transform.position = vect;

        _rb.MovePosition(vect);
            yield return new WaitForSeconds(_speedTime * 5);
    }


    yield return StartCoroutine(RunCoroutine());
    }

    void CheckPosition()
    {
        bool stop = true;
        while (stop)
        {
            stop = Check();
        }
        RotateRight();
        _anim.Play(clips[currentRotate].name);

    }

    bool Check()
    {
        switch (currentRotate)
        {
            case RUN_RIGHT:
                if (currentMaze[currentPoint[0] - 1, currentPoint[1]] != 0)
                {
                    RotateLeft();
                    return true;
                }
                currentPoint[0]--;
                return false;

            case RUN_BOTTOM:
                if (currentMaze[currentPoint[0], currentPoint[1] - 1] != 0)
                {
                    RotateLeft();
                    return true;
                }
                currentPoint[1]--;
                return false;

            case RUN_LEFT:
                if (currentMaze[currentPoint[0] + 1, currentPoint[1]] != 0)
                {
                    RotateLeft();
                    return true;
                }
                currentPoint[0]++;
                return false;

            case RUN_TOP:
                if (currentMaze[currentPoint[0], currentPoint[1] + 1] != 0)
                {
                    RotateLeft();
                    return true;
                }
                currentPoint[1]++;
                return false;

            default:
                return true;
        }
    }
}
