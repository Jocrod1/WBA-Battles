using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : Character {


    #region GesturesManagement

    [Header("Tweaks")]
    [SerializeField] Vector2 deadzone;
    [SerializeField] float doubleTapDelta = 0.5f;
    [SerializeField] float longTapDelta = 0.5f;

    [Header("Logic")]
    public bool BeginTap;
    public bool Tap;
    public bool EndTap;
    public bool doubleTap;
    public bool swipeLeft;
    public bool swipeRight;
    public bool swipeUp;
    public bool SwipeDown;
    public Vector2 swipeDelta;
    Vector2 startTouch;
    float lastTap;
    float sqrDeadzone;
    public bool isTap = false;
    public bool Touching = false;
    public Vector2 Touchlocal;
    public bool longTap;

    public bool Sliding;

    void GesturesExecution()
    {
        //Reset Bools
        BeginTap = Tap = EndTap = doubleTap = swipeLeft = swipeRight = swipeUp = SwipeDown = longTap = false;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            UpdateStandalone();
        }
        else
        {
            UpdateMobile();
        }
    }
    #endregion

    void UpdateStandalone()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginTap = true;
            isTap = true;
            startTouch = Input.mousePosition;
            doubleTap = Time.time - lastTap < doubleTapDelta;
            lastTap = Time.time;
            taplocal = startTouch;
            Touching = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
            Touching = false;
            EndTap = true;
            if (isTap){
                Tap = true;
                isTap = false;
            }
        }

        //Reset distance, get the new swipeDelta
        swipeDelta = Vector2.zero;

        if (Touching) {
            Touchlocal = Input.mousePosition;
        }

        if (Touching && Time.time - lastTap > longTapDelta && isTap) {
            longTap = true;
            isTap = false;
        }

        if (startTouch != Vector2.zero && Touching)
            swipeDelta = (Vector2)Input.mousePosition - startTouch;

        if (Mathf.Abs(swipeDelta.x) > deadzone.x) {
            isTap = false;

            if (swipeDelta.x < 0)
                swipeLeft = true;
            else
                swipeRight = true;

            startTouch = new Vector2(Input.mousePosition.x, startTouch.y);
        }
        if (Mathf.Abs(swipeDelta.y) > deadzone.y) {
            isTap = false;

            if (swipeDelta.y < 0)
                SwipeDown = true;
            else
                swipeUp = true;
        }

        //if (swipeDelta.sqrMagnitude > sqrDeadzone)
        //{

        //    isTap = false;

        //    float x = swipeDelta.x;
        //    float y = swipeDelta.y;

        //    if (Mathf.Abs(x) > Mathf.Abs(y))
        //    {
                
        //    }
        //    else
        //    {
                
        //    }
        //    startTouch = swipeDelta = Vector2.zero;
        //}
    }

    void UpdateMobile()
    {

        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                BeginTap = true;
                isTap = true;
                startTouch = Input.mousePosition;
                doubleTap = Time.time - lastTap < doubleTapDelta;
                lastTap = Time.time;
                taplocal = startTouch;
                Touching = true;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
                Touching = false;
                EndTap = true;
                if (isTap)
                {
                    Tap = true;
                    isTap = false;
                }
            }

            //Reset distance, get the new swipeDelta
            swipeDelta = Vector2.zero;

            if (Touching)
            {
                Touchlocal = Input.mousePosition;
            }

            if (Touching && Time.time - lastTap > longTapDelta && isTap)
            {
                longTap = true;
                isTap = false;
            }

            if (startTouch != Vector2.zero && Touching)
                swipeDelta = (Vector2)Input.mousePosition - startTouch;

            if (Mathf.Abs(swipeDelta.x) > deadzone.x)
            {
                isTap = false;

                if (swipeDelta.x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;

                startTouch = new Vector2(Input.mousePosition.x, startTouch.y);
            }
            if (Mathf.Abs(swipeDelta.y) > deadzone.y)
            {
                isTap = false;

                if (swipeDelta.y < 0)
                    SwipeDown = true;
                else
                    swipeUp = true;
            }
        }
    }

    Vector2 ScreentoScale;


    public Vector2 taplocal;

    public bool Wait;

    


    // Start is called before the first frame update
    public override void LoadData(){
        base.LoadData();
        //So we can use .sqrMagnitude, instead of .magnitude
        //sqrDeadzone = deadzone * deadzone;
    }

    // Update is called once per frame
    public override void UpdateThis()
    {
        base.UpdateThis();
        

        //ScreenManagement
        ScreentoScale = new Vector2(100f / Screen.width, 100f / Screen.height);
        GesturesExecution();

        if (Wait)
            return;

        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        bool IdleState = stateinfo.IsName("BlockV");


        if (Tap && IdleState) {
            if (taplocal.y < 50 / ScreentoScale.y) {
                if (taplocal.x < 50 / ScreentoScale.x)
                {
                    anim.SetTrigger("BottomLeft");
                    info = PIBottom;
                    info.PunchRawLocal = bottom + left;

                }
                else {
                    anim.SetTrigger("BottomRight");
                    info = PIBottom;
                    info.PunchRawLocal = bottom + right;
                }
            }
            else
            {
                if (taplocal.x < 50 / ScreentoScale.x)
                {
                    anim.SetTrigger("UpLeft");
                    info = PIUp;
                    info.PunchRawLocal = up + left;
                }
                else
                {
                    anim.SetTrigger("UpRight");
                    info = PIUp;
                    info.PunchRawLocal = up + right;
                }
            }
        }

        if (longTap && IdleState) {
            if (taplocal.y < 50 / ScreentoScale.y)
            {
                if (taplocal.x < 50 / ScreentoScale.x)
                {
                    anim.SetTrigger("HardBottomLeft");
                    info = PIHardBottom;
                    info.PunchRawLocal = bottom + left;
                }
                else
                {
                    anim.SetTrigger("HardBottomRight");
                    info = PIHardBottom;
                    info.PunchRawLocal = bottom + right;
                }
            }
            else
            {
                if (taplocal.x < 50 / ScreentoScale.x)
                {
                    anim.SetTrigger("HardUpLeft");
                    info = PIHardUp;
                    info.PunchRawLocal = up + left;
                }
                else
                {
                    anim.SetTrigger("HardUpRight");
                    info = PIHardUp;
                    info.PunchRawLocal = up + right;
                }
            }
        }

        if (swipeLeft && IdleState) {
            anim.SetTrigger("DodgeLeft");
        }
        if (swipeRight && IdleState) {
            anim.SetTrigger("DodgeRight");
        }

        if (Touching)
        {
            if ((swipeUp || SwipeDown))
            {
                Sliding = true;
            }
            if (Sliding)
            {
                if (Touchlocal.y < 50 / ScreentoScale.y)
                {
                    anim.SetFloat("InputY", -1);
                }
                else
                {
                    anim.SetFloat("InputY", 1);
                }
            }
        }
        else {
            Sliding = false;
            anim.SetFloat("InputY", 0);
        }
        //anim.SetBool("Blocking", Sliding);

    }


}
