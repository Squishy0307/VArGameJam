using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit_Spawner : MonoBehaviour
{
    public GameObject[] fruits;
    private int currentFruitIndex;

    private SpriteRenderer sr;

    [SerializeField] private AudioSource itemSpawnSound;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        currentFruitIndex = Random.Range(0, fruits.Length);
        sr.sprite = fruits[currentFruitIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        itemSpawnSound.Play();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            shooter shoot = collision.gameObject.GetComponent<shooter>();

            if (!shoot.checkIfInvFull())
            {
                shoot.addFruitToInv(currentFruitIndex);
                Destroy(gameObject);
            }
        }
    }
}
