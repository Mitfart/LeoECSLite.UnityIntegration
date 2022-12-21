using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class Styles{
      public const int Metrics_Lll = 35;
      public const int Metrics_Ll  = 30;
      public const int Metrics_L   = 25;
      public const int Metrics     = 20;
      public const int Metrics_S   = 15;
      public const int Metrics_Ss  = 10;
      public const int Metrics_Sss = 5;

      public const           string UnityContent = "unity-content";
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

      public static readonly StyleLength Metrics_Percents_Lll = new(new Length(Metrics_Lll, LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_Ll  = new(new Length(Metrics_Ll,  LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_L   = new(new Length(Metrics_L,   LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents     = new(new Length(Metrics,     LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_S   = new(new Length(Metrics_S,   LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_Ss  = new(new Length(Metrics_Ss,  LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_Sss = new(new Length(Metrics_Sss, LengthUnit.Percent));

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
         label.style.paddingBottom   = label.style.paddingTop   = Metrics_Ss;
         label.style.paddingLeft     = label.style.paddingRight = Metrics;
         label.style.backgroundColor = Color_D;
         return label;
      }

      public static Button WithSquareButtonStyle(this Button button){
         button.style.Square(Metrics_L);
         button.style.SetBorderRadius(Metrics_Sss);
         button.style.marginLeft = button.style.marginRight = Metrics_Sss;
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
