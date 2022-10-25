using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingShotBehaviour : MonoBehaviour
{
    public float LaunchVelocity = 5f;
    private Rigidbody2D RBRef;

    public int NumberOfBounces = 3;
    int BounceCounter = 0;
    
    private Transform SpriteTransform;
    public float RotationRate = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        RBRef = GetComponent<Rigidbody2D>();
        RBRef.AddForce((transform.right * (Time.deltaTime * LaunchVelocity * 1000)));
        
        SpriteTransform = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        SpriteTransform.transform.Rotate(new Vector3(0, 0, RotationRate * Time.deltaTime * 100));
    }
    
    public void OnCollisionEnter2D(Collision2D col)
    {
        BounceCounter++;
        Debug.Log("Bounce Counter : " + BounceCounter);
        
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerMovement>().playerDed();
            Destroy(gameObject);
        }
        else if (BounceCounter > NumberOfBounces)
        {
            Destroy(gameObject);
        }
    }
}
