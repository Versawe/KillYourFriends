using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespawn : MonoBehaviour
{
    // Start is called before the first frame update
    private float DespawnTime = 5f;
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        DespawnTime -= Time.deltaTime;

        if(DespawnTime <= 0) Destroy(gameObject);
    }
}
