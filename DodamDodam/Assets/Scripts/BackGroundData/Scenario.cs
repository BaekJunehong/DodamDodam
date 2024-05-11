using System.Collections.Generic;
using difficulty;
using sceneData;

namespace scenario
{
    public class Scenario
    {
        private static List<(difficultyLevel diff, sceneType sc)>PlayScenario;

        private static List<(difficultyLevel, sceneType)>TestScenario = new List<(difficultyLevel, sceneType)>
        {
            (difficultyLevel.normal, sceneType.straight),
            (difficultyLevel.normal, sceneType.curve),
            (difficultyLevel.normal, sceneType.zigzag),
            (difficultyLevel.hard, sceneType.curve),
            (difficultyLevel.hard, sceneType.straight)
        };

        public static List<(difficultyLevel, sceneType)> TS
        {
            get { return TestScenario; }
        }
    }
}