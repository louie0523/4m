using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExampleFlow : MonoBehaviour
{
    public CoreVisualNovelManager vnManager;  // CoreVisualNovelManager 참조

    void Start()
    {
        StartCoroutine(GameFlow());  // 게임 흐름 시작
    }

    private IEnumerator GameFlow()
    {
        // 첫 번째 대화 (주인공 등장)
        vnManager.Show("빌헬름 경", new Vector3(-300, 0, 0)); // 빌헬름 경을 특정 위치에 등장
        vnManager.StartDialogue("빌헬름 경", "이제 깊은 숲으로 들어가야 해.", 1);  // 배경 1번
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));  // 마우스 좌클릭 기다림

        // 빌헬름 경 이동
        vnManager.Move("빌헬름 경", new Vector3(0, 0, 0), 1.5f); // 중앙으로 이동
        yield return new WaitForSeconds(1.5f);  // 이동 시간 대기

        // 두 번째 대화 (조력자 등장)
        vnManager.Show("조력자", new Vector3(300, 0, 0)); // 조력자를 특정 위치에 등장
        vnManager.StartDialogue("빌헬름 경", "여기서부터 조심해야 해.", 2);  // 배경 2번
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));  // 마우스 좌클릭 기다림

        // 선택지 표시
        vnManager.ShowChoices(new string[] { "싸운다", "도망친다" }, OnChoiceSelected);
    }

    // 선택지가 클릭되었을 때 호출되는 메서드
    private void OnChoiceSelected(int choice)
    {
        if (choice == 0)
        {
            // "싸운다"를 선택하면 전투 씬으로 이동
            vnManager.GoToBattleScene();
        }
        else
        {
            // "도망친다"를 선택하면 도망친 후, 다음 대사로 넘어감
            StartCoroutine(NextDialogueAfterEscape());
        }
    }

    // "도망친다" 선택 후 다음 대사로 넘어가는 메서드
    private IEnumerator NextDialogueAfterEscape()
    {
        // 도망쳤다고 대사 출력
        vnManager.StartDialogue("주인공", "도망치자!", 3);  // 배경 3번
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));  // 마우스 좌클릭 기다림

        // 조력자 이동 및 대사
        vnManager.Move("조력자", new Vector3(0, 0, 0), 1.5f); // 중앙으로 이동
        yield return new WaitForSeconds(1.5f);
        vnManager.StartDialogue("조력자", "잘 생각했어, 우리가 이길 수 있는 상황이 아니야.", 4);  // 배경 4번
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));  // 마우스 좌클릭 기다림

        // 캐릭터 퇴장
        vnManager.Hide("빌헬름 경");
        vnManager.Hide("조력자");

        // 게임 흐름 계속
        // 다음 대사나 장면으로 넘어가도록 구현할 수 있습니다.
        // 예: vnManager.StartDialogue("주인공", "이제 어떻게 해야 하지?", 5);
    }
}
