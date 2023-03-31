using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeActivator : MonoBehaviour
{

    GameObject bridge;
    public static GameObject spherePrefab;
    public GameObject Prefab;

    private static float Xcoordinate = 8f;
    private static float Ycoordinate = 38f;
    private static float Zcoordinate = 150f;

    private void Start()
    {
        spherePrefab = Prefab;
        bridge = GameObject.Find("Bridge");
        bridge.SetActive(false);
    }

    public static void Spawner()
    {
        Instantiate(spherePrefab, new Vector3(Xcoordinate, Ycoordinate, Zcoordinate), Quaternion.identity);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Sphere")
        {
            bridge.SetActive(true);
        }

        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<CharacterControls>().LoadCheckPoint();
        }
    }
}
