using UnityEngine;
using Mirror;
using Telepathy;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RTSPlayerController : NetworkBehaviour
{
    private float panSpeed = 25f;
    public GameObject SelectedObj;
    public GameObject StartingBuilding;

    Camera CameraMain;
    AudioListener Listener;
    Canvas canvas;

    public Button CreateUnitButton;

    public List<GameObject> playersSelection = new List<GameObject>();

    GraphicRaycaster graph;

    EventSystem eventSys;

    public override void OnStartClient()
    {
        base.OnStartClient();
        CameraMain = GetComponent<Camera>();
        Listener = GetComponent<AudioListener>();
        canvas = GetComponentInChildren<Canvas>();
        eventSys = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        graph = canvas.GetComponent<GraphicRaycaster>();

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            CmdSpawnStartBuilding(hit.point);
        }

        if (isLocalPlayer) return;
        Listener.enabled = false;
        CameraMain.enabled = false;
        canvas.enabled = false;
        graph.enabled = false;
        enabled = false;
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
        if (eventSys.IsPointerOverGameObject()) return null; // if over UI element

        Ray ray = CameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.gameObject.tag == "UI") return null;
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
