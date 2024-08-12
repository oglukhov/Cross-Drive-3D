using System;
using UnityEngine;

public class FirstCarMovement : MonoBehaviour
{
    public GameObject canvasFirst, secondCar, secondCanvas;
    private bool isFirst;
    private CarController _controller;

    private void Start()
    {
        _controller = GetComponent<CarController>();
    }

    private void Update()
    {
        if (transform.position.z > -16.5 && !isFirst)
        {
            isFirst = true;
            _controller.speed = 0;
            canvasFirst.SetActive(true);
        }
            
    }
    
    private void OnMouseDown()
    {
        if(!isFirst || transform.position.z > -15.5) return;
        
        _controller.speed = 15f;
        canvasFirst.SetActive(false);
        secondCar.GetComponent<CarController>().speed = 12f;
        secondCanvas.SetActive(true);
    }
}
