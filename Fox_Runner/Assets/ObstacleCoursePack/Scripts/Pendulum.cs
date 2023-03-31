﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
	public float speed = 1.5f;
	public float limit = 75f; // a mozgás limitje szögben
	public bool randomStart = false; // a kezdő pozíció
	private float random = 0;

	public AudioSource source;

    private void Start()
    {
		source = GetComponent<AudioSource>();
    }

    void Awake()
    {
		if(randomStart)
			random = Random.Range(0f, 1f);
	}

    void Update()
    {
		float angle = limit * Mathf.Sin(Time.time + random * speed);
		transform.localRotation = Quaternion.Euler(0, 0, angle);
	}
}
