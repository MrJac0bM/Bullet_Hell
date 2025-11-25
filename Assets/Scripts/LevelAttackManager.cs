using UnityEngine;
using System.Collections;

[System.Serializable]
public class AttackPhase
{
    public string phaseName = "Fase 1";
    public float duration = 5f;
    public ShotPattern[] patterns;
    public bool simultaneous = false; 
}

public class LevelAttackManager : MonoBehaviour
{
    [SerializeField] private AttackPhase[] phases;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeedPerShot = 15f; 
    
    private float _elapsedTime = 0f;
    private int _currentPhaseIndex = 0;
    private float _currentPhaseDuration = 0f;
    private Coroutine[] _activeCoroutines;
    private float _globalRotationOffset = 0f;

    private void Start()
    {
        if (shootOrigin == null)
            shootOrigin = transform;
        
        if (phases.Length > 0)
        {
            StartPhase(0);
        }
    }

    private void Update()
    {
        _currentPhaseDuration += Time.deltaTime;
        _elapsedTime += Time.deltaTime;

       
        if (_currentPhaseDuration >= phases[_currentPhaseIndex].duration)
        {
            if (_currentPhaseIndex < phases.Length - 1)
            {
                _currentPhaseIndex++;
                StartPhase(_currentPhaseIndex);
            }
        }
    }

    private void StartPhase(int phaseIndex)
    {
        if (phaseIndex >= phases.Length) return;

        AttackPhase phase = phases[phaseIndex];
        _currentPhaseDuration = 0f;
        
        Debug.Log($"=== {phase.phaseName} INICIADA (Duración: {phase.duration}s) ===");

        StopAllCoroutines();

        if (phase.simultaneous)
        {
            _activeCoroutines = new Coroutine[phase.patterns.Length];
            for (int i = 0; i < phase.patterns.Length; i++)
            {
                _activeCoroutines[i] = StartCoroutine(ContinuousAttack(phase.patterns[i]));
            }
        }
        else
        {
            StartCoroutine(SequentialAttack(phase.patterns));
        }
    }

    private IEnumerator ContinuousAttack(ShotPattern pattern)
    {
        while (true)
        {
            Vector2 center = shootOrigin.position;
            Vector2 aimDirection = GetAimDirection();

            for (int i = 0; i < pattern.PatternSettings.Length; i++)
            {
                ShotSetting setting = pattern.PatternSettings[i];
                ShotSystem.ExecutePattern(center, aimDirection, setting, _globalRotationOffset);
                _globalRotationOffset += rotationSpeedPerShot;
                if (_globalRotationOffset >= 360f) _globalRotationOffset -= 360f;
                yield return new WaitForSeconds(setting.CooldownAfterShoot);
            }
        }
    }

    private IEnumerator SequentialAttack(ShotPattern[] patterns)
    {
        int patternIndex = 0;
        while (true)
        {
            ShotPattern pattern = patterns[patternIndex];
            Vector2 center = shootOrigin.position;
            Vector2 aimDirection = GetAimDirection();

            for (int i = 0; i < pattern.PatternSettings.Length; i++)
            {
                ShotSetting setting = pattern.PatternSettings[i];
                ShotSystem.ExecutePattern(center, aimDirection, setting, _globalRotationOffset);
                _globalRotationOffset += rotationSpeedPerShot;
                if (_globalRotationOffset >= 360f) _globalRotationOffset -= 360f;
                yield return new WaitForSeconds(setting.CooldownAfterShoot);
            }

            patternIndex = (patternIndex + 1) % patterns.Length;
        }
    }

    private Vector2 GetAimDirection()
    {
        if (target != null)
        {
            return (target.position - shootOrigin.position).normalized;
        }
        return Vector2.up;
    }

    public float GetElapsedTime() => _elapsedTime;
    public int GetCurrentPhase() => _currentPhaseIndex;
}