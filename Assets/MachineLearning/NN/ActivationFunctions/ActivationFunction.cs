namespace MachineLearning.NN.ActivationFunctions {
    public abstract class ActivationFunction {
        public abstract float Apply(float x);
        public static None None { get; } = new None();
        public static Sigmoid Sigmoid { get; } = new Sigmoid();
    }
}