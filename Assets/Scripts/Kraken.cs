using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kraken : MonoBehaviour {

    public GameObject tentacle;

    public float sinkSpeed = 10;
    public float sinkStart = 5;
    public float rotationSpeed = 6;

    public float lifetime = -1;
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
        if (lifetime < 0)
            return;
        lifetime += Time.deltaTime;
        if(lifetime >= sinkStart)
        {
            shipMesh.transform.position = new Vector3(shipMesh.transform.position.x + sinkSpeed * Time.deltaTime * 0.3f, shipMesh.transform.position.y - sinkSpeed * Time.deltaTime, shipMesh.transform.position.z);
            //shipMesh.transform.localRotation = Quaternion.Euler(new Vector3(0,0, (lifetime-sinkStart) * rotationSpeed));
        }
        if (lifetime > sinkStart+3)
            Destroy(this.gameObject);
	}
}
