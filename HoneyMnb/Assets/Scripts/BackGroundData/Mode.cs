namespace mode{
    public enum playMode
    {
        play = 0,
        test = 1
    }
    public class Mode
    {
        private static playMode mod = playMode.play;

        public static playMode M{
            get => mod;
            set => mod = value;
        }
    }
}
