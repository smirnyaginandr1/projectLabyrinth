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
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    bool isPause;
    Coroutine myCoroutine;
    public void SetPause(bool p)
    {
        isPause = p;
        if (!isPause)
            myCoroutine = StartCoroutine(RunCoroutine());
    }


    public void ClearSpeed()
    {
        _speedTime = 0.001f;
    }

    public void AddSpeed()
    {
        if (_speedTime < 0.0003)
            return;
        _speedTime -= 0.00002f;
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

        /*TODO «¿Ã≈Õ»“‹, œÀŒ’Œ “¿ */
        var ploho = clips[RUN_RIGHT];
        clips[RUN_RIGHT] = clips[RUN_TOP];
        clips[RUN_TOP] = ploho;

        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //_anim.enabled = false;
        _anim.enabled = true;
        _anim.speed = 1f;
        _anim.Play(clips[2].name);

        //«‡ÔÛÒÍ ‰‚ËÊÂÌËÈ
        myCoroutine = StartCoroutine(RunCoroutine());
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

    float checkPosition = 0;

    IEnumerator RunCoroutine()
    {
        if (checkPosition == 0)
            CheckPosition();
        float temp;

        temp = (currentRotate == RUN_BOTTOM || currentRotate == RUN_TOP) ? heightCell : widthCell;

        Vector2 vect = transform.position;
        float offset = temp / 50;
        for (float i = checkPosition; i < temp; i += offset)
        {
            checkPosition = i;
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
            checkPosition = 0;
            transform.position = vect;
            _rb.MovePosition(vect);
            yield return new WaitForSeconds(_speedTime);
        }
        AddSpeed();
        if (!isPause)
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
