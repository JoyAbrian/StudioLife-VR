using UnityEngine;
using System.Collections;

public class GameAmbience : MonoBehaviour
{
    public float delay = 3f;

    private void Start()
    {
        StartCoroutine(PlayAmbienceWithDelay());
    }

    private IEnumerator PlayAmbienceWithDelay()
    {
        yield return new WaitForSeconds(delay);
        SoundManager.PlaySound(SoundType.Ambience, 0.5f);
    }
}