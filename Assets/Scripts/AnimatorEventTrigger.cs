using System;
using UnityEngine;

public class AnimatorEventTrigger : MonoBehaviour {
    [SerializeField]
    private Animator[] animators;

    [SerializeField]
    private GameObject particlePrefab;

    [SerializeField]
    private Transform particlePoint;

    [SerializeField]
    private ParticleSystem particles;

    public Action AnimEvent;

    private string triggerSave;

    public void PlayTrigger(string trigger) {
        foreach (var animator in animators) {
            animator.SetTrigger(trigger);
        }
    }

    public void SetTrigger(string trigger) {
        triggerSave = trigger;
    }

    public void PlayParticle() {
        particles.Play();
    }

    public void PlayTriggerIndex(int index) {
        animators[index].SetTrigger(triggerSave);
    }

    public void CallEvent() {
        AnimEvent?.Invoke();
    }

    public void SpawnParticles(float duration) {
        GameObject particleInstance = Instantiate(particlePrefab, particlePoint.position, particlePrefab.transform.rotation);
        particleInstance.AddComponent<TimedParticles>().StartTimer(duration);
    }
}
