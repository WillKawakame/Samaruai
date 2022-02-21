using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    LineRenderer line;

    private void Awake() {

        line = GetComponent<LineRenderer>();

    }
    
    public void resetLinePosition(){

        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);

    }

    public void linePosition(Vector3 position){

        // position must be direction * distance
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + position);

    }
}
