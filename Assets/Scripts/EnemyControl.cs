using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {
    public Transform player;
    private UnityEngine.AI.NavMeshAgent nav;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Snake Body").transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        nav.SetDestination(player.position);
        nav.nextPosition = transform.position;


    }
}
