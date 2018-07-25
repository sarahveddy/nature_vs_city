using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class InGameScript : MonoBehaviour {

/*
*	FUNCITON:
*	- This script holds the global variables that the other scripts are dependent on.
*	- It provides interaction among scripts.
*	- This script controls game states (launch, pause, death etc.)
*
*	USED BY: This script is a part of the "Player" prefab.
*
*/

private int CurrentEnergy = 100;	//player's energy (set to zero on death)
private int iLevelScore = 0;	//current score (calculated based on distance traveled)

//script references
private MenuScript hMenuScript;
private ControllerScript hControllerScript;
private SoundManager hSoundManager;
private PowerupsMainController hPowerupsMainController;
private EnemyController hEnemyController;
private CameraController hCameraController;

private int iPauseStatus = 0;
private int iDeathStatus = 0;
private int iMenuStatus;

private bool  bGameOver = false;
private bool  bGamePaused = false;

private bool gamePlayStart = false;

public float quitButtonTime = 60 * 5;
public float quitGameTime = 60 * 15;

private float pauseStart = 0.0f;
private float pauseEnd = 0.0f;
private float pauseTime = 0.0f;

	

void Start (){
	//Application.targetFrameRate = 60;		//ceiling the frame rate on 60 (debug only)
	
	RenderSettings.fog = true;				//turn on fog on launch
		
	hSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>() as SoundManager;
	hMenuScript = GameObject.Find("MenuGroup").GetComponent<MenuScript>() as MenuScript;
	hControllerScript = this.GetComponent<ControllerScript>() as ControllerScript;
	hPowerupsMainController = this.GetComponent<PowerupsMainController>() as PowerupsMainController;
    hCameraController = Camera.main.GetComponent<CameraController>() as CameraController;
	hEnemyController = this.GetComponent<EnemyController>() as EnemyController;
	
	CurrentEnergy = 100;
	iPauseStatus = 0;
	iMenuStatus = 1;
	
	bGameOver = false;
	bGamePaused = true;
	
}//end of Start

void Update (){	
	
	
	//Decides if it is time for the quitbutton to appear 
	if (Time.realtimeSinceStartup - PersistentManagerScript.Instance.gameplayStart - pauseTime > quitButtonTime &&
	    PersistentManagerScript.Instance.gameplayStarted &&
	    iPauseStatus == 0) 
	{
		hControllerScript.tQuitButton.gameObject.active = true;
		PersistentManagerScript.Instance.exitButton = true; 
		//Debug.Log("****QUIT BUTTON******* "+Time.realtimeSinceStartup);
	}
	
	//Decides if it is time to automatically close the game
	if (Time.realtimeSinceStartup - PersistentManagerScript.Instance.gameplayStart - pauseTime > quitGameTime &&
	    PersistentManagerScript.Instance.gameplayStarted &&
	    iPauseStatus == 0)
	{
		//Debug.Log("*********QUIT********** " +Time.realtimeSinceStartup);
		Quit();
	}
	
	//there is no menu to be displayed
	if (iMenuStatus == 0)//normal gameplay
	{
		if (PersistentManagerScript.Instance.gameplayStart == 0.0)
		{
			//Debug.Log("SETTING VALUES");
			PersistentManagerScript.Instance.gameplayStart = Time.realtimeSinceStartup;
			PersistentManagerScript.Instance.gameplayStarted = true; 
			//Debug.Log("GAMEPLAY START : "+ PersistentManagerScript.Instance.gameplayStart.ToString()); 	
		}		
	}		
	
	//main menu is to be displayed
	else if (iMenuStatus == 1)//display main menu and pause game
	{
		hMenuScript.setMenuScriptStatus(true);
				
		bGamePaused = true;
		iMenuStatus = 2;
	}
	
	
	//no pause menu to be displayed
	if (iPauseStatus == 0)//normal gameplay
	{
		;
	}
	//pause menu displayed
	else if(iPauseStatus==1)//pause game
	{	
		Debug.Log("***** PAUSE ****** "+Time.timeSinceLevelLoad);
		pauseStart = Time.timeSinceLevelLoad; 
		hMenuScript.setMenuScriptStatus(true);
		hMenuScript.displayPauseMenu();
		
		iPauseStatus = 2;
		
	}
	//game is resumed
	else if(iPauseStatus==3)//resume game
	{	Debug.Log("***** RESUME ****** "+Time.timeSinceLevelLoad);
		pauseEnd = Time.timeSinceLevelLoad;

		pauseTime += (pauseEnd - pauseStart); 
		bGamePaused = false;		
		hMenuScript.setMenuScriptStatus(false);
		
		iPauseStatus = 0;
	}
	
	
	//player is not dead
	if (iDeathStatus == 0)//normal gameplay
	{
		;
	}	
	//player is dead	
	else if(iDeathStatus==1)//call death menu
	
	{
		Debug.Log("*** 7 death status = 1 Update InGameScript ***");
		hPowerupsMainController.deactivateAllPowerups();	//deactivate if a powerup is enabled
		
		iDeathStatus = 2;
	}
	//main menu is displayed and scene is reloaded
	else if (iDeathStatus == 2)
	{
		Debug.Log("*** 8 death status = 2 Update InGameScript ***");
		Debug.Log(iMenuStatus);
		Debug.Log(iPauseStatus);
		//hMenuScript.setMenuScriptStatus(true);
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		
		iDeathStatus = 0;
		bGameOver = false;
		bGamePaused = false;
		CurrentEnergy = 100;
		hControllerScript.launchGame();
	}
	
	if (bGamePaused == true)
		return;
	
	
	
}//end of Update()

/*
*	FUNCTION: Pause the game
*	CALLED BY: ControllerScript.getClicks()
*/
public void pauseGame (){
	hControllerScript.togglePlayerAnimation(false);
	bGamePaused = true;
	iPauseStatus = 1;
	
	hSoundManager.stopAllSounds();
}

/*
*	FUNCTION: start the gameplay and display all related elements
*	CALLED BY: MenuScript.MainMenuGui()
*			   MenuScript.MissionsGui()
*/
public void launchGame (){	
	iMenuStatus = 0;
	bGamePaused = false;	
	hMenuScript.showHUDElements();	
	hControllerScript.launchGame();
	hCameraController.launchGame();
}

/*
*	FUNCTION: Display death menu and end game
*	CALLED BY:	ControllerScript.DeathScene()
*/
public void setupDeathMenu (){	
	Debug.Log("*** 6 setupDeathMenu InGameScript ***");
	bGameOver = true;
	bGamePaused = true;	
	iDeathStatus = 1;
}//end of Setup Death Menu

/*
*	FUNCTION: Execute a function based on button press in Pause Menu
*	CALLED BY: MenuScript.PauseMenu()
*/
    public void processClicksPauseMenu ( MenuScript.PauseMenuEvents index  ){
        if (index == MenuScript.PauseMenuEvents.MainMenu )
	{	
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		
            hMenuScript.InvokeShowMenu((int)MenuScript.MenuIDs.MainMenu);
		iMenuStatus = 1;	//display main menu
	}
        else if (index == MenuScript.PauseMenuEvents.Resume)
	{
		hMenuScript.showHUDElements();
		hControllerScript.togglePlayerAnimation(true);
		iPauseStatus = 3;
	}
}

/*
*	FUNCTION: Execute a function based on button press in Death Menu
*	CALLED BY: MenuScript.GameOverMenu()
*/
    public void procesClicksDeathMenu ( MenuScript.GameOverMenuEvents index  ){
        if (index == MenuScript.GameOverMenuEvents.Play)
	{
		hMenuScript.showHUDElements();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		launchGame();
	}
        else if (index == MenuScript.GameOverMenuEvents.Back)
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		
            hMenuScript.InvokeShowMenu((int)MenuScript.MenuIDs.MainMenu);
		iMenuStatus = 1;	//display main menu
	}
}//end of DM_ProcessClicks

