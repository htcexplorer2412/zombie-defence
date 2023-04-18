using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : EnemyState
{
    bool CanSee;
    NavMeshAgent navMeshAgent;
    public GameObject gameObject;
    [SerializeField]
    Transform _destination;
    List<Transform> playerInfo = new List<Transform>();
    public Animator Animation;
    //public Renderer render; 
    
    public EnemyHunting enemyHunting;
    public float waitTime = 0.1f;
    private IEnumerator coroutine;
    int lastVisited;

    public override EnemyState RunCurrentState()
    {
        
       // Debug.Log("Patrol activated");
        Vector3[] wayPoints = new Vector3[_destination.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints [i] =_destination.GetChild (i).position;
			wayPoints [i] = new Vector3 (wayPoints[i]. x, gameObject.transform.position.y, wayPoints[i]. z);
        }
        coroutine = FollowPath(wayPoints,lastVisited);
      //  render = gameObject.GetComponent<Renderer>();
        Debug.Log(lastVisited);
    

       if(gameObject.GetComponent<FieldOfView>().visiblePlayer.Count != 0)
        {
            CanSee = true;
            StopCoroutine(coroutine);
//            Animation.SetBool("isWalking", true);
            
    //        render.material.color = Color.white;
            //gameObject.GetComponent<Material>
            return enemyHunting;
        }
        else
        {
     //       render.material.color = Color.green;
            CanSee = false;
            StartCoroutine (coroutine);
            return this;
        }
    }

    IEnumerator FollowPath(Vector3[] wayPoints, int _lastVisited)
    {
       int targetWaypointIndex = _lastVisited;
        navMeshAgent = gameObject.GetComponentInChildren<NavMeshAgent>();
        Vector3 targetVector = wayPoints[targetWaypointIndex];
        
        while(!CanSee)
        {
            lastVisited = targetWaypointIndex;
            navMeshAgent.SetDestination(targetVector);
            
            if (Vector3.Distance(gameObject.transform.position, targetVector) < 0.5f)
            {
            
                targetWaypointIndex = (targetWaypointIndex + 1) % wayPoints.Length;
                targetVector = wayPoints[targetWaypointIndex];
              //  yield return new WaitForSeconds(waitTime);

            }
            yield return null;
        }

    }


}
