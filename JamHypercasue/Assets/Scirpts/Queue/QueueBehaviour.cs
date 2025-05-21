using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class QueueBehaviour : MonoBehaviour
{
    public static QueueBehaviour Instance;
    
    [SerializeField] private Transform _queueOrigin; 
    [SerializeField] private Dummy _dummyPrefab;
    [SerializeField] private int _nbInstance;
    [SerializeField] private float _ejectForce;
    [SerializeField] private float _spawnOffset = 1;
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _ejectDistance = 4;

    private float _queueArrivalZ;
    private List<Dummy> _dummiesList = new();

    private Coroutine _spawnRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        for (int i = 0; i < _nbInstance; i++)
        {
            Dummy dummy = Instantiate(_dummyPrefab, _queueOrigin.position + Vector3.back * _spawnOffset * i, Quaternion.identity, transform);
            _dummiesList.Add(dummy);
        }

        _queueArrivalZ = _queueOrigin.position.z * _nbInstance * _spawnOffset;
    }

    private void Start()
    {
        foreach (Dummy el in _dummiesList)
        {
            el.ResetDummy();
        }

        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }
    

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);
            Dummy dummy = null;
            foreach (Dummy el in _dummiesList)
            {
                if (el.transform.localPosition.z > 0)
                    dummy = el;
                if (dummy && el.transform.position.z > dummy.transform.position.z)
                    dummy = el;
            }

            if (dummy != null)
            {
                dummy.transform.position = _queueOrigin.position + Vector3.back * _spawnOffset * _nbInstance;
            }
            else
            {
                dummy = Instantiate(_dummyPrefab, _queueOrigin.position + Vector3.back * _spawnOffset * _nbInstance, Quaternion.identity, transform);
                _dummiesList.Add(dummy);
                dummy.ResetDummy();
            }
        }
    }

    [Button]
    public void EjectADummy()
    {
        Dummy dummy = _dummiesList.FirstOrDefault(x => (transform.position.z - x.transform.position.z) < _ejectDistance);
        
        if(!dummy)
            return;
        
        _dummiesList.Remove(dummy);
        dummy.Ragdoll(_ejectForce);
        Destroy(dummy.gameObject, 2);
    }
}
