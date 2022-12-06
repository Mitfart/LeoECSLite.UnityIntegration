using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class Styles{
      public const int METRICS_LLL = 35;
      public const int METRICS_LL  = 30;
      public const int METRICS_L   = 25;
      public const int METRICS     = 20;
      public const int METRICS_S   = 15;
      public const int METRICS_SS  = 10;
      public const int METRICS_SSS = 5;

      public const           string UnityContent = "unity-content";
      public static readonly Color  COLOR_DDD    = new(26 / 256f, 24 / 256f, 25 / 256f);
      public static readonly Color  COLOR_DD     = new(42 / 256f, 42 / 256f, 42 / 256f);
      public static readonly Color  COLOR_D      = new(48 / 256f, 48 / 256f, 48 / 256f);
      public static readonly Color  COLOR        = new(56 / 256f, 56 / 256f, 56 / 256f);
      public static readonly Color  COLOR_L      = new(88 / 256f, 88 / 256f, 88 / 256f);
      public static readonly Color  COLOR_LL     = new(196 / 256f, 196 / 256f, 196 / 256f);
      public static readonly Color  COLOR_LLL    = new(200 / 256f, 200 / 256f, 196 / 200f);

      public static readonly Color COLOR_H_ERROR  = new(237 / 256f, 37 / 256f, 78 / 256f);
      public static readonly Color COLOR_H_WARNIG = new(255 / 256f, 210 / 256f, 63 / 256f);
      public static readonly Color COLOR_H_GOOD   = new(14 / 256f, 173 / 256f, 105 / 256f);

      public static readonly StyleLength METRICS_PERCENTS_LLL = new(new Length(METRICS_LLL, LengthUnit.Percent));
      public static readonly StyleLength METRICS_PERCENTS_LL  = new(new Length(METRICS_LL,  LengthUnit.Percent));
      public static readonly StyleLength METRICS_PERCENTS_L   = new(new Length(METRICS_L,   LengthUnit.Percent));
      public static readonly StyleLength METRICS_PERCENTS     = new(new Length(METRICS,     LengthUnit.Percent));
      public static readonly StyleLength METRICS_PERCENTS_S   = new(new Length(METRICS_S,   LengthUnit.Percent));
      public static readonly StyleLength METRICS_PERCENTS_SS  = new(new Length(METRICS_SS,  LengthUnit.Percent));
      public static readonly StyleLength METRICS_PERCENTS_SSS = new(new Length(METRICS_SSS, LengthUnit.Percent));

      public static readonly StyleLength METRICS_AUTO = new(StyleKeyword.Auto);
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
         StyleLength length_TR,
         StyleLength length_BR,
         StyleLength lengthBL,
         StyleLength lengthTL){
         style.borderTopRightRadius    = length_TR;
         style.borderBottomRightRadius = length_BR;
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
         label.style.backgroundColor = COLOR_D;
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
