using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangShotBehaviour : MonoBehaviour
{

    public float ProjectileVelocity = 5f;
    public float ReturnSpeed = 5f;
    public float RotationRate = 5f;
    private Rigidbody2D RBRef;
    private Transform SpriteTransform;

    private float initialProjectileVelocity = 5f;

    // Start is called before the first frame update
    void Start()
    {
        RBRef = GetComponent<Rigidbody2D>();
        SpriteTransform = transform.GetChild(0);

        initialProjectileVelocity = ProjectileVelocity;
    }

    // Update is called once per frame
        void FixedUpdate()
        {
            RBRef.velocity = (transform.right * (Time.deltaTime * ProjectileVelocity * 100));
            SpriteTransform.transform.Rotate(new Vector3(0, 0, RotationRate * Time.deltaTime * 100));

            ProjectileVelocity -= Time.deltaTime * ReturnSpeed;
            ProjectileVelocity = Mathf.Clamp(ProjectileVelocity, -initialProjectileVelocity, initialProjectileVelocity);

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

