using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //game camera
    [SerializeField] Camera _gameCamera = null;
    //player
    [SerializeField] Player _player = null;
    //the pool
    [SerializeField] RecyclingObjectPool _recyclingObjectPool = null;
    /// <summary>
    /// game const
    /// </summary>

    // max number of pattern on scene
    private float MAX_PATTERNS_NUMBER = 5;
    // max number of pattern in resources
    private int MAX_PATTERNS_PREFAB = 5;
    // Pattern name prefix
    private string PATTER_NAME_PREFIX = "Pattern_";
    // the minimum value to Player from the first Pattern at the begining
    private float START_PATTERN_X = 5.0f;
    // minimum distance between two patterns
    private float MINIMUM_PATTERN_DISTANCE = 2.0f;
    // starting scrolling speed
    private float PLAYER_START_SPEED = 2.0f;

    /// <summary>
    /// game stats
    /// </summary>
    // current position X to put next pattern (to prevent overlap)
    private float _currentPatternX = 0.0f;
    // is the game started yet? (Default by false, until the first touch
    private bool _isGameStart = false;
    // list current patterns used
    private List<Pattern> _currentPatterns = new List<Pattern>();

    // Start is called before the first frame update
    void Start()
    {
        InitStats();
        InitMaps();
    }

    // Update is called once per frame
    void Update()
    {
        //game have not started yet, nothing to do
        //if (!_isGameStart)
        //{
        //    return;
        //}
        _player.transform.position += new Vector3(PLAYER_START_SPEED * Time.deltaTime, 0, 0);
        _gameCamera.transform.position = new Vector3(_player.transform.position.x + 1.5f, _gameCamera.transform.position.y, _gameCamera.transform.position.z);

        foreach (Pattern pattern in _currentPatterns)
        {
            //remove patterns which out sight of screen
            if (pattern.transform.position.x + pattern.Width / 2.0f < _player.transform.position.x - START_PATTERN_X)
            {
                _currentPatterns.Remove(pattern);
                _recyclingObjectPool.Release(pattern.gameObject);
                InsertRandomPattern();
            }
        }
    }

    //init stats
    private void InitStats ()
    {
        _currentPatternX = START_PATTERN_X;
    }

    //init Maps 
    private void InitMaps ()
    {
        for (int i = 0; i < MAX_PATTERNS_NUMBER; i++)
        {
            InsertRandomPattern();
        }
    }

    //Insert random Pattern
    private void InsertRandomPattern()
    {
        int randIndex = Random.Range(1, MAX_PATTERNS_PREFAB + 1);
        Pattern pattern = CreatePattern(randIndex);
        if (pattern != null)
        {
            float patternWidth = pattern.Width;
            pattern.transform.position = new Vector3(_currentPatternX + patternWidth / 2.0f, 0, 0);
            _currentPatternX += patternWidth + MINIMUM_PATTERN_DISTANCE;
            _currentPatterns.Add(pattern);
        }
    }

    //create Pattern from pool by name
    private Pattern CreatePattern(int index)
    {
        string randPatternName = PATTER_NAME_PREFIX + index;
        GameObject patternObj = _recyclingObjectPool.Get(randPatternName);
        Pattern pattern = patternObj.GetComponent<Pattern>();

        return pattern;
    }
}
