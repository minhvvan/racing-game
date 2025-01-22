using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class RoadManager : MonoBehaviour
{
    private List<Road> _roads = new();
    
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private int _roadPoolSize;
    [SerializeField] private int _activeRoadCount;
    private int _currentIdx;

    [SerializeField] private List<GameObject> _spawnPoints = new();
    
    void Awake()
    {
        for (int i = 0; i < _roadPoolSize; i++)
        {
            var road = Instantiate(_roadPrefab).GetComponent<Road>();
            road.onExit += OnExitRoad;
            road.gameObject.SetActive(false);
            _roads.Add(road);
        }

        _currentIdx = _activeRoadCount;
    }

    private void OnExitRoad(Road road)
    {
        road.gameObject.SetActive(false);
        
        var nextRoad = _roads[_currentIdx].gameObject;
        _currentIdx = (_currentIdx + 1) % _roadPoolSize;
        
        nextRoad.transform.position = _spawnPoints.Last().transform.position;
        nextRoad.SetActive(true);
    }

    public void StartGame()
    {
        //road setting
        for (int i = 0; i < _activeRoadCount; i++)
        {
            var road = _roads[i].gameObject;
            road.transform.position = _spawnPoints[i].transform.position;
            road.SetActive(true);
        }
    }

    public void StopGame()
    {
        foreach (var road in _roads)
        {
            road.gameObject.SetActive(false);
        }
    }
}
