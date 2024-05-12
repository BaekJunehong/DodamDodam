namespace mode{
    public enum modeType
    {
        tutorial = 0,
        play = 1,
        test = 2
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
