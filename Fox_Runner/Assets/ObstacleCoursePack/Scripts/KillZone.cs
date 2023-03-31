﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
			col.gameObject.GetComponent<CharacterControls>().LoadCheckPoint();
		}

        if (col.gameObject.tag == "Sphere")
        {
            Destroy(col);
        }
        if(col.gameObject.tag == "Sphere")
        {
            Destroy(col);
            BridgeActivator.Spawner();
        }
    }
}
