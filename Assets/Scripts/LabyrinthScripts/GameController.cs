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
    [SerializeField] private GameObject _textObj;

    Text _text;

    [SerializeField] private GameObject _buttonRestart;

    [SerializeField] private GameObject _buttonResume;

    [SerializeField] private GameObject _buttonExit;

    [SerializeField] private GameObject _cursor;
    Cursor cursorScript;

    [SerializeField] private GameObject _player;
    Player playerScript;


    private Constructor _generator;

    private int _currentPoint;

    bool isPause = false;

    private int[,] _maze;

    private float _height, _width;

    Vector3 accel;

    [SerializeField] private GameObject background;

    private void Start()
    {
        _text = _textObj.GetComponent<Text>();

        ActiveUIFalse();

        _height = Camera.main.orthographicSize * 1.95f;
        _width = _height / Screen.height * Screen.width;

        var percent = _height / _width;

        var widthCount = (int)(_width);
        var heightCount = (int)(widthCount * percent);

        if (widthCount % 2 == 0)
            widthCount++;

        if (heightCount % 2 == 0)
            heightCount++;

        _generator = GetComponent<Constructor>();
        _maze = _generator.GenerateNewMaze(heightCount, widthCount);

        background.transform.localScale = new Vector3(widthCount, heightCount, 1);

        playerScript = _player.GetComponent<Player>();
        cursorScript = _cursor.GetComponent<Cursor>();
    }

    private void FixedUpdate()
    {
        accel = Input.acceleration;
        if (accel.x > 0.5f)
        {
            if (!isPause)
            {
                ActiveUIPause();
                playerScript.SetPause(true);
                cursorScript.Active(true);
            }
        }
        else if (accel.y < -0.5f)
        {
            
        }

        if (cursorScript.GetButtonTag() == "ButtonResume")
        {
            if (accel.x < -0.5)
                Resume();
        }
        if (cursorScript.GetButtonTag() == "ButtonRestart")
        {
            if (accel.x < -0.5)
                ReloadScene();
        }
        if (cursorScript.GetButtonTag() == "ButtonExit")
        {
            if (accel.x < -0.5)
                ExitMainMenu();
        }
    }

    public void AddCurrentPoint()
    {
        _currentPoint++;
    }

    private int _maxPoint;

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

    public void OpenFinish()
    {
        GameObject finalWall = _generator.GetFinishWall();
        finalWall.GetComponent<Renderer>().enabled = false;
        finalWall.tag = "Finish";
        BoxCollider2D coll = finalWall.GetComponent<BoxCollider2D>();
        coll.isTrigger = true;
    }

    public void FinishLevel()
    {
        ActiveUIFalse();
        ActiveUIWin();

        playerScript.SetPause(true);
        cursorScript.Active(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Resume()
    {
        isPause = false;
        ActiveUIFalse();
        playerScript.SetPause(false);
        cursorScript.Active(false);
    }

    public void ExitMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void ActiveUIFalse()
    {
        _textObj.SetActive(false);
        _text.text = "";
        _buttonExit.SetActive(false);
        _buttonRestart.SetActive(false);
        _buttonResume.SetActive(false);
        _cursor.SetActive(false);
    }

    void ActiveUIWin()
    {
        isPause = true;
        _text.text = "Победа";
        _textObj.SetActive(true);
        _cursor.SetActive(true);
        _buttonExit.SetActive(true);
        _buttonRestart.SetActive(true);
    }

    void ActiveUILose()
    {
        isPause = true;
        _text.text = "Поражение";
        _cursor.SetActive(true);
        _textObj.SetActive(true);
        _buttonExit.SetActive(true);
        _buttonRestart.SetActive(true);
    }

    void ActiveUIPause()
    {
        isPause = true;
        _text.text = "Игра приостановлена";
        _cursor.SetActive(true);
        _textObj.SetActive(true);
        _buttonExit.SetActive(true);
        _buttonRestart.SetActive(true);
        _buttonResume.SetActive(true);
    }
}