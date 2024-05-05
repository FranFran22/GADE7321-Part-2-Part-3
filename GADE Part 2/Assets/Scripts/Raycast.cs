using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public Vector3 clickPosition;
    public static GameObject hitObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //clickPosition = hit.point;
                hitObject = hit.collider.gameObject;
                Debug.Log("hit" + hitObject);
            }
        }
    }
}
