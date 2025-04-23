using System;
using UnityEngine;

public class AnimatorEventTrigger : MonoBehaviour {
    [SerializeField]
    private Animator[] animators;

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

    public void PlayTriggerIndex(int index) {
        animators[index].SetTrigger(triggerSave);
    }

    public void CallEvent() {
        AnimEvent?.Invoke();
    }
}
