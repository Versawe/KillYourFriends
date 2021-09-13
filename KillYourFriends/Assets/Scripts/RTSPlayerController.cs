using UnityEngine;
using Mirror;
using Telepathy;

public class RTSPlayerController : NetworkBehaviour
{
    private float panSpeed = 25f;
    public GameObject SelectedObj;
    public GameObject StartingBuilding;

    Camera CameraMain;
    AudioListener Listener;

    public override void OnStartClient()
    {
        base.OnStartClient();
        CameraMain = GetComponent<Camera>();
        Listener = GetComponent<AudioListener>();
        if (isLocalPlayer) return;
        Listener.enabled = false;
        CameraMain.enabled = false;
    }
    void Start()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            CmdSpawnStartBuilding(hit.point);
        }
    }

    [Command]
    void CmdSpawnStartBuilding(Vector3 hit)
    {
        GameObject Building = Instantiate(StartingBuilding, hit, Quaternion.identity);
        NetworkServer.Spawn(Building);
        //if (isServer) netIdentity.AssignClientAuthority(connectionToClient);
        //else netIdentity.AssignClientAuthority(connectionToServer);
        //NEED to read up on assigning authority with a player as server and to clients..
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetButtonDown("Fire1")) SelectedObj = ClickedObject();

        float horMove = Input.GetAxis("Horizontal");
        float verMove = Input.GetAxis("Vertical");

        Vector3 moveVec = new Vector3(transform.position.x + horMove * panSpeed * Time.deltaTime, transform.position.y, transform.position.z + verMove * panSpeed * Time.deltaTime);
        transform.position = moveVec;

    }

    private GameObject ClickedObject() 
    {
        Ray ray = CameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.gameObject.tag != "Selectable") return null;
            hit.transform.gameObject.GetComponent<Unit>().SelectUnit();
            return hit.transform.gameObject;
        }
        else 
        {
            return null;   
        }
    }
}
