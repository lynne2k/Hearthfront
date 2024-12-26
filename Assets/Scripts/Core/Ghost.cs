using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public static Ghost Instance { get; private set; }
    public Mobile defaultPossesser;

    public Mobile currentPossessor;
    public bool isSwapping;
    public Mobile swapTarget = null;



    public Camera camera = null;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Initialize();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isSwapping)
        {
            // get the game object it is trying to swap to
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseGridPosition = GameUtils.RoundVector3Int(mousePosition);
            Debug.Log("Trying to set Ghost to " + mouseGridPosition.ToString());
            Mobile targetMob = GameManager.Instance.FindMobileByCoordinate(mouseGridPosition);
            if (targetMob != null)
            {
                swapTarget = targetMob;
                isSwapping = true;
            }
        }
    }


    private void LateUpdate()
    {
        /*    Camera Movement   */
        transform.position = currentPossessor.transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(camera.transform.position, targetPosition, 0.05f);
        camera.transform.position = smoothedPosition;
    }

    public void OnTick()
    {
        // * swap

        if (isSwapping)
        {
            currentPossessor.OnUnpossess();
            swapTarget.OnPossess();
            currentPossessor = swapTarget;
            isSwapping = false;
        }
    }

    private void Initialize()
    {
        defaultPossesser.OnPossess();
        currentPossessor = defaultPossesser;
    }
}
