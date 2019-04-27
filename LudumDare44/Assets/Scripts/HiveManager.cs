using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HiveManager : Singleton<HiveManager>
{
    [Header("References")]
    [SerializeField]
    private Encounter _encountersHandler;

    [Header("Time")]

    [Tooltip("The time left in the game. When it reaches 0, player wins")]
    [SerializeField]
    private int _secondsToSurvive = 60 * 5;
    public int SecondsToSurvive
    {
        get { return _secondsToSurvive; }
        set
        {
            if (value < 0) _secondsToSurvive = 0;
            else _secondsToSurvive = value;
            string timeLeft = String.Format("{0:0}:{1:00}", Mathf.Floor(_secondsToSurvive / 60), _secondsToSurvive % 60);
            _timer.text = timeLeft;
        }
    }


    [Header("Honey")]

    [Tooltip("Player starting honey, and amt of honey during runtime")]
    [SerializeField]
    private int _currentHoney = 500;
    public int CurrentHoney
    {
        get { return _currentHoney; }
        set
        {
            if (value < 0) _currentHoney = 0;
            else _currentHoney = value;
            _honeyCount.value = _currentHoney;
            float currentPerc = CurrentHoney / MaxHoney;
            if (currentPerc <= _dangerHoneyThreshold && !_honeyIsLow)
            {
                _honeyCount.GetComponent<PulseObject>().Pulse();
                _honeyIsLow = true;
            }
            else if (currentPerc > _dangerHoneyThreshold && _honeyIsLow)
            {
                _honeyCount.GetComponent<PulseObject>().StopPulse();
                _honeyIsLow = false;
            }
        }
    }

    private bool _honeyIsLow = false;

    [SerializeField]
    private int _maxHoney = 1000;
    public int MaxHoney { get { return _maxHoney; } }

    /// <summary>
    /// At what percentage of honey do we warn players they may lose soon
    /// </summary>
    [SerializeField]
    [Range(0f, 1f)]
    private float _dangerHoneyThreshold = 0.33f;

    //@@@ Should be equal to (number of bees in each colony * honey intake) * 3
    private int _honeyLossPerInterval = 10;
    public int HoneyLossPerInterval
    {
        get { return _honeyLossPerInterval; }
        set
        {
            if (value <= 0) _honeyLossPerInterval = 1;
            else _honeyLossPerInterval = value;
        }
    }

    [Tooltip("How often in seconds hive loses honey")]
    [SerializeField]
    private int _honeyLossIntervalDuration = 1;

    [Header("DNA")]

    [Tooltip("Player starting dna, and amt of dna during runtime")]
    [SerializeField]
    private int _currentDNA = 100;
    public int CurrentDNA
    {
        get { return _currentDNA; }
        set
        {
            _currentDNA = value;
            _dnaCount.text = _currentDNA.ToString();
        }
    }

    [Tooltip("Every second, how much passive dna is gained")]
    [SerializeField]
    private int _dnaGenerationPerSecond = 1;
    public int DNAGenerationPerSecond
    {
        get { return _dnaGenerationPerSecond; }
        set
        {
            if (value < 1) _dnaGenerationPerSecond = 1;
            else _dnaGenerationPerSecond = value;
        }
    }


    [Header("Region Generation")]

    [Tooltip("How often in seconds a new region is generated and placed")]
    [SerializeField]
    private float _generateInterval = 30f;

    [Tooltip("The maximum amount of regions allowed")]
    [SerializeField]
    private int _regionCap = 10;

    [Header("Grid")]

    [SerializeField]
    private Vector2 _gridDimensions = new Vector2(30, 30);

    /// <summary>
    /// Unity units of each grid cell
    /// </summary>
    private Vector2 _cellSize
    {
        get
        {
            return new Vector2(_gridBoundary.bounds.size.x / _gridDimensions.x, _gridBoundary.bounds.size.y / _gridDimensions.y);
        }
    }

    /// <summary>
    /// Contains many indexs which refer to each grid cell
    /// </summary>
    private List<Vector2> _avaliableRegions = new List<Vector2>();

    [Header("Text messages")]

    [SerializeField]
    private string _winMessage = "You Win!";

    [SerializeField]
    private string _loseMessage = "You Lose";

    [Header("UI Elements")]

    [SerializeField]
    private Text _timer;

    [SerializeField]
    private SpriteRenderer _gridBoundary;

    [SerializeField]
    private Slider _honeyCount;

    [SerializeField]
    private Text _dnaCount;

    [SerializeField]
    private GameObject _endGameUi;

    [SerializeField]
    private Region _regionTemplate;

    //@@@ Colonies List

    private List<Region> _allRegions = new List<Region>();

    private void Awake()
    {
        SetAvaliableRegionIndices();

        _honeyCount.maxValue = MaxHoney;
        _honeyCount.value = CurrentHoney;
    }

    private void SetAvaliableRegionIndices()
    {
        for (int i = 0; i < _gridDimensions.x; ++i)
        {
            for (int j = 0; j < _gridDimensions.y; ++j)
            {
                _avaliableRegions.Add(new Vector2(i, j));
            }
        }
    }

    private void Start()
    {
        //Updates ui
        SecondsToSurvive = SecondsToSurvive;
        CurrentDNA = CurrentDNA;
        CurrentHoney = CurrentHoney;

        StartCoroutine(TimeLoss());
        StartCoroutine(HoneyLoss());
        StartCoroutine(DNAGain());
        StartCoroutine(GenerateRegion());

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator HoneyLoss()
    {
        while(true)
        {
            yield return new WaitForSeconds(_honeyLossIntervalDuration);
            CurrentHoney -= HoneyLossPerInterval;
            if (CurrentHoney <= 0) Lose();
        }
    }

    private IEnumerator DNAGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CurrentDNA += _dnaGenerationPerSecond;
        }
    }

    private IEnumerator TimeLoss()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SecondsToSurvive -= 1;
            if (SecondsToSurvive <= 0f) Win();
        }
    }

    private void Lose()
    {
        StopAllCoroutines();
        _endGameUi.SetActive(true);
        _endGameUi.GetComponentInChildren<Text>().text = _winMessage;
    }

    private void Win()
    {
        StopAllCoroutines();
        _endGameUi.SetActive(true);
        _endGameUi.GetComponentInChildren<Text>().text = _loseMessage;
    }

    private IEnumerator GenerateRegion()
    {
        while (_allRegions.Count != _regionCap)
        {
            yield return new WaitForSeconds(_generateInterval);
            int randomIndex = Random.Range(0, _avaliableRegions.Count);
            int temperature = Random.Range(-10, 11);
            int predator = Random.Range(0, 4);

            Vector2 gridOrigin = _gridBoundary.transform.position;
            float width = _gridBoundary.bounds.size.x;
            float height = _gridBoundary.bounds.size.y;
            Vector2 bottomLeftCorner = new Vector3(gridOrigin.x - width / 2f + _cellSize.x / 2f,
                gridOrigin.y - height / 2f + _cellSize.y / 2f);
            Vector2 spawnPos = (_avaliableRegions[randomIndex] * _cellSize + bottomLeftCorner);
            Region regionInstance = Instantiate(_regionTemplate);
            regionInstance.SetRegionData(spawnPos, temperature, predator);
            regionInstance.OnRegionSelected += UpdateEncounter;
            _allRegions.Add(regionInstance);
        }
    }

    public void UpdateEncounter(Region r) {
        _encountersHandler.PopulateInfo(null, r);
    }

    public void UpdateEncounter(beeColony c) {
        _encountersHandler.PopulateInfo(c, null);
    }
}
