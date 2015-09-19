using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour {

    public GameObject roadPrefab;
    public LinkedList<Transform> roads = new LinkedList<Transform>();
    public float scrollSpeed = 20f;
    public int numberOfRoads = 5;

    private Transform roadParent;
    private float roadLength;

	// Use this for initialization
	void Start () {
        Transform tempRoad = transform.GetChild(0);
        Destroy(tempRoad.gameObject);

        roadLength = GetComponentInChildren<MeshFilter>().mesh.bounds.size.z;

        roadParent = new GameObject().transform;
        roadParent.name = "Roads";

        roadPrefab.CreatePool(numberOfRoads);
        // Spawn the road tiles
        for (int i = 0; i <= numberOfRoads; i++) {
            GameObject road = roadPrefab.Spawn();
            road.transform.parent = roadParent.transform;

            road.transform.Translate(0, 0, i * roadLength);
            roads.AddLast(road.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
        Transform firstRoad = roads.First.Value;
        Transform lastRoad = roads.Last.Value;

        // Recycle road tiles as they move out of view
        if (firstRoad.localPosition.z < -15f) {
            roads.Remove(firstRoad);
            firstRoad.Recycle();

            Transform newRoad = roadPrefab.Spawn(new Vector3(0, 0, lastRoad.localPosition.z + roadLength)).transform;
            newRoad.parent = roadParent;
            roads.AddLast(newRoad);
        }

        // Move road tiles like a conveyor belt
        foreach(Transform road in roads) {
            road.Translate(0, 0, -scrollSpeed * Time.deltaTime);
        }
	}
}
