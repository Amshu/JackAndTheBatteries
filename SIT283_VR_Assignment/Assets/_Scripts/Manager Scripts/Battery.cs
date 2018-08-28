using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

    [SerializeField]Manager manager;
    bool hitOnce = false;

    private void Start()
    {
        // Get reference to the manager if its not reffered
        if(manager == null)
        {
            manager = GameObject.Find("SceneManager").GetComponent<Manager>();
        }
    }

    // Function to tell the manager on collection of the battery
    public void Collect()
    {
        if (!hitOnce)
        {
            //Debug.Log("Battery Hit");
            manager.BatteryCollected();
            hitOnce = true;
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
