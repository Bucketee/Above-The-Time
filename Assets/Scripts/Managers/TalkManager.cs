using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public enum Character
{
    Player,
    Empty,
    Senior,
    Boss
}

public class TalkManager : MonoBehaviour
{
    [SerializeField] private GameObject talkCavas;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text talkText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Words words;
    [SerializeField] private CharacterSprites characterSprites;
    
    private Dictionary<Character, Sprite> characterSpriteDict = new();
    private List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)> nowTalk = new();
    private int talkProgressNum;
    private GameStateManager gameStateManager;

    private void Start()
    {
        gameStateManager = GameManager.Instance.GameStateManager;

        SetCharacterSpriteDict();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        StartTalk(0);
    //    }
    //}

    private void SetCharacterSpriteDict()
    {
        characterSpriteDict.Add(Character.Player, characterSprites.playerSprite);
        characterSpriteDict.Add(Character.Empty, characterSprites.emptySprite);
        characterSpriteDict.Add(Character.Senior, characterSprites.senior);
        characterSpriteDict.Add(Character.Boss, characterSprites.boss); 
    }

    public void StartTalk(int talkNum)
    {
        gameStateManager.ChangeGameState(GameState.Talking);
        talkProgressNum = -1;
        nowTalk = words.wordbundle[talkNum];
        talkCavas.SetActive(true);
        ProgressTalk();
    }

    public void ProgressTalk()
    {
        nextButton.gameObject.SetActive(false);

        talkProgressNum++;
        if (talkProgressNum >= nowTalk.Count)
        {
            EndTalk();
            return;
        }
        leftImage.sprite = characterSpriteDict[nowTalk[talkProgressNum].leftCharacter];
        rightImage.sprite = characterSpriteDict[nowTalk[talkProgressNum].rightCharacter];
        characterNameText.text = nowTalk[talkProgressNum].tellerName;
        talkText.text = nowTalk[talkProgressNum].words;

        nextButton.gameObject.SetActive(true);
    }

    public void EndTalk()
    {
        nowTalk = null;
        talkProgressNum = -1;
        talkCavas.SetActive(false);
        gameStateManager.ChangeGameState(GameState.Playing);
    }
}
