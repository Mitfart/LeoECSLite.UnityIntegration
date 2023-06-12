using System;

namespace Mitfart.LeoECSLite.UnityIntegration.Attributes {
  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class PackedEntityAttribute : Attribute { }
}