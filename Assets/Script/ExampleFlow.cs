using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExampleFlow : MonoBehaviour
{
    public CoreVisualNovelManager vnManager;  // CoreVisualNovelManager 참조
    /** 튜토리얼
     * vmManager. 로 함수 호출
     * vnManager.SetBackground(4); = 4번 배경 호출
     * vnManager.StartDialogue("", "이곳인가 나의 새학기가 시작될곳이!"); = 발화자, 말할 내용으로 발화자칸이 비어져있으면 자동으로 대사를 나레이션화시킨다.
     * vnManager.Show("빌헬름 경", new Vector3(-400, 0, 0)); = 캐릭터명, 등장시킬 위치로 특정 캐릭터를 특정 위치에 빠르게 등장시킨다.
     * vnManager.Move("빌헬름 경", new Vector3(0, 0, 0), 0.5f); = 캐릭터명, 이동시킬 위치, 걸리는 시간으로 현재 활성화되어있늩 캐릭터를 특정 위치로 걸리는 시간동안 이동한다.
     * yield return new WaitForSeconds(0.5f);  = move로 이동하는 동안 대기
     * vnManager.Hide("빌헬름 경"); = 특정 캐릭터를 숨긴다.
     * vnManager.ShowChoices(new string[] { "싸운다", "도망친다" }, OnChoiceSelected); 버튼 1, 버튼 2, 버튼 3, 버튼 4, 분기점 으로 버튼의 갯수는 알맞게 정할 수 있다. 뒤에 함수는 따로 만들어야함.
    private void OnChoiceSelected(int choice)
    {
        if (choice == 0)
        {
            //vnManager.GoToBattleScene();
            StartCoroutine(FightMinchelMom());
        }
        else if (choice == 1)
        {
            StartCoroutine(NextDialogueAfterEscape());
        }
    }
     * 싸운다 선택지 파이트 민철맘 분기로 이동 / 도망친다 선택시 도망치고난후 분기로 이동
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * **/
    void Start()
    {
        vnManager.dialogueFrame.SetActive(false);
        vnManager.speakerNameFrame.SetActive(false);
        StartCoroutine(GameFlow());  // 게임 흐름 시작
    }

    private IEnumerator WaitForClick()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        yield return new WaitForSeconds(0.1f); // 짧은 시간 동안 추가 입력 방지
    }

    private IEnumerator GameFlow()
    {
        // 첫 번째 대화 (주인공 등장)
        vnManager.SetBackground(4);  // 배경 1번
        yield return WaitForClick();

        vnManager.StartDialogue("", "......");
        yield return WaitForClick();

        vnManager.SetBackground(0);
        yield return WaitForClick();

        vnManager.StartDialogue("???", "정말로 탑에 들어와 버렸군...");
        yield return WaitForClick();

        vnManager.Show("빌헬름 경", new Vector3(-400, 0, 0));
        vnManager.StartDialogue("빌헬름 경", "이곳이 1층인가, 겉으로 보아선 밖과 구분이 되지 않는데 신비한 공간이로군.);
        yield return WaitForClick();

        vnManager.Move("빌헬름 경", new Vector3(0, 0, 0), 0.1f);
        yield return new WaitForSeconds(0.1f);  // 이동 시간 대기

        vnManager.StartDialogue("빌헬름 경", "일단 주위를 좀 둘러보는 것이 좋겠군, 다른 누군가가 있을 수 도 있으니까");
        yield return WaitForClick();

        vnManager.SetBackground(0);
        yield return WaitForClick();

        vnManager.SetBackground(0);
        yield return WaitForClick();

        vnManager.StartDialogue("", "주위를 둘러보니 인기척 같은 건 느껴지지 않는다.");
        yield return WaitForClick();

        vnManager.StartDialogue("빌헬름 경", "....");
        yield return WaitForClick();

        vnManager.StartDialogue("빌헬름 경", "나무가 너무 우거져있으니, 직진으로 이동하되 가는 길마다 돌을 둬서 길을 잃을 가능성을 배제해야겠군.");
        yield return WaitForClick();

        vnManager.StartDialogue("", "전쟁통에 산에서 고립된 경험이 어느정도 있기에 최대한 빠르고 신속하게 판단하였다.");
        yield return WaitForClick();

        // 선택지 표시
        vnManager.ShowChoices(new string[] { "싸운다", "도망친다" }, OnChoiceSelected);
    }

    private void OnChoiceSelected(int choice)
    {
        if (choice == 0)
        {
            //vnManager.GoToBattleScene();
            StartCoroutine(FightMinchelMom());
        }
        else if (choice == 1)
        {
            StartCoroutine(NextDialogueAfterEscape());
        }
    }


    // 싸운다
    private IEnumerator FightMinchelMom()
    {
        vnManager.SetBackground(3);
        yield return WaitForClick();

        vnManager.StartDialogue("빌헬름 경", "하지만 민철맘은 이기긴 우린 너무 약해");
        yield return WaitForClick();

        vnManager.Move("조력자", new Vector3(0, 0, 0), 1.5f); // 중앙으로 이동
        yield return new WaitForSeconds(1.5f);

        vnManager.SetBackground(4);
        yield return WaitForClick();

        vnManager.StartDialogue("조력자", "잘 생각했어, 우리가 이길 수 있는 상황이 아니야.");
        yield return WaitForClick();

        // 캐릭터 퇴장
        //vnManager.Hide("빌헬름 경");
        //vnManager.Hide("조력자");

    }


    // 도망친다
    private IEnumerator NextDialogueAfterEscape()
    {
        vnManager.SetBackground(3);
        yield return WaitForClick();

        vnManager.StartDialogue("빌헬름 경", "도망치자!");
        yield return WaitForClick();

        vnManager.Move("조력자", new Vector3(0, 0, 0), 1.5f);
        yield return new WaitForSeconds(1.5f);
        vnManager.SetBackground(4);
        yield return WaitForClick();

        vnManager.StartDialogue("조력자", "잘 생각했어, 우리가 이길 수 있는 상황이 아니야.");
        yield return WaitForClick();

        // 캐릭터 퇴장
        //vnManager.Hide("빌헬름 경");
        //vnManager.Hide("조력자");

    }
}