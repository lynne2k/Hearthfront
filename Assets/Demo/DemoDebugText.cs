using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemoDebugText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text =
            Ghost.Instance.currentPossessor.toolTip + "\n" +
            Ghost.Instance.currentPossessor.name + "\n" + 
            GameManager.Instance.timeflowOption.ToString() + ";  " + GameManager.Instance.GetCurrentTick().ToString();
    }
}
