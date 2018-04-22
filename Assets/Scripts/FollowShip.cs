using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShip : MonoBehaviour {
    public GameObject Ship;

	// Use this for initialization
	void Start () {
		this.transform.position = new Vector3(Ship.transform.position.x, Ship.transform.position.y, this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(Ship.transform.position.x, Ship.transform.position.y, this.transform.position.z);
    }
}
