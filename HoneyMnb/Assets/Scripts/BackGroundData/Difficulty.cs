namespace difficulty{

    public enum difficultyLevel{
            easy = 3,
            normal = 2,
            hard = 1
        }
    public class Difficulty
    {
        private static difficultyLevel diff = difficultyLevel.normal;

        public static difficultyLevel DF{
            get => diff;
            set => diff = value;
        }
    }
}
