

namespace MW {

    public enum Direction {
        forward,
        right,
        back,
        left,
        up,
        down
    }

    public enum Equation {
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
    /// <typeparam name="T">The type of the first variable to store.</typeparam>
    /// <typeparam name="Y">The type of the second variable to store.</typeparam>
    public struct Pair<T, Y> {
        public T first { get; set; }
        public Y second { get; set; }

        public Pair(T first, Y second) {
            this.first = first;
            this.second = second;
        }
    }

    /// <summary>Generates a new variable of three types of values.</summary>
    /// <typeparam name="T">The type of the first variable to store.</typeparam>
    /// <typeparam name="Y">The type of the second variable to store.</typeparam>
    /// <typeparam name="U">The type of the third variable to store.</typeparam>
    public struct Triple<T, Y, U> {
        public T first { get; set; }
        public Y second { get; set; }
        public U third { get; set; }

        public Triple(T first, Y second, U third) {
            this.first = first;
            this.second = second;
            this.third = third;
        }
    }
}