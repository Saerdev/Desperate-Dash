using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorManager : MonoBehaviour {

    public GameObject floorPrefab;
    public LinkedList<Transform> floors = new LinkedList<Transform>();
    public float scrollSpeed = 20f;
    public int numberOfFloors = 5;

    private Transform floorParent;

	// Use this for initialization
	void Start () {
        Transform tempFloor = transform.GetChild(0);
        Destroy(tempFloor.gameObject);

        floorParent = new GameObject().transform;
        floorParent.name = "Floors";

        floorPrefab.CreatePool(numberOfFloors);
        // Spawn the floor tiles
        for (int i = 0; i < numberOfFloors; i++) {
            GameObject floor = floorPrefab.Spawn();
            floor.transform.parent = floorParent.transform;

            // MODIFY 10 AFTER IMPORTING ACTUAL TILE ART ASSET
            floor.transform.Translate(0, 0, i * floor.transform.localScale.z * 10);
            floors.AddLast(floor.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
        Transform firstFloor = floors.First.Value;
        Transform lastFloor = floors.Last.Value;

        // Recycle floor tiles as they move out of view
        if (firstFloor.localPosition.z < -15f) {
            floors.Remove(firstFloor);
            firstFloor.Recycle();

            Transform newFloor = floorPrefab.Spawn(new Vector3(0, 0, lastFloor.localPosition.z + lastFloor.localScale.z * 10)).transform;
            newFloor.parent = floorParent;
            floors.AddLast(newFloor);
        }

        // Move floor tiles like a conveyor belt
        foreach(Transform floor in floors) {
            floor.Translate(0, 0, -scrollSpeed * Time.deltaTime);
        }
	}
}
