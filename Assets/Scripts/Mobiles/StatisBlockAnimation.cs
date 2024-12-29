using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisBlockAnimation : MonoBehaviour
{

    DemoPathPoint statisBlock = null;
    SpriteRenderer sprRenderer = null;

    public Sprite[] frames;

    // Start is called before the first frame update
    void Start()
    {
        statisBlock = GetComponentInParent<DemoPathPoint>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


            sprRenderer.sprite = statisBlock.isPossessed ? frames[0] : frames[1];

           

    }
}
