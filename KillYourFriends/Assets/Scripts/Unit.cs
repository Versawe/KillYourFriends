using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class Unit : NetworkBehaviour
{
    public SelectableObj selected;

    NavMeshAgent agent;

    Vector3 currPos;

    private bool IsStopped = false;
    private float closeEnough = 8f;
    private float stopMovementTimer = 4f;
    private float stopMovementAfter = 4f;
    
    private void Start()
    {
        selected = GetComponent<SelectableObj>();
        agent = GetComponent<NavMeshAgent>();
        currPos = transform.position;

        MoveUnitStart();
    }

    private void Update()
    {
        // movement for local player
        if (!netIdentity.hasAuthority) return;

        if (!selected.IsSelected) return;

        if (!IsStopped) UnitChill();
        else
        {
            agent.SetDestination(transform.position);
            stopMovementTimer = stopMovementAfter;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            MoveUnit();
        }
    }
    
    //Functions that happen on client only
    [Client]
    private void UnitChill()
    {
        Vector3 offset = currPos - transform.position;
        float sqrLen = offset.sqrMagnitude;

        // square the distance we compare with
        if (sqrLen < closeEnough * closeEnough)
        {
            stopMovementTimer -= 1 * Time.deltaTime;
        }

        if(stopMovementTimer <= 0)
        {
            IsStopped = true;
            CmdStopMove(transform.position);
        }
    }
    [Client]
    void MoveUnitStart() //Client Moving Locally
    {
        currPos = new Vector3(currPos.x, currPos.y, currPos.z-4f);
        agent.isStopped = false;
        agent.SetDestination(currPos);
        CmdMoveUnit(currPos); //triggers to send this move to Server
    }
    [Client]
    void MoveUnit() //Client Moving Locally
    {
        IsStopped = false;
        stopMovementTimer = stopMovementAfter;
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            currPos = hit.point;
            agent.SetDestination(currPos);
            CmdMoveUnit(currPos); //triggers to send this move to Server
        }
    }

    //functions sent from client to server
    [Command]
    void CmdMoveUnit(Vector3 movePos) //Updating Client move to Server
    {
        agent.SetDestination(movePos);
        RpcUpdateUnitMove(movePos); //should update client's move to other clients connected
    }
    [Command]
    void CmdStopMove(Vector3 stopPos)
    {
        agent.SetDestination(transform.position);
        RpcUpdateUnitStop(transform.position);
    }

    //functions that retreived Cmd and now giving back out to other clients
    [ClientRpc]
    void RpcUpdateUnitMove(Vector3 movePos) //Sends to clients so they see Host move
    {
        agent.SetDestination(movePos);
    }
    [ClientRpc]
    void RpcUpdateUnitStop(Vector3 stopPos)
    {
        agent.SetDestination(stopPos);
    }

}
