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
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private List<AnswerButton> _buttons;
    [SerializeField] private int _pointsForAnswer = 50;
    [SerializeField] private float _timeAfterAnswer = 1f;

    private TrainingSet[] _sets;
    private List<int> _buttonsNumbers;
    private int _countSets = 0;
    private int _setsAmount;
    private int _score = 0;

    private void Start() {
        JsonDataGetter test = GetComponent<JsonDataGetter>();
        test.OnGetTrainingSets += StartGeneratingTest;
        _buttons.ForEach(b => b.OnAnswer += Answer);
    }

    private void StartGeneratingTest(TrainingSet[] sets) {
        _sets = sets;
        _setsAmount = _sets.Length - 1;
        ResetTest();
    }

    private void GenerateSet(TrainingSet trainingSet) {
        _question.text = trainingSet.displaySet[0].text;
        _levelText.text = $"{trainingSet.title} \\ {_setsAmount}";
        _buttonsNumbers = new List<int> { 0, 1, 2};      

        _buttons[GetRandomValue()].InitButton(trainingSet.matchSet[0].text, true);
        _buttons[GetRandomValue()].InitButton(trainingSet.negativeSet[0].text, false);
        _buttons[GetRandomValue()].InitButton(trainingSet.negativeSet[1].text, false);
    }

    private int GetRandomValue() {
        int number = _buttonsNumbers[Random.Range(0, _buttonsNumbers.Count)];
        _buttonsNumbers.Remove(number);
        return number;
    }

    private void Answer(bool answer, AnswerButton button) {
        StartCoroutine(CheckAnswer(answer, button)); 
    }

    private IEnumerator CheckAnswer(bool answer, AnswerButton button) {
        button.SetColor(answer ? Color.green : Color.red);
        yield return new WaitForSeconds(_timeAfterAnswer);
        button.ReturnColor();
        if (!CheckScore(answer)) yield break;
        if (!CheckCount()) yield break;
        GenerateSet(_sets[_countSets]);
    }

    private bool CheckCount() {
        int count = _countSets + 1;
        if (count >= _setsAmount) {
            ResetTest();
            return false;
        } else {
            _countSets = count;
            return true;
        }
    }

    private bool CheckScore(bool answer) {
        _score = answer ? (_score += _pointsForAnswer) : (_score -= _pointsForAnswer);
        if (_score <= 0) {
            ResetTest();
            return false;
        } else {
            _scoreText.text = _score.ToString();
            return true;
        }
    }

    private void ResetTest() {
        _score = 0;
        _scoreText.text = _score.ToString();
        _countSets = 0;
        GenerateSet(_sets[_countSets]);
    }
}
