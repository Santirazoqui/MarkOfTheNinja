using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using Assets.Scripts.Background;

public class CameraPan : MonoBehaviour
{
    [SerializeField] GameObject origin;
    [SerializeField] GameObject player;
    [SerializeField] GameObject camera;
    [SerializeField] GameObject followCamera;
    [SerializeField] public float secondsToPan = 5;
    [SerializeField] FollowPlayerScript followPlayerScript; //pasar el PosReference que es hijo de Background

    private PlayerController playerController;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        followCamera.SetActive(false);
        playerController.Active = false;
        //Debug.Log("Deactivated player & Camera");

        StartCoroutine(PanCamera());
        //PanCamera();
        
        
        //Debug.Log("Reactivated player & Camera");

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PanCamera()
    {
        //yield return new WaitForSeconds(2); 
        //Debug.Log("PanCamera");
        Vector3 startPosition = origin.transform.position;
        startPosition.z = -1;
        Vector3 endPosition = player.transform.position;
        endPosition.z = -1;
        float elapsedTime = 0;

        while (elapsedTime < secondsToPan)
        {
            camera.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / secondsToPan);
            elapsedTime += Time.deltaTime;
            //Debug.Log("elapsedTime: " + elapsedTime);
            yield return null;
        }

        camera.transform.position = endPosition;

        followPlayerScript.player = player;

        followCamera.SetActive(true);
        playerController.Active = true;
    }
}
