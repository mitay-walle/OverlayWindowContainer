	/// <summary>
	/// Allow to draw stacked floating <see cref="EditorWindow"/> in <see cref="SceneView"/>, similar to <see cref="ParticleSystem"/>
	/// <br/><br/>HOW TO:
	/// <br/>create instance of <see cref="OverlayWindowContainer"/>
	/// <br/>call <see cref="OverlayWindowContainer.OnGUI"/> from any your OnGUI function
	/// </summary>
	public class OverlayWindowContainer
	{
		private object Window;
		private MethodInfo _onGUIMethod;

		public delegate void WindowFunction(Object target, SceneView sceneView);

		public void OnGUI() => _onGUIMethod.Invoke(null, new[] { Window });

		public OverlayWindowContainer(
			GUIContent title,
			WindowFunction guiFunction,
			int primaryOrder,
			UnityEngine.Object target,
			WindowDisplayOption option)
		{
			Assembly assembly = typeof(UnityEditor.Editor).Assembly;
			var overlayType = assembly.GetType("UnityEditor.SceneViewOverlay");
			Type delegateType = overlayType.GetNestedType("WindowFunction");
			Delegate deleg = Delegate.CreateDelegate(delegateType, target, guiFunction.Method);
			ConstructorInfo constructor = assembly.GetType("UnityEditor.OverlayWindow").GetConstructors()[0];
			Window = constructor.Invoke(new object[] { title, deleg, primaryOrder, target, (int)option });
			_onGUIMethod = overlayType.GetMethod("ShowWindow");
		}

		public enum WindowDisplayOption
		{
			MultipleWindowsPerTarget,
			OneWindowPerTarget,
			OneWindowPerTitle,
		}
	}
