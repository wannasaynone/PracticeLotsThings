using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDetater2D {

    public struct InputInfo
    {
        public Vector3 InputPosition;
        public Transform RayCastTranform;
        public Collider2D RayCastCollider;
        public bool isOnUGUI;
    }

    public enum State
    {
        None,
        Down,
        Pressing,
        Up
    }

    public static State InputState
    {
        get
        {
            InpuDectat();
            return m_state;
        }
    }

    private static State m_state = State.None;
    public static int StartFingerID = -1;

    public InputDetater2D()
    {
        StartFingerID = -1;
    }

    public static InputInfo InpuDectat()
    {
#if UNITY_STANDALON || UNITY_EDITOR || UNITY_WEBGL
        return ComputerInput();
#elif UNITY_ANDROID
        return MobileInput();
#endif
    }

    private static InputInfo ComputerInput()
    {
        if (Input.GetMouseButtonDown(0))
            m_state = State.Down;
        else
        if (Input.GetMouseButton(0))
            m_state = State.Pressing;
        else
        if (Input.GetMouseButtonUp(0))
            m_state = State.Up;
        else
            m_state = State.None;

        InputInfo info = rayCastCheck();

        return info;
    }

    private static InputInfo MobileInput()
    {
        if (Input.touchCount > 0)
        {
            if (StartFingerID == -1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    m_state = State.Down;
                    StartFingerID = Input.GetTouch(0).fingerId;
                }
            }
            else
            {
                for (int i = 0; i < Input.touches.Length; i++)
                {
                    if (Input.touches[i].fingerId == StartFingerID)
                    {
                        if (Input.touches[i].phase == TouchPhase.Stationary || Input.touches[i].phase == TouchPhase.Moved)
                            m_state = State.Pressing;
                        else
                        if (Input.touches[i].phase == TouchPhase.Ended)
                            m_state = State.Up;
                    }
                }

            }
        }
        else
        {
            m_state = State.None;
            StartFingerID = -1;
        }

        InputInfo info = rayCastCheck();

        return info;
    }

    private static InputInfo rayCastCheck()
    {
        RaycastHit2D rayHit = new RaycastHit2D();
        InputInfo info = new InputInfo();
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        rayHit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (EventSystem.current == null)
            Debug.LogWarning("Unity EventSystem is not exist");
        else
        if (EventSystem.current.IsPointerOverGameObject())
        {
            info.isOnUGUI = true;
        }
        else
        {
            info.isOnUGUI = false;
        }
        info.InputPosition = mousePos;
#elif UNITY_ANDROID
        for (int i = 0; i < Input.touches.Length; i++)
        {
            if (Input.touches[i].fingerId == StartFingerID)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.touches[i].position.x, Input.touches[i].position.y));
                rayHit = Physics2D.Raycast(touchPos, Vector2.zero);
        
                 if(EventSystem.current.IsPointerOverGameObject(StartFingerID))
                 {
                    info.isOnUGUI = true;
                 }
                  else
                 {
                     info.isOnUGUI = false;
                 }
                info.InputPosition = touchPos;
            }
        }
#endif
        info.RayCastTranform = rayHit.transform;
        info.RayCastCollider = rayHit.collider;
        return info;
    }

}
