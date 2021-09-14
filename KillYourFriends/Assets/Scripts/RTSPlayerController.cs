using UnityEngine;
using Mirror;
using Telepathy;
using System.Collections.Generic;

public class RTSPlayerController : NetworkBehaviour
{
    private float panSpeed = 25f;
    public GameObject SelectedObj;
    public GameObject StartingBuilding;

    Camera CameraMain;
    AudioListener Listener;

    public List<GameObject> playersSelection = new List<GameObject>();

    public override void OnStartClient()
    {
        base.OnStartClient();
        CameraMain = GetComponent<Camera>();
        Listener = GetComponent<AudioListener>();

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            CmdSpawnStartBuilding(hit.point);
        }

        if (isLocalPlayer) return;
        Listener.enabled = false;
        CameraMain.enabled = false;
    }

    [Command]
    void CmdSpawnStartBuilding(Vector3 hit)
    {
        GameObject Building = Instantiate(StartingBuilding, hit, Quaternion.identity);
        NetworkServer.Spawn(Building, connectionToClient);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown("escape")) ClearSelection();

        if (Input.GetButtonDown("Fire1")) SelectedObj = ClickedObject();

        float horMove = Input.GetAxis("Horizontal");
        float verMove = Input.GetAxis("Vertical");

        Vector3 moveVec = new Vector3(transform.position.x + horMove * panSpeed * Time.deltaTime, transform.position.y, transform.position.z + verMove * panSpeed * Time.deltaTime);
        transform.position = moveVec;

    }

    [Client]
    private GameObject ClickedObject() 
    {
        Ray ray = CameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if(hit.transform.gameObject.tag == "Ground") 
            {
                ClearSelection();
                return null;
            }
            if (hit.transform.gameObject.tag != "Selectable") return null;
            hit.transform.gameObject.GetComponent<SelectableObj>().SelectObject();
            playersSelection.Add(hit.transform.gameObject);
            return hit.transform.gameObject;
        }
        else 
        {
            return null;   
        }
    }

    [Client]
    private void ClearSelection() 
    {
        foreach(GameObject unit in playersSelection) 
        {
            unit.GetComponent<SelectableObj>().DeSelectObject();
        }
        playersSelection.Clear();
    }
}
