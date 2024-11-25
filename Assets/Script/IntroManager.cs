using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("Intro Sequence")]
    public Image[] introImages; // 출력할 이미지 배열
    public string[] dialogues; // 각 이미지와 연결된 대사 배열
    public int[] imageIndices; // 각 대사에 맞는 이미지 인덱스를 나타내는 배열
    public TextMeshProUGUI dialogueText; // 대사를 표시할 TextMeshPro 객체

    [Header("Settings")]
    public string nextSceneName; // 마지막에 이동할 씬 이름
    public float imageFadeDuration = 1.5f; // 이미지 페이드인 시간
    public float textSpeed = 0.05f; // 텍스트 출력 속도
    public KeyCode skipKey = KeyCode.LeftControl; // 텍스트 스킵 키

    private int currentIndex = 0; // 현재 진행 중인 이미지와 텍스트 인덱스
    private int lastImageIndex = -1; // 이전에 사용한 이미지 인덱스

    void Start()
    {
        // 모든 이미지 초기화 (투명 설정)
        foreach (var image in introImages)
            image.color = new Color(1, 1, 1, 0);

        dialogueText.text = ""; // 텍스트 초기화
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        while (currentIndex < dialogues.Length)
        {
            // imageIndices 배열이 dialogues 배열보다 길이가 길거나 같다고 가정하고, 범위 검사 추가
            if (currentIndex < imageIndices.Length)
            {
                // 현재 대사에 맞는 이미지 인덱스를 가져옴
                int imageIndex = imageIndices[currentIndex];

                // 이미지가 바뀔 때만 페이드인 효과를 적용
                if (imageIndex != lastImageIndex)
                {
                    // 현재 이미지 페이드인
                    yield return StartCoroutine(FadeInImage(introImages[imageIndex]));
                    lastImageIndex = imageIndex; // 이전 이미지 인덱스를 갱신
                }
            }
            else
            {
                Debug.LogError("imageIndices 배열의 길이가 dialogues 배열보다 짧습니다. 배열 길이를 확인하세요.");
                yield break; // 배열의 길이가 맞지 않으면, 에러를 로그에 출력하고 종료
            }

            // 현재 대사 출력
            yield return StartCoroutine(PlayDialogue(dialogues[currentIndex]));

            // 마우스 클릭 대기 후 다음으로 진행
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            currentIndex++;
        }

        // 인트로 종료 후 다음 씬으로 이동
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeInImage(Image image)
    {
        float timer = 0f;
        Color color = image.color;

        while (timer < imageFadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / imageFadeDuration);
            image.color = color;
            yield return null;
        }
    }

    private IEnumerator PlayDialogue(string dialogue)
    {
        dialogueText.text = ""; // 텍스트 초기화
        int currentCharIndex = 0;

        while (currentCharIndex < dialogue.Length)
        {
            if (Input.GetKey(skipKey))
            {
                // 스킵 키를 누르면 전체 텍스트 즉시 출력
                dialogueText.text = dialogue;
                yield break;
            }

            dialogueText.text += dialogue[currentCharIndex];
            currentCharIndex++;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
