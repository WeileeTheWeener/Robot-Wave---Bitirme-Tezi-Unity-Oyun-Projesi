using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailUtil : MonoBehaviour
{
    public LineRenderer lineRendererPrefab;
    public float lineRendererLength;
    public float trailDuration;
    public AnimationCurve alphaCurve;
    public AnimationCurve colorCurve;

    LineRenderer[] lineRenderers;
    public int lineRendererIndex = 0;

    public static BulletTrailUtil bulletTrailInstance = null;

    void Awake()
    {
        if(bulletTrailInstance)
        {
            Destroy(this);
            return;
        }

        bulletTrailInstance = this;
        lineRenderers = new LineRenderer[100];
        for(int i = 0; i < 100; ++i)
        {
            lineRenderers[i] = GameObject.Instantiate(lineRendererPrefab);
        }
    }

    IEnumerator trailAnimation = null;

    public static void PlayTrailAnimation(
        Vector3 startPosition, 
        Vector3 endPosition,
        Color startColor,
        Color endColor)
    {
        bulletTrailInstance.lineRenderers[bulletTrailInstance.lineRendererIndex].positionCount = 10;
        for(int i = 0; i < 10; ++i)
        {
            bulletTrailInstance.lineRenderers[bulletTrailInstance.lineRendererIndex].SetPosition(
                i, 
                Vector3.Lerp(startPosition, endPosition, (float)i/10f));
        }
        bulletTrailInstance.StartCoroutine(bulletTrailInstance.animateTrail(startColor, endColor));
    }
    
    IEnumerator animateTrail(Color startColor, Color endColor)
    {
        Gradient gradient = new Gradient();
        int index = lineRendererIndex;
        lineRendererIndex++;
        lineRendererIndex %= lineRenderers.Length;
        for(int i = 0; i < 100; ++i)
        {
            float time = (float)i/100f;
            float alphaCurveValue = alphaCurve.Evaluate(time);
            float colorCurveValue = colorCurve.Evaluate(time);
            gradient.SetKeys(
                new GradientColorKey[] 
                { 
                    new GradientColorKey(Color.Lerp(startColor, endColor, colorCurveValue), 0.0f), 
                    new GradientColorKey(Color.Lerp(startColor, endColor, colorCurveValue), 1.0f) 
                },
                new GradientAlphaKey[] 
                { 
                    new GradientAlphaKey(alphaCurveValue, 0.0f), 
                    new GradientAlphaKey(alphaCurveValue, 1.0f) 
                }
            );
            lineRenderers[index].colorGradient = gradient;
            yield return new WaitForSeconds(trailDuration / 100f);
        }

        gradient.SetKeys(
                new GradientColorKey[] 
                { 
                    new GradientColorKey(Color.white, 0.0f), 
                    new GradientColorKey(Color.white, 1.0f) 
                },
                new GradientAlphaKey[] 
                { 
                    new GradientAlphaKey(0f, 0.0f), 
                    new GradientAlphaKey(0f, 1.0f) 
                }
            );
        lineRenderers[index].colorGradient = gradient;
    }

}
