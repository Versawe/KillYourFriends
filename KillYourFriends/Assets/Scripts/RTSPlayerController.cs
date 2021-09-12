using UnityEngine;
using Mirror;

public class RTSPlayerController : NetworkBehaviour
{
    private float panSpeed = 25f;
    public GameObject SelectedObj;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) SelectedObj = ClickedObject();

        float horMove = Input.GetAxis("Horizontal");
        float verMove = Input.GetAxis("Vertical");

        Vector3 moveVec = new Vector3(transform.position.x + horMove * panSpeed * Time.deltaTime, transform.position.y, transform.position.z + verMove * panSpeed * Time.deltaTime);
        transform.position = moveVec;

    }

    private GameObject ClickedObject() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.gameObject.tag != "Selectable") return null;
            hit.transform.gameObject.GetComponent<Unit>().isSelected = true;
            return hit.transform.gameObject;
        }
        else 
        {
            return null;   
        }
    }
}
