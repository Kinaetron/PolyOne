using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PolyOne.Input
{
    static public class PolyInput
    {
        static public KeyboardData Keyboard { get; private set; }
        static public MouseData Mouse { get; private set; }
        static public GamePadData[] GamePads { get; private set; }

        static public void Initialize()
        {
            Keyboard = new KeyboardData();
            Mouse = new MouseData();

            GamePads = new GamePadData[4];
            for (int i = 0; i < 4; i++)
            {
                GamePads[i] = new GamePadData((PlayerIndex)i);
            }
        }

        static internal void ShutDown()
        {
            foreach (var gamepad in GamePads)
            {
                gamepad.StopRumble();
            }
        }

        static internal void Update(GameTime gameTime)
        {
            Keyboard.Update();
            Mouse.Update();

            for (int i = 0; i < 4; i++)
                GamePads[i].Update(gameTime);
        }

        static public bool IsNewKeyPress(Keys key, out PlayerIndex playerIndex)
        {
            playerIndex = PlayerIndex.One;
            return Keyboard.Pressed(key);
        }

        static public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue == true)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                return GamePads[i].Pressed(button);
            }
            else
            {
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }

        #region Keyboard

        public class KeyboardData
        {
            public KeyboardState PreviousState;
            public KeyboardState CurrentState;

            internal KeyboardData()
            {
                PreviousState = new KeyboardState();
                CurrentState = new KeyboardState();
            }

            internal void Update()
            {
                PreviousState = CurrentState;
                CurrentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            }

            public bool Check(Keys key)
            {
                return CurrentState.IsKeyDown(key);
            }

            public bool Pressed(Keys key)
            {
                return CurrentState.IsKeyDown(key) && !PreviousState.IsKeyDown(key);
            }

            public bool Released(Keys key)
            {
                return !CurrentState.IsKeyDown(key) && PreviousState.IsKeyDown(key);
            }

            public bool KeyCheck()
            {
                if (CurrentState.GetPressedKeys().Length > 0)
                {
                    return true;
                }

                return false;
            }

            public bool IsNewKeyPress(Keys key, out PlayerIndex playerIndex)
            {
                playerIndex = PlayerIndex.One;
                return Pressed(key);
            }
        }

        #endregion

        #region Mouse

        public class MouseData
        {
            public MouseState PreviousState;
            public MouseState CurrentState;

            internal MouseData()
            {
                PreviousState = new MouseState();
                CurrentState = new MouseState();
            }

            internal void Update()
            {
                PreviousState = CurrentState;
                CurrentState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            }

            public bool CheckLeftButton {
                get { return CurrentState.LeftButton == ButtonState.Pressed; }
            }

            public bool CheckRightButton {
                get { return CurrentState.RightButton == ButtonState.Pressed; }
            }

            public bool CheckMiddleButton {
                get { return CurrentState.MiddleButton == ButtonState.Pressed; }
            }

            public bool PressedLeftButton {
                get { return CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released; }
            }

            public bool PressedRightButton {
                get { return CurrentState.RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released; }
            }

            public bool PressedMiddleButton {
                get { return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Released; }
            }

            public bool ReleasedLeftButton {
                get { return CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed; }
            }

            public bool ReleasedRightButton {
                get { return CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed; }
            }

            public bool ReleasedMiddleButton {
                get { return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed; }
            }

            public int Wheel {
                get { return CurrentState.ScrollWheelValue; }
            }

            public int WheelDelta {
                get { return CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue; }
            }

            public bool WasMoved {
                get { return CurrentState.X != PreviousState.X || CurrentState.Y != PreviousState.Y; }
            }

            public int X
            {
                get { return CurrentState.X; }
                set { Microsoft.Xna.Framework.Input.Mouse.SetPosition(value, Y); }
            }

            public int Y
            {
                get { return CurrentState.Y; }
                set { Microsoft.Xna.Framework.Input.Mouse.SetPosition(X, value); }
            }

            public Vector2 Position
            {
                get { return new Vector2(X, Y); }
                set { Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)Math.Round(value.X), (int)Math.Round(value.Y)); }
            }
        }

        #endregion

        #region GamePad

        public class GamePadData
        {
            public PlayerIndex PlayerIndex { get; private set; }
            public GamePadState PreviousState;
            public GamePadState CurrentState;

            private float rumbleStrength;
            private float rumbleTime;

            internal GamePadData(PlayerIndex playerIndex)
            {
                PlayerIndex = playerIndex;
            }

            internal void Update(GameTime gameTime)
            {
                float elapsed = gameTime.ElapsedGameTime.Milliseconds;


                PreviousState = CurrentState;
                CurrentState = Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex, GamePadDeadZone.Circular);

                if (rumbleTime > 0)
                {
                    rumbleTime -= Engine.Engine.DeltaTime;

                    if (rumbleTime <= 0)
                    {
                        GamePad.SetVibration(PlayerIndex, 0, 0);
                    }
                }
            }

            public void Rumble(float strength, float time)
            {
                if (rumbleTime <= 0 || strength > rumbleStrength || (strength == rumbleStrength && time > rumbleTime))
                {
                    GamePad.SetVibration(PlayerIndex, strength, strength);
                    rumbleStrength = strength;
                    rumbleTime = time;
                }
            }

            public void StopRumble()
            {
                GamePad.SetVibration(PlayerIndex, 0, 0);
                rumbleTime = 0;
            }

            public bool Attached {
                get { return CurrentState.IsConnected; }
            }

            public bool Check(Buttons button) {
                return CurrentState.IsButtonDown(button);
            }

            public bool Pressed(Buttons button) {
                return CurrentState.IsButtonDown(button) && PreviousState.IsButtonUp(button);
            }

            public bool Released(Buttons button) {
                return CurrentState.IsButtonUp(button) && PreviousState.IsButtonDown(button);
            }

            public bool ButtonCheck()
            {
                var buttonList = (Buttons[])Enum.GetValues(typeof(Buttons));

                foreach (var button in buttonList)
                {
                    if (CurrentState.IsButtonDown(button))
                        return true;
                }

                return false;
            }

            public Vector2 GetLeftStick() {
                return CurrentState.ThumbSticks.Left;
            }

            public Vector2 GetLeftStick(float deadzone)
            {
                if (CurrentState.ThumbSticks.Left.LengthSquared() <
                    deadzone * deadzone)
                {
                    return Vector2.Zero;
                }
                return CurrentState.ThumbSticks.Left;
            }

            public Vector2 GetRightStick() {
                return CurrentState.ThumbSticks.Right;
            }

            public Vector2 GetRightStick(float deadzone)
            {
                if (CurrentState.ThumbSticks.Right.LengthSquared()
                    < deadzone * deadzone)
                {
                    return Vector2.Zero;
                }
                return CurrentState.ThumbSticks.Right;
            }

            public bool LeftStickLeftCheck(float deadzone) {
                return CurrentState.ThumbSticks.Left.X <= -deadzone;
            }

            public bool LeftStickLeftPressed(float deadzone) {
                return CurrentState.ThumbSticks.Left.X <= -deadzone && PreviousState.ThumbSticks.Left.X > -deadzone;
            }

            public bool LeftStickLeftReleased(float deadzone) {
                return CurrentState.ThumbSticks.Left.X > -deadzone && PreviousState.ThumbSticks.Left.X <= -deadzone;
            }

            public bool LeftStickRightCheck(float deadzone) {
                return CurrentState.ThumbSticks.Left.X >= deadzone;
            }

            public bool LeftStickRightPressed(float deadzone) {
                return CurrentState.ThumbSticks.Left.X >= deadzone && PreviousState.ThumbSticks.Left.X < deadzone;
            }

            public bool LeftStickRightReleased(float deadzone) {
                return CurrentState.ThumbSticks.Left.X < deadzone && PreviousState.ThumbSticks.Left.X >= deadzone;
            }

            public bool LeftStickDownCheck(float deadzone) {
                return CurrentState.ThumbSticks.Left.Y <= -deadzone;
            }

            public bool LeftStickDownPressed(float deadzone) {
                return CurrentState.ThumbSticks.Left.Y > -deadzone && PreviousState.ThumbSticks.Left.Y > -deadzone;
            }

            public bool LeftStickDownReleased(float deadzone) {
                return CurrentState.ThumbSticks.Left.Y > -deadzone && PreviousState.ThumbSticks.Left.Y <= -deadzone;
            }

            public bool LeftStickUpCheck(float deadzone) {
                return CurrentState.ThumbSticks.Left.Y >= deadzone;
            }

            public bool LeftStickUpPressed(float deadzone) {
                return CurrentState.ThumbSticks.Left.Y >= deadzone && PreviousState.ThumbSticks.Left.Y < deadzone;
            }

            public bool LeftStickUpReleased(float deadzone) {
                return CurrentState.ThumbSticks.Left.Y < deadzone && PreviousState.ThumbSticks.Left.Y >= deadzone;
            }

            public float LeftStickHorizontal(float deadzone)
            {
                float horizontal = CurrentState.ThumbSticks.Left.X;
                if (Math.Abs(horizontal) < deadzone)
                {
                    return 0;
                }
                else
                {
                    return horizontal;
                }
            }

            public float LeftStickVertical(float deadzone)
            {
                float vertical = CurrentState.ThumbSticks.Left.Y;
                if (Math.Abs(vertical) < deadzone)
                {
                    return 0.0f;
                }
                else
                {
                    return vertical;
                }
            }


            public bool RightStickLeftCheck(float deadzone) {
                return CurrentState.ThumbSticks.Right.X <= -deadzone;
            }

            public bool RightStickLeftPressed(float deadzone) {
                return CurrentState.ThumbSticks.Right.X <= -deadzone && PreviousState.ThumbSticks.Right.X > -deadzone;
            }

            public bool RightStickLeftReleased(float deadzone) {
                return CurrentState.ThumbSticks.Right.X > -deadzone && PreviousState.ThumbSticks.Right.X <= -deadzone;
            }

            public bool RightStickRightCheck(float deadzone) {
                return CurrentState.ThumbSticks.Right.X >= deadzone;
            }

            public bool RightStickRightPressed(float deadzone) {
                return CurrentState.ThumbSticks.Right.X >= deadzone && PreviousState.ThumbSticks.Right.X < deadzone;
            }

            public bool RightStickRightReleased(float deadzone) {
                return CurrentState.ThumbSticks.Right.X < deadzone && PreviousState.ThumbSticks.Right.X >= deadzone;
            }

            public bool RightStickDownCheck(float deadzone) {
                return CurrentState.ThumbSticks.Right.Y <= -deadzone;
            }

            public bool RightStickDownPressed(float deadzone) {
                return CurrentState.ThumbSticks.Right.Y > -deadzone && PreviousState.ThumbSticks.Right.Y > -deadzone;
            }

            public bool RightStickDownReleased(float deadzone) {
                return CurrentState.ThumbSticks.Right.Y > -deadzone && PreviousState.ThumbSticks.Right.Y <= -deadzone;
            }

            public bool RightStickUpCheck(float deadzone) {
                return CurrentState.ThumbSticks.Right.Y >= deadzone;
            }

            public bool RightStickUpPressed(float deadzone) {
                return CurrentState.ThumbSticks.Right.Y >= deadzone && PreviousState.ThumbSticks.Right.Y < deadzone;
            }

            public bool RightStickUpReleased(float deadzone) {
                return CurrentState.ThumbSticks.Right.Y < deadzone && PreviousState.ThumbSticks.Right.Y >= deadzone;
            }

            public float RightStickHorizontal(float deadzone)
            {
                float horizontal = CurrentState.ThumbSticks.Right.X;
                if (Math.Abs(horizontal) < deadzone)
                {
                    return 0;
                }
                else
                {
                    return horizontal;
                }
            }

            public float RightStickVertical(float deadzone)
            {
                float vertical = CurrentState.ThumbSticks.Right.Y;
                if (Math.Abs(vertical) < deadzone)
                {
                    return 0.0f;
                }
                else
                {
                    return vertical;
                }
            }

            public int DPadHorizontal
            {
                get
                {
                    return CurrentState.DPad.Right == ButtonState.Pressed ? 1 :
                           (CurrentState.DPad.Left == ButtonState.Pressed ? -1 : 0);
                }
            }

            public int DPadVertical
            {
                get
                {
                    return CurrentState.DPad.Down == ButtonState.Pressed ? 1 :
                          (CurrentState.DPad.Up == ButtonState.Pressed ? -1 : 0);
                }
            }

            public Vector2 DPad {
                get { return new Vector2(DPadHorizontal, DPadVertical); }
            }

            public bool DPadLeftCheck {
                get { return CurrentState.DPad.Left == ButtonState.Pressed; }
            }

            public bool DPadLeftPressed {
                get { return CurrentState.DPad.Left == ButtonState.Pressed && PreviousState.DPad.Left == ButtonState.Released; }
            }

            public bool DPadLeftReleased {
                get { return CurrentState.DPad.Left == ButtonState.Released && PreviousState.DPad.Left == ButtonState.Pressed; }
            }

            public bool DPadRightCheck {
                get { return CurrentState.DPad.Right == ButtonState.Pressed; }
            }

            public bool DPadRightPressed {
                get { return CurrentState.DPad.Right == ButtonState.Pressed && PreviousState.DPad.Right == ButtonState.Released; }
            }

            public bool DPadRightReleased {
                get { return CurrentState.DPad.Right == ButtonState.Released && PreviousState.DPad.Right == ButtonState.Pressed; }
            }

            public bool DPadDownCheck {
                get { return CurrentState.DPad.Down == ButtonState.Pressed; }
            }

            public bool DPadDownPressed {
                get { return CurrentState.DPad.Down == ButtonState.Pressed && PreviousState.DPad.Down == ButtonState.Released; }
            }

            public bool DPadDownReleased {
                get { return CurrentState.DPad.Down == ButtonState.Released && PreviousState.DPad.Down == ButtonState.Pressed; }
            }

            public bool DPadUpCheck {
                get { return CurrentState.DPad.Up == ButtonState.Pressed; }
            }

            public bool DPadUpPressed {
                get { return CurrentState.DPad.Up == ButtonState.Pressed && PreviousState.DPad.Up == ButtonState.Released; }
            }

            public bool DPadUpReleased {
                get { return CurrentState.DPad.Up == ButtonState.Released && PreviousState.DPad.Up == ButtonState.Pressed; }
            }

            public bool LeftTriggerCheck(float threshold) {
                return CurrentState.Triggers.Left >= threshold;
            }

            public bool LeftTriggerPressed(float threshold) {
                return CurrentState.Triggers.Left >= threshold && PreviousState.Triggers.Left < threshold;
            }

            public bool LeftTriggerReleased(float threshold) {
                return CurrentState.Triggers.Left < threshold && PreviousState.Triggers.Left >= threshold;
            }

            public bool RightTriggerCheck(float threshold) {
                return CurrentState.Triggers.Right >= threshold;
            }

            public bool RightTriggerPressed(float threshold) {
                return CurrentState.Triggers.Right >= threshold && PreviousState.Triggers.Right < threshold;
            }

            public bool RightTriggerReleased(float threshold) {
                return CurrentState.Triggers.Right < threshold && PreviousState.Triggers.Right >= threshold;
            }

        }

        #endregion
    }
}
