using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Static instance to ensure global access
    public static GameManager Instance { get; private set; }


    /* Global Variables -- 登记各种全局变量！ */
    
    public int varFoo = 0;
    public string varBar = "Player";
    public Vector3Int varTestCoord = new Vector3Int(1, 3, 3);

    /* 全图Savable寄存 */

    public Mobile[] allMobiles;


    /* 确保唯一性 */

    private void Awake()
    {
        // Check if another instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes

        allMobiles = FindObjectsOfType<Mobile>();
    }



    /* TEMPORARY: 输入检测不应该放在GameManager里面。仅限debug，之后会移除 */
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (Mobile mob in allMobiles)
            {
                mob.LoadData();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (Mobile mob in allMobiles)
            {
                mob.SaveData();
            }
        }
    }
}
