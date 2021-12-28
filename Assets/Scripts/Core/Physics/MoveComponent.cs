using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveComponent: MonoBehaviour
    {
        public Vector3 moveInputThisFrame;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed = 5;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            rb.velocity = speed * moveInputThisFrame ;
            moveInputThisFrame = Vector3.zero;
        }
    }
}