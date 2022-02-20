using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    LineRenderer line;

    private void Awake() {

        line = GetComponent<LineRenderer>();

    }
    
    public void linePosition(Vector2 pos){

        line.SetPosition(0, transform.position);
        line.SetPosition(1, pos);

    }
}
