namespace sceneData{

    public enum sceneType{
            zigzag,
            straight,
            curve
        }
    public class SceneData
    {
        private static sceneType Scene = sceneType.straight;
        public static sceneType SC{
            get => Scene;
            set => Scene = value;
        }
    }
}