using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MW.Vector;

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
	}
}
