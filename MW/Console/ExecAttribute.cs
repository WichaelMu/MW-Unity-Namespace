using System;

namespace MW.Console
{
	/// <summary>The attribute to mark a method executable by <see cref="MConsole.Exec(string[], string, object[])"/>.</summary>
	/// <docs>The attribute to mark a method executable by Console.Exec().</docs>
	/// <decorations decor="public class : Attriubte"></decorations>
	[AttributeUsage(AttributeTargets.Method)]
	public class ExecAttribute : Attribute
	{
		/// <summary>The name of the GameObject to Target this Executable to.</summary>
		/// <decorations decor="public string[]"></decorations>
		public string[] GameObjectTargetsByName;
		/// <summary>The description of this method.</summary>
		/// <decorations decor="public string"></decorations>
		public string Description;
		/// <summary><see langword="true"/> if this method should <see cref="MConsole.Exec(string[], string, object[])"/> on Start.</summary>
		/// <docs>True if this method should Exec() on Start.</docs>
		/// <decorations decor="public bool"></decorations>
		public bool bExecOnAwake;
		/// <summary><see langword="true"/> if this method should not appear in <see cref="MConsole.OnGUI"/>.</summary>
		/// <docs>True if this method should not appear in the Console GUI.</docs>
		/// <decorations decor="public bool"></decorations>
		public bool bHideInConsole;
		/// <summary><see langword="true"/> if this method requires a target to be executed.</summary>
		/// <docs>True if this method requires a target to be executed.</docs>
		/// <decorations decor="public bool"></decorations>
		public bool bRequireTarget;
		/// <summary>The parameters to automatically execute when <see cref="bExecOnAwake"/> is <see langword="true"/>.</summary>
		/// <docs>The parameters to automatically execute when bExecOnAwake is true.</docs>
		/// <decorations decor="public object[]"></decorations>
		public object[] ExecParams;

		internal bool bIsBuiltIn;

		/// <summary>Default Exec.</summary>
		public ExecAttribute() { Description = string.Empty; GameObjectTargetsByName = Array.Empty<string>(); }

		/// <summary>Exec specifying if a Target is Required and a Description.</summary>
		/// <param name="bTargetRequired">True if a Target is Required to Exec.</param>
		/// <param name="Desc">The Description.</param>
		public ExecAttribute(bool bTargetRequired, string Desc) : this(Desc) { bRequireTarget = bTargetRequired; }

		/// <summary>Exec with a Description.</summary>
		/// <param name="Desc">The Description.</param>
		public ExecAttribute(string Desc) { Description = Desc; }

		/// <summary>Exec with a Description and a Target.</summary>
		/// <param name="Desc">The Description.</param>
		/// <param name="Targets">The names of the GameObjects to Target call this Exec on.</param>
		public ExecAttribute(string Desc, params string[] Targets) { Description = Desc; GameObjectTargetsByName = Targets; }

		/// <summary>Exec that can be called from during Awake() with custom parameters.</summary>
		/// <remarks>If bOnAwake is false, this acts as a normal Exec(Desc), and Params will be ignored.</remarks>
		/// <param name="Desc">The Description.</param>
		/// <param name="bOnAwake">If this method should be executed on start.</param>
		/// <param name="Params">The Parameters to execute with bOnAwake.</param>
		public ExecAttribute(string Desc, bool bOnAwake, params object[] Params) : this(Desc) { bExecOnAwake = bOnAwake; ExecParams = Params; }
	}

	[AttributeUsage(AttributeTargets.Method)]
	internal class BuiltInExecAttribute : ExecAttribute
	{
		internal BuiltInExecAttribute(string Desc) : base(Desc) { bIsBuiltIn = true; }
	}

	public struct MethodExec<T1, T2>
	{
		public T1 Method;
		public T2 Exec;

		public MethodExec(T1 M, T2 E)
		{
			Method = M;
			Exec = E;
		}
	}
}