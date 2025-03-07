using UnityEngine;

public class SC_TeamManager : MonoBehaviour
{
    // Singleton instance
    private static SC_TeamManager _instance;
    public static SC_TeamManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Optionally create a new GameObject to hold the manager if none is found
                GameObject obj = new GameObject("TeamManager");
                _instance = obj.AddComponent<SC_TeamManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    private int _team1Count;
    private int _team2Count;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int GetTeam()
    {
        if (_team1Count > _team2Count)
        {
            _team2Count++;
            return 2;
        }
        else
        {
            _team1Count++;
            return 1;
        }
    }
    public void RemovePlayer(int teamNumber)
    {
        if (teamNumber == 1 && _team1Count > 0)
        {
            _team1Count--;
        }
        else if (teamNumber == 2 && _team2Count > 0)
        {
            _team2Count--;
        }
    }
    public int Team1Count => _team1Count;
    public int Team2Count => _team2Count;
}
