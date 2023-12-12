using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CreditsWindow : MonoBehaviour {
    public class CreditsEntry {
        public string description;
        public string url;

        public CreditsEntry(string desc, string url) { 
            this.description = desc;
            this.url = url;
        }
    }

    [SerializeField]
    private ScrollRect ScrollRect;
    [SerializeField]
    private Scrollbar ScrollBar;
    [SerializeField]
    [Min(0)]
    private float AutoScrollSpeed = 1f;
    [SerializeField]
    private bool DoAutoScroll = true;
    [SerializeField]
    private GameObject EntryPrefab;

    private List<CreditsEntry> Entries = new();

    private void Awake() {
        ParseCredits();
    }

    private void Start() {
        var parent = ScrollRect.content;
        foreach (var entry in Entries) {
            var obj = Instantiate(EntryPrefab, parent);
            var textComponents = obj.GetComponentsInChildren<TMPro.TMP_Text>();
            textComponents[0].text = entry.description;
            textComponents[1].text = entry.url;
        }
    }

    private void OnEnable() {
        ScrollBar.value = 0;
    }

    private void ParseCredits() {
        var lines = LoadCreditsLineByLine();
        for (int i = 0; i < lines.Length; i += 2) {
            Entries.Add(new CreditsEntry(lines[i], lines[i+1]));
        }
    }

    private string[] LoadCreditsLineByLine() {
        var path = Path.Combine(Application.streamingAssetsPath, "credits.txt");
#if UNITY_ANDROID
        var request = UnityEngine.Networking.UnityWebRequest.Get(path);
        request.SendWebRequest();
        while (!request.isDone) {}
        var lines = request.downloadHandler.text.Split("\n");
#else
        var lines = File.ReadAllLines(path);
#endif
        return lines.WhereNot(string.IsNullOrWhiteSpace).ToArray();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void Update() {
        if (DoAutoScroll) {
            ScrollBar.value += AutoScrollSpeed * Time.deltaTime;
        }
    }
}
