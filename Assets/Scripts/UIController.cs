using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class UIController : MonoBehaviour {

	public Button loginButton, logoutButton;
	public Text LoginUsername;
	public InputField LoginPassword;
	public Button regButton;
	public Text RegUsername;
	public InputField RegPassword;
	public InputField RegPasswordConfirm;

	public Button starter,level1,level2, level3, level4, level5, level6;

	public GameObject loginWindow, loginError, loginSuccess;
	public GameObject regWindow, regError, regSuccess;

	public GameObject MapSelection, MapDescripton, GoalCheck;
	public Text MapDescriptonTitle, Goal, Gold, Silver, Bronze;
	public Button EnterMap, ExitConfirm;
	// public Button register;//when register button click, go to the void reg
	public InputField In_Accountname;
	public InputField In_Password;

	public GameObject UserPanel, MainMenu;
	public Text UserPanelText;
	public GameObject userStatusOnline, userStatusOffline;

	public static int userId;

	//pass the user name and pw
	// public string usrname;
	// public string pw;
	// public int usrscore;
	// public string regweb="http://www2.comp.polyu.edu.hk/~14109998d/regUser.php";
	// public string updateR="http://www2.comp.polyu.edu.hk/~14109998d/updateRanking.php";
	// public string downloadR="http://www2.comp.polyu.edu.hk/~14109998d/downloadRanking.php";

	// Use this for initialization
	void Start () {
		// startButton.onClick.AddListener(startGame);
		// exitButton.onClick.AddListener(quitGame);
		loginButton.onClick.AddListener(delegate{
			StartCoroutine("login");
		});
		logoutButton.onClick.AddListener(delegate{
			UserPanel.SetActive(false);
			MainMenu.SetActive(true);
			userId = -1;
		});

		regButton.onClick.AddListener(delegate{
			StartCoroutine("register");
		});

		level1.onClick.AddListener(delegate{selectMap(1);});
		level2.onClick.AddListener(delegate{selectMap(2);});
		level3.onClick.AddListener(delegate{selectMap(3);});
		level4.onClick.AddListener(delegate{selectMap(4);});
		level5.onClick.AddListener(delegate{selectMap(5);});
		level6.onClick.AddListener(delegate{selectMap(6);});
		starter.onClick.AddListener(delegate{Application.LoadLevel("Scene_0");});
		ExitConfirm.onClick.AddListener(delegate{Application.Quit();});

		userId = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if(userId == -1){
			userStatusOnline.SetActive(false);
			userStatusOffline.SetActive(true);
		}else{
			userStatusOnline.SetActive(true);
			userStatusOffline.SetActive(false);
		}
	}

	void startGame(){
		// Debug.Log("Start game!");
		// Application.LoadLevel("Main");
	}

	void quitGame(){
		Debug.Log("Quit game!");
		Application.Quit();
	}

	IEnumerator login(){
		Debug.Log("Login with username: "+ LoginUsername.text +", and password: "+LoginPassword.text);
		using (WWW www = new WWW("http://www2.comp.polyu.edu.hk/~14109998d/Login.php?username="+LoginUsername.text+"&password="+LoginPassword.text))
        	{
				yield return www;
            	if(www.text.Equals("E")) 
					loginErr();
				else{
					userId =  Int32.Parse(www.text);
					loginWindow.SetActive(false);
					loginSuccess.SetActive(true);
					UserPanelText.text="You are logged in as "+LoginUsername.text+".";
				}
			}
	}

	IEnumerator register(){
		if(!RegPassword.text.Equals(RegPasswordConfirm.text)){
			regErr();
		}else{
			using (WWW www = new WWW("http://www2.comp.polyu.edu.hk/~14109998d/Register.php?username="+RegUsername.text+"&password="+RegPassword.text))
        	{
				yield return www;
            	if(!www.text.Equals("S")) 
					regErr();
				else{
					regWindow.SetActive(false);
					regSuccess.SetActive(true);
				}
			}
        }
	}

	void loginErr(){
		loginWindow.SetActive(false);
		loginError.SetActive(true);
	}

	void regErr(){
		regWindow.SetActive(false);
		regError.SetActive(true);
	}

	//register and submit the data to mySQL


	void selectMap(int id){
		MapDescripton.SetActive(true);
		MapSelection.SetActive(false);
		MapDescriptonTitle.text="Level "+id;
		switch(id){
			// Load the Goal of the map to Goal ,update code here
			case 1:
				Goal.text = "Get 2 scores in 20 seconds";
				break;
			case 2:
				Goal.text = "Get 2 scores in 30 seconds";
				break;
			case 3:
				Goal.text = "Get 4 scores in 30 seconds";
				break;
			case 4:
				Goal.text = "Get 7 scores in 45 seconds";
				break;
			case 5:
				Goal.text = "Get 8 scores in 60 seconds";
				break;
			case 6:
				Goal.text = "Get 9 scores in 120 seconds";
				break;
		}
		
		//fetch the leaderboard, update code here
		//Bronze.text = "Yu";
		//Silver.text = "Jia";
		//Gold.text = "Li";
		IEnumerator coroutine = GetLeaderBoard(id);
		StartCoroutine(coroutine);
		EnterMap.onClick.AddListener(delegate{Application.LoadLevel("Scene_"+id);});
	}

	IEnumerator GetLeaderBoard(int id){
		Gold.text="";
		Silver.text="";
		Bronze.text="";
		// Debug.Log("dalaotql");   
		using (WWW www = new WWW("http://www2.comp.polyu.edu.hk/~14109998d/Rank.php?Level="+id))
        {
            yield return www;
			string[] result = www.text.Split(new char[] {' '});
			try{
				Gold.text = result[0]+"\t"+result[1];
				Silver.text = result[2]+"\t"+result[3];
				Bronze.text = result[4]+"\t"+result[5];
			}catch(Exception ex){

			}
			// Debug.Log("Complete get leader board.");
        }
	}



	}

