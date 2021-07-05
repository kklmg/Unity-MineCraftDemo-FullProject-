using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NInput
{
    public class InputBevFactory : Singleton<InputBevFactory>
    {
        public ConButtonDown ButtonDown(string key, InputEvent inputevent = null)
        {
            ConButtonDown NewNode = new ConButtonDown(key);
            NewNode.InputEvent = inputevent;
            return NewNode;
        }
        public ConButtonUp ButtonUp(string key, InputEvent inputevent = null)
        {
            ConButtonUp NewNode = new ConButtonUp(key);
            NewNode.InputEvent = inputevent;
            return NewNode;
        }
        public ConButtonPress ButtonPress(string key, InputEvent inputevent = null)
        {
            ConButtonPress NewNode = new ConButtonPress(key);
            NewNode.InputEvent = inputevent;
            return NewNode;
        }
        public ConInputAxis InputAxis(string key, InputEvent inputevent = null)
        {
            ConInputAxis NewNode = new ConInputAxis(key);
            NewNode.InputEvent = inputevent;
            return NewNode;
        }
        public BevConditionBase HasInput()
        {
            BevConditionBase axis = HasInput_Axis();
            BevConditionBase mouse_axis = HasInput_MouseAxis();


            BevCondition_Or hasInput = new BevCondition_Or();

            hasInput.AddChild(axis);
            hasInput.AddChild(mouse_axis);

            return hasInput;
        }
        public BevConditionBase HasInput_Axis()
        {
            ConInputAxis inputHor = new ConInputAxis(KEY_INPUT.HORIZONTAL);
            ConInputAxis inputVer = new ConInputAxis(KEY_INPUT.VERTICAL);

            BevCondition_Or HasInput = new BevCondition_Or();

            HasInput.AddChild(inputHor);
            HasInput.AddChild(inputVer);

            return HasInput;
        }
        public BevConditionBase HasInput_MouseAxis()
        {
            ConInputAxis MouseHor = new ConInputAxis(KEY_INPUT.MOUSE_HORIZONTAL);
            ConInputAxis MouseVer = new ConInputAxis(KEY_INPUT.MOUSE_VERTICAL);

            BevCondition_Or HasInput = new BevCondition_Or();
            HasInput.AddChild(MouseVer);
            HasInput.AddChild(MouseHor);

            return HasInput;
        }
    }
}