using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using Assets.Scripts.Background;

public class CameraPan : MonoBehaviour
{
    [SerializeField] GameObject origin;
    [SerializeField] GameObject player;

    [SerializeField] GameObject ExitGate;

    [SerializeField] GameObject camera;
    [SerializeField] GameObject followCamera;
    [SerializeField] public float secondsToPan = 2;
    [SerializeField] public float secondsBeforePan = 1;

    public float secondsInExitGate = 1;

    [SerializeField] FollowPlayerScript followPlayerScript; //pasar el PosReference que es hijo de Background
    [SerializeField] GameObject background;

    [SerializeField] public bool shouldPan = true;

    public bool finishedPan = false;

    private PlayerController playerController;

    void Start()
    {
        if (!shouldPan)
        {
            return;
        }

        playerController = player.GetComponent<PlayerController>();

        followCamera.SetActive(false);
        playerController.Active = false;
        //Debug.Log("Deactivated player & Camera");


        StartCoroutine(PanCameras());
        //PanCamera();


        //Debug.Log("Reactivated player & Camera");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PanCameras()
    {
        StartCoroutine(PanCamera(origin, ExitGate));
        yield return new WaitForSeconds(secondsBeforePan + secondsToPan + secondsInExitGate);
        StartCoroutine(PanCamera(ExitGate, player));
        yield return new WaitForSeconds(secondsBeforePan + secondsToPan);
        finishedPan = true;
        followPlayerScript.enabled = true;

        followCamera.SetActive(true);
        playerController.Active = true;
    }

    IEnumerator PanCamera(GameObject from, GameObject to)
    {
        Vector3 startPosition = from.transform.position;
        startPosition.z = -1;
        Vector3 endPosition = to.transform.position;
        endPosition.z = -1;
        float elapsedTime = 0;

        Vector3 backgroundOffset = background.transform.position - camera.transform.position;
        
        camera.transform.position = new Vector3(startPosition.x , startPosition.y, -1);

        background.transform.position = new Vector3(
            camera.transform.position.x + backgroundOffset.x,
            camera.transform.position.y + backgroundOffset.y,
            background.transform.position.z);

        yield return new WaitForSeconds(secondsBeforePan);

        while (elapsedTime < secondsToPan)
        {
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / secondsToPan);
            camera.transform.position = currentPosition;
            background.transform.position = new Vector3(currentPosition.x + backgroundOffset.x, currentPosition.y + backgroundOffset.y, background.transform.position.z);

            elapsedTime += Time.deltaTime;
            //Debug.Log("elapsedTime: " + elapsedTime);
            yield return null;
        }

        camera.transform.position = endPosition;
    }
}
