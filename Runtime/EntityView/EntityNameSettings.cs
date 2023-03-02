#if UNITY_EDITOR
namespace Mitfart.LeoECSLite.UnityIntegration.EntityView {
    public struct EntityNameSettings{
        public bool   BakeComponents   { get; private set; }
        public bool   BakeEntity       { get; private set; }
        public string BakeEntityFormat { get; private set; }
    
    
        public EntityNameSettings(bool bakeComponents = false, string bakeEntityFormat = "X8", bool bakeEntity = true){
            BakeComponents   = bakeComponents;
            BakeEntityFormat = bakeEntityFormat;
            BakeEntity       = bakeEntity;
        }


        public EntityNameSettings WithBakingComponents() {
            BakeComponents = true;   
            return this;
        }
        
        public EntityNameSettings WithBakingEntity(string format = null) {
            BakeEntity       = true;
            BakeEntityFormat = format;
            return this;
        }
    }
}

#endif