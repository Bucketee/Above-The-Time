using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Words", menuName = "Words")]
public class Words : ScriptableObject
{
    public List<List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>> wordbundle = new()
    {
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //0
        {
            (Character.Player, Character.Empty, "Player", true, "test1"),
            (Character.Player, Character.Empty, "???", false, "test2"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //1
        {
            (Character.Player, Character.Senior, "나", true, "하암... 피곤하다."),
            (Character.Player, Character.Senior, "???", false, "어이 신입 빨리 와!!"),
            (Character.Player, Character.Senior, "나", true, "네..넵!!."),
            (Character.Player, Character.Senior, "나", true, "(어서 오른쪽으로 가보자.)"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //2
        {
            (Character.Player, Character.Empty, "나", true, "(얼마 전에 이곳에 온 나는 시간 경찰이다.)"),
            (Character.Player, Character.Empty, "나", true, "(무려 시간을 조종하여 각종 문제들을 해결하는 슈퍼 히어로같은 직업이라고 할 수 있다.)"),
            (Character.Player, Character.Empty, "나", true, "(이곳의 다양한 장치에 매번 즐겁다. 시간 여행이 가능하다니!)"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //3
        {
            (Character.Player, Character.Senior, "나", true, "안녕하세요 선배!"),
            (Character.Player, Character.Senior, "선배", false, "그래! 어서 임무에 갈 준비를 해!"),
            (Character.Player, Character.Senior, "나", true, "오늘은 무슨 임무인가요?"),
            (Character.Player, Character.Senior, "선배", false, "뒤쪽에 있는 슬럼가 알지?\n그곳에 나무를 심어 슬럼가를 없애는 게 이번 임무의 목표다."),
            (Character.Player, Character.Senior, "나", true, "아..알겠습니다!"),
            (Character.Player, Character.Senior, "나", true, "(근데 나무 심는 걸로 슬럼가가 사라지나?)"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //4
        {
            (Character.Player, Character.Senior, "나", true, "선배 나무는 어디에 심을까요?"),
            (Character.Player, Character.Senior, "선배", false, "그 전에 일단 저 앞에 있는 도적 두 마리부터 잡자."),
            (Character.Player, Character.Senior, "나", true, "제...제가요??"),
            (Character.Player, Character.Senior, "선배", false, "그래. 너가."),
            (Character.Player, Character.Senior, "나", true, "(마우스 왼쪽 클릭을 통해 총을 쏠 수 있다.\n앞의 도적을 빨리 잡자!)"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //5
        {
            (Character.Player, Character.Senior, "나", true, "선배 다 잡았어요!!"),
            (Character.Player, Character.Senior, "선배", false, "좋아. 그럼 우리는 과거로 간다.\n과거로 가는 법은 기억하고 있겠지?"),
            (Character.Player, Character.Senior, "나", true, "..."),
            (Character.Player, Character.Senior, "선배", false, "하... R을 누르면 시계가 뜬다. 시계의 시침과 분침을 시계, 반시계 방향으로 돌리면 \n미래, 과거로 갈 수 있다. 시침은 한칸당 12년, 분침은 한칸당 1년을 뜻한다. \n시침, 분침을 돌리고 다시 R을 누르면 시간을 여행할 수 있다."),
            (Character.Player, Character.Senior, "선배", false, "그럼 50년 전이면 시침과 분침을 얼마나 돌려야 하는지는 알겠지?"),

            (Character.Player, Character.Senior, "나", true, "넵!"),
            (Character.Player, Character.Senior, "나", true, "(어서 50년 전으로 가자! 시침을 4칸, 분침을 2칸 옮기면 되겠지?)"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //6
        {
            (Character.Player, Character.Senior, "나", true, "어라.. 여긴 어디?? \n선배 이상한 곳으로 온 것 같은데요!?"),
            (Character.Player, Character.Senior, "선배", false, "원래 이곳은 슬럼가가 아니라 평화로운 마을이었다.\n어느날부터 마을의 나무들이 죽기 시작하더니 사람들이 떠나가고\n도적들이 들어와 살기 시작하면서 슬럼가가 됐지."),
            (Character.Player, Character.Senior, "나", true, "그럼 저희는 뭘 하면 되나요?"),
            (Character.Player, Character.Senior, "선배", false, "이 묘목을 심으면 주위의 나무에 백신이 퍼져나가 나무들이 죽지 않게될 거야.\n그러니 어서 심고 와."),
            (Character.Player, Character.Senior, "나", true, "제...제가요?"),
            (Character.Player, Character.Senior, "선배", false, "그래. 너가."),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //7
        {
            (Character.Player, Character.Empty, "나", true, "앞의 길이 끊어져있다. 미래에는 이러지 않았는데...\n어떻게 넘어가지?"),
            (Character.Player, Character.Empty, "나", true, "내 두번째 장비를 꺼낼 때인가?! 바로 이 귀! 저기 앞에 가라앉은 길을 우클릭해서....!!!"),
            (Character.Player, Character.Empty, "나", true, "(마우스 커서를 올렸을 때 노랑색 빛이 나는 물체들을 우클릭을 통해 시간을 멈출 수 있습니다.\n이때 마우스 스크롤을 내리면 물체의 시간이 과거로 흐릅니다.\n반대로 스크롤을 올리면 물체의 시간이 미래로 흐릅니다.)"),
            (Character.Player, Character.Empty, "나", true, "좋았어! 바닥의 시간을 과거로 돌려 길을 만들자고!"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //8
        {
            (Character.Player, Character.Empty, "나", true, "뭐야 이 바위는? 흠.. 그러고보니 예전에 여기 커다란 바위가 있었는데 부숴버렸다 했지. \n미래로 돌려 부술 수 있을까?"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //9
        {
            (Character.Player, Character.Empty, "나", true, "나무는 이쯤에 심으면 되나?"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //10
        {
            (Character.Player, Character.Empty, "나", true, "이 건물은 뭐지..? 들어가면 안될 것 같은데..."),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //11
        {
            (Character.Player, Character.Empty, "나", true, "좋아! 이제 선배한테 돌아가서 보고하자."),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //12
        {
            (Character.Player, Character.Senior, "나", true, "선배~ 저 왔어요!"),
            (Character.Player, Character.Senior, "선배", false, "왜 이렇게 늦은 거야. 어서 돌아가자고."),
            (Character.Player, Character.Senior, "나", true, "칫.."),
            (Character.Player, Character.Senior, "나", true, "(50년 후로 돌아가자.)"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //13
        {
            (Character.Player, Character.Senior, "나", true, "아앗!! 선배 전혀 안변했는데요?"),
            (Character.Player, Character.Senior, "선배", false, "이게 무슨 일이지?!\n흠... 아무래도 그 사이에 무슨 일이 생긴 것 같군."),
            (Character.Player, Character.Senior, "선배", false, "넌 45년 전으로 돌아가 이 일을 조사해봐.\n난 위에 보고하러 가지."),
            (Character.Player, Character.Senior, "나", true, "저 혼자 가라고요? 제...제가요?"),
            (Character.Player, Character.Senior, "선배", false, "그래. 너만."),
            (Character.Player, Character.Senior, "나", true, "...."),
            (Character.Player, Character.Senior, "나", true, "(45년 전으로 돌아가 나무를 조사해보자.)"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //14
        {
            (Character.Player, Character.Empty, "나", true, "나무에 왜 도끼가??"),
            (Character.Player, Character.Empty, "나", true, "흠... 아무래도 도끼의 주인을 찾아 혼내줘야겠는걸?"),
            (Character.Player, Character.Empty, "나", true, "도끼의 시간을 거꾸로 흐르게 해서 주인을 찾아가자!"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //15
        {
            (Character.Player, Character.Empty, "나", true, "아무래도 도끼는 이 타워로 들어간 것 같은데...\n따라가보자!"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //16
        {
            (Character.Player, Character.Empty, "나", true, "도끼는 위쪽으로 올라간 건가? 근데 어떻게 올라가지..."),
            (Character.Player, Character.Empty, "나", true, "앗 바로 위에 약한 벽이 있는 것 같은데?\n저걸 미래로 돌리면..?"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //17
        {
            (Character.Player, Character.Empty, "나", true, "레버에 팔이 안닿는데 멀리서 조정할 수 없을까?"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //18
        {
            (Character.Player, Character.Empty, "나", true, "위에서 철골들이 떨어진 것 같다.\n잘 이용하면 올라갈 수 있을 것 같은데?"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //19
        {
            (Character.Player, Character.Boss, "보스", false, "누구냐!?"),
            (Character.Player, Character.Boss, "나", true, "나는 시간 경찰이다! 너가 나무를 벤 녀석이냐?"),
            (Character.Player, Character.Boss, "나", true, "정의의 이름으로 널 용서하지 않겠다!"),
            (Character.Player, Character.Boss, "보스", false, "이곳에 왔다는 건 다 알고 온건가."),
            (Character.Player, Character.Boss, "보스", false, "어쩔 수 없군... 죽어라!"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //20
        {
            (Character.Player, Character.Boss, "보스", false, "으으.."),
            (Character.Player, Character.Boss, "나", true, "하..하... 힘들다... 일단 타워를 나가자."),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //21
        {
            (Character.Player, Character.Empty, "나", true, "후... 이제 45년 뒤 현재로 가볼까?"),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //22
        {
            (Character.Player, Character.Empty, "나", true, "다시 나무가 있던 곳으로 돌아가보자."),
        },
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>() //23
        {
            (Character.Player, Character.Empty, "나", true, "와아~ 나무다~"),
            (Character.Player, Character.Empty, "나", true, "이제 슬럼가가 없어지고 마을이 평화로워졌겠지?"),
        },
    };
}
