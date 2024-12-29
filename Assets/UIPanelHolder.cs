using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelHolder : MonoBehaviour
{

    List<UIFloatPanel> HeldPanelList = new();
    List<UIFloatPanel> GhostPanelList = new();


    public GameObject panelPrefab;
    private Canvas canvas;
    private CanvasRenderer cvRenderer; 

    bool isOn = false;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        cvRenderer = GetComponentInChildren<CanvasRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOn = !isOn;
            if (isOn)
            {
                SetupPanels(Ghost.Instance.currentPossessor);
                SetupGhostPanels();

                GameManager.Instance.timeflowOption = TimeMode.PAUSED;
            }
            else
            {
                DisablePanels();
                GameManager.Instance.timeflowOption = TimeMode.MANUAL;
            }
        }

        /* Lining up curves */
        LineUpPanels();
        LineUpGhostPanels();


        Ghost.Instance.mainCamera.orthographicSize = 0.95f * Ghost.Instance.mainCamera.orthographicSize + 0.05f * (isOn?2.5f:5f);
        if (Ghost.Instance.mainCamera.orthographicSize > 4.99f) Ghost.Instance.mainCamera.orthographicSize = 5f;
        if (Ghost.Instance.mainCamera.orthographicSize < 2.51f) Ghost.Instance.mainCamera.orthographicSize = 2.5f;

        if (isOn && Input.GetMouseButtonDown(0))
        {
            // Enumerate HeldPanel, if clicked then move
            // for each panel in HeldPanelList (on the right), if clicked then move.
            // If no one is clicked in this group, then enumerate over the
            // GhostPanelList (left panels). 
            bool hasClickedOne = false;
            foreach (var fpanel in HeldPanelList)
            {
                if (fpanel.spellMetadata.Length == 0) continue;

                if (fpanel.IsMouseOverlapped())
                {
                    HeldPanelList.Remove(fpanel);
                    GhostPanelList.Add(fpanel);
                    Ghost.Instance.currentPossessor.spells.Remove(fpanel.spellMetadata + "~" + fpanel.fullText);
                    Ghost.Instance.collectedSpells.Add(fpanel.spellMetadata + "~" + fpanel.fullText);
                    hasClickedOne = true;
                    break;
                }
            }
            if (!hasClickedOne)
            {
                foreach (var fpanel in GhostPanelList)
                {
                    if (fpanel.spellMetadata.Length == 0) continue;
                    if (!Ghost.Instance.currentPossessor.acceptingSpellPrefixes.Contains(fpanel.spellMetadata.Substring(0, 1))) continue;

                    if (fpanel.IsMouseOverlapped())
                    {
                        GhostPanelList.Remove(fpanel);
                        HeldPanelList.Add(fpanel);
                        Ghost.Instance.currentPossessor.spells.Add(fpanel.spellMetadata + "~" + fpanel.fullText);
                        Ghost.Instance.collectedSpells.Remove(fpanel.spellMetadata + "~" + fpanel.fullText);
                        break;
                    }
                }
            }
            
        }
    }

    void DisablePanels()
    {
        foreach (UIFloatPanel fpanel in HeldPanelList)
        {
            Destroy(fpanel.gameObject);
        }
        HeldPanelList.Clear();

        foreach (UIFloatPanel fpanel in GhostPanelList)
        {
            Destroy(fpanel.gameObject);
        }
        GhostPanelList.Clear();
    }

    void SetupGhostPanels()
    {
        foreach (string line in Ghost.Instance.collectedSpells)
        {
            GameObject gobj = Instantiate(panelPrefab);
            UIFloatPanel panel = gobj.GetComponent<UIFloatPanel>();

            var lineparts = line.Split("~");
            if(lineparts.Length == 1)
            {
                panel.fullText = line;
            }
            else
            {
                panel.spellMetadata = lineparts[0];
                panel.fullText = lineparts[1];
            }
            
            gobj.transform.SetParent(transform);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, 100);
            GhostPanelList.Add(panel);
        }
    }


    void SetupPanels(Mobile mob)
    {
        int thepos = 300;
        foreach (string line in mob.spells)
        {
            GameObject gobj = Instantiate(panelPrefab);
            UIFloatPanel panel = gobj.GetComponent<UIFloatPanel>();

            var lineparts = line.Split("~");
            if (lineparts.Length == 1)
            {
                panel.fullText = line;
            }
            else
            {
                panel.spellMetadata = lineparts[0];
                panel.fullText = lineparts[1];
            }

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
            if (fpanel.panelTransform == null) continue;
            var targetPos = GameUtils.CubicBezierVector2(curveLength>0?(idx / curveLength):0.5f, controlPoint0, controlPoint1, controlPoint2, controlPoint3);
            fpanel.panelTransform.anchoredPosition = Vector2.Lerp(fpanel.panelTransform.anchoredPosition, targetPos, Time.deltaTime * 5f);

            idx += 1;
            if (fpanel.isSelected) idx += 2;
        }
    }

    void LineUpGhostPanels()
    {
        if (GhostPanelList.Count == 0)
        {
            return;
        }
        Vector2 controlPoint0 = new Vector2(200, 450);
        Vector2 controlPoint1 = new Vector2(100, 350);
        Vector2 controlPoint2 = new Vector2(100, 250);
        Vector2 controlPoint3 = new Vector2(200, 150);

        float curveLength = GhostPanelList.Count - 1;
        foreach (UIFloatPanel fpanel in GhostPanelList)
        {
            if (fpanel.isSelected)
            {
                curveLength += 2;
            }
        }
        int idx = 0;

        foreach (UIFloatPanel fpanel in GhostPanelList)
        {
            if (fpanel.panelTransform == null) continue;
            var targetPos = GameUtils.CubicBezierVector2(curveLength > 0 ? (idx / curveLength) : 0.5f, controlPoint0, controlPoint1, controlPoint2, controlPoint3);
            fpanel.panelTransform.anchoredPosition = Vector2.Lerp(fpanel.panelTransform.anchoredPosition, targetPos, Time.deltaTime * 5f);

            idx += 1;
            if (fpanel.isSelected) idx += 2;
        }
    }
}
