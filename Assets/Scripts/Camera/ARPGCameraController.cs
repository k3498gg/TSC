using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ARPGCameraController : BaseCameraController
{
    private float eulerY = 0.0f; // The camera y euler angle.
    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
        eulerY = myTransform.eulerAngles.y;
    }

    public float EulerY
    {
        get { return eulerY; }
    }

    public void SetTarget(Transform cache)
    {
        if (null != cache)
        {
            Target = cache;
        }
    }

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

#if UNITY_EDITOR
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0)
        {
            startingDistance -= Time.deltaTime * zoomSpeed;
            if (startingDistance < minDistance)
                startingDistance = minDistance;
        }
        else if (mw < 0)
        {
            startingDistance += Time.deltaTime * zoomSpeed;
            if (startingDistance > maxDistance)
                startingDistance = maxDistance;
        }

        if (Input.GetMouseButton(1))
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            if (h > 0 && h > Math.Abs(v))
            {
                myTransform.RotateAround(Target.transform.position, new Vector3(0, 1, 0), camRotationSpeed * Time.deltaTime);
                eulerY = myTransform.eulerAngles.y;
            }
            else if (h < 0 && h < -Math.Abs(v))
            {
                myTransform.RotateAround(Target.transform.position, new Vector3(0, 1, 0), -camRotationSpeed * Time.deltaTime);
                eulerY = myTransform.eulerAngles.y;
            }
            else if (v > 0 && v > Math.Abs(h))
            {
                camXAngle += camRotationSpeed * Time.deltaTime;
                if (camXAngle > maxCameraAngle)
                {
                    camXAngle = maxCameraAngle;
                }
            }
            else if (v < 0 && v < -Math.Abs(h))
            {
                camXAngle += -camRotationSpeed * Time.deltaTime;
                if (camXAngle < minCameraAngle)
                {
                    camXAngle = minCameraAngle;
                }
            }
        }

        Quaternion rotation = Quaternion.Euler(camXAngle, eulerY, 0);
        myTransform.rotation = rotation;
        Vector3 trm = rotation * Vector3.forward * startingDistance + new Vector3(0, -1 * targetHeight, 0);
        Vector3 position = Target.position - trm;
        myTransform.position = position;
#elif UNITY_ANDROID || UNITY_IPHONE
        //if (Input.touchCount == 2 && pinchZoom)
        //{
        //    Vector2 touch0 = Input.GetTouch(0).position;
        //    Vector2 touch1 = Input.GetTouch(1).position;

        //    float distance = Vector2.Distance(touch0, touch1);

        //    if (prevDistance != 0)
        //    {
        //        if (prevDistance > distance)
        //        {
        //            startingDistance += Time.deltaTime * zoomSpeed;
        //            if (startingDistance > maxDistance)
        //                startingDistance = maxDistance;
        //        }
        //        else if (prevDistance < distance)
        //        {
        //            startingDistance -= Time.deltaTime * zoomSpeed;
        //            if (startingDistance < minDistance)
        //                startingDistance = minDistance;
        //        }
        //    }

        //    prevDistance = distance;
        //}
        //else
        //{
        //    prevDistance = 0;
        //}

        Quaternion rotation = Quaternion.Euler(camXAngle, eulerY, 0);
        myTransform.rotation = rotation;

        Vector3 trm = rotation * Vector3.forward * startingDistance + new Vector3(0, -1 * targetHeight, 0);
        Vector3 position = Target.position - trm;
        myTransform.position = position;
#endif

        FadeObject();
    }

    private Renderer preRender = null;

    void FadeObject()
    {
        if (fadeObjects)
        {
            RaycastHit hit;
            Ray ray = new Ray(myTransform.position, (Target.position - myTransform.position).normalized);
            //ray.origin = myTransform.position;
            //ray.direction = (target.position - myTransform.position).normalized;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Transform objectHit = hit.transform;
                if (layersToTransparent.Contains(objectHit.gameObject.layer))
                {
                    if (preRender != null)
                    {
                        preRender.material.color = new Color(1, 1, 1, 1);
                    }
                    Renderer renderer = objectHit.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        preRender = renderer;
                        renderer.material.color = new Color(1, 1, 1, alpha);
                    }
                }
                else if (preRender != null)
                {
                    preRender.material.color = new Color(1, 1, 1, 1);
                    preRender = null;
                }
            }
        }
    }

}
