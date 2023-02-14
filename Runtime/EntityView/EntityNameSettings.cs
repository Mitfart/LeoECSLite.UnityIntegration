#if UNITY_EDITOR
namespace Mitfart.LeoECSLite.UnityIntegration.EntityView {
    public struct EntityNameSettings{
        public bool   BakeComponents   { get; private set; }
        public bool   BakeEntity       { get; private set; }
        public string BakeEntityFormat { get; private set; }
    
    
        public EntityNameSettings(string bakeEntityFormat = "X8", bool bakeComponents = false, bool bakeEntity = true){
            BakeEntityFormat = bakeEntityFormat;
            BakeComponents   = bakeComponents;
            BakeEntity       = bakeEntity;
        }

    
        public EntityNameSettings WithBakingComponents()                { BakeComponents   = true;   return this; }
        public EntityNameSettings WithBakingEntity()                    { BakeEntity       = true;   return this; }
        public EntityNameSettings WithBakingEntityFormat(string format) { BakeEntityFormat = format; return this; }
    }
}

#endif