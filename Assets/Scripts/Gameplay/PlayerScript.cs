using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {


    #region GesturesManagement

    [Header("Tweaks")]
    [SerializeField] float deadzone = 100.0f;
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

        if (swipeDelta.sqrMagnitude > sqrDeadzone)
        {

            isTap = false;

            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                if (y < 0)
                    SwipeDown = true;
                else
                    swipeUp = true;
            }
            startTouch = swipeDelta = Vector2.zero;
        }
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
                print(Time.time - lastTap);
                lastTap = Time.time;
                Touching = true;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
                Touching = false;
                EndTap = true;
                if (isTap) {
                    Tap = true;
                    isTap = false;
                }
            }

            //Reset distance, get the new swipeDelta
            swipeDelta = Vector2.zero;

            if (Touching) {
                Touchlocal = Input.mousePosition;
            }

            if (Touching && Time.time - lastTap > longTapDelta && isTap)
            {
                longTap = true;
                isTap = false;
            }

            if (startTouch != Vector2.zero && Touching)
                swipeDelta = (Vector2)Input.mousePosition - startTouch;

            if (swipeDelta.sqrMagnitude > sqrDeadzone)
            {

                isTap = true;

                float x = swipeDelta.x;
                float y = swipeDelta.y;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x < 0)
                        swipeLeft = true;
                    else
                        swipeRight = true;
                }
                else
                {
                    if (y < 0)
                        SwipeDown = true;
                    else
                        swipeUp = true;
                }
                startTouch = swipeDelta = Vector2.zero;
            }
        }
    }

    Vector2 ScreentoScale;

    Animator Anim;


    public Vector2 taplocal;
    // Start is called before the first frame update
    void Start(){
        //So we can use .sqrMagnitude, instead of .magnitude
        sqrDeadzone = deadzone * deadzone;

        Anim = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        //ScreenManagement
        ScreentoScale = new Vector2(100f / Screen.width, 100f / Screen.height);
        GesturesExecution();


        if (Tap) {
            if (taplocal.y < 50 / ScreentoScale.y) {
                if (taplocal.x < 50 / ScreentoScale.x)
                {
                    Anim.SetTrigger("BottomLeft");
                }
                else {
                    Anim.SetTrigger("BottomRight");
                }
            }
            else
            {
                if (taplocal.x < 50 / ScreentoScale.x)
                {
                    Anim.SetTrigger("UpLeft");
                }
                else
                {
                    Anim.SetTrigger("UpRight");
                }
            }
        }

        if (longTap) {
            if (taplocal.y < 50 / ScreentoScale.y)
            {
                if (taplocal.x < 50 / ScreentoScale.x)
                {
                    Anim.SetTrigger("HardBottomLeft");
                }
                else
                {
                    Anim.SetTrigger("HardBottomRight");
                }
            }
            else
            {
                if (taplocal.x < 50 / ScreentoScale.x)
                {
                    Anim.SetTrigger("HardUpLeft");
                }
                else
                {
                    Anim.SetTrigger("HardUpRight");
                }
            }
        }

        if (swipeLeft) {
            Anim.SetTrigger("DodgeLeft");
        }
        if (swipeRight) {
            Anim.SetTrigger("DodgeRight");
        }

        if (Touching)
        {
            if (swipeUp || SwipeDown)
            {
                Sliding = true;
            }
            if (Sliding)
            {
                if (Touchlocal.y < 50 / ScreentoScale.y)
                {
                    Anim.SetFloat("InputY", -1);
                }
                else
                {
                    Anim.SetFloat("InputY", 1);
                }
            }
        }
        else {
            Sliding = false;
        }
        Anim.SetBool("Blocking", Sliding);

    }
}
