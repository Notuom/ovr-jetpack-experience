using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RingManagerController : MonoBehaviour
{
    public GameObject RingPrefab;
    public GameObject CollectSoundGameObject;

    private AudioSource _collectAudioSource;
    private List<GameObject> _ringSpawns;

    private GameObject _currentRing;

    private void Start()
    {
        _collectAudioSource = CollectSoundGameObject.GetComponent<AudioSource>();

        // Find all rings 
        _ringSpawns = GameObject.FindGameObjectsWithTag("Ring Spawn").OrderBy(o => o.name).ToList();

        // Reorder them
        Debug.Log("Rings: " + _ringSpawns.Count);


        // Spawn first ring
        NextRing();
    }

    private void NextRing()
    {
        if (_ringSpawns.Count > 0)
        {
            var ringSpawn = _ringSpawns[0];
            Debug.Log("Activating ring " + ringSpawn.name);
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
            Debug.Log("No more rings to activate!");
        }
    }

    public void CollectRing()
    {
        _collectAudioSource.Play();

        Debug.Log("Ring collected. Rings remaining : " + _ringSpawns.Count);
        NextRing();
    }
}