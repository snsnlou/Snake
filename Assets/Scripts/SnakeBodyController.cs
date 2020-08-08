using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnakeBodyController : MonoBehaviour {

	public GameObject Previous;
	public GameObject Next;

	public int direction = 0;
	public int newDirection = 0;
	public int turningBuffer;

	private float prevX;
	private float prevZ;

	public bool isPaused;
	// Use this for initialization
	void Start () {
		isPaused = false;
		try{
			direction = Previous.GetComponent<SnakeBodyController>().direction;
		}catch(Exception ex){
			direction = Previous.GetComponent<SnakeHeadController>().direction;
		}
	}

	void LateUpdate(){
		newDirection = 0;
		try{
			newDirection = Previous.GetComponent<SnakeBodyController>().direction;
		}catch(Exception ex){
			newDirection = Previous.GetComponent<SnakeHeadController>().direction;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// newDirection = 0;
		// try{
		// 	newDirection = Previous.GetComponent<SnakeBodyController>().direction;
		// }catch(Exception ex){
		// 	newDirection = Previous.GetComponent<SnakeHeadController>().direction;
		// }
		if(isPaused) return;

		if(direction!=newDirection){
			if(direction==0){
				transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z + Time.deltaTime*1.0f);
			}
			else if(direction==1){
				transform.localPosition = new Vector3(transform.localPosition.x - Time.deltaTime*1.0f,transform.localPosition.y,transform.localPosition.z);
			}
			else if(direction==2){
				transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z - Time.deltaTime*1.0f);
			}
			else if(direction==3){
				transform.localPosition = new Vector3(transform.localPosition.x + Time.deltaTime*1.0f,transform.localPosition.y,transform.localPosition.z);
			}

			if(turningBuffer != 0) return;
			turningBuffer = (newDirection - direction + 8)%4;
			if(turningBuffer == 3) turningBuffer = -1;
			prevX = transform.localPosition.x;
			prevZ = transform.localPosition.z;

			if(turningBuffer != 0){
				StartCoroutine("Turn");
			}

		}else{
			turningBuffer = 0;
			if(direction==0){
				transform.localPosition = Previous.transform.localPosition + new Vector3(0,0,-1);
			}
			else if(direction==1){
				transform.localPosition = Previous.transform.localPosition + new Vector3(1,0,0);
			}
			else if(direction==2){
				transform.localPosition = Previous.transform.localPosition + new Vector3(0,0,1);
			}
			else if(direction==3){
				transform.localPosition = Previous.transform.localPosition + new Vector3(-1,0,0);
			}
			transform.eulerAngles = new Vector3(transform.eulerAngles.x,-90 * (int)direction + 360,transform.eulerAngles.z);
		}
	}

	IEnumerator Turn(){
		if(direction == 0){
			while(transform.localPosition.z < Previous.transform.localPosition.z){
				while (isPaused)
 				{
     				yield return null;
 				}
				//transform.Rotate(new Vector3(0,-turningBuffer * Time.deltaTime * 90/(Previous.transform.localPosition.z -prevZ),0));
				transform.Rotate(new Vector3(0,-turningBuffer * Time.deltaTime * 90,0));
				yield return null;
			}
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,Previous.transform.localPosition.z);
		}else if(direction ==1){
			
			while(transform.localPosition.x > Previous.transform.localPosition.x){
				while (isPaused)
 				{
     				yield return null;
 				}
				// transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90/(prevX-Previous.transform.localPosition.x),0));
				transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90,0));
				yield return null;
			}
			transform.localPosition = new Vector3(Previous.transform.localPosition.x,transform.localPosition.y, transform.localPosition.z);
		}else if(direction == 2){
			while(transform.localPosition.z > Previous.transform.localPosition.z){
				while (isPaused)
 				{
     				yield return null;
 				}
				// transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90/(prevZ-Previous.transform.localPosition.z),0));
				transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90,0));
				yield return null;
			}
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,Previous.transform.localPosition.z);
		}else{
			while(transform.localPosition.x < Previous.transform.localPosition.x){
				while (isPaused)
 				{
     				yield return null;
 				}
				// transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90/(Previous.transform.localPosition.x -prevX),0));
				transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90,0));
				yield return null;
			}
			transform.localPosition = new Vector3(Previous.transform.localPosition.x,transform.localPosition.y, transform.localPosition.z);
		}
		direction = (direction + turningBuffer + 4)% 4;
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,-90 * (int)direction + 360,transform.eulerAngles.z);
		turningBuffer = 0;
		// if(Next!=null)
		// 	Debug.Log("Body ends turning.");
	}
}
