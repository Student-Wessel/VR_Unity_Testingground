using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class WhackMoleManager : MonoBehaviour
{
    [SerializeField] private float gameTime;
    [SerializeField] private float difficultyIncrease = 0.95f,difficultyDecrease = 1.05f;
    [SerializeField] private WhackMoleHammer hammer;
    [SerializeField] private List<GameObject> holes;
    [SerializeField] private List<Mole> moles;
    [SerializeField] private float minSpawnTime = 1f,maxSpawnTime = 2f,minStayTime = 0.1f,maxStayTime = 4f,digSpeed = 1f, height = 2f;
    [SerializeField] private TextMeshProUGUI timeText, scoreText;
    
    public UnityEvent MoleHit;

    private List<GameObject> availableHoles;
    private List<Mole> availableMoles;

    private bool isPlaying = false;
    private int score = 0;
    private  float timePlaying;

    private void Awake()
    {
        availableHoles = new List<GameObject>();
        availableMoles = new List<Mole>();
        
        hammer.HammerMoleHit += OnMoleHit;

        foreach (var hole in holes)
        {
            availableHoles.Add(hole);
        }

        foreach (var mole in moles)
        {
            availableMoles.Add(mole);
        }

        foreach (var mole in moles)
        {
            mole.leftHole += OnMoleLeftHole;
        }
    }

    private void Update()
    {
        if (isPlaying)
        {
            timePlaying += Time.deltaTime;
            timeText.text = Mathf.CeilToInt(gameTime - timePlaying).ToString();
            if (timePlaying > gameTime)
            {
                isPlaying = false;
                StopCoroutine(MoleLoop());
            }
        }
    }

    public void StartMoleLoop()
    {
        if (!isPlaying)
        {
            timePlaying = 0;
            score = 0;
            timeText.text = Mathf.CeilToInt(gameTime - timePlaying).ToString();
            scoreText.text = score.ToString();
            StartCoroutine(MoleLoop());
        }
    }

    private void OnMoleLeftHole(Mole mole,GameObject hole)
    {
        // Missed a mole
        if (!mole.moleHasBeenHit)
        {
            OnMoleMiss(mole);
        }
        
        availableHoles.Add(hole);
        availableMoles.Add(mole);
    }
    private void OnMoleHit(Mole pMole)
    {
        minSpawnTime *= difficultyIncrease;
        maxSpawnTime *= difficultyIncrease;

        minStayTime *= difficultyIncrease;
        maxStayTime *= difficultyIncrease;
        
        score++;
        scoreText.text = score.ToString();
        MoleHit.Invoke();
    }
    
    private void OnMoleMiss(Mole mole)
    {
        minSpawnTime *= difficultyDecrease;
        maxSpawnTime *= difficultyDecrease;

        minStayTime *= difficultyDecrease;
        maxStayTime *= difficultyDecrease;
    }

    private IEnumerator MoleLoop()
    {
        yield return new WaitForSeconds(2f);
        isPlaying = true;
        while (isPlaying)
        {
            int moleCount = availableMoles.Count;
            int holeCount = availableHoles.Count;
            

            if (moleCount < 1 || holeCount < 1)
            {
                yield return new WaitForSeconds(Random.Range(minSpawnTime,maxSpawnTime));
                continue;
            }

            int moleIndex = Random.Range(0, moleCount);
            int holeIndex = Random.Range(0, holeCount);
            
            availableMoles[moleIndex].Activate(availableHoles[holeIndex],Random.Range(minStayTime,maxStayTime),digSpeed,height);
            
            availableMoles.RemoveAt(moleIndex);
            availableHoles.RemoveAt(holeIndex);
            
            yield return new WaitForSeconds(Random.Range(minSpawnTime,maxSpawnTime));
        }
    }
}
