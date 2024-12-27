using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelHolder : MonoBehaviour
{

    List<UIFloatPanel> HeldPanelList = new();


    public GameObject panelPrefab;

    bool isOn = false;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            isOn = !isOn;
            if (isOn)
            {
                SetupPanels(Ghost.Instance.currentPossessor);
                GameManager.Instance.timeflowOption = TimeMode.MANUAL;
            }
            else
            {
                DisablePanels();
            }
        }

        /* Lining up curves */
        LineUpPanels();
    }

    void DisablePanels()
    {
        foreach (UIFloatPanel fpanel in HeldPanelList)
        {
            Destroy(fpanel.gameObject);
        }
        HeldPanelList.Clear();
    }

    void SetupPanels(Mobile mob)
    {
        int thepos = 300;
        foreach (string line in mob.spells)
        {
            GameObject gobj = Instantiate(panelPrefab);
            UIFloatPanel panel = gobj.GetComponent<UIFloatPanel>();
            panel.fullText = line;
            gobj.transform.SetParent(transform);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, thepos);
            HeldPanelList.Add(panel);
            thepos -= 100;
        }
    }

    void LineUpPanels()
    {
        if (HeldPanelList.Count == 0)
        {
            return;
        }
        Vector2 controlPoint0 = new Vector2(550, 450); 
        Vector2 controlPoint1 = new Vector2(650, 350);
        Vector2 controlPoint2 = new Vector2(650, 250); 
        Vector2 controlPoint3 = new Vector2(550, 150);

        float curveLength = HeldPanelList.Count - 1;
        foreach (UIFloatPanel fpanel in HeldPanelList)
        {
            if (fpanel.isSelected) {
                curveLength += 2;
            }
        }
        int idx = 0;
        
        foreach (UIFloatPanel fpanel in HeldPanelList)
        {
            var targetPos = GameUtils.CubicBezierVector2(idx / curveLength, controlPoint0, controlPoint1, controlPoint2, controlPoint3);
            fpanel.panelTransform.anchoredPosition = Vector2.Lerp(fpanel.panelTransform.anchoredPosition, targetPos, Time.deltaTime * 5f);

            idx += 1;
            if (fpanel.isSelected) idx += 2;
        }

    }
}
