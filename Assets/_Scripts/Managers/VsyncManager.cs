using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class VsyncManager : MonoBehaviour
{
    [SerializeField] private Toggle vSyncToggle;

    private void Start()
    {
        InitVSync();

        // 기존 이벤트 리스너 제거 (중복 방지)
        vSyncToggle.onValueChanged.RemoveAllListeners();

        // 토글 변경 시 V-Sync 옵션 적용
        vSyncToggle.onValueChanged.AddListener(VsyncOption);
    }

    private void InitVSync()
    {
        // 현재 V-Sync 상태를 토글 UI에 반영
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
    }

    public void VsyncOption(bool isOn)
    {
        // V-Sync 적용
        QualitySettings.vSyncCount = isOn ? 1 : 0;

        // 현재 V-Sync 상태 출력
        UnityEngine.Debug.Log("V-Sync 설정 변경됨: " + (isOn ? "활성화" : "비활성화"));
    }
}
