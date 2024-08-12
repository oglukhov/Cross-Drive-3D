using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    private Rigidbody _rb;
    public float speed = 15f, force = 50f; 
    public bool rightTurn, leftTurn;
    private float originalRotationY, rotateMultRight = 6f, rotateMultLeft = 4f;
    private Camera _mainCam;
    public LayerMask carsLayer;
    public bool isMovingFast, _carCrashed;
    [NonSerialized] public bool carPassed;
    [NonSerialized] public static bool isLose;
    public GameObject turnLeftSignal, turnRightSignal, crashAlarm, explosion, exhaust;
    private Coroutine left, right;
    public static int _countCars;
    public AudioClip bumpSound, accelerateSound;
    private AudioSource _source;
    
    private void Start()
    {
        _mainCam = Camera.main;
        originalRotationY = transform.eulerAngles.y;
        _rb = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
        
        if (rightTurn)
            left = StartCoroutine(TurnSignals(turnRightSignal));
        else if (leftTurn)
            right = StartCoroutine(TurnSignals(turnLeftSignal));
    }

    IEnumerator TurnSignals(GameObject turnSignal)
         {
             while (!carPassed && !_carCrashed)
             {
                 turnSignal.SetActive(!turnSignal.activeSelf);
                 yield return new WaitForSeconds(0.5f);
             }
         }

    IEnumerator Signals(GameObject signal)
    {
        while (!carPassed)
        {
            signal.SetActive(!signal.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
#if UNITY_EDITOR
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
#else 
        if(Input.touchCount == 0)
            return;
        Ray ray = _mainCam.ScreenPointToRay(Input.GetTouch(0).position);
#endif 
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, carsLayer))
        {
            string carName = hit.transform.gameObject.name;
            
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !isMovingFast && gameObject.name == carName)
            {
#else   
            if(Input.GetTouch(0).phase == TouchPhase.Began && !isMovingFast && gameObject.name == carName){
#endif

                if(PlayerPrefs.GetString("Music") != "Off"){
                    _source.clip = accelerateSound;
                    _source.Play();}
                GameObject exh = Instantiate(exhaust, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(0,0,0)) as GameObject;
                Destroy(exh, 3f);
                speed *= 2;
                isMovingFast = true;
            }

            if (_carCrashed && turnRightSignal.activeSelf)
                turnRightSignal.SetActive(false);
            if(_carCrashed && turnLeftSignal.activeSelf)
                turnLeftSignal.SetActive(false);
            
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Car") && !_carCrashed)
        {
            isLose = true;
            speed = 0f;
            if(PlayerPrefs.GetString("Music") != "Off"){
                _source.volume = 0.4f;
                _source.clip = bumpSound;
                _source.Play();}
            other.gameObject.GetComponent<CarController>().speed = 0f;

            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(expl, 4f);
            
            if (isMovingFast)
                force *= 1.4f;
            _rb.AddRelativeForce(Vector3.forward * force);
            _carCrashed = true;
            
            if (_carCrashed && rightTurn == false && leftTurn == false)
                StartCoroutine(Signals(crashAlarm));
            if (_carCrashed && rightTurn)
            {
                StartCoroutine(Signals(crashAlarm));
                /*StopCoroutine(right);*/
            }

            if (_carCrashed && leftTurn)
            { 
                StartCoroutine(Signals(crashAlarm)); 
                /*StopCoroutine(left);*/
            } 
        }
    }

    private void OnTriggerStay(Collider other)
    { 
        if (other.transform.CompareTag("TurnBlock Right") && rightTurn)
        {
            RotateCar(rotateMultRight);
        }
        else if(other.transform.CompareTag("TurnLeft") && leftTurn)
            RotateCar(rotateMultLeft, -1);
        else if (other.transform.CompareTag("Delete Trigger"))
        {
            Destroy(gameObject);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car") && other.GetComponent<CarController>().carPassed)
            other.GetComponent<CarController>().speed = speed + 5f;
    }
    
    

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("PassTriger"))
        {
            carPassed = true;
            Collider[] colliders = GetComponents<BoxCollider>();
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }
            
            _countCars++;
        }
            
        if (other.transform.CompareTag("TurnBlock Right") && rightTurn)
            _rb.rotation = Quaternion.Euler(0, originalRotationY + 90f, 0);
        else if(other.transform.CompareTag("TurnLeft") && leftTurn)
            _rb.rotation = Quaternion.Euler(0, originalRotationY - 90f, 0);
    }

    private void RotateCar(float speedRotate, int dir = 1)
    {
        if(dir == -1 && transform.localRotation.eulerAngles.y < originalRotationY - 90f)
            return;
        float rotateSpeed = speed * speedRotate * dir;
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, rotateSpeed, 0) * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }
}

