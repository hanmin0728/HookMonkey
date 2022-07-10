using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpenWindow : MonoBehaviour
{
    private GameManager _gM;

    [SerializeField] private GameObject _settingWindow;
    [SerializeField] private GameObject _invenWindow;
    [SerializeField] private GameObject _tipWindow;


    #region 플레이어가 키를 눌러 여는 창들 열렸는지 확인해주는 변수들
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
    /// 레프트 컨트롤 눌러서 커서 생기게 해주는 함수
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
    /// Tap키 눌러 인벤토리여는 함수
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
    /// Esc키 눌러 설정창 여는 함수
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
    /// H키 눌러 도움말창 여는 함수
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
