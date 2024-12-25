using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSlimeAnimation : MonoBehaviour
{

    DemoSlime slime = null;
    Animator animator = null;
    // Start is called before the first frame update
    void Start()
    {
        slime = GetComponentInParent<DemoSlime>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slime.gridPosition.x > 0)
            animator.SetFloat("facing_x", 1f);
        else
            animator.SetFloat("facing_x", -1f);

    }
}
