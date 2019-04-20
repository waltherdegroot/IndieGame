using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourScript : MonoBehaviour
{
    public float lookRadius = 5f;
    public float health = 100;

    private int waitedTicks = 0;
    private int ticksToWait;
    private bool playerIsGone = false;
    private Transform target;
    private NavMeshAgent agent;

    public enum State
    {
        idle,
        walking,
        attacking,
        fleeing,
        patrolling,
    }
    public State state;

    public enum AttackState
    {
        idle,
        attacking,
        lowHealth,
        fleeing,
        findHelp,
    }
    public AttackState attackState;

    public enum FleeState
    {
        idle,
        lowHealth,
        findHelp,
    }
    public FleeState flee;

    public enum PatrolState{
        idle,
        attacking,
        playerNotFound,
    }

    public void TakeDamage (float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        state = State.idle;
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyFSM();

        //print(state);
    }

    void EnemyFSM()
    {
        float Distance = Vector3.Distance(target.position, transform.position);

        switch (state)
        {
            case State.idle:
                
                if (Distance <= lookRadius)
                {
                    state = State.attacking;
                }

                break;

            case State.walking:

                break;

            case State.attacking:

                if (Distance <= lookRadius)
                {
                    agent.SetDestination(target.position);

                    if (Distance <= agent.stoppingDistance)
                    {
                        //Attack Target
                        FaceTarget();
                    }
                }
                else
                {
                    ticksToWait = 200;
                    state = State.patrolling;
                }

                break;

            case State.fleeing:

                break;

            case State.patrolling:


                if (Distance <= lookRadius)
                {
                    state = State.attacking;
                }
                else if (playerIsGone)
                {
                    playerIsGone = false;

                    state = State.idle;
                }
                else
                {
                    print("patrolling:" + waitedTicks);
                }
                break;
        }
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        if (!playerIsGone && state == State.patrolling)
        {
            waitedTicks += 1;
            if(waitedTicks > ticksToWait)
            {
                playerIsGone = true;
                waitedTicks = 0;
            }
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            PlayerManager.instance.playerHealth = PlayerManager.instance.playerHealth - UnityEngine.Random.Range(1,10);
            print("Player has " + PlayerManager.instance.playerHealth + " HP remaining");
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, lookRadius);
    }

}


