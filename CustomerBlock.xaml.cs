using Data.Types;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Palmer_Disposal___Management_System
{
    /// <summary>
    /// Interaction logic for CustomerBlock.xaml
    /// </summary>
    public partial class CustomerBlock : UserControl
    {

        public int index { get; set; }
        public bool clicked = false;

        public CustomerBlock()
        {
            InitializeComponent();
        }

        public void GetCustomerData(Customer customer)
        {
            Number.Content = customer.accountNumber;
            Name.Content = customer.name;
            Address.Content = customer.address;
            Phone.Content = customer.phoneNumber;
            Town.Content = customer.town;
            Comment.Content = customer.comment;
            index = customer.index;
        }

        //Custom Click Event

        public static RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomerBlock));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        protected virtual void OnClick()
        {
            RoutedEventArgs args = new RoutedEventArgs(ClickEvent, this);
            RaiseEvent(args);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            OnClick();
        }





        //Background Color
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom("#92c8f3");
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if(clicked == false)
            {
                this.Background = Brushes.Transparent;
            }
        }

    }
}
