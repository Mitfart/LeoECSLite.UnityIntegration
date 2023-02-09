using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Nav;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor{
   public static class StyleUtils{
      public const int METRICS_LLL = 35;
      public const int METRICS_LL  = 30;
      public const int METRICS_L   = 25;
      public const int METRICS     = 20;
      public const int METRICS_S   = 15;
      public const int METRICS_SS  = 10;
      public const int METRICS_SSS = 5;

      public const           string UNITY_CONTENT = "unity-content";
      public static readonly Color  Color_Ddd    = new(26 / 256f, 24 / 256f, 25 / 256f);
      public static readonly Color  Color_Dd     = new(42 / 256f, 42 / 256f, 42 / 256f);
      public static readonly Color  Color_D      = new(48 / 256f, 48 / 256f, 48 / 256f);
      public static readonly Color  Color        = new(56 / 256f, 56 / 256f, 56 / 256f);
      public static readonly Color  Color_L      = new(88 / 256f, 88 / 256f, 88 / 256f);
      public static readonly Color  Color_Ll     = new(196 / 256f, 196 / 256f, 196 / 256f);
      public static readonly Color  Color_Lll    = new(200 / 256f, 200 / 256f, 196 / 200f);

      public static readonly Color Color_H_Error  = new(237 / 256f, 37 / 256f, 78 / 256f);
      public static readonly Color Color_H_Warnig = new(255 / 256f, 210 / 256f, 63 / 256f);
      public static readonly Color Color_H_Good   = new(14 / 256f, 173 / 256f, 105 / 256f);

      public static readonly StyleLength Metrics_Percents_Lll = new(new Length(METRICS_LLL, LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_Ll  = new(new Length(METRICS_LL,  LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_L   = new(new Length(METRICS_L,   LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents     = new(new Length(METRICS,     LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_S   = new(new Length(METRICS_S,   LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_Ss  = new(new Length(METRICS_SS,  LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_Sss = new(new Length(METRICS_SSS, LengthUnit.Percent));

      public static readonly StyleLength Metrics_Auto = new(StyleKeyword.Auto);
      public static StyleLength GetPercentsLength(int length){
         return new(new Length(length, LengthUnit.Percent));
      }


      public static void SetPadding(this IStyle style, StyleLength length){
         style.paddingBottom = style.paddingTop = style.paddingLeft = style.paddingRight = length;
      }

      public static void SetMargin(this IStyle style, StyleLength length){
         style.marginBottom = style.marginTop = style.marginLeft = style.marginRight = length;
      }

      public static void SetBorderRadius(
         this IStyle style,
         StyleLength lengthTR,
         StyleLength lengthBR,
         StyleLength lengthBL,
         StyleLength lengthTL){
         style.borderTopRightRadius    = lengthTR;
         style.borderBottomRightRadius = lengthBR;
         style.borderBottomLeftRadius  = lengthBL;
         style.borderTopLeftRadius     = lengthTL;
      }

      public static void SetBorderRadius(this IStyle style, StyleLength length){
         style.SetBorderRadius(length, length, length, length);
      }

      public static void Square(this IStyle style, StyleLength length){
         style.width  = length;
         style.height = length;
      }


      public static T WithLabelStyle<T>(this T label) where T : VisualElement{
         label.style.paddingBottom   = label.style.paddingTop   = METRICS_SS;
         label.style.paddingLeft     = label.style.paddingRight = METRICS;
         label.style.backgroundColor = Color_D;
         return label;
      }

      public static Button WithSquareButtonStyle(this Button button){
         button.style.Square(METRICS_L);
         button.style.SetBorderRadius(METRICS_SSS);
         button.style.marginLeft = button.style.marginRight = METRICS_SSS;
         button.style.SetPadding(0);

         return button;
      }

      public static ButtonsContainer AddButtonsContainer(this VisualElement element){
         var container = element.AddAndGet(new ButtonsContainer());

         container.style.flexDirection  = new StyleEnum<FlexDirection>(FlexDirection.Row);
         container.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);
         container.style.position       = Position.Absolute;
         container.style.right          = 0;
         container.style.top            = 0;

         return container;
      }
   }
}
