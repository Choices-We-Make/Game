using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Reference: http://catlikecoding.com/unity/tutorials/maze/
public class GameManager : MonoBehaviour {
	
	public Maze mazePrefab;
	private Maze mazeInstance;
	private int winCount;
	private int lossCount;
	public Text winText;
	public Text lossText;
	public Text timerText;
	public Text criticalText;
	private int priorTime;
	private float timeLimit;

	private void Start () {
		BeginGame();
		winCount=0;
		lossCount=0;
	}
	
	private void Update () {
		timerText.text = Mathf.Ceil(timeLimit).ToString();
		if(timeLimit<10.0f){
			timerText.color = new Color(1,0,0);
			Color myColor = criticalText.color;
			if(myColor.a>0)
			{
				myColor.a = Mathf.Max (0, myColor.a-2.0f*Time.deltaTime);
			}
			if(Mathf.Ceil(timeLimit)<priorTime)
			{
				myColor.a = 1.0f;
				criticalText.text = Mathf.Ceil (timeLimit).ToString();
			}
			criticalText.color = myColor;
		}
		else
		{

			criticalText.color = new Color(1,0,0,0);	
		}
		if(mazeInstance.cellPrefab.reset){
			winCount+=1;
			winText.text = "Wins: "+winCount.ToString();
			RestartGame();
		}
		priorTime = (int) Mathf.Ceil(timeLimit);
		timeLimit -= Time.deltaTime;
		if (timeLimit<0) {
			lossCount+=1;
			lossText.text = "Losses: "+lossCount.ToString();
			RestartGame();
		}
		if(Input.GetKeyDown(KeyCode.Space))
		{
			int pathDifficulty=0;
			List<IntVector2> bestPath;
			bestPath = mazeInstance.FindShortestPath(mazeInstance.PlayerLocation(),mazeInstance.exit, ref pathDifficulty);
			foreach(IntVector2 coord in bestPath)
			{
				mazeInstance.GetCell(coord).lightFloor(true);
			}
		}
		if(Input.GetKeyDown(KeyCode.End))
		{
			RestartGame();
		}
 	}
	private void BeginGame () {
		mazeInstance = Instantiate(mazePrefab) as Maze;
		//StartCoroutine(mazeInstance.Generate());
		mazeInstance.Generate();
		timeLimit = mazeInstance.difficulty *1.5f;
		timerText.color = new Color(0,1,213.0f/255.0f);
	}
	
	public void RestartGame () {
		StopAllCoroutines();
		mazeInstance.killPlayer();
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}
}
