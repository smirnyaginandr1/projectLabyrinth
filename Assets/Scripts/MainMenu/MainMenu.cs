using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject[] ButtonsNoActive;

    [SerializeField]
    GameObject[] ButtonsActive;

    int current = 0;

    Vector3 accel;
    // Start is called before the first frame update
    void Start()
    {
        StaticClass.SetFirstWaitScene(false);
        ButtonsNoActive[current].SetActive(false);
        ButtonsActive[current].SetActive(true);
        Input.gyro.enabled = true;
    }

    bool check = false;
    // Update is called once per frame
    void Update()
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
        
        //TODO
        if (accel.x > 0.5f && check == false)
        {
            switch (current)
            {
                case 0:
                    StaticClass.SetCurrentScene(StaticClass.GameScene.SceneLabyrinth);
                    SceneManager.LoadScene("WaitScene");
                    break;

                case 1:
                    StaticClass.SetCurrentScene(StaticClass.GameScene.SceneTarget);
                    SceneManager.LoadScene("WaitScene");
                    break;

                case 2:
                    StaticClass.SetCurrentScene(StaticClass.GameScene.ScenePingPong);
                    SceneManager.LoadScene("WaitScene");
                    break;

                case 3:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Control
                    break;

                case 4:
                    SceneManager.LoadScene("Calibration"); //Calibration
                    break;

                case 5:
                    Application.Quit();
                    break;

                default:
                    break;
            }

            //Display.SetCoordinateCurrent(_rb.transform.position);
        }
        
    }

    // if (other.CompareTag("ButtonLabyrinth"))
    // currentButtonTag = "";

    void NextButton()
    {
        if (current == ButtonsActive.Length - 1)
            return;
        ButtonsNoActive[current].SetActive(true);
        ButtonsActive[current].SetActive(false);

        current++;

        ButtonsNoActive[current].SetActive(false);
        ButtonsActive[current].SetActive(true);
    }

    void PreviousButton()
    {
        if (current == 0)
            return;

        ButtonsNoActive[current].SetActive(true);
        ButtonsActive[current].SetActive(false);

        current--;

        ButtonsNoActive[current].SetActive(false);
        ButtonsActive[current].SetActive(true);
    }

}

/*
Добавить сцены с других приложений.
Добавить включение сцен по наклону
Добавить сцену после игры
добавить счёт в игру
Добавить точку возврата к игроку в лабиринте.
Добавить смену сложности в зависимости от типа прохождения.
Переделать калибровку под два экрана*/