using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class shooter : MonoBehaviour
{
    [SerializeField] Transform shootPos;

    [SerializeField] public AudioSource shootItemSoundEffect;

    public GameObject[] fruits;
    public List<int> fruitToSpawnIndex = new List<int>();

    public Transform rotObj;
    private Vector2 aimDirection;


    private void Update()
    {
        Vector3 rot = Quaternion.AngleAxis(-90, Vector3.forward) * (Vector3.up * aimDirection.x + Vector3.left * aimDirection.y);

        if (aimDirection.x != 0 || aimDirection.y != 0)
        {
            rotObj.rotation = Quaternion.LookRotation(Vector3.forward, rot);
        }
    }

    public void OnAim(InputValue value)
    {
        aimDirection = value.Get<Vector2>();

    }

    public void OnShoot(InputValue value)
    {
        if (value.isPressed && fruitToSpawnIndex.Count > 0)
        {
            shootItemSoundEffect.Play();

            Vector3 rot = shootPos.eulerAngles + new Vector3(0, 0, 90);

            Instantiate(fruits[fruitToSpawnIndex[0]], shootPos.position, Quaternion.Euler(rot));
            fruitToSpawnIndex.RemoveAt(0);
        }
    }

    public bool checkIfInvFull()
    {
        if(fruitToSpawnIndex.Count >= 3)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public void addFruitToInv(int index)
    {
        fruitToSpawnIndex.Add(index);
    }
}
