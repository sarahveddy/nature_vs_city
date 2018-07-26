using UnityEngine;
using System.Collections;

public class PatchesRandomizer : MonoBehaviour {


/*
*	FUNCTION:
*	- This scirpt handles the creation and destruction of the environment patches.
*
*	USED BY:
*	This script is a part of the "Player" prefab.
*
*/

public GameObject[] patchesPrefabs;//patches that will be generated

private GameObject goPreviousPatch;//the patch the the player passed
private GameObject goCurrentPatch;//the patch the player is currently on
private GameObject goNextPatch;//the next patch located immediatly after current patch
private float fPatchDistance = 3000.0f;//default displacement of patch
private Transform tPlayer;//player transform

private float fPreviousTotalDistance = 0.0f;//total displacement covered
private int iCurrentPNum = 1;//number of patches generated

//script references
private InGameScript hInGameScript;
private ElementsGenerator hElementsGenerator;
private CheckPointsMain hCheckPointsMain;


public int[] presetPatchOrder =  {0, 1, 2, 3, 4, 5};
private int patchOrderIndex = 0; 

//get the current path length
public float getCoveredDistance (){ return fPreviousTotalDistance; } 

void Start (){
	iCurrentPNum = 1;	
	fPreviousTotalDistance = 0.0f;
	
	hInGameScript = this.GetComponent<InGameScript>() as InGameScript;
	hCheckPointsMain = GetComponent<CheckPointsMain>() as CheckPointsMain;
	hElementsGenerator = this.GetComponent<ElementsGenerator>() as ElementsGenerator;
	
	instantiateStartPatch();	
	goPreviousPatch = goCurrentPatch;	
	
	tPlayer = GameObject.Find("Player").transform;
	hCheckPointsMain.setChildGroups();
	
	hCheckPointsMain.SetCurrentPatchCPs();
	hCheckPointsMain.SetNextPatchCPs();
}

void Update (){
	if(hInGameScript.isGamePaused()==true)
		return;
	
	if(tPlayer.position.x>(iCurrentPNum*fPatchDistance)+100.0f)
	{
		Destroy(goPreviousPatch);
		iCurrentPNum++;
	}
}//end of update

/*
*	FUNCTION: Create a new Patch after the player reaches goNextPatch
*/
public void createNewPatch (){
	goPreviousPatch = goCurrentPatch;
	goCurrentPatch = goNextPatch;
	
	instantiateNextPatch();	
	hCheckPointsMain.setChildGroups();
	
	fPreviousTotalDistance += CheckPointsMain.fPathLength;
	
	//hElementsGenerator.generateElements();	//generate obstacles on created patch
}

private void instantiateNextPatch (){	
	goNextPatch = Instantiate(patchesPrefabs[patchIndexSelector()],new Vector3(fPatchDistance*(iCurrentPNum+1),0,0),Quaternion.identity) as GameObject;
//	if (!PersistentManagerScript.Instance.obstacles)
//	{ 
//		goNextPatch.transform.Find("Obstacles").gameObject.SetActiveRecursively(false);
//	}
}

/*
*	FUNCTION: Instantiate the first patch on start of the game.
*	CALLED BY: Start()
*/
private void instantiateStartPatch (){
	
	goCurrentPatch = Instantiate(patchesPrefabs[patchIndexSelector()], new Vector3(0,0,0),Quaternion.identity) as GameObject;
	goNextPatch = Instantiate(patchesPrefabs[patchIndexSelector()],new Vector3(fPatchDistance,0,0),Quaternion.identity) as GameObject;
//	if (!PersistentManagerScript.Instance.obstacles)
//	{
//		Collider colliders = goCurrentPatch.GetComponentsInChildren(typeof(Collider)) as Collider;
//		foreach (Component col in colliders)
//		{
//			col.enabled = false
//		}
//		goNextPatch.GetComponentsInChildren(typeof(Collider));
//		
//		goCurrentPatch.GetComponentsInChildren(typeof(Renderer));
//		goNextPatch.GetComponentsInChildren(typeof(Renderer));
//
//		
////		goCurrentPatch.transform.Find("Obstacles").gameObject.SetActiveRecursively(false);
////		goNextPatch.transform.Find("Obstacles").gameObject.SetActiveRecursively(false);
//	}
}

private int patchIndexSelector()
{
	patchOrderIndex++;
	if(patchOrderIndex>presetPatchOrder.Length-1)
	{
		patchOrderIndex = 0; 
	}
	
	return presetPatchOrder[patchOrderIndex];
}

public GameObject getCurrentPatch (){ return goCurrentPatch; }
public GameObject getNextPatch (){ return goNextPatch; }
}