using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;

public class PlayerController : MonoBehaviour
{
    private SwipeManager swipeManager;

   [SerializeField] private CharacterAnimator animator;
   [Space]
   [SerializeField] private float moveSpeed = .2f;

    private Transform mainCamera;

    private void Start()
    {
        swipeManager = SwipeManager.Instance;
        mainCamera = Camera.main.transform;

        swipeManager.onSwipe += DetectSwipeAction;
        swipeManager.onShortPress += DetectTapAction;
        swipeManager.onLongPress += DetectHoldAction;

        if (animator != null)
            swipeManager.onSwipe += animator.UpdateMovement;
    }

    private void DetectSwipeAction(SwipeAction swipeAction)
    {
        switch (swipeAction.direction)
        {
            case SwipeDirection.Up:
                break;

            case SwipeDirection.UpRight:
                break;

            case SwipeDirection.Right:
                break;

            case SwipeDirection.DownRight:
                break;

            case SwipeDirection.Down:
                break;

            case SwipeDirection.DownLeft:
                break;

            case SwipeDirection.Left:
                break;

            case SwipeDirection.UpLeft:
                break;
        }

        //Vector3 angleDir = playerPosition - mainCamera.position;
        //Vector3 angleForward = mainCamera.forward;

        //float angle = Vector3.Angle(angleDir, angleForward);

        // Find the players current position
        Vector3 playerPosition = transform.position;
        // Convert screen direction to transform local direction, then normalise to get direction
        Vector3 facingDirection = new Vector3(swipeAction.clampedDirection.x, 0, swipeAction.clampedDirection.y).normalized;
        // Rotate the vector by angle, the * by the original direction
        Vector3 rotatedVector = Quaternion.AngleAxis(45, Vector3.up) * facingDirection;
        // Set the final position = the original position + the rotated facing direction
        Vector3 finalPos = playerPosition + rotatedVector;

#if UNITY_EDITOR
        Vector3 debugVectorForward = Quaternion.AngleAxis(45, Vector3.up) * Vector3.forward;
        if (finalPos - transform.position == debugVectorForward)
            Debug.DrawLine(transform.position, finalPos, Color.red, 5);
        else
            Debug.DrawLine(transform.position, finalPos, Color.blue, 5);
#endif

        NNConstraint constraint = NNConstraint.None;

        // Constrain the search to walkable nodes only
        constraint.constrainWalkability = true;
        constraint.walkable = true;

        // Constrain the search to only nodes with tag 1 or tag 2
        constraint.constrainTags = true;
        constraint.tags = (1 << 1) | (1 << 2);

        NNInfo info = AstarPath.active.GetNearest(finalPos);
        GraphNode node = info.node;
        Vector3 nodePos = (Vector3)node.position;
        Vector3 finalNode = new Vector3(finalPos.x, nodePos.y, finalPos.z);

        //var dist = Vector3Int.Distance(new Vector3Int(node.position.x, node.position.y, node.position.z), new Vector3Int((int)finalPos.x, node.position.y, (int)finalPos.z)));

        if (nodePos == finalNode)
        {
            if (node.Walkable)
            {
                transform.DOMove(finalPos, moveSpeed, false);
                transform.DOLookAt(finalPos, 0.2f, AxisConstraint.Y, Vector2.up);
            }
            else
            {
                Debug.LogError("Shouldnt Walk Here!");
            }
        }

        //GameManager.Instance.UpdateWorldTurn();
    }

    private void DetectHoldAction(SwipeAction swipeAction)
    {

    }

    private void DetectTapAction(SwipeAction swipeAction)
    {

    }
}
