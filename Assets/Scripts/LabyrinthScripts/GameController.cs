using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 *@brief: Класс игрового контроллера. Запускает создание игровых элементов
 */
[RequireComponent(typeof(Constructor))]

public class GameController : MonoBehaviour
{

    [SerializeField] GameObject[] ButtonsNoActive;

    [SerializeField] GameObject[] ButtonsActive;

    [SerializeField] GameObject _player;
    Player playerScript;

    [SerializeField] GameObject _monster;
    Monster monsterScript;

    [SerializeField] TMPro.TextMeshPro textCount;

    [SerializeField] GameObject HelpTable;
    Constructor _generator;

    int _currentPoint;

    bool isPause = false;

    bool check = false;

    bool isFinishOrLose = false;

    int[,] _maze;

    Vector3 accel;

    void Start()
    {
        playerScript = _player.GetComponent<Player>();
        monsterScript = _monster.GetComponent<Monster>();
        
        if (StaticClass.GetFirstRun())
        {
            HelpTable.SetActive(true);
            playerScript.SetPause(true);
            monsterScript.SetPause(true);
            StartCoroutine(WaitStart());
            StaticClass.SetTimerValue(3000);
            StaticClass.SetPointCount(0);
        }
        else
            StartCoroutine(TimerCoroutine());

        textCount.text = "Счёт: " + StaticClass.GetPointCount().ToString();
        ActiveUIFalse();

        StaticClass.InitJSON("Labyrinth", gameObject);
        StaticClass.StartOrStopWriteInFile(true);

        var percent = Display.GetHeightDisplay() / Display.GetWidthDisplay();

        var widthCount = (int)(Display.GetWidthDisplay());
        var heightCount = (int)(widthCount * percent);

        if (widthCount % 2 == 0)
            widthCount++;

        if (heightCount % 2 == 0)
            heightCount++;

        _generator = GetComponent<Constructor>();
        _maze = _generator.GenerateNewMaze(heightCount, widthCount);
    }

    void FixedUpdate()
    {
        accel = Input.acceleration;


        if (accel.z < 0.3f && accel.z > -0.3f)
            check = false;

        if (accel.z < -0.5f && check == false)
        {
            check = true;
            NextButton();
        }

        if (accel.z > 0.5f && check == false)
        {
            check = true;
            PreviousButton();
        }

        if (accel.x < -0.5f && !isPause)
        {
            ActiveUIPause();
            StaticClass.StartOrStopWriteInFile(false);
            playerScript.SetPause(true);
            monsterScript.SetPause(true);
        }

        if (accel.x > 0.5f && isPause)
        {
            switch (currentButton)
            {
                case 0:
                    isPause = false;
                    ActiveUIFalse();
                    playerScript.SetPause(false);
                    monsterScript.SetPause(false);
                    break;

                case 1:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;

                case 2:
                    StaticClass.FinalCreateFileJSON();
                    StaticClass.SetFirstWaitScene(true);
                    StaticClass.InitFirstRun();
                    SceneManager.LoadScene("WaitScene");
                    break;

                default:
                    break;
            }
        }
        if (StaticClass.GetTimerValue() <= 0)
        {
            StaticClass.FinalCreateFileJSON();
            StaticClass.SetFirstWaitScene(true);
            StaticClass.InitFirstRun();
            SceneManager.LoadScene("WaitScene");
        }
    }

    int currentButton = 0;

    public void ClearSpeedMonster()
    {
        monsterScript.ClearSpeed();
    }

    void NextButton()
    {
        if (!isPause || currentButton == 2)
            return;

        ButtonsActive[currentButton].SetActive(false);
        ButtonsNoActive[currentButton].SetActive(true);

        currentButton++;

        ButtonsActive[currentButton].SetActive(true);
        ButtonsNoActive[currentButton].SetActive(false);
    }

    void PreviousButton()
    {
        if (!isPause || currentButton == 0 || (currentButton == 1 && isFinishOrLose))
            return;
        ButtonsActive[currentButton].SetActive(false);
        ButtonsNoActive[currentButton].SetActive(true);

        currentButton--;

        ButtonsActive[currentButton].SetActive(true);
        ButtonsNoActive[currentButton].SetActive(false);
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
        StaticClass.SetPointCount(StaticClass.GetPointCount() + 1);
        textCount.text = "Счёт: " + StaticClass.GetPointCount().ToString();
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
        isFinishOrLose = true;
        ActiveUINoContinue();

        playerScript.SetPause(true);
        monsterScript.SetPause(true);
    }

    public void Finish()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ActiveUIFalse()
    {
        currentButton = 0;
        foreach (var t in ButtonsActive)
            t.SetActive(false);

        foreach (var t in ButtonsNoActive)
            t.SetActive(false);
    }

    void ActiveUINoContinue()
    {
        isPause = true;

        ButtonsActive[1].SetActive(true);
        ButtonsNoActive[2].SetActive(true);
        currentButton = 1;
    }


    void ActiveUIPause()
    {
        isPause = true;
        foreach (var t in ButtonsNoActive)
            t.SetActive(true);
        currentButton = 0;
        ButtonsNoActive[currentButton].SetActive(false);
        ButtonsActive[currentButton].SetActive(true);
    }

    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(8f);

        HelpTable.SetActive(false);
        playerScript.SetPause(false);
        monsterScript.SetPause(false);
        StartCoroutine(TimerCoroutine());
    }
    IEnumerator TimerCoroutine()
    {
        StaticClass.SetTimerValue(StaticClass.GetTimerValue() - 1);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(TimerCoroutine());
    }
}