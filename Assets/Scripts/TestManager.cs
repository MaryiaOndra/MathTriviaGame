using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _question;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _levelsText;
    [SerializeField] private List<AnswerButton> _buttons;
    [SerializeField] private int _pointsForAnswer = 50;
    [SerializeField] private float _timeAfterAnswer = 1f;

    TrainingSet[] _sets;
    List<int> _buttonsNumbers;

    Test test;
    int _countSets = 0;
    int _setsAmount;
    int _score = 0;

    private void Start() {
        Test test = GetComponent<Test>();
        test.OnGetTrainingSets += StartGeneratingTest;
        _buttons.ForEach(b => b.OnAnswer += Answer);
    }

    void StartGeneratingTest(TrainingSet[] sets) {
        ResetTest();
        _sets = sets;
        _setsAmount = _sets.Length - 1;
        GenerateSet(sets[_countSets]);
    }

    void GenerateSet(TrainingSet trainingSet) {
        _question.text = trainingSet.displaySet[0].text;
        _levelsText.text = $"{trainingSet.title} \\ {_setsAmount}";

        _buttonsNumbers = new List<int> { 0, 1, 2};        

        _buttons[GetRandomValue()].InitButton(trainingSet.matchSet[0].text, true);
        _buttons[GetRandomValue()].InitButton(trainingSet.negativeSet[0].text, false);
        _buttons[GetRandomValue()].InitButton(trainingSet.negativeSet[1].text, false);
    }

    int GetRandomValue() {
        int number = _buttonsNumbers[Random.Range(0, _buttonsNumbers.Count)];
        _buttonsNumbers.Remove(number);
        return number;
    }

    void Answer(bool answer, AnswerButton button) {
        StartCoroutine(CheckAnswer(answer, button)); 
    }

    IEnumerator CheckAnswer(bool answer, AnswerButton button) {
        button.SetColor(answer ? Color.green : Color.red);
        _scoreText.text = answer ? (_score += _pointsForAnswer).ToString() : (_score -= _pointsForAnswer).ToString();
        if (_score <= 0) ResetTest();
        yield return new WaitForSeconds(_timeAfterAnswer);
        button.ReturnColor();
        _countSets++;
        if (_countSets >= _setsAmount) ResetTest();
        GenerateSet(_sets[_countSets]);
    }

    void ResetTest() {
        Debug.Log("RESET TEST");
        _score = 0;
        _scoreText.text = _score.ToString();
        _countSets = 0;
        _levelsText.text = $"{_countSets} \\ {_setsAmount}";
    }
}
