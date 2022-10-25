using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightShotBehaviour : MonoBehaviour
{
    public float ProjectileVelocity = 5f;
    public float RotationRate = 5f;
    private Rigidbody2D RBRef;
    private Transform SpriteTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        RBRef = GetComponent<Rigidbody2D>();
        SpriteTransform = transform.GetChild(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RBRef.velocity = (transform.right * (Time.deltaTime * ProjectileVelocity * 100));
        SpriteTransform.transform.Rotate(new Vector3(0, 0, RotationRate * Time.deltaTime * 100));
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {

        }
        if (col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
