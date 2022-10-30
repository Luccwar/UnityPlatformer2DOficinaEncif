using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Animator anim;

    public float speed;

    public Transform upCol;
    public Transform downCol;

    public Transform heightPoint;
    private bool colliding;

    public LayerMask layer;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        upCol = this.transform.Find("upCol");
        downCol = this.transform.Find("downCol");
        heightPoint = this.transform.Find("heightPoint");

    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);

        if(rigidbody.velocity.x > 0)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if(rigidbody.velocity.x < 0)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {
            anim.SetBool("walk", false);
        }

        colliding = Physics2D.Raycast(upCol.position, downCol.position, 0.1f, layer);

        if(colliding)
        {
            speed = -speed;
        }
    }
}
