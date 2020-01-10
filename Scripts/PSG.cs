using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSG : MonoBehaviour
{
    int x;
    int z;
    int y;
    Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        x = Random.Range(5, 65);
        z = Random.Range(5, 65);
        y = Random.Range(5, 45);
        position = new Vector3(x, y, z);
        gameObject.transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.flockers != null)
        {
            foreach (GameObject flocker in Manager.flockers)
            {
                if (Vector3.Distance(flocker.transform.position, position) < 3)
                {
                    x = Random.Range(5, 65);
                    z = Random.Range(5, 65);
                    y = Random.Range(5, 45);
                    position = new Vector3(x, y, z);
                    gameObject.transform.position = position;
                }
            }
        }
    }
}