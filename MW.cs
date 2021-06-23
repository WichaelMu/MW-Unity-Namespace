

namespace MW {

    public enum Direction {
        forward,
        right,
        back,
        left,
        up,
        down
    }

    public enum EEquation {
        Linear,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        Spring,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
    };

    public enum Units {
        MetresPerSecond,
        KilometresPerSecond,
        MetresPerHour,
        KilometrsePerHour,
        FeetPerSecond,
        MilesPerSecond,
        FeetPerHour,
        MilesPerHour
    }

    /// <summary>Generates a new pair of two types of values.</summary>
    /// <typeparam name="T_First">The type of the first variable to store.</typeparam>
    /// <typeparam name="T_Second">The type of the second variable to store.</typeparam>
    public struct Pair<T_First, T_Second> {
        public T_First first { get; set; }
        public T_Second second { get; set; }

        public Pair(T_First first, T_Second second) {
            this.first = first;
            this.second = second;
        }
    }

    /// <summary>Generates a new variable of three types of values.</summary>
    /// <typeparam name="T_First">The type of the first variable to store.</typeparam>
    /// <typeparam name="T_Second">The type of the second variable to store.</typeparam>
    /// <typeparam name="T_Third">The type of the third variable to store.</typeparam>
    public struct Triple<T_First, T_Second, T_Third> {
        public T_First first { get; set; }
        public T_Second second { get; set; }
        public T_Third third { get; set; }

        public Triple(T_First first, T_Second second, T_Third third) {
            this.first = first;
            this.second = second;
            this.third = third;
        }
    }
}