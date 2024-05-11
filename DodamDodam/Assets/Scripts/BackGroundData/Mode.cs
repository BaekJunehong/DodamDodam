namespace mode{
    public enum modeType
    {
        play = 0,
        test = 1
    }
    public class Mode
    {
        private static modeType mod = modeType.play;

        public static modeType M{
            get => mod;
            set => mod = value;
        }
    }
}
