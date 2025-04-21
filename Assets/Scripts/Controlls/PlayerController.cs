using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 touchPos;
    private Vector2 originalTouchPos;

    private bool updateTouch = false;

    private bool validBlockSelected = false;

    [SerializeField]
    private TowerController TowerController;

    private float screenWidth;
    private float screenHeight;

    // Start is called before the first frame update
    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (updateTouch)
        {
            updateTouch = false;
            if ((touchPos - originalTouchPos).magnitude > 50)
            {
                if (Math.Abs(touchPos.x - originalTouchPos.x) > Math.Abs(touchPos.y - originalTouchPos.y))
                {
                    TowerController.Rotate(0);
                }
                else
                {
                    if (touchPos.y > originalTouchPos.y)
                    {
                        TowerController.reverseDown = true;
                    }
                    else
                    {
                        TowerController.reverseDown = false;
                    }

                    TowerController.Reverse(0);
                }
            }
            touchPos = Vector2.zero;
            originalTouchPos = Vector2.zero;
        }
    }

    public void ChangeAim(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        if (touchPos == Vector2.zero)
        {
            originalTouchPos = context.ReadValue<Vector2>();
            var point3 = Camera.main.ScreenToWorldPoint(new Vector3(originalTouchPos.x, originalTouchPos.y, 10));
            validBlockSelected = TowerController.SelectPoint(point3);
        }

        if (validBlockSelected)
        {
            touchPos = context.ReadValue<Vector2>();
        }
        else
        {
            touchPos = Vector2.zero;
        }
    }
    public void Stop(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        if (validBlockSelected)
        {
            var value = context.ReadValue<UnityEngine.InputSystem.TouchPhase>();
            if (value.ToString() == "Ended")
            {
                updateTouch = true;
            }
            else
            {
                updateTouch = false;
            }
        }
    }
}
