using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorMainMenu : MonoBehaviour
{
    Rigidbody2D _rb;

    string currentButtonTag = "";



    private readonly float _speed = 10.0f;

    private Vector2 _lastGyro;

    Vector3 accel;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

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
                SceneManager.LoadScene(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentButtonTag = other.tag;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ButtonLabyrinth"))
            currentButtonTag = "";
    }
}
