using Mirror;
using UnityEngine;

public class Building : NetworkBehaviour
{
    public SelectableObj selected;

    private void Start()
    {
        selected = GetComponent<SelectableObj>();
    }

}
