using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Vehicle
{
    public GameObject target;
    public Vector3 avgPos;
    public Vector3 avgDir;
    public GameObject centerMarker;
    public Material debugMaterial;
    public override void CalcSteeringForces() //Sets up the steering forces of the vehicle
    {
        ApplyForce(Seek(target));
        ApplyForce(Seperate(Manager.flockers));
        ApplyForce(Alignment(Manager.flockers));
        ApplyForce(Cohesion(Manager.flockers));
        foreach (GameObject o in Manager.obstacles)
        {
            ApplyForce(AvoidObstacle(o, 10) * 5);
        }
    }

    public void OnRenderObject() //Draws Debug lines
    {
        avgPos = Vector3.zero;
        foreach (GameObject g in Manager.flockers)
        {
            avgDir += g.GetComponent<Vehicle>().direction;
            avgPos += g.GetComponent<Vehicle>().transform.position;
        }
        avgPos = avgPos / Manager.flockers.Count;
        avgDir = avgDir.normalized;
        centerMarker.transform.position = avgPos;
        centerMarker.transform.forward = avgDir;
        if (Manager.debugLines)
        {
            if (centerMarker.transform.localScale != Vector3.one) //Scales up sphere so it's visible
            {
                centerMarker.transform.localScale = Vector3.one;
            }
            debugMaterial.SetPass(0); //Draw using the lines
            GL.Begin(GL.LINES);
            GL.Vertex(avgPos);
            GL.Vertex(avgPos+avgDir * 2);
            GL.End();
        }
        else
        {
            if (centerMarker.transform.localScale != Vector3.zero) //Scales down sphere so it's invisible but the camera can still follow it
            {
                centerMarker.transform.localScale = Vector3.zero;
            }
        }
    }

}
