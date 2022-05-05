using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerLast : MonoBehaviour
{
    [SerializeField]
    GameObject Head;
    [SerializeField]
    TMPro.TextMeshPro textMeshPro;

    [SerializeField]
    TMPro.TextMeshPro textMeshProInfo;

    int currentScene = 0;

    void Start()
    {
        //Head.SetActive(false);
        StaticClass.InitJSON("WaitSceneVoid", gameObject);
        Input.gyro.enabled = true;
        _lastGyro = Input.gyro.rotationRateUnbiased;
        _lastAccel = Input.acceleration.normalized;
        StartCoroutine(waitCoroutine());
    } 
    // Update is called once per frame
    void Update()
    {
        _lastGyro = Gyro();
        _lastAccel = Input.acceleration.normalized;
        StaticClass.SetValue(_lastGyro, _lastAccel);

        if (StaticClass.GetTime() > 30)
        {
            StaticClass.StartOrStopWriteInFile(false);
            switch (currentScene)
            {
                case 0:
                    StaticClass.FinalCreateFileJSON();
                    StaticClass.InitJSON("WaitSceneHead", gameObject);
                    textMeshProInfo.text = "Держи голову прямо";
                    currentScene++;
                    Head.SetActive(true);
                    StartCoroutine(waitCoroutine());
                    break;
                default:
                    textMeshProInfo.text = "Молодец!";
                    StartCoroutine(lastWaitCoroutine());
                    break;
            }

        }
    }

    Vector3 _lastGyro;
    Vector3 Gyro()
    {
        var gyro = Vector3.Lerp(_lastGyro, Input.gyro.rotationRateUnbiased, 2f * Time.deltaTime);

        Head.transform.rotation = new Quaternion(-gyro.x, gyro.y, 0, 5);

        return gyro;
    }

    private Vector3 _lastAccel;

    IEnumerator waitCoroutine()
    {
        StaticClass.StartOrStopWriteInFile(false);
        for (int i = 5; i > 0; i--)
        {
            textMeshPro.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        textMeshProInfo.text = "";
        textMeshPro.text = "";
        StaticClass.StartOrStopWriteInFile(true);
    }
    IEnumerator lastWaitCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        StaticClass.FinalCreateFileJSON();
        if (StaticClass.GetFirstWaitScene())
        {
            //SceneManager.LoadScene("MainMenu");
            Application.Quit();
        }
        else
        {
            StaticClass.SetFirstWaitScene(true);
            switch (StaticClass.GetCurrentScene())
            {
                case StaticClass.GameScene.SceneLabyrinth:
                    SceneManager.LoadScene("GameLabyrinth");
                    break;
                case StaticClass.GameScene.ScenePingPong:
                    //todo: Добавить загрузку сцены Пинг-понг
                    SceneManager.LoadScene("MainMenu");
                    break;
                case StaticClass.GameScene.SceneTarget:
                    //todo: Добавить загрузку сцены Мишени
                    SceneManager.LoadScene("MainMenu");
                    break;
                default:
                    SceneManager.LoadScene("MainMenu");
                    break;
            }
        }
        
    }


}
