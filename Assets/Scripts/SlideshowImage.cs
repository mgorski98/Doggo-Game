using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlideshowImage : MonoBehaviour {
    [SerializeField]
    private Image Img1;
    [SerializeField]
    private Image Img2;

    public float ImageChangeInterval = 30f;
    public float FadeOutDuration = 4f;
    public Sprite[] Slides;

    private int CurrentTexIndex = 0;
    private float SlideshowProgressTimer = 0f;

    private void Awake() {
        SlideshowProgressTimer = ImageChangeInterval;
        Img1.color = Img1.color.Copy(a: 1f);
        Img2.color = Img2.color.Copy(a: 0f);
        Img1.sprite = Slides.First();
    }

    private void ProgressSlideShow() {
        CurrentTexIndex++;
        CurrentTexIndex = Utils.ClampWrapping(CurrentTexIndex, 0, Slides.Length - 1);
        FadeInAndOut();
    }

    private void FadeInAndOut() {
        var nextSlide = Slides[CurrentTexIndex];
        var currentlyActiveImg = Img1.color.a > 0 ? Img1 : Img2;
        var otherImage = Img1.color.a > 0 ? Img2 : Img1;
        otherImage.sprite = nextSlide;
        currentlyActiveImg.DOFade(0f, FadeOutDuration);
        otherImage.DOFade(1f, FadeOutDuration);
    }

    private void Update() {
        if (SlideshowProgressTimer > 0) {
            SlideshowProgressTimer -= Time.deltaTime;
        } else {
            SlideshowProgressTimer = ImageChangeInterval;
            ProgressSlideShow();
        }
    }
}
