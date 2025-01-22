using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class RoadManager : MonoBehaviour
{
    private Queue<Road> _roads = new();
    
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private int _roadPoolSize;
    [SerializeField] private int _activeRoadCount;

    [SerializeField] private List<GameObject> _spawnPoints = new();
    
    void Awake()
    {
        for (int i = 0; i < _roadPoolSize; i++)
        {
            var road = Instantiate(_roadPrefab).GetComponent<Road>();
            road.onExit += OnExitRoad;
            road.gameObject.SetActive(false);
            _roads.Enqueue(road);
        }
    }

    private void OnExitRoad(Road road)
    {
        road.gameObject.SetActive(false);
        _roads.Enqueue(road);
        
        var nextRoad = _roads.Dequeue().gameObject;
        nextRoad.transform.position = _spawnPoints.Last().transform.position;
        nextRoad.SetActive(true);
    }

    void Start()
    {
        //road setting
        for (int i = 0; i < _activeRoadCount; i++)
        {
            var road = _roads.Dequeue().gameObject;
            road.transform.position = _spawnPoints[i].transform.position;
            road.SetActive(true);
        }
    }
}
