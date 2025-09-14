using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public enum Mode
    {
        Menu,
        Level,

    }

    public Mode mode;


    void ChangeMode(Mode _mode)
    {
        mode = _mode;
        if(mode == Mode.Menu)
        {
            this.transform.rotation = Quaternion.Euler(20, 180, 0);
        }
    }

    void FixedUpdate()
    {
        if (mode == Mode.Menu)
        {
            transform.rotation *= Quaternion.Euler(.2f, 0, 0);
        }


    }
}
