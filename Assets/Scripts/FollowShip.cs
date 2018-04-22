using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShip : MonoBehaviour {
    public GameObject Ship;

	// Use this for initialization
	void Start () {
		this.transform.position = new Vector3(Ship.transform.position.x, this.transform.position.y, Ship.transform.position.z);
    //this.transform.rotation = Ship.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(Ship.transform.position.x, this.transform.position.y, Ship.transform.position.z);
        //this.transform.rotation = Ship.transform.rotation;
    }
}
