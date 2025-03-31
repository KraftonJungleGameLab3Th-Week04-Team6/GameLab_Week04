using UnityEngine;

public static class EndingPrefs
{
    // 엔딩 해금 저장
    public static void UnlockEnding(int index)
    {
        PlayerPrefs.SetInt($"Ending_{index}", 1);
        PlayerPrefs.Save(); // 저장을 명시적으로 해주는 게 안전함
    }

    // 엔딩 해금 여부 확인
    public static bool IsEndingUnlocked(int index)
    {
        return PlayerPrefs.GetInt($"Ending_{index}", 0) == 1;
    }

    // 전체 초기화 (디버깅/테스트용)
    public static void ResetAllEndings()
    {
        for (int i = 0; i < 6; i++)
        {
            PlayerPrefs.DeleteKey($"Ending_{i}");
        }
        PlayerPrefs.Save();
    }
}
