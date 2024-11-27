using Assets.Scripts.Enemy;
using Assets.Scripts.Enemy.Animation;
using Assets.Scripts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class EnemyAnimationControllerV2: MonoBehaviour, IEnemyAnimationController
{
    public float WaitTimeSearching=1f;
    // Start is called before the first frame update
    private Animator animator;
	private ExclamationSignsController exclamationSignsController;
    private EnemyController parent;
    private IEnumerator confusedTimer;
    private PlayerController playerController;
	private enum AnimationStates
	{
		Still,
		ChillWalking,
		AlertWalking,
		Alert,
		ThrowingFireballs,
		Killing
	}

	private readonly Dictionary<AnimationStates, string> mapper = new() {
		{AnimationStates.Still, "StandingStill"},
		{AnimationStates.ChillWalking, "Idle"},
		{AnimationStates.AlertWalking, "AlertWalk"},
		{AnimationStates.Alert, "Alert"},
		{AnimationStates.ThrowingFireballs, "Throwing_fireballs"},
		{AnimationStates.Killing, "Killing_Player"}
	};


    private void Start()
    {
        StartCoroutine(InitializeAnimator());
        StartCoroutine(InitializeExlamationSignController());
        StartCoroutine(InitializeParent());

    }

    private void SetAnimationState(AnimationStates state)
	{
		foreach (var (s, n) in mapper)
		{
			if (s == state) animator?.SetTrigger(n);
			else animator?.ResetTrigger(n);
		}
	}

    public void Walking()
    {
		exclamationSignsController?.NoSign();
        SetAnimationState(AnimationStates.ChillWalking);
    }

    public void AlertWalking()
    {
		SetAnimationState(AnimationStates.AlertWalking);
    }

    public void StayStill()
    {
        exclamationSignsController?.NoSign();
        SetAnimationState(AnimationStates.Still);
    }

    public void Killing()
    {
        EndConfusedTimer();
        exclamationSignsController?.NoSign();
        SetAnimationState(AnimationStates.Killing);
        var deathGobbo = playerController.KickDeathAnimation();
        var enemyDirection = Math.Sign(parent.transform.localScale.x);
        deathGobbo.transform.localScale = new Vector2(enemyDirection*Math.Abs(deathGobbo.transform.localScale.x), deathGobbo.transform.localScale.y);
        var controller = deathGobbo.GetComponent<PlayerDeathAnimationController>();
        controller.KillPlayer();
    }

    public void Confused()
    {
        EndConfusedTimer();
		exclamationSignsController?.ExclamationSign();
        SetAnimationState(AnimationStates.Alert);
    }

    public void SearchAtSound()
    {
		exclamationSignsController?.QuestionSign();
        SetAnimationState(AnimationStates.Still);
        StartConfusedTimer();

    }

    public void ThrowFireball()
    {
        exclamationSignsController?.NoSign();
        SetAnimationState(AnimationStates.ThrowingFireballs);
    }

    private void StartConfusedTimer()
    {
        confusedTimer = ConfusedTimer();
        StartCoroutine(confusedTimer);
    }

    private void EndConfusedTimer()
    {
        if (confusedTimer!=null)
        {
            StopCoroutine(confusedTimer);
            confusedTimer = null;
        }
            
    }


    private IEnumerator ConfusedTimer()
    {
        yield return new WaitForSeconds(WaitTimeSearching);
        parent.AnimationEventFired("searchAnimationEnded");
        yield break;
    }

    private IEnumerator InitializeAnimator()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        else
        {
            yield break;
        }
        yield return null;
    }

    private IEnumerator InitializeExlamationSignController()
    {
        if (exclamationSignsController == null)
        {
            exclamationSignsController = GetComponentInChildren<ExclamationSignsController>();
        }
        else
        {
            yield break;
        }
        yield return null;
    }

    private IEnumerator InitializeParent()
    {
        if (parent == null)
        {
            parent = GetComponent<EnemyController>();
            playerController = parent.player.GetComponent<PlayerController>();
        }
        else
        {
            yield break;
        }
        yield return null;
    }
}
