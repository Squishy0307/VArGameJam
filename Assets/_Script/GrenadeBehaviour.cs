using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour
{

    public float LaunchVelocity = 5f;
    private Rigidbody2D RBRef;
    public LayerMask TargetLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        RBRef = GetComponent<Rigidbody2D>();
        RBRef.AddForce((transform.right * (Time.deltaTime * LaunchVelocity * 1000)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Watermelon collided");
        
        Collider2D[] FoundColliders = Physics2D.OverlapCircleAll(transform.position, 50f, TargetLayer);

        for (int i = 0; i < FoundColliders.Length; i++)
        {
            Debug.Log("Found Player: " + i);
            FoundColliders[i].gameObject.GetComponent<PlayerMovement>().playerDed();
        }
        
        Destroy(gameObject);
    }
}
