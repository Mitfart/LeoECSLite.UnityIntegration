using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Style{
   public static class Icons{
      // https://www.programmersought.com/article/45577357558/
      
      public static string Text_Plus   => "✚";
      public static string Text_Reload => "↻";
      public static string Text_Close  => "✕";
      public static string Text_ArrowUp   => "▲";
      public static string Text_ArrowDown => "▼";
      
      public static readonly Texture2D Plus_More = EditorGUIUtility.FindTexture("d_Toolbar Plus More");
      public static readonly Texture2D Plus      = EditorGUIUtility.FindTexture("d_Toolbar Plus");
      public static readonly Texture2D Help      = EditorGUIUtility.FindTexture("_Help");
      public static readonly Texture2D Settings  = EditorGUIUtility.FindTexture("_Popup");
      public static readonly Texture2D Refresh   = EditorGUIUtility.FindTexture("d_Refresh");
      public static readonly Texture2D Trash     = EditorGUIUtility.FindTexture("d_TreeEditor.Trash");
      public static readonly Texture2D Close     = EditorGUIUtility.FindTexture("d_winbtn_win_close");
   }
}
