using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public float rotateSpeed;

    private Vector3 focusPoint;

    private void Start()
    {
        focusPoint = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(focusPoint);
        transform.Translate(rotateSpeed * Time.deltaTime, 0, 0);
    }

    public void NextScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
