using Mirror;
using UnityEngine;

public class Unit : NetworkBehaviour
{
    public bool isSelected = false;

    public Material selectedMAT;
    public Material unselectedMAT;



    public void SelectUnit() 
    {
        isSelected = true;
        gameObject.GetComponent<Renderer>().material = selectedMAT;
    }
}
