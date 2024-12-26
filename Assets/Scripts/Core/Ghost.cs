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



    public Camera mainCamera = null;


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
            mouseGridPosition.z = 0;

            //Debug.Log("Trying to set Ghost to " + mouseGridPosition.ToString() + ", dist: " + Vector3Int.Distance(mouseGridPosition, currentPossessor.gridPosition).ToString());
            if (Vector3Int.Distance(mouseGridPosition, currentPossessor.gridPosition) < 3.58f)
            {
                Mobile targetMob = GameManager.Instance.FindMobileByCoordinate(mouseGridPosition);
                if (targetMob != null)
                {
                    swapTarget = targetMob;
                    isSwapping = true;
                }
            }
            
        }
    }


    private void LateUpdate()
    {
        /*    Camera Movement   */
        transform.position = currentPossessor.transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, targetPosition, 0.05f);
        mainCamera.transform.position = smoothedPosition;
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
