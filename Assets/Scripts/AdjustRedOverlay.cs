using UnityEngine;
using UnityEngine.UI;

public static class EnemyUtilities
{
    public static void AdjustRedOverlay(Image redOverlay, float distanceToPlayer, float followRadius)
    {
        float intensity = 0.8f - Mathf.Clamp01(distanceToPlayer / followRadius);
        redOverlay.color = new Color(redOverlay.color.r, redOverlay.color.g, redOverlay.color.b, intensity);
    }
}
