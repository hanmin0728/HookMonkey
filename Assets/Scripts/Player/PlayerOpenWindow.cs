using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpenWindow : MonoBehaviour
{
    private GameManager _gM;

    [SerializeField] private GameObject _settingWindow;
    [SerializeField] private GameObject _invenWindow;
    [SerializeField] private GameObject _tipWindow;


    #region �÷��̾ Ű�� ���� ���� â�� ���ȴ��� Ȯ�����ִ� ������
    private bool _isOpenInven = false;
    private bool _isOpenTip = false;
    private bool _isOpenEsc = false;
    #endregion

    private void Awake()
    {
        _gM = GameObject.Find("GamaManager").GetComponent<GameManager>();
    }
    private void Update()
    {
        OpenCursor();
        OpenTip(_isOpenTip);
        OpenInventory(_isOpenInven);
        OpenEsc(_isOpenEsc);
    }

    /// <summary>
    /// ����Ʈ ��Ʈ�� ������ Ŀ�� ����� ���ִ� �Լ�
    /// </summary>
    public void OpenCursor()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Cursor.visible = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// TapŰ ���� �κ��丮���� �Լ�
    /// </summary>
    public void OpenInventory(bool isActive)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _isOpenInven = isActive ? false : true;
            Cursor.visible = isActive ? false : true;

            _gM.InventoryManager.ListItems();
            _invenWindow.SetActive(_isOpenInven);
        }

    }
    
    /// <summary>
    /// EscŰ ���� ����â ���� �Լ�
    /// </summary>
    public void OpenEsc(bool isActive)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isOpenInven = isActive ? false : true;
            Cursor.visible = isActive ? false : true;
            Time.timeScale = 0;
            _settingWindow.SetActive(_isOpenInven);
        }
    }

    /// <summary>
    /// HŰ ���� ����â ���� �Լ�
    /// </summary>
    public void OpenTip(bool isActive)
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            _isOpenTip = isActive ? false : true;
            Cursor.visible = isActive ? false : true;
            _tipWindow.SetActive(_isOpenTip);
        }
    }
}
