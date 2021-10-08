using UnityEditor;
using IdleEngine.Session;

namespace IdleEngine.Editor
{
    [CustomEditor (typeof(Session.Session))]
    public class GeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var session = (Session.Session) target;
        } 
    }
}