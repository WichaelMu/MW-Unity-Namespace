using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MW.Vector;

namespace MW.Behaviour
{
	/// <summary>The base class of a Player in a two-dimensional world. Extends Player.</summary>
	[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
	public class MPlayer2D : Player
	{
		[Header("2D Player Settings")]

		[SerializeField, Tooltip("Flips this Sprite Renderer over X when moving horizontally?")]
		/// <summary>Should the SpriteRenderer flip according to input?</summary>
		bool bFlipSpriteOnInput = false;
		[SerializeField, Tooltip("Is this sprite defaulted to facing -X?")]
		/// <summary>Does this SpriteRenderer 'face' the -X axis by default?</summary>
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

		/// <summary>The default implementation for movement input.</summary>
		/// <param name="UpwardThrow">Forward input. Default is Y axis.</param>
		/// <param name="ForwardThrow">Right input. Default is X axis.</param>
		public override void MovementInput(float UpwardThrow, float ForwardThrow)
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Velocity = new MVector(ForwardThrow, UpwardThrow);
			Velocity *= MovementSpeed;

			if (bFlipSpriteOnInput)
			{
				if (ForwardThrow != 0)
				{
					SpriteRenderer.flipX = bSpriteIsFacingNegativeX ? ForwardThrow > 0 : ForwardThrow < 0;
				}
			}
		}

		/// <summary>This Player's SpriteRenderer.</summary>
		/// <returns>The SpriteRenderer attached to this Player.</returns>
		public SpriteRenderer GetSpriteRenderer()
		{
			return SpriteRenderer;
		}

		/// <summary>This Player's Rigidbody2D.</summary>
		/// <returns>The Rigidbody2D attached to this Player.</returns>
		public Rigidbody2D GetRigidbody()
		{
			return Rigidbody;
		}
	}
}
