using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Scriptables/Doggo data", fileName = "New Doggo")]
public class DoggoData : SerializedScriptableObject
{
    public string ID;
    public int MergeScore;
    public Sprite DoggoIcon;
    public AudioClip[] BarkSounds;
    public DoggoData MergesInto;

    public bool IsBiggestDoggo => MergesInto == null;
}
