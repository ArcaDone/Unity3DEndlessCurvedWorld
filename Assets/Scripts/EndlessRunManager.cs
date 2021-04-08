using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndlessRunManager : MonoBehaviour
{
	public static int difficulty;
	private string[] difficultyNames = { "NaN", "Al all", "A Bit", "A Lot" };

	private float score;
	public Text scoreUI;
	private int highscore;
	public Text highscoreUI;

	public Transform player;
	public PlayerMovement movement;

	public GameObject[] obstaclePrefabs;
	public GameObject[] treePrefabs;
	public Transform obstacleSpawner;
	public Transform treeSpawner;
	public float[] TreesPositionX;
	public int obstacleStartDist = 100;

	public GameObject deathOverlayUI;

	public Fade fade;

	IEnumerator DeathOverlayTransition()
	{
		yield return new WaitForSeconds(1);
		yield return new WaitForSeconds(fade.BeginFade(1));

		deathOverlayUI.SetActive(true);

		yield return new WaitForSeconds(fade.BeginFade(-1));
	}

	IEnumerator SceneTransition(int scene)
	{
		yield return new WaitForSeconds(fade.BeginFade(1));

		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	public void SwitchScene(int scene)
	{
		StartCoroutine(SceneTransition(scene));
	}

	public void InitiateDeath()
	{
		CancelInvoke("Spawn");
		CancelInvoke("SpawnTree");

		FindObjectOfType<PlayerMovement>().enabled = false;

		foreach (Transform obstacle in obstacleSpawner)
		{
			obstacle.gameObject.GetComponent<ObstacleMovement>().enabled = false;
		}

		foreach (Transform tree in treeSpawner)
		{
			tree.gameObject.GetComponent<TreeMovements>().enabled = false;
		}

		UpdateHighscore();
		highscoreUI.text = difficultyNames[difficulty] + " highscore: " + highscore;

		StartCoroutine(DeathOverlayTransition());
	}

	private void UpdateHighscore()
	{
		highscore = PlayerPrefs.GetInt("Highscore" + difficulty);

		if (score > highscore)
		{
			highscore = (int)score;
			PlayerPrefs.SetInt("Highscore" + difficulty, highscore);
		}
	}

	private void Spawn()
	{
		int i;
		 
		GameObject _obstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

		for (i = -7; i < 7; i += 7)
		{
			Instantiate(_obstacle,
			            new Vector3(Mathf.Floor(Random.Range(i, i + 7)), 1, obstacleStartDist),
			            Quaternion.identity, obstacleSpawner);
		}
	}

	private void SpawnTree()
	{
		GameObject trees = treePrefabs[Random.Range(0, treePrefabs.Length)];

		for ( int i = 0; i < 2; i ++)
		{
			Instantiate(trees,
				new Vector3(TreesPositionX[Random.Range(0,TreesPositionX.Length)], -1.5f, obstacleStartDist),
						trees.gameObject.transform.rotation, treeSpawner);
		}
	}

	private void Update()
	{
		if (FindObjectOfType<PlayerMovement>().enabled)
		{
			score += Time.deltaTime * 10;
			scoreUI.text = "Score: " + (int)score;
		}

		if (Input.GetKey("r"))
		{
			SwitchScene(1);
			return;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (deathOverlayUI.activeSelf)
			{
				SwitchScene(0);
			}
			else
			{
				InitiateDeath();
			}
			return;
		}
	}

	private void Start()
	{
		fade.BeginFade(-1);

		InvokeRepeating("Spawn", 1f, 3.5f / difficulty);
		InvokeRepeating("SpawnTree", Random.Range(2f,5f), 3f / difficulty);

	}
}
