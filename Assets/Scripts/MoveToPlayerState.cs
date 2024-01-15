using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayerState : IState
{
    private NavMeshAgent _agent;

    public MoveToPlayerState(NavMeshAgent agent)
    {
        this._agent = agent;
    }
    public void Enter()
    {
        Debug.Log("Enter Move to player state");
    }

    public void Exit()
    {
        return;
    }

    public void Update()
    {
        _agent.SetDestination(GameManager.Instance.Player.transform.position);
    }
}