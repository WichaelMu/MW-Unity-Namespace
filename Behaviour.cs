using UnityEngine;
using MW.Vector;

namespace MW.Behaviour
{
	public class MBehaviour : MonoBehaviour
	{
		public MVector Position { get => transform.position; set { transform.position = value; } }
	}
}
