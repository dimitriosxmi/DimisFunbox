using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<ObjectBehaviour>().KillMe();
        //if (other.transform.parent != null && other.transform.parent.name != "Tiles Placed")
        //{
        //    Destroy(other.transform.parent.gameObject);
        //}
        //else
        //{
        //    Destroy(other.gameObject);
        //}
    }
}
