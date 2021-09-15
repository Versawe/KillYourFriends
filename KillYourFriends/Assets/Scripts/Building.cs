using Mirror;
using Telepathy;
using UnityEngine;

public class Building : NetworkBehaviour
{
    public SelectableObj selected;
    public GameObject Panel;

    NetworkIdentity ni;
    public GameObject unitFab;
    public Transform SpawnLoc;
    RTSPlayerController owner;

    private void Start()
    {
        ni = NetworkClient.connection.identity;
        owner = ni.gameObject.GetComponent<RTSPlayerController>();

        selected = GetComponent<SelectableObj>();
        Panel = GameObject.Find("BuildingPanel");
        Panel.SetActive(false);

        owner.CreateUnitButton.onClick.AddListener(CmdSpawnUnit);
    }

    void Update()
    {
        if (selected.IsSelected) Panel.SetActive(true);
        else Panel.SetActive(false);
    }

    [Command]
    public void CmdSpawnUnit() 
    {
        GameObject unit = Instantiate(unitFab, SpawnLoc.position, SpawnLoc.rotation);
        NetworkServer.Spawn(unit, connectionToClient);
    }
}
