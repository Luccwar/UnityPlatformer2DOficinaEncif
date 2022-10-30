using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int boxLife = 3;
    public bool boxUp;
    public float boxForce = 5f;
    public int boxScore = 10;
    public GameObject explosionEffect;

    void Start(){
        if(boxUp)
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(this.GetComponentInChildren<BoxCollider2D>().offset.x, this.GetComponentInChildren<BoxCollider2D>().offset.y * -1);
    }

    void OnDestroy() {
        Instantiate(explosionEffect, transform.position, transform.rotation);
    }

}
