using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
	private int gridRows = 2;
	private int gridCols = 3;

    public float delayBeforeShowingVictoryImage = 0.5f;
	private bool endgameflag= false;
	private bool isDead;
	public GameObject gameOverUI1;
	public GameObject gameOverUI2;
	[SerializeField] private MemoryCard originalCard;
	[SerializeField] private Sprite[] images;
	[SerializeField] private TextMeshProUGUI scoreLabel;
	[SerializeField] private GameObject particleEffectPrefab;
	public const float offsetX = 1f;
	public const float offsetY = 1.5f;

	private int _score = 0;
	private bool updateRan = false;
	private int[] selectedNumbers;
	private MemoryCard _firstRevealed;
	private MemoryCard _secondRevealed;
    private Vector3 startingposition1;
	private Vector3 startingposition2;
	public GameObject particleEffect;
	public GameObject particleEffect2;
    public float shakevalue1 = 0.1f;
    public float shaketime = 0.7f;
    private float recordoftime = 0f;




	public bool canReveal {
		get {return _secondRevealed == null;}
	}

	void Awake()
	{
		if (PlayerPrefs.GetInt("rows") != 0)
		{
		 gridRows = PlayerPrefs.GetInt("rows", 2);
		 gridCols = PlayerPrefs.GetInt("columns", 4);
		}
	}
	// Use this for initialization
	void Start() {
		gameOverUI1.SetActive(false);
    	gameOverUI2.SetActive(false);

        selectedNumbers = GetRandomNumbers(gridRows * gridCols / 2);
        int[] numbers = new int[gridRows * gridCols];
        int indexA = 0;
        // Repeat each number twice
        foreach (int number in selectedNumbers)
        {
            numbers[indexA] = number;
            numbers[indexA + 1] = number;
            indexA += 2;
        }
		Vector3 startPos = originalCard.transform.position;

		// create shuffled list of cards
		//call random method select from 41 cards
		numbers = ShuffleArray(numbers); 

		// place cards in a grid
		for (int i = 0; i < gridCols; i++) {
			for (int j = 0; j < gridRows; j++) {
				MemoryCard card;

				// use the original for the first grid space
				if (i == 0 && j == 0) {
					card = originalCard;
				} else {
					card = Instantiate(originalCard) as MemoryCard;
				}

				// next card in the list for each grid space
				int index = j * gridCols + i;
				int id = numbers[index];
				card.SetCard(id, images[id]);

				float posX = (offsetX * i) + startPos.x;
				float posY = -(offsetY * j) + startPos.y;
				card.transform.position = new Vector3(posX, posY, startPos.z);
			}
		}
	}

		//random number method
	private int[] GetRandomNumbers(int count)
    	{
        	int[] numbers = new int[count];
        	List<int> numberList = new List<int>();

        	for (int i = 0; i < 41; i++)
        	{
            	numberList.Add(i);
        	}

        	for (int i = 0; i < count; i++)
        	{
            	int randomIndex = UnityEngine.Random.Range(0, numberList.Count);
            	numbers[i] = numberList[randomIndex];
            	numberList.RemoveAt(randomIndex);
        	}
		return numbers;
    }


	// Knuth shuffle algorithm
	private int[] ShuffleArray(int[] numbers) {
		int[] newArray = numbers.Clone() as int[];
		for (int i = 0; i < newArray.Length; i++ ) {
			int tmp = newArray[i];
			int r = Random.Range(i, newArray.Length);
			newArray[i] = newArray[r];
			newArray[r] = tmp;
		}
		return newArray;
	}

	public void CardRevealed(MemoryCard card) {
		if (_firstRevealed == null) {
			_firstRevealed = card;
		} else {
			_secondRevealed = card;
			StartCoroutine(CheckMatch());
		}
	}
	
	private IEnumerator CheckMatch() {

		// increment score if the cards match
		if (_firstRevealed.id == _secondRevealed.id) {
			_score++;
			scoreLabel.text = "Score: " + _score;
			int targetscore = (gridCols * gridRows) / 2;
			//flag set to true if all cards 
			if(_score == targetscore){
				endgameflag = true;
			}
		
			//method of effect when matching two cards
			startingposition1 = _firstRevealed.transform.position;
			startingposition2 = _secondRevealed.transform.position;
			while (recordoftime < shaketime) {
				float point1 = startingposition1.x + Random.Range(-1f, 1f) * shakevalue1;
				float point2 = startingposition1.y + Random.Range(-1f, 1f) * shakevalue1;
				_firstRevealed.transform.localPosition = new Vector3(point1, point2, startingposition1.z);
				float point3 = startingposition2.x + Random.Range(-1f, 1f) * shakevalue1;
				float point4 = startingposition2.y + Random.Range(-1f, 1f) * shakevalue1;
				_secondRevealed.transform.localPosition = new Vector3(point3, point4, startingposition2.z);
				recordoftime += Time.deltaTime;
				yield return null;
			}

			shakevalue1 = 0.1f;
			shaketime = 0.5f;
			recordoftime = 0f;
			_firstRevealed.transform.localPosition = startingposition1;
			_secondRevealed.transform.localPosition = startingposition2;

			particleEffect = Instantiate(particleEffectPrefab, _firstRevealed.transform.position, Quaternion.identity) as GameObject;
			particleEffect.GetComponent<ParticleSystem>().Play();
			particleEffect2 = Instantiate(particleEffectPrefab, _secondRevealed.transform.position, Quaternion.identity) as GameObject;
			particleEffect2.GetComponent<ParticleSystem>().Play();
			// destroy the cards and the particle effect after a short delay
			Destroy(_firstRevealed.gameObject, 0.5f);
			Destroy(_secondRevealed.gameObject, 0.5f);
			Destroy(particleEffect, 1.0f);
			Destroy(particleEffect2, 1.0f);
		}

		// otherwise turn them back over after .5s pause
		else {
			yield return new WaitForSeconds(.5f);
			_firstRevealed.Unreveal();
			_secondRevealed.Unreveal();
		}
		
		_firstRevealed = null;
		_secondRevealed = null;
	}

		// Update is called once per frame
		void Update()
		{
        	if (endgameflag && !isDead)
        	{
				isDead = true;
				gameOver();
        	}
        	if (!updateRan)
        	{
				updateRan = true;
        	}
		}

	public void Restart() {

        //Application.LoadLevel("Scene"); /* obsolete since Unity 2017 */
        if (updateRan)
        {
			SceneManager.LoadScene("Scene");
		}
	}

	public void SetSize(int row, int col)
	{/* modified to resize card grid */
		gridRows = row;
		gridCols = col;
		Restart();
	}


	public void gameOver()
	{
		gameOverUI1.SetActive(true);
    	StartCoroutine(GameOverCoroutine());
	}

	private IEnumerator GameOverCoroutine()
	{
    	yield return new WaitForSeconds(1f);
 		gameOverUI2.SetActive(true);
	}
    public void QuitGame()
    {
        Application.Quit();
    }

}
