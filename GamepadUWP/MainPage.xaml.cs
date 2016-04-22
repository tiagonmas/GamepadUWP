using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Gaming.Input;
using System.Threading;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.Foundation;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GamepadUWP
{
        /// <summary>
        /// An empty page that can be used on its own or navigated to within a Frame.
        /// </summary>
        public sealed partial class MainPage : Page
        {
                Gamepad controller;
                DispatcherTimer dispatcherTimer;
                TimeSpan period = TimeSpan.FromMilliseconds(100);

                public MainPage()
                {

                        this.InitializeComponent();

                        dispatcherTimer = new DispatcherTimer();
                        dispatcherTimer.Tick += dispatcherTimer_Tick;
                        dispatcherTimer.Start();

                        //public static event EventHandler<Gamepad> GamepadAdded
                        Gamepad.GamepadAdded += Gamepad_GamepadAdded;
                        //public static event EventHandler<Gamepad> GamepadRemoved
                        Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
                        //public event TypedEventHandler<IGameController, Headset> HeadsetConnected


                }

                #region EventHandlers

                private async void Gamepad_GamepadAdded(object sender, Gamepad e)
                {
                        e.HeadsetConnected += E_HeadsetConnected;
                        e.HeadsetDisconnected += E_HeadsetDisconnected;
                        e.UserChanged += E_UserChanged;
                        await Log("Gamepad Added");
                }

                private async void Gamepad_GamepadRemoved(object sender, Gamepad e)
                {
                        await Log("Gamepad Removed");
                }
                private async void E_UserChanged(IGameController sender, Windows.System.UserChangedEventArgs args)
                {
                        await Log("User changed");
                }

                private async void E_HeadsetDisconnected(IGameController sender, Headset args)
                {
                        await Log("HeadsetDisconnected");
                }

                private async void E_HeadsetConnected(IGameController sender, Headset args)
                {
                        await Log("HeadsetConnected");
                }

                #endregion

                
                private void dispatcherTimer_Tick(object sender, object e)
                {
                        if (Gamepad.Gamepads.Count > 0)
                        {
                                controller = Gamepad.Gamepads.First();
                                var reading = controller.GetCurrentReading();

                                pbLeftThumbstickX.Value = reading.LeftThumbstickX;
                                pbLeftThumbstickY.Value = reading.LeftThumbstickY;
                                
                                pbRightThumbstickX.Value = reading.RightThumbstickX;
                                pbRightThumbstickY.Value = reading.RightThumbstickY;

                                pbRightThumbstickY.Value = reading.RightThumbstickY;

                                pbLeftTrigger.Value = reading.LeftTrigger;
                                pbRightTrigger.Value = reading.RightTrigger;

                                //https://msdn.microsoft.com/en-us/library/windows/apps/windows.gaming.input.gamepadbuttons.aspx
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.A), lblA);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.B), lblB);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.X), lblX);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.Y), lblY);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.Menu), lblMenu);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.DPadLeft), lblDPadLeft);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.DPadRight), lblDPadRight);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.DPadUp), lblDPadUp);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.DPadDown), lblDPadDown);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.View), lblView);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.RightThumbstick), ellRightThumbstick);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.LeftThumbstick), ellLeftThumbstick);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.LeftShoulder), rectLeftShoulder);
                                ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.RightShoulder), recRightShoulder);
                                
                        }

                }

                #region Helper methods
                private void ChangeVisibility(bool flag, UIElement elem)
                {
                        if (flag) 
                        { elem.Visibility = Visibility.Visible; }
                        else
                        { elem.Visibility = Visibility.Collapsed; }
                }

                private async Task Log(String txt)
                {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, 
                        () =>
                        {
                                txtEvents.Text = DateTime.Now.ToString("hh:mm:ss.fff ")+txt + "\n" + txtEvents.Text;
                        }
                        );
                        
                }
                #endregion

        }
}

