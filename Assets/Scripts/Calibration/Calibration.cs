using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Calibration : MonoBehaviour
{

    [SerializeField] GameObject textDialogObject;
    Text textDialog;

    [SerializeField] GameObject textCounterObject;
    Text textCounter;

    float sizeArrowX;
    float sizeArrowY;
    [SerializeField] GameObject[] arrows;

    [SerializeField] GameObject calibrationPoint;
    bool _pointActive = false;
    Vector2 _lastGyro;
    readonly float _speed = 10.0f;
    Rigidbody2D _pointRb;

    Vector3 pointFirstPosition;
    Vector3 pointCurentPosition;

    int cnt = 0;
    bool arrowFlag = false;

    

    int textCount = 0;
    string[] textPlayer = { "Привет",
                            "Перед игрой нужно немного размяться",
                            "Следуй инструкциям",
                            
                            "Смотри прямо",
                            "Опусти голову в левый нижний угол",
                            "Подними голову в левый верхний угол",
                            "Поверни голову в правый верхний угол",
                            "Опусти голову в правый нижний угол",

                            "Молодец! Приятной игры =)"};

    void Start()
    {
        Display.debug = true;

        Display.SetHeightDisplay(Camera.main.orthographicSize * 1.95f);
        Display.SetWidthDisplay(Display.GetHeightDisplay() / Screen.height * Screen.width);

        sizeArrowX = arrows[0].transform.localScale.x;
        sizeArrowY = arrows[0].transform.localScale.y;

        textCounter = textCounterObject.GetComponent<Text>();
        textDialog = textDialogObject.GetComponent<Text>();
        StartCoroutine(StartText());

        _pointRb = calibrationPoint.GetComponent<Rigidbody2D>();
        _pointRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        pointFirstPosition = _pointRb.transform.position;

        Input.gyro.enabled = true;
        _lastGyro = Input.gyro.rotationRateUnbiased;


    }

    void Update()
    {
        if (arrowFlag)
        {
            sizeArrowX += 0.001f;
            sizeArrowY += 0.001f;
        }
        else
        {
            sizeArrowX -= 0.001f;
            sizeArrowY -= 0.001f;
        }
        cnt++;
        if (cnt == 150)
        {
            cnt = 0;
            arrowFlag = !arrowFlag;
        }

        foreach (var temp in arrows)
            temp.transform.localScale = new Vector3(sizeArrowX, sizeArrowY, arrows[0].transform.localScale.z);

        if (_pointActive)
        {
            var gyro = Vector2.Lerp(_lastGyro, Input.gyro.rotationRateUnbiased, 2f * Time.deltaTime);
            var move = new Vector2(-_lastGyro.y, _lastGyro.x);
            _lastGyro = gyro;
            _pointRb.MovePosition(_pointRb.position + move * _speed);
        }
        else
        {
            _pointRb.transform.position = pointFirstPosition;
        }
    }
    IEnumerator StartText()
    {
        for (int i = 0; i < 3; i++)
        {
            NextText();
            yield return StartCoroutine(VisibleText());
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(InvisibleText());   
        }
        for (int i = 0; i < 5; i++)
        {
            NextText();
            if (i == 0)
                yield return StartCoroutine(StartCalibration(null));
            else
            {
                _pointActive = true;
                yield return StartCoroutine(StartCalibration(arrows[i - 1]));
            }
            switch (i)
            {
                case 0:
                    Display.SetCoordinateCenter( pointCurentPosition);
                    break;
                case 1:
                    Display.SetCoordinateLB(pointCurentPosition);
                    break;
                case 2:
                    Display.SetCoordinateLT(pointCurentPosition);
                    break;
                case 3:
                    Display.SetCoordinateRT(pointCurentPosition);
                    break;
                case 4:
                    Display.SetCoordinateRB(pointCurentPosition);
                    break;
            }
        }

        NextText();
        yield return StartCoroutine(VisibleText());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(InvisibleText());
        Display.SetCoordinateCurrent(pointCurentPosition);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator InvisibleText()
    {
        for (float ft = 1f; ft >= 0; ft -= 0.01f)
        {
            Color color = textDialog.color;
            color.a = ft;
            textDialog.color = color;
            yield return new WaitForSeconds(.01f);
        }
    }

    IEnumerator VisibleText()
    {
        for (float ft = 0f; ft < 1f; ft += 0.01f)
        {
            Color color = textDialog.color;
            color.a = ft;
            textDialog.color = color;
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(1.0f);
    }

    bool NextText()
    {
        textDialog.text = textPlayer[textCount];
        textCount++;
        if (textCount == textPlayer.Length)
            return false;
        return true;
    }

    IEnumerator StartCalibration(GameObject arrow)
    {
        yield return StartCoroutine("VisibleText");

        textCounterObject.SetActive(true);
        if (arrow != null)
            arrow.SetActive(true);

        yield return new WaitForSeconds(2f);

        for (int i = 5; i > 0; i--)
        {
            textCounter.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        pointCurentPosition = calibrationPoint.transform.position;

        textCounter.fontSize = textDialog.fontSize;
        textCounter.text = "Молодец!";
        
        yield return new WaitForSeconds(1f);
        
        textCounterObject.SetActive(false);
        if (arrow != null)
            arrow.SetActive(false);
        
        textCounter.text = "5";
        textCounter.fontSize = 140;

        yield return StartCoroutine("InvisibleText");
    }
}
