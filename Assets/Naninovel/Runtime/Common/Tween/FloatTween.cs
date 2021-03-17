// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

using System;

namespace Naninovel
{
    public readonly struct FloatTween : ITweenValue, IEquatable<FloatTween>
    {
        public float TweenDuration { get; }
        public EasingType EasingType { get; }
        public bool TimeScaleIgnored { get; }
        public bool TargetValid => onTween != null && (!targetProvided || target);

        private readonly float startValue;
        private readonly float targetValue;
        private readonly Action<float> onTween;
        private readonly UnityEngine.Object target;
        private readonly bool targetProvided;

        public FloatTween (float from, float to, float time, Action<float> onTween, 
            bool ignoreTimeScale = false, EasingType easingType = default, UnityEngine.Object target = default)
        {
            startValue = from;
            targetValue = to;
            TweenDuration = time;
            EasingType = easingType;
            TimeScaleIgnored = ignoreTimeScale;
            this.onTween = onTween;

            targetProvided = this.target = target;
        }

        public void TweenValue (float tweenPercent)
        {
            if (!TargetValid) return;

            var newValue = EasingType.Tween(startValue, targetValue, tweenPercent);
            onTween.Invoke(newValue);
        }
        
        public bool Equals (FloatTween other)
        {
            return startValue.Equals(other.startValue) && 
                   targetValue.Equals(other.targetValue) && 
                   Equals(onTween, other.onTween) && 
                   Equals(target, other.target) && 
                   targetProvided == other.targetProvided && 
                   TweenDuration.Equals(other.TweenDuration) && 
                   EasingType == other.EasingType && 
                   TimeScaleIgnored == other.TimeScaleIgnored;
        }

        public override bool Equals (object obj)
        {
            return obj is FloatTween other && Equals(other);
        }

        public override int GetHashCode ()
        {
            unchecked
            {
                var hashCode = startValue.GetHashCode();
                hashCode = (hashCode * 397) ^ targetValue.GetHashCode();
                hashCode = (hashCode * 397) ^ (onTween != null ? onTween.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (target != null ? target.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ targetProvided.GetHashCode();
                hashCode = (hashCode * 397) ^ TweenDuration.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)EasingType;
                hashCode = (hashCode * 397) ^ TimeScaleIgnored.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator == (FloatTween left, FloatTween right)
        {
            return left.Equals(right);
        }

        public static bool operator != (FloatTween left, FloatTween right)
        {
            return !left.Equals(right);
        }
    }
}
