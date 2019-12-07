using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Jasmine.Core.StateIndicator
{
    public abstract class StateIndicator<T, T1> : TextBlock where T : StateIndicator<T, T1>, new()
    {
        public static readonly DependencyProperty CurrentStateProperty = DependencyProperty.Register(
            "CurrentState", typeof(T1), typeof(StateIndicator<T, T1>), new FrameworkPropertyMetadata(OnStateChanged));

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock)
            {
                textBlock.Inlines.Clear();
                var t = new T();
                t.CreateRuns(textBlock, (T1)e.NewValue);
            }
        }

        public abstract void CreateRuns(TextBlock textBlock, T1 currentState);

        public static Run[] GetState(string text, T1 currentState)
        {
            var color = text == GetEnumValue(currentState)
                ? Colors.OrangeRed
                : Colors.LightSlateGray;
            var active = text == GetEnumValue(currentState);
            if (active)
            {
                return new[]
                {
                    new Run(text)
                    {
                        FontWeight = FontWeights.DemiBold, TextDecorations = System.Windows.TextDecorations.Underline
                    },
                    new Run(" g ")
                    {
                        FontFamily = new FontFamily("Wingdings 3"), FontSize = 12
                    }
                };
            }

            return new[]
            {
                new Run(text)
                {
                    //   FontWeight = FontWeights.Bold//  Foreground = new SolidColorBrush(color)
                },
                new Run(" g ")
                {
                    FontFamily = new FontFamily("Wingdings 3"), FontSize = 10
                }
            };
        }

        public static string GetEnumValue(T1 state)
        {
            var type = state.GetType();
            MemberInfo memberInfo =
                type.GetMember(state.ToString()).First();
            var displayAttribute =
                memberInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                return displayAttribute.Name;
            }
            else
            {
                return state.ToString();
            }
        }

        public static Run GetStateWithoutArrow(string text, T1 currentState)
        {
            var color = text == GetEnumValue(currentState)
                ? Colors.OrangeRed
                : Colors.LightSlateGray;
            var active = text == GetEnumValue(currentState);
            if (active)
            {
                return new Run(text)
                { FontWeight = FontWeights.DemiBold, TextDecorations = System.Windows.TextDecorations.Underline };
            }
            else
            {
                return new Run(text);
            }
        }

        public T1 CurrentState
        {
            get => (T1)GetValue(CurrentStateProperty);
            set => SetValue(CurrentStateProperty, value);
        }
    }
}