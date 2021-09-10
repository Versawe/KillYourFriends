using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Mirror.Examples.Tanks
{
    public class Tank : NetworkBehaviour
    {
        [Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;

        [Header("Movement")]
        public float rotationSpeed = 100;

        [Header("Firing")]
        public KeyCode shootKey = KeyCode.Space;
        public GameObject projectilePrefab;
        public Transform projectileMount;

        //[SyncVar] public Vector3 currPos;
        public Vector3 currPos;

        void Update()
        {
            // movement for local player
            if (!isLocalPlayer) return;

            // rotate
            /*float horizontal = Input.GetAxis("Horizontal");
            transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);

            // move
            float vertical = Input.GetAxis("Vertical");
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            agent.velocity = forward * Mathf.Max(vertical, 0) * agent.speed;
            animator.SetBool("Moving", agent.velocity != Vector3.zero);*/

            // shoot
            /*if (Input.GetKeyDown(shootKey))
            {
                CmdFire();
            }*/

            if (Input.GetButtonDown("Fire1")) 
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
                RpcMoveHostUnit(currPos, gameObject); //triggers move to send to clients
            }
        }

        [ClientRpc]
        void RpcMoveHostUnit(Vector3 movePos, GameObject serverPlayer) //Sends to clients so they see Host move
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
                CmdMoveUnit(currPos, gameObject); //triggers to send this move to Server
            }
        }

        [Command]
        void CmdMoveUnit(Vector3 movePos, GameObject client) //Updating Client move to Server
        {
            agent.SetDestination(movePos);
            RpcClientMoveToClients(movePos, gameObject); //should update client's move to other clients connected
        }

        [ClientRpc]
        void RpcClientMoveToClients(Vector3 movePos, GameObject client) //This should update the client's move to other client observing 
        {
            agent.SetDestination(movePos);
        }

        // this is called on the server
        /*[Command]
        void CmdFire()
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, transform.rotation);
            NetworkServer.Spawn(projectile);
            RpcOnFire();
        }*/

        // this is called on the tank that fired for all observers
        /*[ClientRpc]
        void RpcOnFire()
        {
            animator.SetTrigger("Shoot");
        }*/
    }
}
