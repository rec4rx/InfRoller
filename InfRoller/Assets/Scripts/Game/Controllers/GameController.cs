using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The <c>Game controller</c> class.
/// All Ingame activities
/// </summary>
public class GameController : MonoBehaviour
{
    // game camera
    [SerializeField] Camera _gameCamera = null;
    // player
    [SerializeField] Player _player = null;
    // the pool
    [SerializeField] RecyclingObjectPool _recyclingObjectPool = null;
    // ground
    [SerializeField] Ground _ground = null;

    //ui
    //pauseUI
    [SerializeField] GameObject _pauseUIObj = null;
    //endUI
    [SerializeField] GameObject _endUIObj = null;
    //score tect
    [SerializeField] Text _scoreText = null;

    // game const(s)

    // max number of pattern on scene
    private readonly float MAX_PATTERNS_NUMBER = 5;
    // max number of pattern in resources
    private readonly int MAX_PATTERNS_PREFAB = 5;
    // Pattern name prefix
    private readonly string PATTER_NAME_PREFIX = "Pattern_";
    // the minimum value to Player from the first Pattern at the begining
    private readonly float START_PATTERN_X = 5.0f;
    // minimum distance between two patterns
    private readonly float MINIMUM_PATTERN_DISTANCE = 2.0f;
    // starting scrolling speed
    private readonly float PLAYER_START_SPEED = 2.5f;
    // player jump power
    private readonly float PLAYER_JUMP_POWER = 9f;
    // expand Ground Width
    private readonly float GROUND_EXPAND_DISTANCE = 24.0f;
    // score step
    private readonly int SCORE_STEP = 10;

    // game stats

    // current position X to put next pattern (to prevent overlap)
    private float _currentPatternX = 0.0f;
    // list current patterns used
    private List<Pattern> _currentPatterns = new List<Pattern>();
    //next goal to expand ground
    private float _nextGroundExpandX = 0.0f;
    //check if the game is end or not
    private bool _isEnd = false;
    //score
    private int _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitStats();
        InitMaps();
    }

    // Update is called once per frame
    void Update()
    {
        //game has ended, nothing to do
        if (_isEnd)
        {
            return;
        }

        HandleTouch();

        if (_player == null || _gameCamera == null)
        {
            return;
        }

        if (!_player.IsJumping)
        {
            _player.Move(PLAYER_START_SPEED);
        }
       
        _gameCamera.transform.position = new Vector3(_player.transform.position.x + 1.5f, _gameCamera.transform.position.y, _gameCamera.transform.position.z);

        //temporary list to keep patterns out of screen
        List<Pattern> willRemovedPatterns = new List<Pattern>();
        foreach (Pattern pattern in _currentPatterns)
        {
            //Add patterns out of screen to temp list
            if (pattern.transform.position.x + pattern.Width / 2.0f < _player.transform.position.x - START_PATTERN_X)
            {
                willRemovedPatterns.Add(pattern);
            }
        }

        //remove patterns out of screen
        foreach (Pattern pattern in willRemovedPatterns)
        {
            //remove from pattern list
            _currentPatterns.Remove(pattern);

            //pool release
            _recyclingObjectPool.Release(pattern.gameObject);

            //imediately insert new pattern
            InsertRandomPattern();
        }

        //clear the tmp list
        willRemovedPatterns.Clear();

        //grounds
        if (_player.transform.position.x >= _nextGroundExpandX)
        {
            _nextGroundExpandX += GROUND_EXPAND_DISTANCE;

            if (_ground != null)
            {
                _ground.VirtualExpand(GROUND_EXPAND_DISTANCE);
            }
        }
    }

    private void OnDisable()
    {
        //remove events
        Player.endgameDelegate -= End;
        Player.scoreDelegate -= AddScore;
    }

    //init stats
    private void InitStats ()
    {
        //default max fps is 60
        Application.targetFrameRate = 60;

        _currentPatternX = START_PATTERN_X;
        _nextGroundExpandX = GROUND_EXPAND_DISTANCE / 1.5f;

        //delegate
        Player.endgameDelegate += End;
        Player.scoreDelegate += AddScore;
    }

    //init Maps 
    private void InitMaps ()
    {
        for (int i = 0; i < MAX_PATTERNS_NUMBER; i++)
        {
            InsertRandomPattern();
        }
    }

    //Touch Handler
    private void HandleTouch ()
    {
        if (_player == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse was clicked over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
                return;
            }

            if (_player.IsJumping)
            {
                //on jump, can't jump
                return;
            }

            _player.Jump(PLAYER_JUMP_POWER);
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
            pattern.transform.position = new Vector3(_currentPatternX, 0, 0);
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

    //addscore
    public void AddScore ()
    {
        _score += SCORE_STEP;
        _scoreText.text = _score.ToString();
    }

    //end
    public void End()
    {   
        if (_isEnd)
        {
            return;
        }

        _isEnd = true;
        Time.timeScale = 0;
        _endUIObj.SetActive(true);

        StartCoroutine(SubmitCurrentScore());
    }

    //submit score 
    private IEnumerator SubmitCurrentScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", "Quai");
        form.AddField("score", _score.ToString());
        UnityWebRequest www = UnityWebRequest.Post(NetDef.API_SUBMIT_SCORE, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string msg = NetDef.Response[www.responseCode];
            Debug.Log(msg);
        }
    }

    /// <summary>
    /// Replay clicked on End.
    /// </summary>
    public void OnClickedReplay ()
    {
        Time.timeScale = 1;
        _endUIObj.SetActive(false);
        SceneManager.LoadScene(SceneName.GAME);
    }

    //pause 
    public void OnClickedPause ()
    {
        if (_pauseUIObj.activeSelf)
        {
            return;
        }

        Time.timeScale = 0;
        _pauseUIObj.SetActive(true);
    }

    //go home from pause
    public void OnClickedHome ()
    {
        Time.timeScale = 1;
        _pauseUIObj.SetActive(false);
        _endUIObj.SetActive(false);
        SceneManager.LoadScene(SceneName.HOME);
    }

    //resume from pause
    public void OnClickedResume ()
    {
        _pauseUIObj.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// On the application pause.
    /// </summary>
    /// <param name="pause">If set to <c>true</c> pause.</param>
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            _pauseUIObj.SetActive(true);
        }
        else
        {
            _pauseUIObj.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
