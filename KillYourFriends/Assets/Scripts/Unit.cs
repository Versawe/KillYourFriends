using Mirror;
using UnityEngine;

public class Unit : NetworkBehaviour
{
    public SelectableObj selected;

    private void Start()
    {
        selected = GetComponent<SelectableObj>();
    }

}
