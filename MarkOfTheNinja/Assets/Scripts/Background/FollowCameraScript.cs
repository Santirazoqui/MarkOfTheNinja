using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    // Referencia a la cámara que se va a seguir
    [SerializeField] public Camera mainCamera;
    private Vector3 initalPositionRespectFromCamera;
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        initalPositionRespectFromCamera = transform.position - mainCamera.transform.position;
    }

    void LateUpdate()
    {
        // Asignar la posición del fondo a la de la cámara (1:1)
        transform.position = mainCamera.transform.position + initalPositionRespectFromCamera;
    }
}
