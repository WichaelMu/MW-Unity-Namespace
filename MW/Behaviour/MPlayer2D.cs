﻿#if RELEASE
using MW.Diagnostics;
using UnityEngine;

namespace MW.Behaviour
{
	/// <summary>The base class of a Player in a two-dimensional world. Extends <see cref="PlayerBase"/>.</summary>
	/// <docs>The base class of a Player in a two-dimensional world. Extends PlayerBase.</docs>
	/// <decorations decor="[RequireComponent{Rigidbody2D}, {SpriteRenderer}] public class : PlayerBase"></decorations>
	[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
	public class MPlayer2D : PlayerBase
	{
		/// <summary>True to flip this Sprite Renderer over X when moving horizontally.</summary>
		/// <decorations decor="[SerializeField] bool"></decorations>
		[Header("2D Player Settings")]

		[SerializeField, Tooltip("Flips this Sprite Renderer over X when moving horizontally?")]
		bool bFlipSpriteOnInput = false;
		/// <summary>True if this sprite is defaulted to facing -X.</summary>
		/// <decorations decor="[SerializeField] bool"></decorations>
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

		/// <remarks>Must call base.FixedUpdate().</remarks>
		/// <decorations decor="public override void"></decorations>
		public override void FixedUpdate()
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Rigidbody.MovePosition(Position + (Time.fixedDeltaTime * Velocity));
		}

		/// <summary>The default implementation for <see cref="MovementInput(float, float)"/>.</summary>
		/// <docs>The default implementation for movement input.</docs>
		/// <decorations decor="public override void"></decorations>
		/// <param name="UpwardThrow">Upward input. Default is Y axis.</param>
		/// <param name="ForwardThrow">Forward input. Default is X axis.</param>
		public override void MovementInput(float UpwardThrow, float ForwardThrow)
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Velocity = new MVector(ForwardThrow, UpwardThrow);
			Velocity = MovementSpeed * Velocity;

			FlipOnInput(ForwardThrow);
		}

		/// <summary>Flips <see cref="GetSpriteRenderer"/> according to a <see cref="float"/>.</summary>
		/// <docs>Flips SpriteRenderer according to a float.</docs>
		/// <decorations decor="public void"></decorations>
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
		/// <decorations decor="public SpriteRenderer"></decorations>
		/// <returns>The SpriteRenderer attached to this Player.</returns>
		public SpriteRenderer GetSpriteRenderer()
		{
			return SpriteRenderer;
		}

		/// <summary>This Player's Rigidbody2D.</summary>
		/// <decorations decor="public Rigidbody2D"></decorations>
		/// <returns>The Rigidbody2D attached to this Player.</returns>
		public Rigidbody2D GetRigidbody()
		{
			return Rigidbody;
		}

		/// <summary>Performs a <see cref="Physics2D.Raycast(Vector2, Vector2, float, int)"/> under <see cref="Input.mousePosition"/>.</summary>
		/// <docs>Performs a raycast under the mouse.</docs>
		/// <remarks>ReferenceCamera must be orthographic, otherwise null is returned.</remarks>
		/// <decorations decor="public static RaycastHit2D?"></decorations>
		/// <param name="ReferenceCamera">The Camera to shoot a ray from, screen-wise.</param>
		/// <param name="Distance">The maximum distance of the ray to shoot.</param>
		/// <param name="LayerMask">The layers this raycast will intercept.</param>
		/// <docreturns>RaycastHit2D information about the raycast. Null if ReferenceCamera is not orthographic.</docreturns>
		/// <returns><see cref="RaycastHit2D"/> information about the raycast. Null if ReferenceCamera is not <see cref="Camera.orthographic"/>.</returns>
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
#endif // RELEASE
