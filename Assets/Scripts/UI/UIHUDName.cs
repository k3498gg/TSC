using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHUDName : MonoBehaviour
{
    public Transform m_target;
    public Vector3 m_offset;
    public Camera m_camera;
    public Camera m_UICamera;
    private Transform cache;
    private GameObject m_imageGo;
    private GameObject m_nameGo;
    private Image m_Image;
    private Text m_Name;
    private bool init = false;

    public Transform Cache
    {
        get
        {
            return cache;
        }

        set
        {
            cache = value;
        }
    }

    //private void Start()
    //{
    //    Init();
    //}

    public void Init()
    {
        if (init)
        {
            return;
        }
        init = true;
        Cache = transform;
        m_imageGo = Cache.Find("Image").gameObject;
        m_nameGo = Cache.Find("Text").gameObject;
        m_Name = m_nameGo.GetComponent<Text>();
        m_Image = m_imageGo.GetComponent<Image>();
        m_camera = GameMgr.Instance.CameraController.Camera;
        m_UICamera = UIManager.Instance.GetUiCamera();
    }

    public void SetOffset(float x,float y)
    {
        m_offset.x = x;
        m_offset.y = y;
    }

    public void SetName(string text)
    {
        if (null != m_Name)
        {
            m_Name.text = text;
        }
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
    }

    public void SetImage(Sprite sprite)
    {
        if(null != m_Image)
        {
            m_Image.sprite = sprite;
        }
    }


    private void LateUpdate()
    {
        if (null == m_target)
        {
            return;
        }

        if (null == m_camera)
        {
            return;
        }

        if (null == m_UICamera)
        {
            return;
        }

        Vector3 pos = m_camera.WorldToViewportPoint(m_target.position);
        bool isVisible = pos.x > 0 && pos.x < 1 && pos.y > 0 && pos.y < 1;

        if (isVisible)
        {
            Util.SetEnable(m_Image, true);
            Util.SetEnable(m_Name, true);
            pos = m_UICamera.ViewportToWorldPoint(pos);
            pos.z = 0;
            Cache.position = pos + m_offset;
        }
        else
        {
            Util.SetEnable(m_Image, false);
            Util.SetEnable(m_Name, false);
        }
    }
}
