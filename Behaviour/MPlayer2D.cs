using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MW.Vector;
using MW.Diagnostics;

namespace MW.Behaviour
{
	[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
	public class MPlayer2D : Player
	{
		[Header("2D Player Settings")]
		[SerializeField, Tooltip("Flips this Sprite Renderer over X when moving horizontally?")]
		bool bFlipSpriteOnInput = false;
		[SerializeField, Tooltip("Is this sprite defaulted to facing -X?")]
		bool bSpriteIsFacingNegativeX = false;

		Rigidbody2D Rigidbody;
		SpriteRenderer SpriteRenderer;

		public override void Awake()
		{
			InitialisePlayer();

			Rigidbody = GetComponent<Rigidbody2D>();
			SpriteRenderer = GetComponent<SpriteRenderer>();
		}

		public virtual void FixedUpdate()
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Rigidbody.MovePosition(Position + Velocity * Time.fixedDeltaTime);
		}

		public override void MovementInput(float ForwardThrow, float RightThrow)
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Velocity = new MVector(RightThrow, ForwardThrow);
			Velocity *= MovementSpeed;

			if (bFlipSpriteOnInput)
			{
				if (RightThrow != 0)
				{
					SpriteRenderer.flipX = bSpriteIsFacingNegativeX ? RightThrow > 0 : RightThrow < 0;
				}
			}
		}

		public SpriteRenderer GetSpriteRenderer()
		{
			return SpriteRenderer;
		}

		public Rigidbody2D GetRigidbody()
		{
			return Rigidbody;
		}

		public RaycastHit2D? RayUnderMouse(Camera ReferenceCamera, float Distance, int LayerMask)
		{
			if (!ReferenceCamera.orthographic)
			{
				Log.E(ReferenceCamera.name, "is not orthographic.");

				return null;
			}

			Vector3 MouseScreenCoordinates = Input.mousePosition;
			MouseScreenCoordinates += new Vector3(0, 0, ReferenceCamera.transform.position.z);
			Vector3 WorldMouseCoordinates = ReferenceCamera.ScreenToWorldPoint(MouseScreenCoordinates);

			return Physics2D.Raycast(WorldMouseCoordinates, Vector3.forward, Distance, LayerMask);
		}
	}
}
