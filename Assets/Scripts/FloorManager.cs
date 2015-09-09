﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorManager : MonoBehaviour {

    public Transform floorParent;
    public List<Transform> floors = new List<Transform>();
    public float speed;

    private int numberOfFloors = 5;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        floorParent.Translate(Vector3.back * speed * Time.deltaTime);
	}
}
