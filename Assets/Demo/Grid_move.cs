using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_move : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform move_point;
    public float speed = 5;
    
    void Start()
    {
        move_point.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, move_point.position, speed * Time.deltaTime);

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {
            move_point.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) == 1f)
        {
            move_point.position += new Vector3(0f, Input.GetAxis("Vertical"), 0f);
        }
    }
}
