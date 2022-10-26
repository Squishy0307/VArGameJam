using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeWayShotBehaviour : MonoBehaviour
{
    public GameObject Bullet;

    public float Spread = 30f;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Bullet, transform);
        
        GameObject Bullet2 = Instantiate(Bullet, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, Spread)));
        
        //Bullet2.transform.eulerAngles = new Vector3 (transform.rotation.x, transform.rotation.y, transform.rotation.z + Spread);
        
        GameObject Bullet3 = Instantiate(Bullet, transform.position ,Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,-Spread)));
        
        //Bullet3.transform.eulerAngles = new Vector3 (transform.rotation.x, transform.rotation.y, transform.rotation.z - Spread);
        
        transform.GetChild(0).gameObject.SetActive(false);
        
        Destroy(gameObject, 1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
