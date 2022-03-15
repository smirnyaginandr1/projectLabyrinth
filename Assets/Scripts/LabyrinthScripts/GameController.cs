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
    [SerializeField] GameObject _textObj;

    Text _text;

    [SerializeField] GameObject _buttonRestart;

    [SerializeField] GameObject _buttonResume;

    [SerializeField] GameObject _buttonExit;

    [SerializeField] GameObject _cursor;
    Cursor cursorScript;

    [SerializeField] GameObject _player;
    Player playerScript;

    [SerializeField] GameObject _monster;
    Monster monsterScript;

    Constructor _generator;

    int _currentPoint;

    bool isPause = false;

    int[,] _maze;

    Vector3 accel;

    [SerializeField] GameObject background;

    void Start()
    {
        Display.SetHeightDisplay(Camera.main.orthographicSize * 1.95f);
        Display.SetWidthDisplay(Display.GetHeightDisplay() / Screen.height * Screen.width);

        _text = _textObj.GetComponent<Text>();

        //ActiveUIFalse();

        var percent = Display.GetHeightDisplay() / Display.GetWidthDisplay();

        var widthCount = (int)(Display.GetWidthDisplay());
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
        monsterScript = _monster.GetComponent<Monster>();
    }

    void FixedUpdate()
    {
        accel = Input.acceleration;
        if (accel.x > 0.5f)
        {
            if (!isPause)
            {
                ActiveUIPause();
                playerScript.SetPause(true);
                cursorScript.SetActive(true);
                monsterScript.SetPause(true);
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

    int maxPoint;

    public void SetMaxPoint(int m)
    {
        maxPoint = m;
    }

    public int GetMaxPoint()
    {
        return maxPoint;
    }

    public int GetCurrentPoint()
    {
        return _currentPoint;
    }
    public void AddCurrentPoint()
    {
        _currentPoint++;
    }

    public void OpenFinish()
    {
        GameObject finalWall = _generator.GetFinishWall();
        finalWall.GetComponent<Renderer>().enabled = false;
        finalWall.tag = "Finish";
        BoxCollider2D coll = finalWall.GetComponent<BoxCollider2D>();
        coll.isTrigger = true;
    }

    public void LoseLevel()
    {
        ActiveUIFalse();
        ActiveUILose();

        playerScript.SetPause(true);
        cursorScript.SetActive(true);
        monsterScript.SetPause(true);
    }

    public void FinishLevel()
    {
        ActiveUIFalse();
        ActiveUIWin();

        playerScript.SetPause(true);
        monsterScript.SetPause(true);
        cursorScript.SetActive(true);
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
        cursorScript.SetActive(false);
        monsterScript.SetPause(false);
    }

    public void ExitMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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