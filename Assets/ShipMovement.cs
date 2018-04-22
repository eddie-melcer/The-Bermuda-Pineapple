using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

    public float Thrust;
    public int AngularVelocity;
    public GameObject Target;
    public GameManager manager;
    public List<GameObject> Mines;
    public DistanceClass PreviousClass;
    private AudioSource superDangerSource;
    private AudioSource dangerSource;
    private AudioSource warningSource;
    private AudioSource slightWarningSource;
    public float SuperDangerThreshold = 1.0f;
    public float DangerThreshold = 2.0f;
    public float WarningThreshold = 3.0f;
    public float SlightWarningThreshold = 4.0f;
    public float worldRadius = 5.0f;
    public float shipSpawnRadius = 4.0f;

    // An enum for maintaining the different sound effects available
    public enum DistanceClass
    {
        SuperDanger = -2,
        Danger = -1,
        Warning = 0,
        SlightWarning = 1,
        Safe = 2
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        // Handle User Input
        if (this.GetComponentInChildren<MeshRenderer>().enabled)
        {
            this.GetComponent<Rigidbody2D>().AddForce(this.transform.up * Thrust);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.GetComponent<Rigidbody2D>().angularVelocity = AngularVelocity;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.GetComponent<Rigidbody2D>().angularVelocity = -AngularVelocity;
        }
        else
        {
            this.GetComponent<Rigidbody2D>().angularVelocity = 0;
        }

        // Wrap Ship Around
        if((this.transform.position.x * this.transform.position.x + this.transform.position.y*this.transform.position.y) > worldRadius*worldRadius) {
            this.transform.position = new Vector2(this.transform.position.x * -0.99f, this.transform.position.y * -0.99f);
        }

        // Mine Sounds
        GameObject[] Mines = GameObject.FindGameObjectsWithTag("Mine");
        if(Mines.Length > 0)
        {
            float mindist = Vector3.Distance(this.transform.position, Mines[0].transform.position);
            DistanceClass CurrentClass = DistanceClass.Safe;
            GameObject closestMine = Mines[0];
            for(int i = 0; i < Mines.Length; i++)
            {
                if (Vector3.Distance(this.transform.position, Mines[i].transform.position) < mindist)
                {
                    mindist = Vector3.Distance(this.transform.position, Mines[i].transform.position);
                }
            }

            if (mindist <= SuperDangerThreshold)
            {
                CurrentClass = DistanceClass.SuperDanger;
            }
            else if (mindist <= DangerThreshold)
            {
                CurrentClass = DistanceClass.Danger;
            } else if (mindist <= WarningThreshold)
            {
                CurrentClass = DistanceClass.Warning;
            } else if (mindist <= SlightWarningThreshold)
            {
                CurrentClass = DistanceClass.SlightWarning;
            }

            if (CurrentClass == DistanceClass.Safe)
            {
                // stop playing everything
                if (superDangerSource != null) SoundManager.instance.StopSFX(superDangerSource);
                if (dangerSource != null) SoundManager.instance.StopSFX(dangerSource);
                if (warningSource != null) SoundManager.instance.StopSFX(warningSource);
                if (slightWarningSource != null) SoundManager.instance.StopSFX(slightWarningSource);
            }
            else
            {
                if(PreviousClass > CurrentClass)
                {
                    switch(CurrentClass)
                    {
                        case DistanceClass.SuperDanger: superDangerSource = SoundManager.instance.PlaySFX(SoundEffect.Warning4, true, 0, null, 0, 0, closestMine.transform.position); break;
                        case DistanceClass.Danger: dangerSource = SoundManager.instance.PlaySFX(SoundEffect.Warning3, true, 0, null, 0, 0, closestMine.transform.position); break;
                        case DistanceClass.Warning: warningSource = SoundManager.instance.PlaySFX(SoundEffect.Warning2, true, 0, null, 0, 0, closestMine.transform.position); break;
                        case DistanceClass.SlightWarning: slightWarningSource = SoundManager.instance.PlaySFX(SoundEffect.Warning1, true, 0, null, 0, 0, closestMine.transform.position); break;
                    }
                } else if (CurrentClass > PreviousClass)
                {
                    switch (CurrentClass)
                    {
                        case DistanceClass.Danger: if (superDangerSource != null) SoundManager.instance.StopSFX(superDangerSource); break;
                        case DistanceClass.Warning: if(dangerSource != null) SoundManager.instance.StopSFX(dangerSource); break;
                        case DistanceClass.SlightWarning: if(warningSource != null) SoundManager.instance.StopSFX(warningSource); break;
                    }
                }
            }

            PreviousClass = CurrentClass;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Mine")
        {
            Destroy(collision.gameObject);
            SoundManager.instance.PlaySFX(SoundEffect.Death);
            manager.resetShip(this.gameObject);
        }
        else if (collision.gameObject.name == "Pineapple")
        {
            //TODO: add delay to play success sound before resetting position
            //resetPosition();
            SoundManager.instance.PlaySFX(SoundEffect.Success);
            manager.WinReset(this.gameObject, Target);
        }
    }

    public void resetPosition()
    {
        // Target
        Target.transform.position = generateRandomCoords(shipSpawnRadius);

        // Mines

    }

    public Vector2 generateRandomCoords(float radius) {
        float x = Random.Range(-radius, radius);
        return new Vector2(x, (float)Mathf.Sqrt(radius*radius - x*x));
    }
}
