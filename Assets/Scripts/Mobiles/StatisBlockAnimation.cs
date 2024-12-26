using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisBlockAnimation : MonoBehaviour
{

    DemoPathPoint statisBlock = null;
    Animator animator = null;

    public Sprite[] frames;

    // Start is called before the first frame update
    void Start()
    {
        statisBlock = GetComponentInParent<DemoPathPoint>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (statisBlock.isPossessed)
        {
            animator.SetInteger("frame", 1);
        }
        else
        {
            animator.SetInteger("frame", 0);
        }
           

    }
}
