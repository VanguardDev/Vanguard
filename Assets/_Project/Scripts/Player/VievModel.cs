using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//a class to set the vievmodel
namespace Vanguard
{
    public class VievModel : MonoBehaviour
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public void SetVievmodel(Transform GunModel)
        {
            GunModel.localPosition = Position;
            GunModel.localEulerAngles = Rotation;
        }
    }
}
