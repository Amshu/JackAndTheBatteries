using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawner : MonoBehaviour {

    [SerializeField] Material on;
    [SerializeField] Material off;

    // An array to contain all the spawn locations
    GameObject[] spawns;

    // An array to contain all the available batteries.
    GameObject[] batteries = new GameObject[3];

    // Reference to the manager script
    [SerializeField] Manager manager;

    //Battery Prefab reference
    [SerializeField] GameObject BatteryPrefab;

    // Boolean to get hte state of the switch
    bool switchOn;

    [SerializeField] AudioClip button;
    new AudioSource audio;

    public GameObject gg;

    private void Start()
    {
        manager = GameObject.Find("SceneManager").GetComponent<Manager>();

        audio = gameObject.AddComponent<AudioSource>();

        // Change the colour of the button to green
        this.gameObject.GetComponent<Renderer>().material = on;
        switchOn = true;

        // Fill the in the array with all the spawn locations
        if (spawns == null)
            spawns = GameObject.FindGameObjectsWithTag("Respawn");

        Spawn(3);
    }

    // Function of the spawnner
    public void Spawn(int bat)
    {
        GameObject temp = null;

        for (int i = 0; i < bat; i++)
        {
            int j = Random.Range(0, 5);
            temp = spawns[j].gameObject;

            // Check if location already has a child(battery)
            if (temp.transform.childCount == 0)
            {
                Instantiate(BatteryPrefab, temp.transform.position, BatteryPrefab.transform.rotation, temp.transform);
            }
            else
            {
                i--;
                continue;
            }
        }
    }

    // If the button is pressed
    private void OnCollisionEnter(Collision collision)
    {
        if (switchOn)
        {
            audio.PlayOneShot(button);
            // Change the colour of the button to re
            this.gameObject.GetComponent<Renderer>().material = off;
            switchOn = false;

            // Change the locations of the batteries again
            ChangeLocation();

            StartCoroutine(TurnOff());
        }
    }

    void ChangeLocation()
    {
        // Get available batteries
        GetCurrentBatteries();

        for (int i = 0; i < batteries.Length - 1; i++)
        {
            int j = Random.Range(0, 5);
            GameObject temp = spawns[j].gameObject;

            // Check if location already has a child(battery)
            if (temp.transform.childCount == 0)
            {
                batteries[i].transform.parent = temp.transform;
            }
            else{
                i--;
                continue;
            }
        }
    }

    // Function which gets all the currently available batteries in scene
    void GetCurrentBatteries()
    {
        if (batteries != null)
        {
            batteries = null;
        }
        batteries = GameObject.FindGameObjectsWithTag("Battery");

    }

    // Turn the switch back on again after a few seconds
    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<Renderer>().material = on;
        switchOn = true;
    }
}
