using System.Collections;
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
    public float stuffRadius = 4.5f;
    public ShipMovement shipMovement;

    // Use this for initialization
    void Start () {
        Pineapple.transform.position = generateRandomCoords(stuffRadius);
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
        // stop playing everything
        if (shipMovement.superDangerSource != null) SoundManager.instance.StopSFX(shipMovement.superDangerSource);
        if (shipMovement.dangerSource != null) SoundManager.instance.StopSFX(shipMovement.dangerSource);
        if (shipMovement.warningSource != null) SoundManager.instance.StopSFX(shipMovement.warningSource);
        if (shipMovement.slightWarningSource != null) SoundManager.instance.StopSFX(shipMovement.slightWarningSource);
        Win.transform.position = new Vector2(pineapple.transform.position.x, pineapple.transform.position.z);
        pineapple.SetActive(false);
        Win.Play();
        yield return new WaitForSeconds(2.0f);
        pineapple.SetActive(true);
        pineapple.transform.position = generateRandomCoords(stuffRadius);
        GameObject[] OldMines = GameObject.FindGameObjectsWithTag("Mine");
        foreach(GameObject OldMine in OldMines)
        {
            Destroy(OldMine);
        }
        RandomMinePlacement(NumberMines);
    }

    public IEnumerator reset(GameObject ship)
    {
        // stop playing everything
        if (shipMovement.superDangerSource != null) SoundManager.instance.StopSFX(shipMovement.superDangerSource);
        if (shipMovement.dangerSource != null) SoundManager.instance.StopSFX(shipMovement.dangerSource);
        if (shipMovement.warningSource != null) SoundManager.instance.StopSFX(shipMovement.warningSource);
        if (shipMovement.slightWarningSource != null) SoundManager.instance.StopSFX(shipMovement.slightWarningSource);
   
        Death.transform.position = new Vector2(ship.transform.position.x,ship.transform.position.z);
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
        ship.transform.position = generateRandomCoords(stuffRadius);
    }

    void RandomMinePlacement(int number)
    {
        for(int i = 0; i < number; i++)
        {
            Vector3 pos = generateRandomCoords(stuffRadius);
            while(Vector3.Distance(pos, Pineapple.transform.position) < MineThreshold && Vector3.Distance(pos, Ship.transform.position) < MineThreshold)
            {
                pos = generateRandomCoords(stuffRadius);
            }

            Instantiate(Mine, pos, Quaternion.identity).SetActive(true);
        }
    }

    public Vector3 generateRandomCoords(float radius) {
        float x = Random.Range(-radius, radius);
        float zDist = Mathf.Sqrt(radius * radius - x * x);
        float z = Random.Range(-zDist, zDist);
        return new Vector3(x, 0, z);
    }

}
