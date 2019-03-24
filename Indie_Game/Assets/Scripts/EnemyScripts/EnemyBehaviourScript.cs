using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourScript : MonoBehaviour
{
    public float lookRadius = 5f;
    Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float Distance = Vector3.Distance(target.position, transform.position);
        if (Distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (Distance <= agent.stoppingDistance)
            {
                //Attack Target
                FaceTarget();
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, lookRadius);
    }

}


