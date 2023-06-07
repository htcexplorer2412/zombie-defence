using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : EnemyState
{
    bool CanSee;
    NavMeshAgent navMeshAgent;
    public GameObject enemyObject;
    FieldOfView fieldOfView;
    [SerializeField]
    Transform _destination;
    List<Transform> playerInfo = new List<Transform>();
    public Animator Animation;
    //public Renderer render; 
    
    public EnemyHunting enemyHunting;
    public float waitTime = 0.1f;
    private IEnumerator coroutine;
    int lastVisited;


    private bool isDead = false;

    public void SetIsDead(bool value)
    {
        isDead = value;
    }

    public void stopFollowingPath(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }
    public override EnemyState RunCurrentState()
    {
        
        
        Vector3[] wayPoints = new Vector3[_destination.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints [i] =_destination.GetChild (i).position;
			wayPoints [i] = new Vector3 (wayPoints[i]. x, enemyObject.transform.position.y, wayPoints[i]. z);
        }
        coroutine = FollowPath(wayPoints,lastVisited);
        if(isDead)
        {
            stopFollowingPath(FollowPath(wayPoints,lastVisited));
        }

        Debug.Log(lastVisited);
    

       if(enemyObject.GetComponent<FieldOfView>().visiblePlayer.Count != 0)
        {
            CanSee = true;
            StopCoroutine(FollowPath(wayPoints,lastVisited));
            return enemyHunting;
        }
        else
        {
            CanSee = false;
            StartCoroutine (FollowPath(wayPoints,lastVisited));
            return this;
        }
    }

    IEnumerator FollowPath(Vector3[] wayPoints, int _lastVisited)
    {
       int targetWaypointIndex = _lastVisited;
        navMeshAgent = enemyObject.GetComponentInChildren<NavMeshAgent>();
        Vector3 targetVector = wayPoints[targetWaypointIndex];
        
        while(!CanSee)
        {
            lastVisited = targetWaypointIndex;
            if(!isDead)
            {
                navMeshAgent.SetDestination(targetVector);
            }
            
            if (Vector3.Distance(enemyObject.transform.position, targetVector) < 0.5f)
            {
            
                targetWaypointIndex = (targetWaypointIndex + 1) % wayPoints.Length;
                targetVector = wayPoints[targetWaypointIndex];

            }
            yield return null;
        }

    }

    private void OnDestroy()
    {
            StopCoroutine(coroutine);
        

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }
    }


}
