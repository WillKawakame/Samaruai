using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInGame : MonoBehaviour
{
    GameObject player;

    float heightestPoint;

    public float height;

    private void Awake() { 

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update() {
        if(player != null){
            CheckPlayerHeight();
        }
    }

    private void CheckPlayerHeight(){

        if(player.transform.position.y - 1.5f >= heightestPoint){

            heightestPoint = player.transform.position.y - height;
            transform.position = new Vector3(transform.position.x, heightestPoint, transform.position.z);

        }
    }
}
