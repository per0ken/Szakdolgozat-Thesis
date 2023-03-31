using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHit : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    private void Start()
    {
        source = GameObject.Find("Rotator").GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            source.PlayOneShot(clip);
        }
    }
}
