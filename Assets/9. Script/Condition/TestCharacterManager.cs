using UnityEngine;

public class TestCharacterManager : MonoBehaviour
{
    private static TestCharacterManager _instance;
    public static TestCharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacerManager").AddComponent<TestCharacterManager>();
            }
            return _instance;
        }
    }

    public TestPlayer Player
    {
        get { return _player; }
        set { _player = value; }
    }
    private TestPlayer _player;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}