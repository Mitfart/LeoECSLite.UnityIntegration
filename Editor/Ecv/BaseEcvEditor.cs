using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Ecv {
  public abstract class BaseEcvEditor : UnityEditor.Editor  {
    public override VisualElement CreateInspectorGUI() {
      var root = new VisualElement();

      CreateEditor(root);

      return root;
    }
    
    

    protected abstract void CreateEditor(VisualElement root);
    protected abstract void OnEditorUpdate();
      
      
    
    protected virtual void OnEnable() {
      EditorApplication.update -= EditorUpdate;
      EditorApplication.update += EditorUpdate;
    }
    protected virtual void OnDisable() {
      EditorApplication.update -= EditorUpdate;
    }
      
      
    private void EditorUpdate() {
      if (!Application.isPlaying){
        EditorApplication.update -= EditorUpdate;
        return;
      }

      OnEditorUpdate();
    }
  }
}
