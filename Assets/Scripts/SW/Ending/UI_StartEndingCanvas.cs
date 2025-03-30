using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartEndingCanvas : MonoBehaviour
{
    public Sprite[] endingSceneClearImage;
    public Transform _endingSceneContent;
    private Image[] _endingSceneImage;
    
    private void Start()
    {
        // 매니저에서 엔딩이 해금되었는지 가져옴 혹은 로컬에 저장한 sprite 넘버를 넣고 시작

        _endingSceneImage = new Image[_endingSceneContent.childCount];

        for (int i=0;i < _endingSceneContent.childCount;i++)
        {
            _endingSceneImage[i] = _endingSceneContent.GetChild(i).GetComponent<Image>();
            if (EndingPrefs.IsEndingUnlocked(i))
            {
                _endingSceneImage[i].sprite = endingSceneClearImage[i];
            }
        }

        for (int i = 0; i < 6; i++)
        {
            if (EndingPrefs.IsEndingUnlocked(i))
            {
                Debug.Log($"엔딩 {i} 클리어함!");
                // → 여기에 UI 처리 (예: 도감에 색깔 표시 등)
            }
            else
            {
                Debug.Log($"엔딩 {i} 미클리어");
            }
        }
    }

    public void GoToMain()
    {
        Manager.Game.GoMain();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            EndingPrefs.ResetAllEndings();
        }
    }

}
