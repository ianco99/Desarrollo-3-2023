using UnityEngine;

public interface IPatrolSettings 
{
    public float patrolSpeed { get => patrolSpeed; set => patrolSpeed = value; }
    public float patrolAcceleration { get => patrolAcceleration; set => patrolAcceleration = value; }


    public float groundCheckDistance { get => groundCheckDistance; set => groundCheckDistance = value; }
    public float wallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask wallLayer { get => wallLayer; set => wallLayer = value; }
    public LayerMask groundLayer { get => groundLayer; set => groundLayer = value; }
}
