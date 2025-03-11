using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 touchPos;

    private bool updateTouch = false;

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
            Debug.Log(touchPos.x / screenWidth);
            Debug.Log(touchPos.y / screenHeight);
            updateTouch = false;
            var point3 = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10));
            TowerController.selectPoint(point3);
            Debug.Log(point3);
        }
    }

    public void ChangeAim(InputAction.CallbackContext context)
    {
        touchPos = context.ReadValue<Vector2>();
    }
    public void Stop(InputAction.CallbackContext context)
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
