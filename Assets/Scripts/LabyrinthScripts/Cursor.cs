using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    bool active = false;

    Rigidbody2D _rb;

    string currentButtonTag = "";

    private readonly float _speed = 10.0f;

    private Vector2 _lastGyro;

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
        if (active)
        {
            var gyro = Vector2.Lerp(_lastGyro, Input.gyro.rotationRateUnbiased, 2f * Time.deltaTime);
            var move = new Vector2(-_lastGyro.y, _lastGyro.x);
            _lastGyro = gyro;
            _rb.MovePosition(_rb.position + move * _speed);
        }
    }

    public void Active(bool activ)
    {
        active = activ;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ButtonResume") || other.CompareTag("ButtonRestart") || other.CompareTag("ButtonExit"))
            currentButtonTag = other.tag;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ButtonResume") || other.CompareTag("ButtonRestart") || other.CompareTag("ButtonExit"))
            currentButtonTag = "";
    }

    public string GetButtonTag()
    {
        return currentButtonTag;
    }
    
}
