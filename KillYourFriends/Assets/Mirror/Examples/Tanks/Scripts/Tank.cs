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

        //[SyncVar]
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
                if(!isServer) CmdMoveUnit();
                if (isServer) MoveUnit();
            }
        }

        // this is called on the server
        [Command]
        void CmdFire()
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, transform.rotation);
            NetworkServer.Spawn(projectile);
            RpcOnFire();
        }

        void MoveUnit()
        {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                //Vector3 movePoint = hit.point;
                currPos = hit.point;
                agent.SetDestination(currPos);
            }
        }

        [Command]
        void CmdMoveUnit()
        {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100)) 
            {
                //Vector3 movePoint = hit.point;
                currPos = hit.point;
                agent.SetDestination(currPos);
            }
        }

        // this is called on the tank that fired for all observers
        [ClientRpc]
        void RpcOnFire()
        {
            animator.SetTrigger("Shoot");
        }

        [ClientRpc]
        void RpcOnMove() 
        {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                //Vector3 movePoint = hit.point;
                currPos = hit.point;
                agent.SetDestination(currPos);
            }
        }
    }
}
