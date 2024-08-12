using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] maps;
    public bool isMainScene;
    public GameObject[] cars;
    public GameObject canvasLosePanel, turnCollider;
    public float timeToSpawnMin = 2f, timeToSpawnMax = 4.5f;
    private int _countCars = 0;
    private Coroutine _bottomCars, _leftCars, _rightCars, _topCars;
    private bool isLoseOnce;
    public Text nowScore, topScore, coins;
    public GameObject background;
    [NonSerialized] public static int countLooses;
       
    private void Start()
    {
        if(PlayerPrefs.GetInt("NowMap") == 2){
            Destroy(maps[0]);
            maps[1].SetActive(true);
            Destroy(maps[2]);
        }else if(PlayerPrefs.GetInt("NowMap") == 3){
            Destroy(maps[0]);
            Destroy(maps[1]);
            maps[2].SetActive(true);
        }else {
            maps[0].SetActive(true);
            Destroy(maps[1]);
            Destroy(maps[2]);
        }

        CarController.isLose = false;
        CarController._countCars = 0;
        
        if (isMainScene)
        {
            timeToSpawnMin = 4f;
            timeToSpawnMax = 6f;
        }
        _bottomCars = StartCoroutine(BottomCars());
        _leftCars = StartCoroutine(LeftCars());
        _rightCars = StartCoroutine(RightCars());
        _topCars = StartCoroutine(TopCars());
    }


    private void Update()
    {  
        if (CarController.isLose && !isLoseOnce)
        {
            countLooses++;
            StopCoroutine(_bottomCars);
            StopCoroutine(_leftCars);
            StopCoroutine(_rightCars);
            StopCoroutine(_topCars);
            nowScore.text = "<color=#FD3636>Score: </color> " + CarController._countCars;
            if (PlayerPrefs.GetInt("Score") < CarController._countCars)
            {
                PlayerPrefs.SetInt("Score", CarController._countCars);
            }

            topScore.text = "<color=#FD3636>Best: </color> " + PlayerPrefs.GetInt("Score");
            
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + CarController._countCars);
            coins.text =  PlayerPrefs.GetInt("Coins").ToString();
            
            
            canvasLosePanel.SetActive(true);
            turnCollider.SetActive(false);
            isLoseOnce = true;
        }
    }

    IEnumerator BottomCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(4.04f, 0.09f, -35.3f), 0f);
            float timeToSpawn = Random.Range(timeToSpawnMin, timeToSpawnMax);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    IEnumerator LeftCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(-3.13f, 0.09f, 55), 180f);
            float timeToSpawn = Random.Range(timeToSpawnMin, timeToSpawnMax);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    IEnumerator RightCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(-43f, 0.09f, -7.4f), 90f);
            float timeToSpawn = Random.Range(timeToSpawnMin, timeToSpawnMax);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    IEnumerator TopCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(40f, 0.09f, -0.9f), 270f);
            float timeToSpawn = Random.Range(timeToSpawnMin, timeToSpawnMax);
            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    void SpawnCar(Vector3 pos, float rotationY)
    {
        GameObject newObj =
            Instantiate(cars[Random.Range(0, cars.Length)], pos, Quaternion.Euler(0, rotationY, 0)) as GameObject;
        newObj.name = "Car - " + ++_countCars;
        

        int random = isMainScene ? 1 : Random.Range(1, 6);
        if (isMainScene)
            newObj.GetComponent<CarController>().speed = 10f;
            
        switch (random)
        {
            case 1:
            case 2:
                //Move right
                newObj.GetComponent<CarController>().rightTurn = true;
                break;
            case 3:
                //Move left
                newObj.GetComponent<CarController>().leftTurn = true;
                break;
            case 4:
            case 5:
                //Move forward
                break;

        }
    }
}
