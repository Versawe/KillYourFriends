using UnityEngine;
using Mirror;

public class BulletDespawn : NetworkBehaviour
{
    // Start is called before the first frame update
    private float DespawnTime = 5f;
    private float thrust = 50f;
    Rigidbody rig;

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), DespawnTime);
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.AddForce(transform.forward * thrust, ForceMode.Impulse);
    }

    [Server]
    void DestroySelf() 
    {
        NetworkServer.Destroy(gameObject);
    }
}