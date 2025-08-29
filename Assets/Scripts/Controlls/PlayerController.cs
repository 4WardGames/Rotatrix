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

    [SerializeField]
    private GameObject rotationArrow;
    private GameObject reverseArrows;

    // Start is called before the first frame update
    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        rotationArrow = Instantiate((GameObject)Resources.Load("Textures/Arrow_Rotate"));
        reverseArrows = Instantiate((GameObject)Resources.Load("Textures/Arrow_UpDown"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotationArrow.SetActive(false);
        reverseArrows.SetActive(false);
        TowerController.highlightedBlocks = HighlightedBlocks.None;

        if ((touchPos - originalTouchPos).magnitude > 200)
        {
            TowerController.highlightedBlocks = HighlightedBlocks.Top;
            if (Math.Abs(touchPos.x - originalTouchPos.x) > Math.Abs(touchPos.y - originalTouchPos.y))
            {
                if (touchPos.x > originalTouchPos.x)
                {
                    rotationArrow.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 5));
                    rotationArrow.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                else
                {
                    rotationArrow.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 5));

                    rotationArrow.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    TowerController.highlightedBlocks = HighlightedBlocks.Bottom;
                }
                rotationArrow.SetActive(true);
            }
            else
            {
                if (touchPos.y > originalTouchPos.y)
                {
                    reverseArrows.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 5));
                    TowerController.highlightedBlocks = HighlightedBlocks.Bottom;
                }
                else
                {
                    reverseArrows.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 5));
                }

                reverseArrows.SetActive(true);
            }
        }


        if (updateTouch)
        {
            updateTouch = false;
            rotationArrow.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10));

            if ((touchPos - originalTouchPos).magnitude > 200)
            {
                if (Math.Abs(touchPos.x - originalTouchPos.x) > Math.Abs(touchPos.y - originalTouchPos.y))
                {
                    if (touchPos.x > originalTouchPos.x)
                    {
                        TowerController.rotateDown = false;
                    }
                    else
                    {
                        TowerController.rotateDown = true;
                    }

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
            TowerController.ResetPoint();
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
