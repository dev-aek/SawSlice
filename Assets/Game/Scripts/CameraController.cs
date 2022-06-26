using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject cineV2;

    void Start()
    {
        InputManager.Instance.onTouchStart += disableCamV2;

    }

    private void OnDisable()
    {
        InputManager.Instance.onTouchStart -= disableCamV2;
    }

    void disableCamV2()
    {
        cineV2.gameObject.SetActive(false);
    }
}
