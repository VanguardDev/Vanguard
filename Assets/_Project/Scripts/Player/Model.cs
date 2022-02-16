using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public Vector3 fpspos,fpsscale,fpsrot;
    public Vector3 tpspos, tpsscale, tpsrot;
    public Vector3 GroundScale;
    //mode 0 fps 1 tps 2 Ground
    public void setModel(int mode)
    {
        if (mode ==0)//sets the position based on how it should be seen
        {
            transform.localPosition = fpspos;
            transform.localScale = fpsscale;
            transform.localEulerAngles = fpsrot;
        }
        else if(mode==1)
        {
            transform.localPosition = tpspos;
            transform.localScale = tpsscale;
            transform.localEulerAngles = tpsrot;
        }
        else if(mode == 2)
        {
            transform.localScale = GroundScale;
        }
    }
}
