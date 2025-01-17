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

        // ���� �̺�Ʈ ������ ���� (�ߺ� ����)
        vSyncToggle.onValueChanged.RemoveAllListeners();

        // ��� ���� �� V-Sync �ɼ� ����
        vSyncToggle.onValueChanged.AddListener(VsyncOption);
    }

    private void InitVSync()
    {
        // ���� V-Sync ���¸� ��� UI�� �ݿ�
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
    }

    public void VsyncOption(bool isOn)
    {
        // V-Sync ����
        QualitySettings.vSyncCount = isOn ? 1 : 0;

        // ���� V-Sync ���� ���
        UnityEngine.Debug.Log("V-Sync ���� �����: " + (isOn ? "Ȱ��ȭ" : "��Ȱ��ȭ"));
    }
}
