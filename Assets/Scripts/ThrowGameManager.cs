using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText,throwsText;
    [SerializeField] private ThrowTarget throwTarget;
    [SerializeField] private List<ThrowProjectile> projectiles;
    [SerializeField] private float ResetTime = 2f;

    private int throwsLeft = 0;
    private int currentScore = 0;

    private void Awake()
    {
        throwsLeft = projectiles.Count;
        
        throwTarget.OnPointsScore += OnPointScore;
    }
    private void Update()
    {
        CheckForProjectiles();
    }

    private void CheckForProjectiles()
    {
        if (projectiles.Count <= 0)
            return;

        bool wantsReset = true;
        throwsLeft = projectiles.Count;
        
        foreach (var projectile in projectiles)
        {
            if (projectile.CurrentState is ThrowProjectile.ProjectileState.START or ThrowProjectile.ProjectileState.THROWING)
            {
                wantsReset = false;
            }
            else if (projectile.CurrentState is ThrowProjectile.ProjectileState.STUCK or ThrowProjectile.ProjectileState.TUMBLING)
            {
                throwsLeft--;
            }
        }
        throwsText.text = throwsLeft.ToString();


        if (wantsReset)
        {
            StartCoroutine(ResetAfterSeconds());
        }
    }

    private IEnumerator ResetAfterSeconds()
    {
        yield return new WaitForSeconds(ResetTime);
        ResetRound();
    }

    private void ResetRound()
    {
        throwsLeft = projectiles.Count;
        scoreText.text = "0";
        currentScore = 0;
        throwsText.text = throwsLeft.ToString();

        foreach (var projectile in projectiles)
        {
            projectile.ResetToStart();
        }
    }
    
    private void OnPointScore(int pScore)
    {
        currentScore += pScore;
        scoreText.text = currentScore.ToString();
    }
}
