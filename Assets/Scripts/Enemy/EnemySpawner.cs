﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour {
	[Header("Enemy Spawn Management")]
    //Enemy spawn every 10 sec at each spawnpoints
	public float respawnDuration = 5.0f;
	public List<GameObject> spawnPoints = new List<GameObject>();
	public GameObject target;
	
	[Header("Enemy Status")]
	public float startHealth = 100f;
	public float startMoveSpeed = 1f;
	public float startDamage = 15f;
	public int startEXP = 3;
	public int startFund = 5;

	// Increase all enemy stats every 30 seconds
	public float upgradeDuration = 60f;
	//Every 30 secs enemy stats increase and upgardeTimer reset to 0
    private float upgradeTimer;
	[SerializeField]
	private float currentHealth;
	[SerializeField]
	private float currentMoveSpeed;
	[SerializeField]
	private float currentDamage;
	[SerializeField]
	private int currentEXP;
	[SerializeField]
	private int currentFund;

	private NetworkManager networkManager;
	
	//Every 10 secs enemy spawn and SpawnTmer reset to 0
	private float spawnTimer;

	private PrefabManager prefabManager;
	private List<GameObject> enemies = new List<GameObject>();

	void Start() {
		currentHealth = startHealth;
		currentMoveSpeed = startMoveSpeed;
		currentDamage = startDamage;
		currentEXP = startEXP;
		currentFund = startFund;

		prefabManager = PrefabManager.GetInstance();
		//enemies.Add(prefabManager.GetPrefab("Zombie"));
		enemies.Add(prefabManager.GetPrefab("Enemy"));

		networkManager = GameObject.Find("GameManager").GetComponent<NetworkManager>();
	}

	void Update() {
		if(spawnTimer < respawnDuration) {
			spawnTimer += Time.deltaTime;
		}
		else {
			SpawnEnemy();
		}

		if(upgradeTimer < upgradeDuration) {
			upgradeTimer += Time.deltaTime;
		}
		else {
			UpgradeEnemy();
		}
	}

	float GetDistanceFrom(Vector3 src, Vector3 dist) {
		return Vector3.Distance(src, dist);
	}

	// GameObject getClosestPlayer(Transform spawnPoint) {
	// 	float minDist = 10000000f;
	// 	GameObject closestTarget = null;
	// 	List<GameObject> players = networkManager.Players;

	// 	foreach(GameObject player in players) {
	// 		float dist = GetDistanceFrom(spawnPoint.position, player.transform.position);
			
	// 		if(dist < minDist) {
	// 			minDist = dist;
	// 			closestTarget = player;
	// 		}
	// 	}

	// 	return closestTarget;
	// }

	void SpawnEnemy() {
		if(spawnTimer < respawnDuration) return;
		print("ENEMY Spawn");

		foreach(GameObject spawnPoint in spawnPoints) {
			GameObject zombie = enemies[0];
		/*	zombie.GetComponentIn<Chasing>().damage = currentDamage;
			zombie.GetComponent<Chasing>().target = target;
			zombie.GetComponent<NavMeshAgent>().speed = currentMoveSpeed;
			zombie.GetComponent<HealthManager>().SetHealth(currentHealth);
			zombie.GetComponent<KillReward>().exp = currentEXP;
			zombie.GetComponent<KillReward>().fund = currentFund;*/

			zombie.GetComponentInChildren<Chasing>().damage = currentDamage;
			zombie.GetComponentInChildren<Chasing>().target = target;
			zombie.GetComponentInChildren<NavMeshAgent>().speed = currentMoveSpeed;
			zombie.GetComponentInChildren<HealthManager>().SetHealth(currentHealth);
			zombie.GetComponentInChildren<KillReward>().exp = currentEXP;
			zombie.GetComponentInChildren<KillReward>().fund = currentFund;

			// Boost rotating speed
			float rotateSpeed = 120f + currentMoveSpeed;
			rotateSpeed = Mathf.Max(rotateSpeed, 200f);	// Max 200f
			zombie.GetComponentInChildren<NavMeshAgent>().angularSpeed = rotateSpeed;

			// PhotonNetwork.Instantiate("Zombie", spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
			Instantiate(zombie, spawnPoint.transform.position, spawnPoint.transform.rotation);
		}
		
		spawnTimer = 0f;
	}

	void UpgradeEnemy() {
		print("ENEMY UPGRADED");

		currentHealth += 5;

		if(currentMoveSpeed < 1f) {
			currentMoveSpeed += 0.2f;
		}
		if(currentDamage < 51f) {
			currentDamage += 2f;
		}
		
		currentEXP++;
		currentFund++;

		upgradeTimer = 0;
	}
}
