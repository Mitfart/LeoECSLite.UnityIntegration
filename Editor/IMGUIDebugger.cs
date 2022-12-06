using System;
using UnityEditor;

namespace Mitfart.LeoECSLite.UnityIntegration{
	public static class IMGUIDebugger {
		static Type type = Type.GetType( "UnityEditor.GUIViewDebuggerWindow,UnityEditor" );

		[MenuItem("Window/IMGUI Debugger" )]
		public static void Open() => EditorWindow.GetWindow( type ).Show();
	
	}
}
