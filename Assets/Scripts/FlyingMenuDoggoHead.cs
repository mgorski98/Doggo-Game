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
    public float Direction = 1;

    private static readonly FlyingPatternType[] PatternTypes = Enum.GetValues(typeof(FlyingPatternType)).Cast<FlyingPatternType>().ToArray();

    private void Awake() {
        PatternType = PatternTypes.RandomElement();
    }

    private void Update() {
        switch (this.PatternType) {
            case FlyingPatternType.ParametricCircle: {
                break;
            }
            case FlyingPatternType.SineWave: {
                break;
            }
            case FlyingPatternType.Linear: {
                break;
            }
        }
    }
}
