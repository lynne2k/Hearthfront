using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("HorizontalSpeed", Input.GetAxis("Horizontal"));
        /*Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);*/

    }
}
