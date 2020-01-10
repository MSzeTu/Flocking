using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static GameObject[] obstacles;
    private GameObject[] flockerArray;
    public static List<GameObject> flockers;
    public static bool debugLines = false;
    // Start is called before the first frame update
    void Start()
    {
        flockers = new List<GameObject>();
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        flockerArray = GameObject.FindGameObjectsWithTag("Flocker");
        foreach (GameObject g in flockerArray)
        {
            flockers.Add(g);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) //toggles debug lines 
        {
            if (debugLines)
            {
                debugLines = false;
            }
            else
            {
                debugLines = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) //shuffle the flocker positions
        {
            foreach (GameObject g in flockers)
            {
                Vehicle properties = g.GetComponent<Vehicle>();
                int x = Random.Range(5, 65);
                int z = Random.Range(5, 65);
                int y = Random.Range(5, 45);
                properties.position = new Vector3(x, y, z);
            }
        }
    }
}
