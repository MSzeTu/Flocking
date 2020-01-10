using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : Vehicle
{
    public GameObject[] markers;
    public Material markerLines;
    int currentTarget = 99;
    SpriteInfo markerInfo;
    RaycastHit hit;
    public override void CalcSteeringForces()
    {
        //Keep object grounded
        Physics.Raycast(new Vector3(0, 5, 0), Vector3.down, out hit, Mathf.Infinity); //Raycast to keep object grounded
        position.y = 0;
        if (currentTarget >= markers.Length)
        {
            currentTarget = 0;
        }
        markers[currentTarget].GetComponent<SpriteInfo>();
        ApplyForce(Seek(markers[currentTarget]));
        if (Vector3.Distance(position, markers[currentTarget].transform.position) < 2)
        {
            currentTarget++;
        }
        int nextMarker;
        if (Manager.debugLines)
        {
            for (int i = 0; i < markers.Length; i++)
            {
                markerLines.SetPass(0); //Draw using the lines
                GL.Begin(GL.LINES);
                GL.Vertex(markers[i].transform.position);
                nextMarker = i++;
                if (nextMarker >= markers.Length)
                {
                    nextMarker = 0;
                }
                GL.Vertex(markers[nextMarker].transform.position);
                GL.End();
            }
        }
    }

    public void OnRenderObject() //Draws Debug lines
    {
        int nextMarker;
        if (Manager.debugLines)
        {
            for (int i = 0; i < markers.Length; i++)
            {
                markerLines.SetPass(0); //Draw using the lines
                GL.Begin(GL.LINES);
                GL.Vertex(new Vector3(markers[i].transform.position.x, markers[i].transform.position.y+1, markers[i].transform.position.z));
                nextMarker = i + 1;
                if (nextMarker >= markers.Length)
                {
                    nextMarker = 0;
                }
                GL.Vertex(new Vector3(markers[nextMarker].transform.position.x, markers[nextMarker].transform.position.y + 1, markers[nextMarker].transform.position.z));
                GL.End();
            }
        }
    }
}
