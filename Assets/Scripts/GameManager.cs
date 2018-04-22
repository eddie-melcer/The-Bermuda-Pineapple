using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public int NumberMines = 4;
    public float MineThreshold = 30;
    public GameObject Mine;
    public GameObject Ship;
    public GameObject Pineapple;
    public ParticleSystem Death;
    public ParticleSystem Win;
    public float stuffRadius = 4.5f;
    public ShipMovement shipMovement;
    public Camera WholeScene;
	public float safeRadius = 30;

    // Use this for initialization
    void Start () {
        if(Display.displays.Length > 1)
        {
          Display.displays[1].Activate();
        }

        Pineapple.transform.position = generateRandomCoords(stuffRadius) + new Vector3(0, 1.5f, 0);
        RandomMinePlacement(NumberMines);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void resetShip(GameObject ship, Collision collision)
    {
        StartCoroutine(reset(ship, collision));
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
        Win.transform.position = new Vector3(pineapple.transform.position.x, 0.0f,pineapple.transform.position.z);
        pineapple.SetActive(false);
        Win.Play();
        yield return new WaitForSeconds(4.5f);
        pineapple.SetActive(true);
        pineapple.transform.position = generateRandomCoords(stuffRadius) + new Vector3(0, 1.5f, 0);
        GameObject[] OldMines = GameObject.FindGameObjectsWithTag("Mine");
        foreach(GameObject OldMine in OldMines)
        {
            Destroy(OldMine);
        }
        RandomMinePlacement(NumberMines);
        //shipMovement.revive();
        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }

    public IEnumerator reset(GameObject ship,Collision collision)
    {
        // stop playing everything
        if (shipMovement.superDangerSource != null) SoundManager.instance.StopSFX(shipMovement.superDangerSource);
        if (shipMovement.dangerSource != null) SoundManager.instance.StopSFX(shipMovement.dangerSource);
        if (shipMovement.warningSource != null) SoundManager.instance.StopSFX(shipMovement.warningSource);
        if (shipMovement.slightWarningSource != null) SoundManager.instance.StopSFX(shipMovement.slightWarningSource);
   
        Death.transform.position = new Vector3(ship.transform.position.x,0.0f,ship.transform.position.z);
        Renderer[] rendererArray = ship.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in rendererArray)
        {
            r.enabled = false;
        }
        //Death.Play();
        yield return new WaitForSeconds(8.0f);
        foreach (MeshRenderer r in rendererArray)
        {
            r.enabled = true;
        }

		GameObject[] Mines = GameObject.FindGameObjectsWithTag("Mine");
		float minDistance = Vector3.Distance(ship.transform.position, Mines[0].transform.position);
        float currentDistance = 0;
        
        while(minDistance < safeRadius) {
            ship.transform.position = generateRandomCoords(stuffRadius);
            for(int i = 0; i < Mines.Length; i++) {
                currentDistance = Vector3.Distance(ship.transform.position, Mines[i].transform.position);
                if(currentDistance < minDistance) minDistance = currentDistance;
            }

        }
       
		shipMovement.revive();
        //Destroy(collision.gameObject);
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

    public Vector3 generateRandomCoords(float radius,float y = 0) {
        float x = Random.Range(-radius, radius);
        float zDist = Mathf.Sqrt(radius * radius - x * x);
        float z = Random.Range(-zDist, zDist);
        return new Vector3(x, y, z);
    }

}
