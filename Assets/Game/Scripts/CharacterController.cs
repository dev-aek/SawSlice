using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CharacterController : MonoBehaviour
{
    Vector3 randomAngle;
    bool isRotating;
    int health = 2;
    public GameObject sav1;
    public GameObject sav2;

    public ParticleSystem smokeEffect;

    void Start()
    {
        InputManager.Instance.onTouchStart += ProcessPlayerSwere;
        InputManager.Instance.onTouchMove += ProcessPlayerSwere;
        InputManager.Instance.onTouchStart += ProcessPlayerRotation;
        InputManager.Instance.onTouchMove += ProcessPlayerRotation;
    }

    private void OnDisable()
    {
        InputManager.Instance.onTouchStart -= ProcessPlayerSwere;
        InputManager.Instance.onTouchMove -= ProcessPlayerSwere;
        InputManager.Instance.onTouchStart -= ProcessPlayerRotation;
        InputManager.Instance.onTouchMove -= ProcessPlayerRotation;
    }

    void Update()
    {
        ProcessPlayerForwardMovement();
        ProcessPlayerHealth();
    }

    private void ProcessPlayerForwardMovement()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.Normal)
        {
            GetComponent<Mover>().MoveTo(new Vector3(
                0f,
                0f,
                GameManager.Instance.forwardSpeed));
        }
    }

    private void ProcessPlayerSwere()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.Normal)
        {
            GetComponent<Mover>().MoveTo(new Vector3(
                InputManager.Instance.GetDirection().x * GameManager.Instance.horizontalSpeed, 0f, 0f));
        }
    }

    private void ProcessPlayerRotation()
    {
        if(GameManager.Instance.currentState == GameManager.GameState.Normal&& -InputManager.Instance.GetDirection().y > 0.015)
        {
            isRotating = true;
            transform.DORotate(new Vector3(0f, 0f, 90f), 0.2f, RotateMode.FastBeyond360);
            smokeEffect.Stop();

        }
        else if(GameManager.Instance.currentState == GameManager.GameState.Normal && -InputManager.Instance.GetDirection().y < -0.015){
            isRotating = true;
            transform.DORotate(new Vector3(0f, 0f, 0f), 0.2f, RotateMode.FastBeyond360);
            smokeEffect.Play();
        }
        isRotating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SliceLeft")
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.GetComponent<Rigidbody>().AddTorque(-Vector3.forward *-300000000f, ForceMode.Impulse);
            //randomAngle = new Vector3(Random.Range(-10f, 10f), Random.Range(0.2f, 0.3f), Random.Range(-2f, 2f));

            //other.gameObject.GetComponent<Rigidbody>().AddTorque(randomAngle * Random.Range(650, 1500), ForceMode.Impulse);

            Destroy(other.gameObject, 2f);

        }

        if (other.gameObject.tag == "SliceRight")
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.GetComponent<Rigidbody>().AddTorque(-Vector3.forward * 300000000f, ForceMode.Impulse);
            /*randomAngle = new Vector3(Random.Range(-10f, 10f), Random.Range(0.2f, 0.3f), Random.Range(-2f, 2f));

            other.gameObject.GetComponent<Rigidbody>().AddTorque(randomAngle * Random.Range(650, 1500), ForceMode.Impulse);*/
            Destroy(other.gameObject, 2f);

        }

        if (other.gameObject.tag == "Finish") {
            Debug.Log("health");
            GameManager.Instance.currentState = GameManager.GameState.Victory;
            GameManager.onWinEvent?.Invoke();

        }

        if (other.gameObject.tag == "YEngel")
        {
            health -= 1;
            Debug.Log(health);
            Destroy(other.gameObject);
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 20f);
            gameObject.GetComponent<Rigidbody>().AddTorque(transform.forward * 7f);
        }

        if (other.gameObject.tag == "Spaces")
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            Invoke("diePlayer", 1f);
        }

        if (other.gameObject.tag == "Engel")
        {
            health -= 1;
            Debug.Log(health);
            Destroy(other.gameObject);
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * 10f);
            gameObject.GetComponent<Rigidbody>().AddTorque(transform.forward * 7f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void ProcessPlayerHealth()
    {
        if(health == 1)
        {
            Debug.Log(health);
            sav1.SetActive(false);
            sav2.SetActive(true);
            GameManager.Instance.forwardSpeed=0.6f;

        }
        else if( health == 0)
        {
            GameManager.Instance.currentState = GameManager.GameState.Failed;
            GameManager.onLoseEvent?.Invoke();

        }
    }

    void diePlayer()
    {
        health = 0;
        gameObject.AddComponent<BoxCollider>().isTrigger = false;

    }
}
