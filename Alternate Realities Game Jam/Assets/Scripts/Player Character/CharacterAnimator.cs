using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    public void UpdateMovement(SwipeAction swipeAction)
    {
        Anim.SetTrigger("Run");
    }
}
