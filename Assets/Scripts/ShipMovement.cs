using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

    public float Thrust;
    public int AngularVelocity;
    public GameObject Target;
    public List<GameObject> Mines;
    public DistanceClass PreviousClass;
    public float SuperDangerThreshold = 1.0f;
    public float DangerThreshold = 2.0f;
    public float WarningThreshold = 3.0f;
    public float SlightWarningThreshold = 4.0f;
	public float worldRadius = 5.0f;
    public float shipSpawnRadius = 4.0f;
    public bool  shipAlive;


    public AudioSource superDangerSource { get; protected set; }
    public AudioSource dangerSource { get; protected set; }
    public AudioSource warningSource { get; protected set; }
    public AudioSource slightWarningSource { get; protected set; }

    protected GameManager manager;
    protected Rigidbody rigidBody;

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
    void Awake () {
        manager = FindObjectOfType<GameManager>();
        rigidBody = GetComponentInChildren<Rigidbody>();
        shipAlive = true;
    }

    void MoveShip()
    {
        Renderer[] rendererArray = this.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer r in rendererArray)
        {
            if(!r.enabled)
            {
                rigidBody.velocity = Vector2.zero;
                return;
            }
        }
        rigidBody.AddForce(this.transform.forward * Thrust);
    }

    private float rotAngle = 0;
    public float sensitivity = 1;

    // Update is called once per frame
    void Update() {

        if(shipAlive) {
            // Handle User Input
            MoveShip();

            if ((Input.GetAxis("Horizontal") != 0) || (Input.GetAxis("Vertical") != 0))
            {
                //rigidBody.angularVelocity = -30 * Input.GetAxis("Horizontal");
                float x_input = Input.GetAxis("Horizontal");
                float y_input = Input.GetAxis("Vertical");

                int heading = (int) (Mathf.Atan2(y_input , x_input) * 180 / Mathf.PI) - 90;

                rotAngle += x_input * sensitivity;

                this.transform.eulerAngles = new Vector3(0, rotAngle, 0);
            }
            else
            {
                rigidBody.angularVelocity = Vector3.zero;
            }
        }
        else {
            rigidBody.velocity = Vector3.zero;
        }
        

        // Wrap Ship Around
        if((this.transform.position.x * this.transform.position.x + this.transform.position.z*this.transform.position.z) > worldRadius*worldRadius) {
            this.transform.position = new Vector3(this.transform.position.x * -0.99f, this.transform.position.y, this.transform.position.z * -0.99f);
        }

        // Mine Sounds
        GameObject[] Mines = GameObject.FindGameObjectsWithTag("Mine");
        DistanceClass CurrentClass = DistanceClass.Safe;
        GameObject closestMine = null;
        if (Mines.Length > 0)
        {
            float mindist = Vector3.Distance(this.transform.position, Mines[0].transform.position);
            closestMine = Mines[0];
            for (int i = 0; i < Mines.Length; i++)
            {
                if (Vector3.Distance(this.transform.position, Mines[i].transform.position) < mindist)
                {
                    closestMine = Mines[i];
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
            }
            else if (mindist <= WarningThreshold)
            {
                CurrentClass = DistanceClass.Warning;
            }
            else if (mindist <= SlightWarningThreshold)
            {
                CurrentClass = DistanceClass.SlightWarning;
            }
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
                    case DistanceClass.SuperDanger: superDangerSource = SoundManager.instance.PlaySFX(SoundEffect.Warning4, true, 0, null, 0, 0, closestMine); break;
                    case DistanceClass.Danger: dangerSource = SoundManager.instance.PlaySFX(SoundEffect.Warning3, true, 0, null, 0, 0, closestMine); break;
                    case DistanceClass.Warning: warningSource = SoundManager.instance.PlaySFX(SoundEffect.Warning2, true, 0, null, 0, 0, closestMine); break;
                    case DistanceClass.SlightWarning: slightWarningSource = SoundManager.instance.PlaySFX(SoundEffect.Warning1, true, 0, null, 0, 0, closestMine); break;
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

    private void OnCollisionEnter(Collision collision)
    {
        print(collision);
        if (collision.gameObject.tag == "Mine")
        {
            shipAlive = false;
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
        Target.transform.position = manager.generateRandomCoords(shipSpawnRadius);

        // Mines

    }

	public void revive()
	{
		shipAlive = true;
	}
}
