using UnityEngine;
using System.Collections;
using TMPro;
using NaughtyAttributes;

public class PassScreen : Screen
{
    [SerializeField] private TMP_Text text;
    private const float FADE_DURATION = 0.5f;
    [Button]
    public override void Enable()
    {
        base.Enable();
        text.color = Color.white;
        StartCoroutine(FadeText());
    }
    private IEnumerator FadeText()
    {
        yield return new WaitForSeconds(0.5f);
        float time = 0f;
        while (time < FADE_DURATION)
        {
            text.color = new Color(1f,1f,1f, -1 * (time/FADE_DURATION) + 1);
            time += Time.deltaTime;
            yield return null;
        }
        Disable();
    }
}
