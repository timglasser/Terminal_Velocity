
// To the extent possible under law, 
// Tim Glasser (tim_glasser@hotmail.com)     https://www.facebook.com/tim.glasser.75 
// has waived all copyright and related or neighboring rights and responsibilties to
// MovementMotor C# Classes. This work is published from California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice
using UnityEngine;

// Vehicle base attributes for a controlled moving object
public class MovementMotor : MonoBehaviour
{
//    [HideInInspector]
    protected Vector3 movementDirection;
//    [HideInInspector]
    protected Vector3 movementVel;
    //    [HideInInspector]
    protected Vector3 facingDirection;

    public Vector3 MovementDirection
    {
        get { return movementDirection; }
        set { movementDirection = value; }
    }

    public Vector3 MovementVel
    {
        get { return movementVel; }
        set { movementVel = value; }
    }

    public Vector3 FacingDirection
    {
        get { return facingDirection; }
        set { facingDirection = value; }
    }

    protected static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);

        float angle = Vector3.Angle(dirA, dirB);

        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    }
}
