using System;

namespace MW.ConsoleDebugger
{
	/// <summary>The attribute to mark a method executable by <see cref="Console.Exec(string, object[])"/>.</summary>
	/// <docs>The attribute to mark a method executable by Console.Exec().</docs>
	/// <decorations decor="public class : Attriubte"></decorations>
	[AttributeUsage(AttributeTargets.Method)]
	public class ExecAttribute : Attribute
	{
		/// <summary>The description of this method.</summary>
		/// <decorations decor="public string"></decorations>
		public string Description;
		/// <summary><see langword="true"/> if this method should <see cref="Console.Exec(string, object[])"/> on Start.</summary>
		/// <docs>True if this method should Exec() on Start.</docs>
		/// <decorations decor="public bool"></decorations>
		public bool bExecOnStart;
		/// <summary><see langword="true"/> if this method should not appear in <see cref="Console.OnGUI"/>.</summary>
		/// <docs>True if this method should not appear in the Console GUI.</docs>
		/// <decorations decor="public bool"></decorations>
		public bool bHideInConsole;
		/// <summary>The parameters to automatically execute when <see cref="bExecOnStart"/> is <see langword="true"/>.</summary>
		/// <docs>The parameters to automatically execute when bExecOnStart is true.</docs>
		/// <decorations decor="public object[]"></decorations>
		public object[] ExecParams;

		public ExecAttribute() { Description = string.Empty; }

		public ExecAttribute(string Desc) { Description = Desc; }

		public ExecAttribute(string Desc, bool bOnStart, params object[] Params) : this(Desc) { bExecOnStart = bOnStart; ExecParams = Params; }

		public ExecAttribute(bool bIsTestExec) : this(string.Empty, true) { bHideInConsole = bIsTestExec; }
	}

	internal struct MethodExec<T1, T2>
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