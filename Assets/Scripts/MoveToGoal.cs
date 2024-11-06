using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
public class MoveToGoal : MonoBehaviour
{
    public Transform Goal;
    public NavMeshAgent Agent { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(Time.frameCount % 10 != 0) return;
        Agent.destination = Goal.position;
    }
}
