using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    
    private bool start = false;
    [SerializeField] private GameObject[] playerPrefab;

    private GameObject playerGameObject;

    private GameObject currentObject;
    public void CreatePlayer(int[,] maze)
    {
        var height = Camera.main.orthographicSize * 1.95f;
        var width = height / Screen.height * Screen.width;
    
        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        var heightCell = height / (rMax + 1);
        var widthCell = width / (cMax + 1);

        foreach (var t in playerPrefab)
        {
            t.transform.localScale = new Vector3(widthCell + 0.5f, heightCell + 0.5f, 2);
           // t.SetActive(false);
        }
        playerPrefab[1].SetActive(true);
        var firstCell = new Vector2(-width / 2 + widthCell / 2, height / 2 - heightCell / 2);

        
        for (var i = rMax; i >= 0; i--)
        {
            for (var j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    currentObject = Instantiate(playerPrefab[1], firstCell, Quaternion.identity);
                    start = true;
                    return;
                }
                firstCell.x += widthCell;
            }

            firstCell.y -= heightCell;
            firstCell.x = -width / 2 + widthCell / 2;
        }

        
    }

    private const float speed = 0.02f;
    private void Update()
    {
        if (!start) return;
        var xDirection = Input.GetAxis("Horizontal");
        var yDirection = Input.GetAxis("Vertical");

        var move = new Vector3(xDirection, yDirection, playerPrefab[0].transform.position.z);

        currentObject.transform.position += speed * move;

        var temp = currentObject.transform.position;
        
        if (Input.GetKey ("w") && currentObject != playerPrefab[0]) {
            Destroy(currentObject);
            currentObject = Instantiate(playerPrefab[0], temp, Quaternion.identity);
        }
        if (Input.GetKey ("s") && currentObject != playerPrefab[1]) {
            Destroy(currentObject);
            currentObject = Instantiate(playerPrefab[1], temp, Quaternion.identity);
        }
        if (Input.GetKey ("d") && currentObject != playerPrefab[3]) {
            Destroy(currentObject);
            currentObject = Instantiate(playerPrefab[3], temp, Quaternion.identity);
        }
        if (Input.GetKey ("a") && currentObject != playerPrefab[2]) {
            Destroy(currentObject);
            currentObject = Instantiate(playerPrefab[2], temp, Quaternion.identity);
        }
    }
}
