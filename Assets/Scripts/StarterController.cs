using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterController : MonoBehaviour {

	private int state;
	public Text instruction;

	public GameObject GoalWindow, TimeWindow, LifeWindow, ScoreWindow;
	public GameObject SnakeHead, Egglife, Poison, Coin, Apple, Enemy;

	public GameObject GoalAccomplish;

	// Use this for initialization
	void Start () {
		state = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.LoadLevel("UI");
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			state ++;
		}
		switch(state){
			case 0:
				instruction.text = "Welcome to Snake, this part shows you how to play this game!";
				break;
			
			case 1:
				instruction.text = "The boxes on your right shows the goal, score, time and life respectively. The game ends when you run out of time or life.";
				GoalWindow.SetActive(true);
				ScoreWindow.SetActive(true);
				TimeWindow.SetActive(true);
				LifeWindow.SetActive(true);
				break;

			case 2:
				instruction.text = "Here is your snake, use left/right arrow key to turn left/right. You can also press Y to change your view. Keep the snake AWAY FROM the walls and its own body, or the game will end immediately.";
				Camera.main.transform.LookAt(SnakeHead.transform);
				break;
			case 3:
				instruction.text = "This is a coin, collect it to score 1 point.";
				Coin.SetActive(true);
				Camera.main.transform.LookAt(Coin.transform);
				break;
			case 4:
				instruction.text = "This is an apple, collect it to score 2 points...Don't be too greedy!";
				Apple.SetActive(true);
				Camera.main.transform.LookAt(Apple.transform);
				break;
			case 5:
				instruction.text = "This is a mushroom. It seems tasty, but it is toxic. You will lose one life when eating it. Keep away from it unless you have enough lives.";
				Poison.SetActive(true);
				Camera.main.transform.LookAt(Poison.transform);
				break;
			case 6:
				instruction.text = "This is a heart. You can get an extra life from it.";
				Egglife.SetActive(true);
				Camera.main.transform.LookAt(Egglife.transform);
				break;
			case 7:
				instruction.text = "**DANGER**\nThis frog is trying to eat this snake, keep the snake head away from the frog.";
				Enemy.SetActive(true);
				Camera.main.transform.LookAt(Enemy.transform);
				break;
			case 8:
				instruction.text = "Congratulations! You have completed this tutorial. Press space to go back.";
				Camera.main.transform.LookAt(SnakeHead.transform);
				GoalAccomplish.SetActive(true);
				break;
			default:
				Application.LoadLevel("UI");
				break;
		}
	}
}
