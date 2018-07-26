using UnityEngine;
using System; 
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class InGameScript : MonoBehaviour
	{
	
	/*
	*	FUNCITON:
	*	- This script holds the global variables that the other scripts are dependent on.
	*	- It provides interaction among scripts.
	*	- This script controls game states (launch, pause, death etc.)
	*
	*	USED BY: This script is a part of the "Player" prefab.
	*
	*/
		private int CurrentEnergy = 100; //player's energy (set to zero on death)
		private int iLevelScore = 0; //current score (calculated based on distance traveled)
	
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
	
		private bool bGameOver = false;
		private bool bGamePaused = false;
	
		private bool gamePlayStart = false;
	
		private float quitButtonTime = 60 * 5;
		private float quitGameTime = 60 * 15;
	
		private float pauseStart = 0.0f;
		private float pauseEnd = 0.0f;
		private float pauseTime = 0.0f;
		
		private float timePlayed = 0.0f;
		public float timeLeft;

		
	
	
		void Start()
		{
			RenderSettings.fog = true; //turn on fog on launch
	
			hSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>() as SoundManager;
			hMenuScript = GameObject.Find("MenuGroup").GetComponent<MenuScript>() as MenuScript;
			hControllerScript = this.GetComponent<ControllerScript>() as ControllerScript;
			hPowerupsMainController = this.GetComponent<PowerupsMainController>() as PowerupsMainController;
			hCameraController = Camera.main.GetComponent<CameraController>() as CameraController;
			hEnemyController = this.GetComponent<EnemyController>() as EnemyController;
	
			CurrentEnergy = 100;
			iPauseStatus = 0;
			iMenuStatus = 1;

			quitButtonTime = PersistentManagerScript.Instance.secondsBeforeQuitButton;
			quitGameTime = PersistentManagerScript.Instance.secondsBeforeGameEnd; 
	
			bGameOver = false;
			bGamePaused = true;
	
		} 
	
		void Update()
		{
			//Keeps track of gameplay time
			timePlayed = Time.realtimeSinceStartup - pauseTime - PersistentManagerScript.Instance.gameplayStart;
			timeLeft = quitGameTime - timePlayed;

			//Decides if it is time for the quitbutton to appear 
			if (Time.realtimeSinceStartup - PersistentManagerScript.Instance.gameplayStart - pauseTime > quitButtonTime &&
				PersistentManagerScript.Instance.gameplayStarted &&
				iPauseStatus == 0)
			{
				hControllerScript.tQuitButton.gameObject.active = true;
				PersistentManagerScript.Instance.exitButton = true;
			}
	
			//Decides if it is time to automatically close the game
			if (Time.realtimeSinceStartup - PersistentManagerScript.Instance.gameplayStart - pauseTime > quitGameTime &&
				PersistentManagerScript.Instance.gameplayStarted &&
				iPauseStatus == 0)
			{
				Quit();
			}
			
			//there is no menu to be displayed
			if (iMenuStatus == 0) //normal gameplay
			{
				if (PersistentManagerScript.Instance.gameplayStart == 0.0)
				{
					PersistentManagerScript.Instance.gameplayStart = Time.realtimeSinceStartup;
					PersistentManagerScript.Instance.gameplayStarted = true;
				}
			}
	
			//main menu is to be displayed
			else if (iMenuStatus == 1) //display main menu and pause game
			{
				hMenuScript.setMenuScriptStatus(true);
	
				bGamePaused = true;
				iMenuStatus = 2;
			}
	
			//no pause menu to be displayed
			if (iPauseStatus == 0) //normal gameplay
			{
				;
			}
			//pause menu displayed
			else if (iPauseStatus == 1) //pause game
			{
				//Debug.Log("***** PAUSE ****** "+Time.timeSinceLevelLoad);
				if (PersistentManagerScript.Instance.gameplayStarted)
				{
					pauseStart = Time.timeSinceLevelLoad;
				}
	
				hMenuScript.setMenuScriptStatus(true);
				hMenuScript.displayPauseMenu();
	
				iPauseStatus = 2;
			}
			
			//game is resumed
			else if (iPauseStatus == 3) //resume game
			{
				//Debug.Log("***** RESUME ****** "+Time.timeSinceLevelLoad);
				if (PersistentManagerScript.Instance.gameplayStarted)
				{
					pauseEnd = Time.timeSinceLevelLoad;
	
					pauseTime += (pauseEnd - pauseStart);
				}
	
				bGamePaused = false;
				hMenuScript.setMenuScriptStatus(false);
	
				iPauseStatus = 0;
				hControllerScript.setCurrentWalkSpeed(100);
			}
	
			//player is not dead
			if (iDeathStatus == 0) //normal gameplay
			{
				;
			}
			
			//player is dead	
			else if (iDeathStatus == 1) //call death menu
	
			{
				//Debug.Log("*** 7 death status = 1 Update InGameScript ***");
				hPowerupsMainController.deactivateAllPowerups(); //deactivate if a powerup is enabled
	
				iDeathStatus = 2;
			}
			
			//main menu is displayed and scene is reloaded
			else if (iDeathStatus == 2)
			{
				//Debug.Log("*** 8 death status = 2 Update InGameScript ***");
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
	
		} 
	
	/*
	*	FUNCTION: Pause the game
	*	CALLED BY: ControllerScript.getClicks()
	*/
		public void pauseGame()
		{
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
		public void launchGame()
		{
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
		public void setupDeathMenu()
		{
			//Debug.Log("*** 6 setupDeathMenu InGameScript ***");
			bGameOver = true;
			bGamePaused = true;
			iDeathStatus = 1;
		} //end of Setup Death Menu
	
	/*
	*	FUNCTION: Execute a function based on button press in Pause Menu
	*	CALLED BY: MenuScript.PauseMenu()
	*/
		public void processClicksPauseMenu(MenuScript.PauseMenuEvents index)
		{
			if (index == MenuScript.PauseMenuEvents.MainMenu)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	
				hMenuScript.InvokeShowMenu((int) MenuScript.MenuIDs.MainMenu);
				iMenuStatus = 1; //display main menu
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
		public void processClicksDeathMenu(MenuScript.GameOverMenuEvents index)
		{
			if (index == MenuScript.GameOverMenuEvents.Play)
			{
				hMenuScript.showHUDElements();
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				launchGame();
			}
			else if (index == MenuScript.GameOverMenuEvents.Back)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	
				hMenuScript.InvokeShowMenu((int) MenuScript.MenuIDs.MainMenu);
				iMenuStatus = 1; //display main menu
			}
		} 
	
	/*
	*	FUNCTION: Is called when a collision occurs
	*	CALLED BY:	PlayerFrontColliderScript.OnCollisionEnter
	*				processStumble()
	*/
		public void collidedWithObstacle()
		{
			//Debug.Log("*** 2 collidedWithObstacle InGameScript ***");
			decrementEnergy(100); // deduct energy after collision
			//Debug.Log("*** 3 energy decremented collidedWithObstacle InGameScript ***");
			hCameraController.setCameraShakeImpulseValue(5);
		} //end of Collided With Obstacle
	
	/*
	*	FUNCTION: Pause game if application closed/ switched on device
	*/
		void OnApplicationPause(bool pause)
		{
			//Debug.Log("Application Paused : "+pause);
			if (Application.isEditor == false)
			{
				if (bGamePaused == false && pause == false)
				{
					pauseGame();
				}
			}
		}
	
	/*
	*	FUNCTION: Close game (in editor), or redirect to next page
	*/
		public void Quit()
		{
			string[] s = Application.absoluteURL.Split('/');
			string newURL = s[0] + "/redirect_next_page";
			FindObjectOfType<Logging>().SendLog("QUIT BUTTON PRESSED",
				(Time.realtimeSinceStartup - PersistentManagerScript.Instance.gameplayStart - pauseTime).ToString());
			#if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying = false;
			#else
					Application.OpenURL(newURL); 
				#endif
		}
	
		public bool isGamePaused()
		{
			return bGamePaused;
		}
	
		public int getLevelScore()
		{
			return iLevelScore;
		}
	
		public void incrementLevelScore(int iValue)
		{
			iLevelScore += iValue;
		}
	
		public int getCurrentEnergy()
		{
			return CurrentEnergy;
		}
	
		public bool isEnergyZero()
		{
			return (CurrentEnergy <= 0 ? true : false);
		}
	
		public void decrementEnergy(int iValue)
		{
			CurrentEnergy -= iValue;
		}
	
		public string getTimeLeftFormatted()
		{
			double t = Math.Truncate(timeLeft);
	
			return String.Format("{0}:{1}", Math.Truncate(t / 60).ToString("00"), (t % 60).ToString("00"));
	
		}
	}