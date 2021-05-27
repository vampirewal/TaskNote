#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：GridLengthAnimation
// 创 建 者：杨程
// 创建时间：2021/5/27 13:58:40
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace TaskNote.Theme
{
    public class GridLengthAnimation : AnimationTimeline
    {
        static GridLengthAnimation()
        {
            FromProperty = DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));
            ToProperty = DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));
        }

        public static readonly DependencyProperty FromProperty;
        public GridLength From
        {
            get
            {
                return (GridLength)GetValue(GridLengthAnimation.FromProperty);
            }
            set
            {
                SetValue(GridLengthAnimation.FromProperty, value);
            }
        }

        public static readonly DependencyProperty ToProperty;
        public GridLength To
        {
            get
            {
                return (GridLength)GetValue(GridLengthAnimation.ToProperty);
            }
            set
            {
                SetValue(GridLengthAnimation.ToProperty, value);
            }
        }

        public override Type TargetPropertyType
        {
            get
            {
                return typeof(GridLength);
            }
        }

        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public override object GetCurrentValue(object defaultOriginValue,
            object defaultDestinationValue, AnimationClock animationClock)
        {
            double fromVal = ((GridLength)GetValue(GridLengthAnimation.FromProperty)).Value;
            double toVal = ((GridLength)GetValue(GridLengthAnimation.ToProperty)).Value;

            if (fromVal > toVal)
            {
                return new GridLength((1 - animationClock.CurrentProgress.Value) * (fromVal - toVal) + toVal,
                    ((GridLength)GetValue(GridLengthAnimation.FromProperty)).GridUnitType);
            }
            else
                return new GridLength(animationClock.CurrentProgress.Value * (toVal - fromVal) + fromVal,
                    ((GridLength)GetValue(GridLengthAnimation.ToProperty)).GridUnitType);
        }

        //public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        //{
        //    double fromVal = ((GridLength)GetValue(FromProperty)).Value;
        //    double toVal = ((GridLength)GetValue(ToProperty)).Value;
        //    double progress = animationClock.CurrentProgress.Value;

        //    IEasingFunction easingFunction = EasingFunction;
        //    if (easingFunction != null)
        //    {
        //        progress = easingFunction.Ease(progress);
        //    }


        //    if (fromVal > toVal)
        //        return new GridLength((1 - progress) * (fromVal - toVal) + toVal, GridUnitType.Star);

        //    return new GridLength(progress * (toVal - fromVal) + fromVal, GridUnitType.Star);
        //}

        /// <summary>
        /// The <see cref="EasingFunction" /> dependency property's name.
        /// </summary>
        public const string EasingFunctionPropertyName = "EasingFunction";

        /// <summary>
        /// Gets or sets the value of the <see cref="EasingFunction" />
        /// property. This is a dependency property.
        /// </summary>
        public IEasingFunction EasingFunction
        {
            get
            {
                return (IEasingFunction)GetValue(EasingFunctionProperty);
            }
            set
            {
                SetValue(EasingFunctionProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="EasingFunction" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
            EasingFunctionPropertyName,
            typeof(IEasingFunction),
            typeof(GridLengthAnimation),
            new UIPropertyMetadata(null));

    }
}
