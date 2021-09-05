using Mirror;
using System;
using UnityEngine;

public class SimpleMoveTest : NetworkBehaviour
{
    private float speed = 200;
    Rigidbody rb;
    public override void OnStartClient()
    {
        base.OnStartClient();

        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKey("w")) MoveForward();
    }

    private void MoveForward()
    {
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }
}
