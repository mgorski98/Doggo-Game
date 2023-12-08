using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FlyingMenuDoggoHead : MonoBehaviour {
    public enum FlyingPatternType {
        ParametricCircle, SineWave, Linear
    }

    public float Speed;
    public float Amplitude;
    public FlyingPatternType PatternType;

    private void Update() {
        
    }
}
