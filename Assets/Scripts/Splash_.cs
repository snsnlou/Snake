using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash_ : MonoBehaviour {

   public int level = 1;
   public float setTime = 4.0f;
   public float zoomTime = 2.0f;
  // public float dimTime = 2.0f;
  // public Light dimLight;
   public float zoomSpeed = 0.1f;
   
   Camera c;
   float timer;
	// Use this for initialization
	void Start () {
		c = GetComponent<Camera> ();
      timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
      timer += Time.deltaTime;
      c.fieldOfView -= zoomSpeed;
  //    if (timer > dimTime && timer < setTime) {
  //       dimLight.intensity -= zoomSpeed;
  //    }
      if (timer > setTime) {
         Application.LoadLevel("UI");
      }
	}
}
