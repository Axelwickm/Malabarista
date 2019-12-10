//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: This object will get hover events and can be attached to the hands
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	public class Interactable : MonoBehaviour
	{
        public Limit_Velocity script;
        public GameController debugScript;
        public Hand Hand1;
        public Hand Hand2;
        private VelocityEstimator velocityEstimator;
        private float lastHeld;
    

        void RumbleController(float duration, float strength, Hand hand)
        {
            StartCoroutine(RumbleControllerRoutine(duration, strength, hand));
        }

        IEnumerator RumbleControllerRoutine(float duration, float strength, Hand hand)
        {
            strength = Mathf.Clamp01(strength);
            float startTime = Time.realtimeSinceStartup;

            while (Time.realtimeSinceStartup - startTime <= duration)
            {
                int valveStrength = Mathf.RoundToInt(Mathf.Lerp(0, 3999, strength));

                hand.controller.TriggerHapticPulse((ushort)valveStrength);

                yield return null;
            }
        }

        void Start()
        {
            lastHeld = Time.time;
            velocityEstimator = GetComponent<VelocityEstimator>();
            script = GetComponent<Limit_Velocity>();
        }


        public delegate void OnAttachedToHandDelegate( Hand hand );
		public delegate void OnDetachedFromHandDelegate( Hand hand );

		[HideInInspector]
		public event OnAttachedToHandDelegate onAttachedToHand;
		[HideInInspector]
		public event OnDetachedFromHandDelegate onDetachedFromHand;

		//-------------------------------------------------
		private void OnAttachedToHand( Hand hand )
		{
            float points = Time.time - lastHeld;
            Player playerInstance = Player.instance;
            GameController gc = playerInstance.gameObject.GetComponent<GameController>();
            gc.AddPoints(points);

            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().detectCollisions = true;

            if ( onAttachedToHand != null )
			{
				onAttachedToHand.Invoke( hand );
			}

            if (hand.controller == null)
            {
                velocityEstimator.BeginEstimatingVelocity();
            }
            RumbleController(0.12f, 1500, hand);
        }


        private void OnCollisionEnter(Collision collision)
        {
            //if (collision.gameObject.name == "Hand1") 
            //{
            //    RumbleController(0.2f, 1500, Hand1);
            //}
            //if (collision.gameObject.name == "Hand2")
            //{
            //    RumbleController(0.2f, 1500, Hand2);
            //}
        }

        void OnCollisionStay(Collision collision)
        {
            lastHeld = Time.time;
            if (collision.gameObject.name == "ball_plane")
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                
            }
        }

        private void OnCollisionExit(Collision collision)
        {
        }


        //-------------------------------------------------
        private void OnDetachedFromHand( Hand hand )
		{
            lastHeld = Time.time;
            if ( onDetachedFromHand != null )
			{
				onDetachedFromHand.Invoke( hand );

			}

            Vector3 aux = Player.instance.trackingOriginTransform.TransformVector(hand.controller.velocity);
        
            Debug.Log("WE ARE DETACHED FROM LIFE AND LOVE!");
            if (hand.GetInstanceID() == Hand1.GetInstanceID())
            {
                script.startPoint = Hand1.transform.position;
                script.endPoint = Hand2.transform.position;
                script.heightVector = aux;

                if(2.0f * aux.y > Mathf.Sqrt(aux.x * aux.x + aux.z * aux.z) && aux.y > 0.03f)
                {
                    script.followTrajectory = true;
                }
                else
                {
                    script.followTrajectory = false;
                }
                
            }

            if (hand.GetInstanceID() == Hand2.GetInstanceID())
            {
                script.startPoint = Hand2.transform.position;
                script.endPoint = Hand1.transform.position;
                script.heightVector = aux;

                if (2.0f * aux.y > Mathf.Sqrt(aux.x * aux.x + aux.z * aux.z) && aux.y > 0.03f)
                {
                    script.followTrajectory = true;
                }
                else
                {
                    script.followTrajectory = false;
                }
            }

            script.count = 0.0f;

            if (hand.GuessCurrentHandType() == Hand.HandType.Right)
            {
                
            }

            if (hand.GuessCurrentHandType() == Hand.HandType.Left)
            {

            }
		}
	}
}
