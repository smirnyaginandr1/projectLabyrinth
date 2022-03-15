using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorMainMenu : MonoBehaviour
{
    Rigidbody2D _rb;

    string currentButtonTag = "";

    readonly float _speed = 10.0f;

    Vector2 _lastGyro;

    Vector3 accel;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 test = Display.GetCoordinateCenter();
        Vector3 test1 = Display.GetCoordinateCurrent();
        Vector3 test2 = Display.GetCoordinateLB();
        Vector3 test3 = Display.GetCoordinateLT();
        Vector3 test4 = Display.GetCoordinateRB();
        Vector3 test5 = Display.GetCoordinateRT();
        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.transform.position = Display.GetCoordinateCurrent();

        Input.gyro.enabled = true;
        _lastGyro = Input.gyro.rotationRateUnbiased;
    }

    // Update is called once per frame
    void Update()
    {
        accel = Input.acceleration;
        var gyro = Vector2.Lerp(_lastGyro, Input.gyro.rotationRateUnbiased, 2f * Time.deltaTime);
        var move = new Vector2(-_lastGyro.y, _lastGyro.x);
        _lastGyro = gyro;
        _rb.MovePosition(_rb.position + move * _speed);
        if (currentButtonTag == "ButtonLabyrinth")
        {
            if (accel.x < -0.5)
            {
                Display.SetCoordinateCurrent(_rb.transform.position);
                SceneManager.LoadScene("GameLabyrinth");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        currentButtonTag = other.tag;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ButtonLabyrinth"))
            currentButtonTag = "";
    }
}
