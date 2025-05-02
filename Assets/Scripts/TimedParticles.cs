using System.Collections;
using UnityEngine;

/// <summary>
/// Nate
/// A basic script to clean up particles once they are done.
/// Manually sets a delay to destroy the gameObject after.
/// </summary>
public class TimedParticles : MonoBehaviour {
    public void StartTimer(float seconds) {
        StartCoroutine(DestroyAfterSeconds(seconds));
    }

    private IEnumerator DestroyAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
