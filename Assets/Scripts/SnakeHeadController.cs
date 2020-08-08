using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeHeadController : MonoBehaviour {
	public int MapId; //Used to store the current map id
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    public int lifeValue;
	public int time;
	public int goal;
	private bool topView;
    private GameController gameController;
    public Text countText;
	public Text timeText;
	public Text goalText;

	public Text gameOverText;
	public Text gameOverTitle;
	public GameObject GameOverWindow;
	public Text loseLifeText;
	public GameObject LoseLifeWindow;
	public GameObject ExitWindow;

	public Button LoseLifeConfirm;
	public Button ExitCancel;
	public Button ExitConfirm;
	public Button EndGameConfirm;

	public GameObject GoalAccomplish;

    public int direction;
	public GameObject camera;
	public GameObject Next;
	public GameObject Body;
	public GameObject Enemy;
	private int turningBuffer;

	public GameObject Life3, Life2, Life1;

	private float prevX;
	private float prevZ;

	public bool isPaused;

	// Use this for initialization
	void Start () {
		isPaused = false;
		direction = 0;
		// transform.localPosition = Vector3.zero;
		turningBuffer = 0;
        scoreValue = 0;
        lifeValue = 1;
        countText.text = "";
        SetCountText();
        SetLifeText();
		StartCoroutine("CountDown");
		topView = false;

		if(time%60 >= 10){
			timeText.text = time/60+ " : " + time%60;
		}else{
			timeText.text = time/60+ " : 0" + time%60;
		}

		goalText.text="Get "+goal+" points in "+time+" seconds.";

		LoseLifeConfirm.onClick.AddListener(delegate{
			LoseLifeWindow.SetActive(false);
			ResumeGame();
		});
		ExitCancel.onClick.AddListener(delegate{
			ExitWindow.SetActive(false);
			ResumeGame();
		});
		ExitConfirm.onClick.AddListener(delegate{
			Application.LoadLevel("UI");
		});
		EndGameConfirm.onClick.AddListener(delegate{
			//Save the game (record the scores, the progress..)
			//Update code here

			Application.LoadLevel("UI");
		});
    }

    // Update is called once per frame
    void Update () {
		if(isPaused) {
			if(Input.GetKeyDown(KeyCode.Escape)){
				ExitWindow.SetActive(false);
				ResumeGame();
			}
			return;
		}
		Camera.main.transform.LookAt(this.transform);
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

		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			// direction = (direction+1)%4;
			//Debug.Log("Left key detected");
			if(turningBuffer == 0){
				prevX = transform.localPosition.x;
				prevZ = transform.localPosition.z;
				turningBuffer = 1;
				StartCoroutine("Turn");
			}else{//Debug.Log("Blockd from turning left");
			}
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			// direction = (direction+3)%4;
			//Debug.Log("Right key detected");
			if(turningBuffer == 0){
				turningBuffer = -1;
				prevX = transform.localPosition.x;
				prevZ = transform.localPosition.z;
				StartCoroutine("Turn");
			}else{//Debug.Log("Blockd from turning right");
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape)){
			PauseGame();
			ExitWindow.SetActive(true);
		}

		if(Input.GetKeyDown(KeyCode.Y)){
			if(topView){
				Camera.main.transform.localPosition = new Vector3(0f,2.73f,-5f);
				topView = false;
			}else{
				Camera.main.transform.localPosition = new Vector3(0f,10f,-0.2f);
				topView = true;
			}
		}
	}

	void AddLength(){
		GameObject cursor=Next;
			while(cursor.GetComponent<SnakeBodyController>().Next!=null){
				cursor = cursor.GetComponent<SnakeBodyController>().Next;
			}
			GameObject Tail = cursor;
			cursor = cursor.GetComponent<SnakeBodyController>().Previous;
			GameObject newBody = Instantiate(Body, Tail.transform.localPosition, Tail.transform.rotation);
			switch(Tail.GetComponent<SnakeBodyController>().direction){
				case 0:
					// Tail.transform.localposition = new Vector3(cursor.transform.localPosition.x, cursor.transform.localPosition.y, cursor.transform.localPosition.z-1);
					Tail.transform.localPosition = Tail.transform.localPosition+new Vector3(0,0,-1);
					break;
				case 1:
					// position = new Vector3(cursor.transform.localPosition.x+1, cursor.transform.localPosition.y, cursor.transform.localPosition.z);
					Tail.transform.localPosition = Tail.transform.localPosition+new Vector3(1,0,0);
					break;
				case 2:
					// position = new Vector3(cursor.transform.localPosition.x, cursor.transform.localPosition.y, cursor.transform.localPosition.z+1);
					Tail.transform.localPosition = Tail.transform.localPosition+new Vector3(0,0,1);
					break;
				case 3:
					// position = new Vector3(cursor.transform.localPosition.x-1, cursor.transform.localPosition.y, cursor.transform.localPosition.z);
					Tail.transform.localPosition = Tail.transform.localPosition+new Vector3(-1,0,0);
					break;
			}
			// GameObject newBody = Instantiate(Body, position, cursor.transform.rotation);
			cursor.GetComponent<SnakeBodyController>().Next = newBody;
			newBody.GetComponent<SnakeBodyController>().Previous = cursor;
			newBody.GetComponent<SnakeBodyController>().Next = Tail;
			Tail.GetComponent<SnakeBodyController>().Previous=newBody;
			newBody.GetComponent<SnakeBodyController>().direction = Tail.GetComponent<SnakeBodyController>().direction;
			
			Tail.GetComponent<SnakeBodyController>().StopCoroutine("Turn");
			Tail.GetComponent<SnakeBodyController>().turningBuffer = 0;
			switch(Tail.GetComponent<SnakeBodyController>().direction){
				case 0:
					Tail.transform.eulerAngles = new Vector3(0,0,0);
					break;
				case 1:
					Tail.transform.eulerAngles = new Vector3(0,-90,0);
					break;
				case 2:
					Tail.transform.eulerAngles = new Vector3(0,180,0);
					break;
				case 3:
				Tail.transform.eulerAngles = new Vector3(0,90,0);
					break;
			}

			switch(cursor.GetComponent<SnakeBodyController>().direction){
				case 0:
					newBody.transform.eulerAngles = new Vector3(0,0,0);
					break;
				case 1:
					newBody.transform.eulerAngles = new Vector3(0,-90,0);
					break;
				case 2:
					newBody.transform.eulerAngles = new Vector3(0,180,0);
					break;
				case 3:
					newBody.transform.eulerAngles = new Vector3(0,90,0);
					break;
			}
	}

	IEnumerator Turn(){
		if(direction == 0){
			while(transform.localPosition.z < ((int)(prevZ+0.5f+60)) + 0.5f - 60){
				 while (isPaused)
 				{
     				yield return null;
 				}
				transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90/(((int)(prevZ+0.5f+60)) + 0.5f - 60 -prevZ),0));
				yield return null;
			}
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,(int)(prevZ+0.5f+60) + 0.5f - 60);
			direction = (direction + turningBuffer + 4)% 4;
		}else if(direction ==1){
			while(transform.localPosition.x > ((int)(prevX-0.5f+60)) + 0.5f - 60){
				 while (isPaused)
 				{
     				yield return null;
 				}
				transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90/(prevX-(((int)(prevX-0.5f+60)) + 0.5f - 60)),0));
				yield return null;
			}
			transform.localPosition = new Vector3((int)(prevX-0.5f+60) + 0.5f - 60,transform.localPosition.y, transform.localPosition.z);
			direction = direction +turningBuffer;
		}else if(direction == 2){
			while(transform.localPosition.z > ((int)(prevZ-0.5f+60)) + 0.5f - 60){
				 while (isPaused)
 				{
     				yield return null;
 				}
				transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90/(prevZ-(((int)(prevZ-0.5f+60)) + 0.5f - 60)),0));
				yield return null;
			}
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,(int)(prevZ-0.5f+60) + 0.5f - 60);
			direction = direction +turningBuffer;
		}else{
			while(transform.localPosition.x < ((int)(prevX+0.6f+60)) + 0.5f - 60){
				 while (isPaused)
 				{
     				yield return null;
 				}
				transform.Rotate(new Vector3(0,-turningBuffer* Time.deltaTime * 90/(((int)(prevX+0.6f+60)) + 0.5f - 60 -prevX),0));
				yield return null;
			}
			transform.localPosition = new Vector3((int)(prevX+0.5f+60) + 0.5f - 60,transform.localPosition.y, transform.localPosition.z);
			direction = (direction + turningBuffer)% 4;
		}
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,-90 * direction + 360,transform.eulerAngles.z);
		turningBuffer = 0;
		//Debug.Log("End Turning");
	}
    void SetCountText() {
        countText.text = scoreValue.ToString();
		if(scoreValue>=goal){
			GoalAccomplish.SetActive(true);
		}
    }
    void SetLifeText()
    {
        if(lifeValue >= 3){
			Life3.GetComponent<Image>().color = new Color32(255,0,0,255);
		}
		else{
			Life3.GetComponent<Image>().color = new Color32(128,128,128,255);
		}

		if(lifeValue >= 2){
			Life2.GetComponent<Image>().color = new Color32(255,0,0,255);
		}
		else{
			Life2.GetComponent<Image>().color = new Color32(128,128,128,255);
		}
		
		if(lifeValue >= 1){
			Life1.GetComponent<Image>().color = new Color32(255,0,0,255);
		}
		else{
			Life1.GetComponent<Image>().color = new Color32(128,128,128,255);
			EndGame();
		}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boundary")|| other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("Snake Body"))
        {
            EndGame();
            //countText.text = "Game Over!";
            //gameController.GameOver();
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
			AddLength();
            scoreValue += 1;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Apple"))
        {
            other.gameObject.SetActive(false);
			AddLength();
			AddLength();
            scoreValue += 2;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Poison"))
        {
            other.gameObject.SetActive(false);
            lifeValue -= 1;
            SetLifeText();

			if(lifeValue>0){
				PauseGame();
				LoseLifeWindow.SetActive(true);
				loseLifeText.text = "You have lost a life...\nYou have "+lifeValue+" lives left, take care!";
			}
        }
        if (other.gameObject.CompareTag("Life"))
        {
            other.gameObject.SetActive(false);
            lifeValue += 1;
            SetLifeText();
        }
    }

	IEnumerator CountDown(){
		while(time > 0){
			while (isPaused)
 				{
     				yield return null;
 				}
			yield return new WaitForSeconds(1);
			time--;
			if(time%60 >= 10){
				timeText.text = time/60+ " : " + time%60;
			}else{
				timeText.text = time/60+ " : 0" + time%60;
			}
		}
		EndGame();
	}

	IEnumerator UploadScore(){
		using (WWW www = new WWW("http://www2.comp.polyu.edu.hk/~14109998d/Upload.php?id="+UIController.userId+"&level="+MapId+"&score="+scoreValue))
        {
            yield return www;
        }
	}

	void EndGame(){
		PauseGame();
		gameOverText.text = "You have obtained "+scoreValue+" score(s)!";
		if(scoreValue>=goal){
			gameOverTitle.text = "You Win!";
		}else{
			gameOverTitle.text = "You Lose!";
		}
		GameOverWindow.SetActive(true);
		if(UIController.userId!=-1){
			StartCoroutine("UploadScore");
		}
	}

	void PauseGame(){
		isPaused = true;
		GameObject cursor=Next;
		while(cursor.GetComponent<SnakeBodyController>().Next!=null){
			cursor.GetComponent<SnakeBodyController>().isPaused = true;
			cursor = cursor.GetComponent<SnakeBodyController>().Next;
		}
		cursor.GetComponent<SnakeBodyController>().isPaused = true;
		Enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().Stop();
	}

	void ResumeGame(){
		isPaused = false;
		GameObject cursor=Next;
		while(cursor.GetComponent<SnakeBodyController>().Next!=null){
			cursor.GetComponent<SnakeBodyController>().isPaused = false;
			cursor = cursor.GetComponent<SnakeBodyController>().Next;
		}
		cursor.GetComponent<SnakeBodyController>().isPaused = false;
		Enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().Resume();
	}
}
