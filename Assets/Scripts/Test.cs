using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour { 
    static string url = "https://api.npoint.io/eddef1e4d9af278a43a0";

    public TrainingSet[] TrainingSets { get; private set; }

    public event Action<TrainingSet[]> OnGetTrainingSets;

    void Start() {
        StartCoroutine(GetTrainingSets(OnGotSets));
    }

    void OnGotSets(TrainingSet[] sets) {
        TrainingSets = sets;
    }

    public IEnumerator GetTrainingSets(Action<TrainingSet[]> callback) {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
        OnGetTrainingSets?.Invoke(response.trainingSet);
        callback?.Invoke(response.trainingSet);
    }
}

[Serializable]
public class ResponseData {
    public TrainingSet[] trainingSet;
}

[Serializable]
public class TrainingSet {
    public string title;
    public List<Set> displaySet;
    public List<Set> matchSet;
    public List<Set> negativeSet;
}

[Serializable]
public class Set {
    public string id;
    public string text;
}

