using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSlimeAnimation : MonoBehaviour
{

    DemoSlime slime = null;

    public Sprite[] sprites;
    private SpriteRenderer sprRenderer;


    private Vector3 absolutePosition;
    private float moveSpeed = 4.5f;
    private Vector3 currentVelocity;


    // Start is called before the first frame update
    void Start()
    {
        slime = GetComponentInParent<DemoSlime>();
        absolutePosition = slime.transform.position;
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        // Moving
        var targetPosition = slime.IsMoving() ? slime.GetTargetPositionBuffer() : slime.transform.position;
        absolutePosition = Vector3.SmoothDamp(absolutePosition, targetPosition, ref currentVelocity, 1f / moveSpeed);
        transform.position = absolutePosition;
        if (Vector3.Distance(targetPosition, transform.position) < 0.01f)
            transform.position = targetPosition;
        else
        {
            // shaking
            transform.position += new Vector3(0f, Mathf.Sin(Time.time * 0.1f) * 0.02f, 0f);
        }


        if (0 <= slime.stamina && slime.stamina <= 3)
            sprRenderer.sprite = sprites[3 - slime.stamina];
    }
}
