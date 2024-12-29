using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public static Ghost Instance { get; private set; }
    public Mobile defaultPossesser;

    public Mobile currentPossessor;
    public bool isSwapping;
    public float teleportCooldown = 0f;
    public Mobile swapTarget = null;

    public GameObject glowerPrefabbo;
    public Sprite hintSprite;

    [TextArea]
    public List<string> collectedSpells;

    private bool previewPositions = false;



    private List<SpriteRenderer> glowers = new();





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
            if (GameUtils.ManhattanDistance(mouseGridPosition, currentPossessor.gridPosition) < 3.1f)
            {
                
                Mobile targetMob = GameManager.Instance.FindMobileByCoordinate(mouseGridPosition);
                if (targetMob != null && (targetMob.keyFilter.Length == 0 || CheckKeys(targetMob.keyFilter)))
                {
                    swapTarget = targetMob;
                    isSwapping = true;
                }
            }
            
        }
        teleportCooldown -= Time.deltaTime;
        teleportCooldown = teleportCooldown > 0 ? teleportCooldown : 0f;

        if (Input.GetKey(KeyCode.LeftShift) && !previewPositions)
        {
            previewPositions = true;
            foreach (Mobile mob in GameManager.Instance.allMobiles)
            {
                GameObject gobj = Instantiate(glowerPrefabbo);
                SpriteRenderer rnd = gobj.GetComponent<SpriteRenderer>();
                gobj.transform.SetParent(transform);
                gobj.transform.position = mob.transform.position;
                if (mob.spells.Count > 0)
                {
                    rnd.color = new Color(0, 1, 0, 0.5f);
                }
                glowers.Add(rnd);
            }
            GameObject gobjb = Instantiate(glowerPrefabbo);
            SpriteRenderer rndb = gobjb.GetComponent<SpriteRenderer>();
            rndb.sprite = hintSprite;
            gobjb.transform.SetParent(transform);
            gobjb.transform.position = Ghost.Instance.currentPossessor.transform.position;
            glowers.Add(rndb);

        }
        else if (!Input.GetKey(KeyCode.LeftShift) && previewPositions)
        {
            previewPositions = false;
            foreach (SpriteRenderer glower in glowers)
            {
                Destroy(glower.gameObject);
            }
            glowers.Clear();
        }
    }


    private void LateUpdate()
    {
        /*    Camera Movement   */
        transform.position = currentPossessor.transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);

        Vector2 cursorPos = Input.mousePosition;
        var cursorDelta = mainCamera.ScreenToWorldPoint(cursorPos) - mainCamera.transform.position;
        targetPosition.x += cursorDelta.x / 30;
        targetPosition.y += cursorDelta.y / 30;




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
            teleportCooldown = 0.1f;
            GetComponent<AudioSource>().Play();
        }
    }

    private void Initialize()
    {
        defaultPossesser.OnPossess();
        currentPossessor = defaultPossesser;
    }

    public bool CallSwap(Mobile targetMob)
    {
        if (!isSwapping && targetMob != null && teleportCooldown == 0f)
        {
            swapTarget = targetMob;
            isSwapping = true;
            return true;
        }
        return false;
    }

    private bool CheckKeys(string keyFilter)
    {
        foreach (string key in Ghost.Instance.collectedSpells)
        {
            if (keyFilter.Contains(key[0]))
            {
                return true;
            }
        }
        return false;
    }
}
