using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class Unit : NetworkBehaviour
{
    public SelectableObj selected;

    NavMeshAgent agent;

    Vector3 currPos;
    private void Start()
    {
        selected = GetComponent<SelectableObj>();
        agent = GetComponent<NavMeshAgent>();
        currPos = transform.position;
    }

    private void Update()
    {
        // movement for local player
        if (!netIdentity.hasAuthority) return;

        if (!selected.IsSelected) return;

        if (Input.GetButtonDown("Fire2"))
        {
            if (!isServer) MoveUnit();
            else
            {
                MoveHostUnit();
            }
        }
    }

    [Server]
    void MoveHostUnit() //Host moves locally
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            currPos = hit.point;
            agent.SetDestination(currPos);
            UpdateUnitMove(currPos); //triggers move to send to clients
        }
    }

    [ClientRpc]
    void UpdateUnitMove(Vector3 movePos) //Sends to clients so they see Host move
    {
        agent.SetDestination(movePos);
    }

    [Client]
    void MoveUnit() //Client Moving Locally
    {
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

    [Command]
    void CmdMoveUnit(Vector3 movePos) //Updating Client move to Server
    {
        agent.SetDestination(movePos);
        UpdateUnitMove(movePos); //should update client's move to other clients connected
    }

}