/*
*	FUNCTION: Is called when a collision occurs
*	CALLED BY:	PlayerFrontColliderScript.OnCollisionEnter
*				processStumble()
*/
public void collidedWithObstacle (){
	Debug.Log("*** 2 collidedWithObstacle InGameScript ***");
	decrementEnergy(100);		// deduct energy after collision
	Debug.Log("*** 3 energy decremented collidedWithObstacle InGameScript ***");
	hCameraController.setCameraShakeImpulseValue(5);
}//end of Collided With Obstacle

/*
*	FUNCTION: Pause game if application closed/ switched on device
*/
void OnApplicationPause ( bool pause  ){
	//Debug.Log("Application Paused : "+pause);
	if(Application.isEditor==false)
	{
		if(bGamePaused==false&&pause==false)
		{
			pauseGame();
		}
	}	
}
	
public void Quit()
{
//	Debug.Log(Application.absoluteURL);
	string[] s = Application.absoluteURL.Split('/');
//	Debug.Log(s[0] + "       "+ s[1]);
	string newURL = s[0] + "/redirect_next_page";
//	Debug.Log(newURL);
	FindObjectOfType<Logging>().SendLog("QUIT BUTTON PRESSED", (Time.realtimeSinceStartup - PersistentManagerScript.Instance.gameplayStart - pauseTime).ToString());
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false; 
	#else
		Application.OpenURL(newURL); 
	#endif
}
	
public bool isGamePaused (){ return bGamePaused; }
public int getLevelScore (){ return iLevelScore; }
public void incrementLevelScore ( int iValue  ){ iLevelScore += iValue; }
public int getCurrentEnergy (){ return CurrentEnergy; }
public bool isEnergyZero (){  return (CurrentEnergy <= 0 ? true : false); }
public void decrementEnergy ( int iValue  ){ CurrentEnergy -= iValue; }

}