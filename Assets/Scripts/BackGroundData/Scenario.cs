using System.Collections.Generic;
using difficulty;
using sceneData;

namespace scenario
{
    public class Scenario
    {
        private static List<sceneType>PlayScenario = new List<sceneType>
        {
            sceneType.straight,
            sceneType.curve,
            sceneType.zigzag
        };

        private static List<(difficultyLevel, sceneType)>TestScenario = new List<(difficultyLevel, sceneType)>
        {
            (difficultyLevel.normal, sceneType.straight),
            (difficultyLevel.normal, sceneType.curve),
            (difficultyLevel.normal, sceneType.zigzag),
            (difficultyLevel.hard, sceneType.curve),
            (difficultyLevel.hard, sceneType.straight)
        };

        public static List<sceneType> PS
        {
            get { return PlayScenario; }
        }
        public static List<(difficultyLevel, sceneType)> TS
        {
            get { return TestScenario; }
        }
    }
}