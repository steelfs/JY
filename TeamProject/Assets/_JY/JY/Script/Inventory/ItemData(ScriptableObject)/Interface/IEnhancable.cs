using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnhancable 
{
    bool LevelUp(uint darkForceCount);
    float CalculateSuccessRate(uint darkForceCount);
}
