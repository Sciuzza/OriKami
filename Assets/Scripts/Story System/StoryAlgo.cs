using System.Collections.Generic;

using UnityEngine;


#region Player Effect Classes

[System.Serializable]
public class PlayerEffect
{
    public PlayerReposition PlayerRepositionEffect;

    public PlayerMove PlayerMoveEffect;
}

[System.Serializable]
public class PlayerReposition
{
    public GameObject GbRef;
}

[System.Serializable]
public class PlayerMove
{
    public GameObject GbRef;

    public float LerpSpeed;
}

#endregion Player Effect Classes

#region Camera Effect Classes

[System.Serializable]
public class CameraEffect
{
    public CameraMove CameraMoveEffect;

}

[System.Serializable]
public class CameraMove
{
    public GameObject GbRef;
    public float LerpSpeed;
}



#endregion Player Effect Classes

#region Environment Npc Effects

[System.Serializable]
public class EnvEffect
{
    public ObjectActivation ObjActiEffect;

    public ObjectMoving ObjMovEffect;

    public ObjectAnimation ObjAniEffect;
}

[System.Serializable]
public class ObjectActivation
{
    public GameObject GbRef;
}

[System.Serializable]
public class ObjectMoving
{
    public GameObject GbRef;
    public float LerpSpeed;
}

public class ObjectAnimation
{
  
}
#endregion

#region Story Element

[System.Serializable]
public class StoryElement
{
    public PlayerEffect PlayerEffects;

    public CameraEffect CameraEffects;

    public List<EnvEffect> EnvEffects;
}

#endregion

public class StoryAlgo : MonoBehaviour
{


    public StoryElement[] StoryParts;
}