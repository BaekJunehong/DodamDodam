using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EvaluationText : MonoBehaviour
{
    public TextMeshProUGUI evaluationText; // UI Text 컴포넌트 할당 필요

    private void Start()
    {
        StartCoroutine(UpdateEvaluationText());
        StartCoroutine(SwitchSceneAfterDelay(3f)); // 3초 뒤 씬을 변경하는 코루틴 시작
    }

    private IEnumerator UpdateEvaluationText()
    {
        string[] messages = { "평가 중.", "평가 중..", "평가 중..." };
        int index = 0;

        while (true)
        {
            evaluationText.text = messages[index];
            index = (index + 1) % messages.Length;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator SwitchSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Result"); // 지정한 씬으로 전환
    }
}