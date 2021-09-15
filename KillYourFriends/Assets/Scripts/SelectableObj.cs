using Mirror;
using UnityEngine;

public class SelectableObj : NetworkBehaviour
{
    public bool IsSelected = false;
    public Material selectedMAT;
    public Material unselectedMAT;

    public void SelectObject()
    {
        if (!netIdentity.hasAuthority) return;

        IsSelected = true;
        gameObject.GetComponentInChildren<Renderer>().material = selectedMAT;
    }
    public void DeSelectObject() 
    {
        if (!netIdentity.hasAuthority) return; //don't know if this is needed

        IsSelected = false;
        gameObject.GetComponentInChildren<Renderer>().material = unselectedMAT;
    }
}
