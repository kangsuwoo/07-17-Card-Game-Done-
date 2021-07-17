using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public void OnGameStart()
    {
        //  빌드 셋팅 리스트에 있는 1번째 씬을 로드한다
        //  씬 로드 시 기존에 보여주고 있는 씬은 제거된다!
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    //  게임 종료
    //  Application.Quit();
    //  단, 유니티 에티터 상에서 진짜 에디터가 꺼지면 안되므로 실행이 안된다
    public void OnExitGame()
    {
//  전처리기!
#if UNITY_EDITOR    //  유니티 에디서 상에서 코드가 돌아갈 때
        UnityEditor.EditorApplication.isPlaying = false;
#else               //  유니티 에디터 상태가 아닐 때
        Application.Quit();
#endif
    }

}
