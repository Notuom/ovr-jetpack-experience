using System;
using System.Collections.Generic;
using System.IO;
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

    private int _currentRingNumber;
    private DateTime _ringStartTime;
    private StreamWriter _csvWriter;


    private void Start()
    {
        _collectAudioSource = CollectSoundGameObject.GetComponent<AudioSource>();
        _finishAudioSource = FinishSoundGameObject.GetComponent<AudioSource>();

        // Find all ring children and order them by name so they spawn in the right order
        _ringSpawns = GameObject.FindGameObjectsWithTag("Ring Spawn").OrderBy(o => o.name).ToList();

        // Print all distances to a CSV
        var distanceCsvWriter = File.CreateText("distances.csv");
        distanceCsvWriter.WriteLine("RingId,DistanceUnits");
        var prevPosition = new Vector3(0, 0, 0);
        var ringId = 1;
        foreach (var ringSpawn in _ringSpawns)
        {
            distanceCsvWriter.WriteLine(ringId + "," + Vector3.Distance(prevPosition, ringSpawn.transform.position));
            prevPosition = ringSpawn.transform.position;
            ringId++;
        }
        distanceCsvWriter.Close();

        // Spawn first ring
        SpawnNextRing();

        // Initiate CSV file writing
        _ringStartTime = DateTime.Now;
        _csvWriter = File.CreateText("data_" + _ringStartTime.ToString("yy-MM-dd-HH-mm-ss") + ".csv");
        _csvWriter.AutoFlush = true;
        _csvWriter.WriteLine("RingId,SecondsToCollect");
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
            _currentRingNumber++;
        }
        else
        {
            _finishAudioSource.Play();
            Destroy(_currentRing);
            _csvWriter.Close();
        }
    }

    public void CollectRing()
    {
        _collectAudioSource.Play();

        // Collect statistics to file, restart timer for next ring
        _csvWriter.WriteLine(_currentRingNumber + "," + (DateTime.Now - _ringStartTime).TotalSeconds);
        _ringStartTime = DateTime.Now;

        SpawnNextRing();
    }
}