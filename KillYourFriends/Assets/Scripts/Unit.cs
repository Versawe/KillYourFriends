using Mirror;
using UnityEngine;

public class Unit : NetworkBehaviour
{
    public bool isSelected = false;

    public Material selectedMAT;
    public Material unselectedMAT;

    public NetworkIdentity owner;

    public void SelectUnit() 
    {
        if (!hasAuthority) return;

        isSelected = true;
        gameObject.GetComponent<Renderer>().material = selectedMAT;
    }
}
