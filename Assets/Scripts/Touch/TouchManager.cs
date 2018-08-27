using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region TouchStructure
public class TouchStruct
{
    public Touch info;
    public bool isDown;
    public bool isPending;
    public float cooler;
    public Vector2 previousPosition;
    public Vector2 startPosition;
}
#endregion 

public class TouchManager : MonoBehaviour
{
    public static TouchManager instance = null;

    #region Manager Parameters
    public static int MAX_FINGER_COUNT = 5;
    public static int SWIPE_DISTANCE_DETECTION = 150;
    public static float DOUBLE_TOUCH_COOLER = 0f;
    #endregion

    #region Private Touch List
    static Dictionary<int, TouchStruct> touches = new Dictionary<int, TouchStruct>();
    #endregion

    #region Touch Events 
    public delegate void onStartTouch(TouchStruct touch);
    public event onStartTouch OnStartTouch;
    public delegate void onStayTouch(TouchStruct touch);
    public event onStayTouch OnStayTouch;
    public delegate void onMoveTouch(TouchStruct touch);
    public event onMoveTouch OnMoveTouch;
    public delegate void onDownTouch(TouchStruct touch);
    public event onDownTouch OnDownTouch;
    public delegate void onEndTouch(TouchStruct touch);
    public event onEndTouch OnEndTouch;
    public delegate void onDoubleTouch(TouchStruct touch);
    public event onDoubleTouch OnDoubleTouch;
    public delegate void onSwipe(TouchStruct touch, Vector2 direction);
    public event onSwipe OnSwipe;
    #endregion

    #region Singleton Initialization 
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    #endregion

    #region Touch Basic Function
    public static bool isTouching()
    {
        return (InputHelper.GetTouches().Count > 0);
    }
    #endregion

    #region Touchphase Dispatcher
    void GetTouches()
    {
        List<Touch> uTouch = InputHelper.GetTouches();
        for (var i = 0; i < uTouch.Count; i++)
        {
            if (touches.ContainsKey(uTouch[i].fingerId))
            {
                touches[uTouch[i].fingerId].info = uTouch[i];
                switch (uTouch[i].phase)
                {
                    case TouchPhase.Began:
                        StartTouch(uTouch[i], touches[uTouch[i].fingerId]);
                        break;
                    case TouchPhase.Stationary:
                        StayTouch(uTouch[i], touches[uTouch[i].fingerId]);
                        break;
                    case TouchPhase.Moved:
                        MoveTouch(uTouch[i], touches[uTouch[i].fingerId]);
                        break;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        EndTouch(uTouch[i], touches[uTouch[i].fingerId]);
                        break;
                }
                touches[uTouch[i].fingerId].previousPosition = uTouch[i].position;
            }
            else
            {
                NewFinger(uTouch[i], uTouch[i].fingerId);
            }
        }
    }
    #endregion

    #region Event Dispatcher
    void StartTouch(Touch touch, TouchStruct touchStruct)
    {
        touchStruct.startPosition = touch.position;
        if (touchStruct.isPending)
        {
            if(OnDoubleTouch != null)
                OnDoubleTouch(touchStruct);
        }
        else
        {
            if (OnStartTouch != null)
                OnStartTouch(touchStruct);
        }

        touchStruct.isDown = true;
    }

    void MoveTouch(Touch touch, TouchStruct touchStruct)
    {
        if (OnMoveTouch != null)
            OnMoveTouch(touchStruct);

        if (OnDownTouch != null)
            OnDownTouch(touchStruct);
    }

    void StayTouch(Touch touch, TouchStruct touchStruct)
    {
        if (OnStayTouch != null)
            OnStayTouch(touchStruct);

        if (OnDownTouch != null)
            OnDownTouch(touchStruct);
    }

    void EndTouch(Touch touch, TouchStruct touchStruct)
    {
        CheckIfIsSwipe(touch, touchStruct);

        if (!touchStruct.isDown)
            return;

        if (OnEndTouch != null)
            OnEndTouch(touchStruct);

        touchStruct.isDown = false;
        touchStruct.isPending = true;
        touchStruct.cooler = DOUBLE_TOUCH_COOLER;
    }

    void CheckIfIsSwipe(Touch touch, TouchStruct touchStruct)
    {
        Vector2 heading = touchStruct.startPosition - touchStruct.info.position;
        float distance = heading.magnitude;
        if (distance < SWIPE_DISTANCE_DETECTION)
            return;

        Vector2 direction = heading / distance;
        if (OnSwipe != null)
            OnSwipe(touchStruct, direction);
    }
    #endregion

    #region New Finger
    void NewFinger(Touch touch, int fingerID)
    {
        if(touches.Count < MAX_FINGER_COUNT)
        {
            touches.Add(fingerID, CreateTouchStructure(touch));
            StartTouch(touch, touches[fingerID]);
        }
    }

    TouchStruct CreateTouchStructure(Touch touch)
    {
        TouchStruct tStruct = new TouchStruct();
        tStruct.info = touch;
        tStruct.isDown = false;
        tStruct.isPending = false;
        tStruct.cooler = DOUBLE_TOUCH_COOLER;
        tStruct.previousPosition = touch.position;
        return tStruct;
    }
    #endregion

    #region Update
    void Update()
    {
        GetTouches();
        CooldownTouches();
    }
    #endregion

    #region Cooldown Handler
    void CooldownTouches()
    {
        List<int> keys = new List<int>(); 
            
        foreach (KeyValuePair<int, TouchStruct> item in touches)
        {
            if (item.Value.isDown)
                continue;

            item.Value.cooler -= Time.deltaTime;
            if(item.Value.cooler < 0)
            {
                keys.Add(item.Key);
            }
        }

        for(var i = 0; i < keys.Count; i++)
        {
            touches.Remove(keys[i]);
        }
    }
    #endregion
}
