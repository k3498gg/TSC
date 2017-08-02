#define TouchControl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if TouchControl
using TouchControlsKit;

public class UITest : MonoBehaviour
{
    private TCKJoystick m_Joystick;

    // Use this for initialization
    void Start()
    {
        m_Joystick = GetComponent<TCKJoystick>();
        //m_Joystick.broadcast = true;
        //m_Joystick.downEvent.AddListener(DownEvent);
        //m_Joystick.pressEvent.AddListener(DownEvent);
        m_Joystick.BindAxes(DownEvent, EActionEvent.Down);
        m_Joystick.BindAxes(DownEvent, EActionEvent.Press);
    }

    void Devent(float a, float b)
    {
        Debug.LogError("onDown: " + a + " " + b);
    }

    public float speed = 5;
    void DownEvent(float a, float b)
    {
        if (a == 0 && b == 0)
        {
            return;
        }

        if (null == GameMgr.Instance.MainEntity)
        {
            return;
        }

        if (null == GameMgr.Instance.CameraController)
        {
            return;
        }

        float angle = Mathf.Rad2Deg * (Mathf.Atan2(a, b));

        GameMgr.Instance.MainEntity.CacheModel.rotation = Quaternion.Euler(0, angle + GameMgr.Instance.CameraController.EulerY, 0);
        //CacheEntity.CacheModel.position += CacheEntity.CacheModel.forward * Time.deltaTime * speed;
        GameMgr.Instance.CharacController.SimpleMove(GameMgr.Instance.MainEntity.CacheModel.forward * Time.deltaTime * GameMgr.Instance.MainEntity.Attribute.Speed);
        //CollisionFlags flag = GameMgr.Instance.CharacController.Move(GameMgr.Instance.MainEntity.CacheModel.forward * Time.deltaTime * speed);
        //if(flag != CollisionFlags.None)
        //{
        //    Debuger.LogError(flag);
        //}
    }

    private void OnDestroy()
    {
        if (null != m_Joystick)
        {
            m_Joystick.downEvent.RemoveAllListeners();
            m_Joystick.UnBindAxes(DownEvent, EActionEvent.Press);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
#endif
