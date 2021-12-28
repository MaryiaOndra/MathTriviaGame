using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    [SerializeField] TMP_Text _question;
    [SerializeField] List<Button> _buttons;

    Test test;

    int countSets;


    private void Start() {
        countSets = 0;
        Test test = GetComponent<Test>();

    }

    void GenerateSet(TrainingSet trainingSet) {
    
    
    }
}
