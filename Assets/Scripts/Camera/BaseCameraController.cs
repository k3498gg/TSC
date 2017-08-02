using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum CameraFollowModel
{
    TDFollow = 1, //�̶��Ӿ�3DЧ��
    RDFollow = 2  //��ת�Ӿ�3DЧ��
}


public class BaseCameraController : MonoBehaviour
{
    private Transform target;
    public float startingDistance = 10f;
    public float maxDistance = 20f;
    public float minDistance = 3f;
    public float zoomSpeed = 20f;
    public float camXAngle = 45.0f;
    public bool fadeObjects = false;
    public List<int> layersToTransparent = new List<int>();
    public float alpha = 0.3f;
    public float targetHeight = 2.0f;
    public float rotationDamping = 3.0f;

#if UNITY_EDITOR
    public float camRotationSpeed = 70;
    public float minCameraAngle = 0.0f;
    public float maxCameraAngle = 90.0f;
#elif UNITY_ANDROID || UNITY_IPHONE
    public bool pinchZoom = true;  // ���ڿ���ĳЩ״̬�Ƿ��������ž�ͷ
    public float prevDistance = 0.0f; 
#endif

    private Camera m_camera;
    public Camera Camera
    {
        get
        {
            if (null == m_camera)
            {
                m_camera = GetComponent<Camera>();
            }
            return m_camera;
        }
        set
        {
            m_camera = value;
        }
    }

    public Transform Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }
}
