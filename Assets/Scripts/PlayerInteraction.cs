using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private LayerMask rayCastMask;
    private RaycastHit hit;
    private float rayCastDistance = 100f;
    

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, rayCastDistance, rayCastMask))
            {
                Interactive obj = hit.transform.GetComponent<Interactive>();
                if (obj != null)
                {
                    obj.StartInteraction();
                }
            }
        }
    }
}
