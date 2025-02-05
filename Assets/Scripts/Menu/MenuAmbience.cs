using UnityEngine;
using System.Collections;

public class MenuAmbience : MonoBehaviour
{
    public float delay = 3f;

    private void Start()
    {
        StartCoroutine(PlayAmbienceWithDelay1());
    }

    private IEnumerator PlayAmbienceWithDelay1()
    {
        yield return new WaitForSeconds(delay);
        SoundManager.PlaySound(SoundType.MenuAmbience, 0.5f);
    }
}