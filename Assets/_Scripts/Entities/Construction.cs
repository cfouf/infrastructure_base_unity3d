using System;
using System.Collections.Generic;
using _Scripts.Utilities;
using _Scripts.Utilities.EventBus;
using UnityEngine;

namespace _Scripts.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class Construction : MonoBehaviour
    {
        public List<Cube> cubes = new List<Cube>();
        public List<Collider> cubesColliders = new List<Collider>();
        public Rigidbody rb;
        public Center center;
        public int maxSpeed;


        public float spinRadius;

        public State state = State.Returning;

        private void RotateAroundCenter()
        {
            var tangentialSpeed = rb.velocity.magnitude;
            var centripetalForce = rb.mass * (tangentialSpeed * tangentialSpeed) / spinRadius;

            rb.AddForce(-transform.position.normalized * centripetalForce, ForceMode.Force);
        }

        private void ReturnToCenter()
        {
            var position = transform.position;
            rb.AddForce(-position.normalized * center.gravityAcceleration, ForceMode.Acceleration);
            var velocityDirection = rb.velocity.normalized;
            var positionDirection = position.normalized;

            if (Mathf.Approximately(Vector3.Dot(velocityDirection, positionDirection), 1.0f)) return;
            var angle = Vector3.Angle(velocityDirection, positionDirection);
            if (angle > 90)
                rb.velocity = -positionDirection * rb.velocity.magnitude;
            else
                rb.velocity = positionDirection * rb.velocity.magnitude;
        }

        private void GoAwayFromCenter()
        {
            var force = transform.position.normalized * (center.gravityAcceleration * 5);
            rb.AddForce(force.sqrMagnitude < maxSpeed * maxSpeed ? force : force.normalized * maxSpeed,
                ForceMode.VelocityChange);
            if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
                rb.velocity = rb.velocity.normalized * maxSpeed;
        }


        private void OnCollisionEnter(Collision other)
        {
            if (cubesColliders.Contains(other.collider)) return;
            if (other.gameObject == center.gameObject)
            {
                state = State.Orbiting;
                return;
            }

            state = State.GoingAway;
            EventBus<ConstructionsCollision>.Raise(new ConstructionsCollision(this, other));
        }

        public void Move()
        {
            if (state == State.Orbiting &&
                Vector3.Distance(transform.position, center.transform.position) > spinRadius / 5)
                state = State.Returning;

            switch (state)
            {
                case State.Returning:
                    ReturnToCenter();
                    break;
                case State.Orbiting:
                    RotateAroundCenter();
                    break;
                case State.GoingAway:
                    GoAwayFromCenter();
                    state = State.Returning;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}