using System;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    public bool IsTestMode = true;

    BackTrackingVisualizer backTrackingVisualizer;
    Pools pools;
    public static BackTrackingVisualizer BackTrackingVisualizer => Inst.backTrackingVisualizer;
    public static Pools Pools => Inst.pools;

    private void Awake()
    {
        backTrackingVisualizer = FindAnyObjectByType<BackTrackingVisualizer>();
        pools = FindAnyObjectByType<Pools>();
    }
}
