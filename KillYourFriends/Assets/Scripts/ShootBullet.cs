using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public GameObject bulletPrefab;

    private float delayShot = 0.25f;
    private bool waitingDelay = false;

    private float thrust = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !waitingDelay) 
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Rigidbody rig = bullet.GetComponent<Rigidbody>();

            rig.AddForce(bullet.transform.forward * thrust, ForceMode.Impulse);
            waitingDelay = true;
        }

        if (waitingDelay) 
        {
            delayShot -= 1 * Time.deltaTime;
        }
        else 
        {
            delayShot = 0.25f;    
        }

        if (delayShot <= 0)
        {
            waitingDelay = false;
        }
    }
}
