using System;
using System.Linq;
namespace Assets.Scripts
{
    public enum Response
    {
        Yes = 1,
        No = 2,
        Maybe = 3
    }

    public enum TutorialStages
    {
        Rotation, BackRotation, Reverse, BackReverse, Finish
    }

    public class TutorialController
    {
        public TutorialStages stages = TutorialStages.Rotation;
        private UIController _controller;

        public int[] TutorialBlocks = { 1, 2, 0, 3, -1 };

        public int GetCurrentTutorialBlock()
        {
            return TutorialBlocks[(int)stages];
        }

        private string[] stageDescriptions = { "Welcome to the Rotatrix tutorial." +
                " To start begin click a block and drag it to the right to rotate the top of the tower."
                , "Now click a block and drag it to the left to rotate the bottom of the tower."
                , "Now click a block and drag it down to move it and the blocks above it to the bottom."
                , "Now click a block and drag it up to move it and the blocks bellow it to the top."
                , "Now use the moves you've learned to match the tower to the one in the background."};

        public TutorialController(UIController controller)
        {
            stages = TutorialStages.Rotation;
            _controller = controller;
        }

        public void ChangeTutorialDescription()
        {
            _controller.SetTutorialText(stageDescriptions[(int)stages]);
        }

        public bool SwitchTutorialStage()
        {
            if (stages == TutorialStages.Finish)
            {
                return true;
            }
            var values = Enum.GetValues(typeof(TutorialStages));
            stages = values.Cast<TutorialStages>().ToList()[(int)stages + 1];
            ChangeTutorialDescription();
            return false;
        }

        public bool CheckMoveValidity(bool rotate, bool backwards, int selectedBlock)
        {
            if (selectedBlock != TutorialBlocks[(int)stages] && TutorialBlocks[(int)stages] != -1)
            {
                return false;
            }

            switch (stages)
            {
                case TutorialStages.Rotation:
                    if (!rotate || backwards)
                        return false;
                    break;
                case TutorialStages.BackRotation:
                    if (!rotate || !backwards)
                        return false;
                    break;
                case TutorialStages.Reverse:
                    if (rotate || backwards)
                        return false;
                    break;
                case TutorialStages.BackReverse:
                    if (rotate || !backwards)
                        return false;
                    break;
            }

            SwitchTutorialStage();

            return true;
        }

    }

}
