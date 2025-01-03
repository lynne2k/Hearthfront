using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFloatPanel : MonoBehaviour
{
    public RectTransform panelTransform;
    private TextMeshProUGUI panelText;
    private RectTransform panelTextTransform;
    private Canvas canvas;

    public string fullText;
    public string spellMetadata;
    private Vector2 expandedSize;
    public float animationSpeed = 5f;

    public bool isSelected = false;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        panelText = GetComponentInChildren<TextMeshProUGUI>();
        panelTransform = GetComponent<RectTransform>();
        panelTextTransform = panelText.GetComponent<RectTransform>();
        panelText.text = fullText;

        expandedSize = panelText.GetPreferredValues(270, 120) + new Vector2(30, 45);
        expandedSize.x = 300;
    }

    void Update()
    {
        panelText.text = fullText;
        Vector2 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelTransform, mousePosition, canvas.worldCamera, out Vector2 localMousePosition);
        isSelected = panelTransform.rect.Contains(localMousePosition);


            if (isSelected)
        {
            // Animate the panel expanding
            panelTransform.sizeDelta = Vector2.Lerp(panelTransform.sizeDelta, expandedSize, Time.deltaTime * animationSpeed);
            panelTextTransform.sizeDelta = panelTransform.sizeDelta - new Vector2(30, 30);
        }
        else
        {
            // Animate the panel shrinking
            panelTransform.sizeDelta = Vector2.Lerp(panelTransform.sizeDelta, new Vector2(200, 60), Time.deltaTime * animationSpeed); // Minimized size
            panelTextTransform.sizeDelta = panelTransform.sizeDelta - new Vector2(30, 30);
        }
    }

    public bool IsMouseOverlapped()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelTransform, mousePosition, canvas.worldCamera, out Vector2 localMousePosition);
        return panelTransform.rect.Contains(localMousePosition);
    }
}




