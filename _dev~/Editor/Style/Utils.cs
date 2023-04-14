using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Nav;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Style{
   public static class Utils{
      public const int METRICS_MAX  = int.MaxValue;
      public const int METRICS_2000 = 32;
      public const int METRICS_1750 = 28;
      public const int METRICS_1500 = 24;
      public const int METRICS_1250 = 20;
      public const int METRICS_1000 = 16;
      public const int METRICS_0750 = 12;
      public const int METRICS_0500 =  8;
      public const int METRICS_0250 =  4;
      public const int METRICS_0125 =  2;
      public const int METRICS_MIN  =  1;

      public const           string UNITY_CONTENT = "unity-content";
      public static readonly Color  Color_DDD     = new(26  / 256f, 24  / 256f, 25  / 256f);
      public static readonly Color  Color_DD      = new(42  / 256f, 42  / 256f, 42  / 256f);
      public static readonly Color  Color_D       = new(48  / 256f, 48  / 256f, 48  / 256f);
      public static readonly Color  Color         = new(56  / 256f, 56  / 256f, 56  / 256f);
      public static readonly Color  Color_L       = new(64  / 256f, 64  / 256f, 64  / 256f);
      public static readonly Color  Color_LL      = new(88  / 256f, 88  / 256f, 88  / 256f);
      public static readonly Color  Color_LLL     = new(196 / 256f, 196 / 256f, 196 / 256f);
      public static readonly Color  Color_LLLL    = new(200 / 256f, 200 / 256f, 196 / 200f);

      public static readonly Color Color_Primary_LL = new(1f, 1f, 1f);
      public static readonly Color Color_Primary_L  = new(151 / 256f, 244 / 256f, 1f);
      public static readonly Color Color_Primary    = new(116 / 256f, 203 / 256f, 238 / 256f);
      
      public static readonly Color Color_H_Error  = new(237 / 256f, 37  / 256f, 78  / 256f);
      public static readonly Color Color_H_Warnig = new(255 / 256f, 210 / 256f, 63  / 256f);
      public static readonly Color Color_H_Good   = new(14  / 256f, 173 / 256f, 105 / 256f);

      public static readonly StyleLength Metrics_Percents_Lll = new(new Length(METRICS_2000, LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_Ll  = new(new Length(METRICS_1750, LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_L   = new(new Length(METRICS_1500, LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents     = new(new Length(METRICS_1250, LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_S   = new(new Length(METRICS_1000, LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_Ss  = new(new Length(METRICS_0750, LengthUnit.Percent));
      public static readonly StyleLength Metrics_Percents_Sss = new(new Length(METRICS_0500, LengthUnit.Percent));

      public static readonly StyleLength Metrics_Auto = new(StyleKeyword.Auto);
      public static StyleLength GetPercentsLength(int length){
         return new(new Length(length, LengthUnit.Percent));
      }


      public static Color GetColorByString(string str) {
         byte[] asciiBytes = Encoding.ASCII.GetBytes(str);

         float r = 0f, g = 0f, b = 0f;

         for (var i = 0; i < asciiBytes.Length; i++) {
            if (i % 3 == 0) 
               r += asciiBytes[i];
            else if (i % 2 == 0) 
               g += asciiBytes[i];
            else 
               b += asciiBytes[i];
         }

         r /= 256f;
         g /= 256f;
         b /= 256f;
         
         r %= 1f;
         g %= 1f;
         b %= 1f;

         return new Color(r, g, b);
      }


      public static string Compact(this string str) {
         const int charsToShow = 3;
         
         List<char> chars          = str.ToCharArray().ToList();
         List<char> uppercaseChars = chars.Where(char.IsUpper).ToList();
         
         List<char> result = uppercaseChars.Count <= 0 
            ? chars.Take(charsToShow).ToList() 
            : uppercaseChars;

         return new string(result.ToArray());
      }



      public static T AddIcon<T>(this T root, Texture2D icon) where T : VisualElement{
         return root.AddChild(
            new VisualElement {
               style = {
                  backgroundImage = icon,
                  width  = GetPercentsLength(100), 
                  height = GetPercentsLength(100),
               }
            }
         );
      }
      

      public static void SetPadding(
         this IStyle style, 
         StyleLength lengthTop, 
         StyleLength lengthBottom, 
         StyleLength lengthLeft,
         StyleLength lengthRight) {
         style.paddingTop    = lengthTop;
         style.paddingBottom = lengthBottom;
         style.paddingLeft   = lengthLeft;
         style.paddingRight  = lengthRight;
      }
      
      public static void SetPadding(
         this IStyle style, 
         StyleLength hor, 
         StyleLength ver) {
         style.paddingTop  = style.paddingBottom = ver;
         style.paddingLeft = style.paddingRight  = hor;
         style.SetPadding(ver, ver, hor, hor);
      }
      
      public static void SetPadding(this IStyle style, StyleLength length){
         style.SetPadding(length, length);
      }

      
      public static void SetMargin(
         this IStyle style, 
         StyleLength lengthTop, 
         StyleLength lengthBottom, 
         StyleLength lengthLeft,
         StyleLength lengthRight) {
         style.marginTop    = lengthTop;
         style.marginBottom = lengthBottom;
         style.marginLeft   = lengthLeft;
         style.marginRight  = lengthRight;
      }
      public static void SetMargin(
         this IStyle style, 
         StyleLength hor, 
         StyleLength ver) {
         style.SetMargin(ver, ver, hor, hor);
      }
      public static void SetMargin(this IStyle style, StyleLength length){
         style.SetMargin(length, length);
      }

      
      
      public static void SetBorderRadius(
         this IStyle style,
         StyleLength TR,
         StyleLength BR,
         StyleLength BL,
         StyleLength TL){
         style.borderTopRightRadius    = TR;
         style.borderBottomRightRadius = BR;
         style.borderBottomLeftRadius  = BL;
         style.borderTopLeftRadius     = TL;
      }

      public static void SetBorderRadius(this IStyle style, StyleLength length){
         style.SetBorderRadius(length, length, length, length);
      }
      
      
      public static void SetBorderColor(
         this IStyle style,
         StyleColor  top,
         StyleColor  bot,
         StyleColor  left,
         StyleColor  right) {
         style.borderTopColor    = top;
         style.borderBottomColor = bot;
         style.borderLeftColor   = left;
         style.borderRightColor  = right;
      }
      
      public static void SetBorderColor(this IStyle style, StyleColor color) {
         style.SetBorderColor(color, color, color, color);
      }
      
      
      public static void SetBorderWidth(
         this IStyle style,
         StyleFloat  top,
         StyleFloat  bot,
         StyleFloat  left,
         StyleFloat  right) {
         style.borderTopWidth    = top;
         style.borderBottomWidth = bot;
         style.borderLeftWidth   = left;
         style.borderRightWidth  = right;
      }
      
      public static void SetBorderWidth(this IStyle style, StyleFloat width) {
         style.SetBorderWidth(width, width, width, width);
      }
      
      

      public static void Square(this IStyle style, StyleLength size){
         style.width  = size;
         style.height = size;
      }


      public static T WithLabelStyle<T>(this T label) where T : VisualElement{
         label.style.paddingBottom   = label.style.paddingTop   = METRICS_0750;
         label.style.paddingLeft     = label.style.paddingRight = METRICS_1250;
         label.style.backgroundColor = Color_D;
         return label;
      }

      public static Button WithSquareButtonStyle(this Button button){
         button.style.Square(METRICS_1500);
         button.style.SetBorderRadius(METRICS_0500);
         button.style.marginLeft = button.style.marginRight = METRICS_0500;
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
