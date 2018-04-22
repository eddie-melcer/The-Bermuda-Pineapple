﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int NumberMines = 4;
    public float MineThreshold = 2;
    public GameObject Mine;
    public GameObject Ship;
    public GameObject Pineapple;
    public ParticleSystem Death;
    public ParticleSystem Win;

    // Use this for initialization
    void Start () {
        RandomMinePlacement(NumberMines);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void resetShip(GameObject ship)
    {
        StartCoroutine(reset(ship));
    }

    public void WinReset(GameObject ship, GameObject pineapple)
    {
        StartCoroutine(WinResetHelper(ship,pineapple));
    }

    public IEnumerator WinResetHelper(GameObject ship, GameObject pineapple)
    {
        Win.transform.position = new Vector2(pineapple.transform.position.x, pineapple.transform.position.y);
        pineapple.SetActive(false);
        Win.Play();
        yield return new WaitForSeconds(2.0f);
        pineapple.SetActive(true);
        pineapple.transform.position = new Vector2(Random.Range(-9f, 9f), Random.Range(-5f, 5f));
        GameObject[] OldMines = GameObject.FindGameObjectsWithTag("Mine");
        foreach(GameObject OldMine in OldMines)
        {
            Destroy(OldMine);
        }
        RandomMinePlacement(NumberMines);
    }

    public IEnumerator reset(GameObject ship)
    {
        
        Death.transform.position = new Vector2(ship.transform.position.x,ship.transform.position.y);
        Renderer[] rendererArray = ship.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in rendererArray)
        {
            r.enabled = false;
        }
        Death.Play();
        yield return new WaitForSeconds(2.0f);
        foreach (MeshRenderer r in rendererArray)
        {
            r.enabled = true;
        }
        ship.transform.position = new Vector2(Random.Range(-9f, 9f), Random.Range(-5f, 5f));
    }

    void RandomMinePlacement(int number)
    {
        for(int i = 0; i < number; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-9f, 9f), Random.Range(-5f, 5f), 0);
            while(Vector3.Distance(pos, Pineapple.transform.position) < MineThreshold && Vector3.Distance(pos, Ship.transform.position) < MineThreshold)
            {
                pos = new Vector3(Random.Range(-9f, 9f), Random.Range(-5f, 5f), 0);
            }

            Instantiate(Mine, pos, Quaternion.identity).SetActive(true);
        }
    }
}
