namespace handSide{
    public enum whichSide{
            left,
            right
    }
    public class HandSide
    {
        private static whichSide side = whichSide.left;

        public static whichSide HS{
            get => side;
            set => side = value;
        }
    }
}