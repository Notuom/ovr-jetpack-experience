using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RingManagerController : MonoBehaviour
{
    public GameObject RingPrefab;
    public GameObject CollectSoundGameObject;
    public GameObject FinishSoundGameObject;

    private AudioSource _collectAudioSource;
    private AudioSource _finishAudioSource;
    private List<GameObject> _ringSpawns;

    private GameObject _currentRing;

    private void Start()
    {
        _collectAudioSource = CollectSoundGameObject.GetComponent<AudioSource>();
        _finishAudioSource = FinishSoundGameObject.GetComponent<AudioSource>();

        // Find all rings 
        _ringSpawns = GameObject.FindGameObjectsWithTag("Ring Spawn").OrderBy(o => o.name).ToList();

        // Spawn first ring
        SpawnNextRing();
    }

    private void SpawnNextRing()
    {
        if (_ringSpawns.Count > 0)
        {
            var ringSpawn = _ringSpawns[0];
            if (_currentRing == null)
            {
                _currentRing = Instantiate(RingPrefab, ringSpawn.transform.position, ringSpawn.transform.rotation);
            }
            else
            {
                _currentRing.transform.position = ringSpawn.transform.position;
                _currentRing.transform.rotation = ringSpawn.transform.rotation;
            }
            _ringSpawns.RemoveAt(0);
            Destroy(ringSpawn);
        }
        else
        {
            _finishAudioSource.Play();
            Destroy(_currentRing);
        }
    }

    public void CollectRing()
    {
        _collectAudioSource.Play();

        Debug.Log("Ring collected. Rings remaining : " + _ringSpawns.Count);
        SpawnNextRing();
    }
}