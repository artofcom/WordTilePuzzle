using System.Collections;
using UnityEngine;
using Core.Events;

public class DirectorMain : MonoBehaviour
{
    public enum ScreenType
    {
        TITLE = 0, LOBBY, PLAY, MAX_SIZE
    }

    [SerializeField] AView[] ScreenViews;
    [SerializeField] AView PopUpScreenView;

    EventsGroup Events = new EventsGroup();

    IMVCS[] MVCSScreens;
    IMVCS PopUpScreen;
    IContext _context;

    private void Awake() { }
    private IEnumerator Start()
    {
        Application.targetFrameRate = 61;

        Events.RegisterEvent("TitleScreenView_OnClickBtnStart", TitleScreenView_OnClickBtnStart);
        Events.RegisterEvent("LobbyScreenView_OnBtnBackClicked", LobbyScreenView_OnBtnBackClicked);
        Events.RegisterEvent("LobbyScreenView_OnGamePrefabLoaded", LobbyScreenView_OnGamePrefabLoaded);
        Events.RegisterEvent("PlayScreenView_OnBtnBackClicked", PlayScreenView_OnBtnBackClicked);

        ActivateScreen(ScreenType.TITLE);

        _context = new GameContext();
        yield return StartCoroutine(_context.Init(this));

        InitScreen();

        PopUpScreen = new PopUpScreen(PopUpScreenView, _context);
        PopUpScreen.Initialize();

        yield return null;
    }

    void InitScreen()
    {
        MVCSScreens = new IMVCS[(int)ScreenType.MAX_SIZE];
        for (int q = 0; q < (int)ScreenType.MAX_SIZE; ++q)
        {
            switch((ScreenType)q)
            {
                case ScreenType.TITLE:
                    MVCSScreens[q] = new TitleScreen(ScreenViews[q], _context);
                    break;
                case ScreenType.LOBBY:
                    MVCSScreens[q] = new LobbyScreen(ScreenViews[q], _context);
                    break;
                case ScreenType.PLAY:
                    MVCSScreens[q] = new PlayScreen(ScreenViews[q], _context);
                    break;
            }
            MVCSScreens[q].Initialize();
        }
    }


    void ActivateScreen(ScreenType screenType)
    {
        for (int q = 0; q < (int)ScreenType.MAX_SIZE; ++q)
            ScreenViews[q].gameObject.SetActive(false);

        ScreenViews[(int)screenType].gameObject.SetActive(true);
    }

    void TitleScreenView_OnClickBtnStart(object data)
    {
        ActivateScreen(ScreenType.LOBBY);
    }

    void LobbyScreenView_OnBtnBackClicked(object data)
    {
        ActivateScreen(ScreenType.TITLE);
    }

    void LobbyScreenView_OnGamePrefabLoaded(object data)
    {
        ActivateScreen(ScreenType.PLAY);
    }

    void PlayScreenView_OnBtnBackClicked(object data)
    {
        ActivateScreen(ScreenType.LOBBY);
    }

}
