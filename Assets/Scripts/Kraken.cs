using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kraken : MonoBehaviour {

    public GameObject tentacle;

    public float sinkSpeed = 10;
    public float sinkStart = 5;

    protected float lifetime = -1;
    private ShipMovement ship;
    private GameObject shipMesh;

    public void StartAnimation()
    {
        lifetime = 0;
    }

	void Start () {
        ship = FindObjectOfType<ShipMovement>();
        shipMesh = ship.transform.Find("SpacePirateShip").gameObject;
	}

	void Update () {
        if (lifetime < -1)
            return;
        lifetime += Time.deltaTime;
        if(lifetime >= sinkStart)
        {
            shipMesh.transform.position = new Vector3(shipMesh.transform.position.x, shipMesh.transform.position.y - sinkSpeed * Time.deltaTime, shipMesh.transform.position.z);
        }
        if (lifetime > sinkStart+3)
            Destroy(this.gameObject);
	}
}
