using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAnimation : MonoBehaviour
{

    DemoTrain train = null;

    public Sprite[] sprites;
    private SpriteRenderer sprRenderer;


    private Vector3 absolutePosition;
    private float moveSpeed = 3.5f;
    private Vector3 currentVelocity;

    void Start()
    {
        train = GetComponentInParent<DemoTrain>();
        absolutePosition = train.transform.position;
        sprRenderer = GetComponent<SpriteRenderer>();
        sprRenderer.sprite = sprites[0];
    }


    void LateUpdate()
    {
        // Moving
        var targetPosition = train.transform.position;
        absolutePosition = Vector3.SmoothDamp(absolutePosition, targetPosition, ref currentVelocity, 1f / moveSpeed);
        transform.position = absolutePosition;
        if (Vector3.Distance(targetPosition, transform.position) < 0.01f)
            transform.position = targetPosition;
        else
        {
            // shaking
            transform.position += new Vector3(0f, Mathf.Sin(Time.time * 0.1f) * 0.02f, 0f);
        }
    }
}



