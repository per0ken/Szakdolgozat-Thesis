using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{

    public GameObject spherePrefab;
    private float Xcoordinate;
    private float Ycoordinate = 25f;
    private float Zcoordinate = 170f;
    float RandomTime = 0.5f;

    private void Start()
    {
        Invoke("Spawner", 0.01f);
    }

    void Spawner()
    {
        RandomTime = Random.Range(1.25f, 3.5f);
        float RandomX = Random.Range(-3.5f, 14.8f);
        Xcoordinate = RandomX;

        GameObject clone = Instantiate(spherePrefab, new Vector3(Xcoordinate, Ycoordinate, Zcoordinate), Quaternion.identity);
        //Debug.Log("Sphere spawned!");
        Destroy(clone, 10);
        Debug.Log("clone destroyed");
        Invoke("Spawner",RandomTime);
    }

}
