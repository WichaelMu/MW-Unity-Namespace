using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MW.Diagnostics;

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

		/// <remarks>Must call base.FixedUpdate().</remarks>
		public virtual void FixedUpdate()
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Rigidbody.MovePosition(Position + Velocity * Time.fixedDeltaTime);
		}

		/// <summary>The default implementation for movement input.</summary>
		/// <param name="UpwardThrow">Upward input. Default is Y axis.</param>
		/// <param name="ForwardThrow">Forward input. Default is X axis.</param>
		public override void MovementInput(float UpwardThrow, float ForwardThrow)
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Velocity = new MVector(ForwardThrow, UpwardThrow);
			Velocity *= MovementSpeed;

			FlipOnInput(ForwardThrow);
		}

		/// <summary>Flips SpriteRenderer according to a float.</summary>
		/// <remarks>bFlipOnSpriteInput needs to be true to execute.</remarks>
		/// <param name="FlipIndependentInput">The input throw to determine a flip of the Sprite Renderer.</param>
		public void FlipOnInput(float FlipIndependentInput)
		{
			if (bFlipSpriteOnInput)
			{
				if (FlipIndependentInput != 0)
				{
					SpriteRenderer.flipX = bSpriteIsFacingNegativeX ? FlipIndependentInput > 0 : FlipIndependentInput < 0;
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

		/// <summary>Performs a raycast under the mouse.</summary>
		/// <remarks>ReferenceCamera must be orthographic, otherwise null is returned.</remarks>
		/// <param name="ReferenceCamera">The Camera to shoot a ray from, screen-wise.</param>
		/// <param name="Distance">The maximum distance of the ray to shoot.</param>
		/// <param name="LayerMask">The layers this raycast will intercept.</param>
		/// <returns>RaycastHit2D information about the raycast. Null if ReferenceCamera is not orthographic.</returns>
		public static RaycastHit2D? RayUnderMouse(Camera ReferenceCamera, float Distance, int LayerMask)
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
