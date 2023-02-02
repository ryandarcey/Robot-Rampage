using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    // Used to update ammo
    PlayerStats stats;

    // Damage and range for the current gun
    public float damage = 5f;
    public float range = 30f;

    // Layer for checking enemy collisions
    public LayerMask enemyLayer;

    // Crosshair for player
    public CanvasRenderer crosshair;

    // Player camera
    public Camera camera;

    // Sound effect for shooting
    public AudioSource shot;

    // Animator for player
    public Animator animator;

    // Check if player can shoot an enemy
    bool canShoot;
    public float horizontalRange = 10; // angle within which enemies can still be shot (degrees)

    // Used to manage time between shots
    float coolDownTime = .5f;
    float nextShotTime = 0f;

    private ArrayList enemyList;
    public GameObject playerModel;
    public GameObject playerCameraRoot;


    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
        {
            animator = transform.GetComponent<Animator>();
        }

        if (camera == null)
        {
            camera = Camera.main;
        }

        if (crosshair == null)
        {
            crosshair = FindObjectsOfType<CanvasRenderer>()[3];
        }

        if (shot == null)
        {
            shot = transform.GetComponent<AudioSource>();
        }

        if (stats == null)
        {
            stats = transform.GetComponentInParent<PlayerStats>();
        }
    }

    // Update is called once per frame
    void Update()
    {
		// DEBUG RAYS 
		Debug.DrawRay(playerCameraRoot.transform.position, range*playerModel.transform.forward, Color.cyan, Time.deltaTime);
        Vector3 forward = playerModel.transform.forward;
		Vector3 DEBUGminus = Vector3.Normalize( Quaternion.Euler(0, -horizontalRange/2, 0) * forward );
		Vector3 DEBUGplus = Vector3.Normalize(Quaternion.Euler(0, horizontalRange / 2, 0) * forward);
		Debug.DrawRay(playerCameraRoot.transform.position, range * DEBUGminus, Color.yellow, Time.deltaTime);
		Debug.DrawRay(playerCameraRoot.transform.position, range * DEBUGplus, Color.yellow, Time.deltaTime);

		// new way of shooting (only based on horizonal position of enemies)
		GetEnemyList();
        FindEnemiesThatCanBeShot();

		if (enemyList.Count > 0)
		{
			canShoot = true;
			crosshair.SetColor(Color.red);
		}
		else
		{
			canShoot = false;
			crosshair.SetColor(Color.white);
		}

		// Shoots the gun if the cooldown time is over
		if (nextShotTime < Time.time)
		{
			if (Input.GetButtonDown("Fire1"))   // currently left mouse button
			{
                if (canShoot)
                {
                    // Sets the next time a shot can be made and checks if a shot connected
                    nextShotTime = coolDownTime + Time.time;
                    animator.SetBool("Shoot", true);
                    ShootGun();
                }
			}
		}
	}

    void ShootGun()
    {
        GameObject enemy = GetEnemyWithSmallestXZAngle();   // get enemy that is closest to direction player is facing
        EnemyAction enemyAction = enemy.GetComponent<EnemyAction>();

		float newAmmo = stats.loseAmmo();
		if (newAmmo > 0)
		{
			// Play sound
			shot.Play();

			// Only deal damage if the player is shooting at an enemy. Calls specific script within enemy that contains health
			if (canShoot)
			{
				if (enemyAction != null)
				{
					enemyAction.isHit(damage);
                    stats.ShotGun();
                    stats.ShotHit();
                    // TODO: tell player stats / round manager if shot that was fired hit enemy
                    //          --> may have enemy itself tell RM this
                    //          (currently not necessary -- all shots fired hit enemy)
				}
			}
		}
	}

    void GetEnemyList()
    {
        enemyList = new ArrayList();
        GameObject[] GOArray = SceneManager.GetActiveScene().GetRootGameObjects();// GetComponents<GameObject>();

        for(int i = 0; i < GOArray.Length; i++)
        {
            GameObject go = GOArray[i];
            if(go.layer == 6 && go.GetComponent<EnemyAction>() != null)
            {
                enemyList.Add(go);
            }
        }
    }

    private float GetXZDistanceToPlayer(GameObject gameObject)
    {
        Vector3 playerPos = playerModel.transform.position;
        Vector3 gameObjectPos = gameObject.transform.position;
        float xDist = playerPos.x - gameObjectPos.x;
        float zDist = playerPos.z - gameObjectPos.z;
        return ( Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(zDist, 2)) );
    }

    private float GetXZAngleToPlayer(GameObject gameObject)
    {
		Vector3 playerPos = playerModel.transform.position;
        playerPos[1] = 0;   // set Y to 0
        Vector3 playerFacing = playerModel.transform.forward;
        playerFacing[1] = 0;    // set Y to 0

		Vector3 gameObjectPos = gameObject.transform.position;
        gameObjectPos[1] = 0;   // set Y to 0
        Vector3 gameObjectDir = gameObjectPos - playerPos;
        gameObjectDir[1] = 0;   // set Y to 0

        float angle = Vector3.Angle(playerFacing, gameObjectDir);
        return angle;
	}

    private bool CanPlayerSeeEnemy(GameObject gameObject)
    {
        RaycastHit hit;
        Vector3 playerPos = playerCameraRoot.transform.position;    // really want camera position for this (?)
        Vector3 gameObjectPos = gameObject.transform.position;
        Vector3 gameObjectDir = gameObjectPos - playerPos;

        
        if (Physics.Raycast(playerPos, gameObjectDir, out hit))
        {
            bool canSeeEnemy = hit.collider.gameObject.layer == 6;
            //Debug.Log("CAN PLAYER SEE ENEMY:  " + canSeeEnemy);
            if (canSeeEnemy)
            {
                Debug.DrawRay(playerPos, gameObjectDir, Color.green, 0.5f);
            }
            else
            {
                Debug.DrawRay(playerPos, gameObjectDir, Color.red, 0.5f);
            }
			return canSeeEnemy;
        }
        return false;
    }

    private void FindEnemiesThatCanBeShot()
    {
        // set enemyList to only enemies within range of player
		ArrayList updatedEnemyList = new ArrayList();
        for(int i = 0; i < enemyList.Count; i++)
        {
            GameObject enemy = (GameObject)enemyList[i];
            if(GetXZDistanceToPlayer(enemy) < range)
            {
                updatedEnemyList.Add(enemy);
            }
        }

        enemyList = updatedEnemyList;

        // set enemyList to only enemies within 
        // horizontal angle of the direction the player is facing
        updatedEnemyList = new ArrayList();
		for (int i = 0; i < enemyList.Count; i++)
		{
			GameObject enemy = (GameObject)enemyList[i];
			if (GetXZAngleToPlayer(enemy) < horizontalRange/2)
			{
				updatedEnemyList.Add(enemy);
			}
		}

		enemyList = updatedEnemyList;

        // set enemy list to only enemies the player has line of sight to
		updatedEnemyList = new ArrayList();
		for (int i = 0; i < enemyList.Count; i++)
		{
			GameObject enemy = (GameObject)enemyList[i];
			if (CanPlayerSeeEnemy(enemy))
			{
				updatedEnemyList.Add(enemy);
			}
		}

		enemyList = updatedEnemyList;
	}

    private GameObject GetEnemyWithSmallestXZAngle()
    {
        GameObject enemy = (GameObject)enemyList[0];
        float xzAngle = GetXZAngleToPlayer(enemy);

        for (int i = 0; i < enemyList.Count; i++)
        {
            GameObject e = (GameObject)enemyList[i];
            float xzA = GetXZAngleToPlayer(e);
            if (xzA < xzAngle)
            {
                xzAngle = xzA;
                enemy = e;
            }
        }

        return enemy;
    }

}
