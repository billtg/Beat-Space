using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotcollision : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hit!");
        if (other.gameObject.GetComponent<ShipControl>())
            return;
        Destroy(other.gameObject);
        Destroy(this.gameObject);
        GameManager.instance.AddScore(100);
    }

    void Update()
    {
        if (this.gameObject.transform.position.x > 10)
        {
            Destroy(this.gameObject);
            GameManager.instance.ClearMultiplier();
        }
    }
}
