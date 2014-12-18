using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PocoGen.Gui.Presentation.Controls
{
    internal class ObjectDbTextEdit : Control
    {
        public static readonly DependencyProperty DbElementNameProperty =
            DependencyProperty.Register("DbElementName", typeof(string), typeof(ObjectDbTextEdit), new PropertyMetadata(null));

        public static readonly DependencyProperty CodeElementNameProperty =
            DependencyProperty.Register("CodeElementName", typeof(string), typeof(ObjectDbTextEdit), new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public static readonly DependencyProperty IsInEditModeProperty =
            DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(ObjectDbTextEdit), new PropertyMetadata(false));

        public string DbElementName
        {
            get { return (string)this.GetValue(ObjectDbTextEdit.DbElementNameProperty); }
            set { this.SetValue(ObjectDbTextEdit.DbElementNameProperty, value); }
        }

        public string CodeElementName
        {
            get { return (string)this.GetValue(ObjectDbTextEdit.CodeElementNameProperty); }
            set { this.SetValue(ObjectDbTextEdit.CodeElementNameProperty, value); }
        }

        public bool IsInEditMode
        {
            get { return (bool)this.GetValue(ObjectDbTextEdit.IsInEditModeProperty); }
            set { this.SetValue(ObjectDbTextEdit.IsInEditModeProperty, value); }
        }

        static ObjectDbTextEdit()
        {
            ObjectDbTextEdit.DefaultStyleKeyProperty.OverrideMetadata(typeof(ObjectDbTextEdit), new FrameworkPropertyMetadata(typeof(ObjectDbTextEdit)));
        }

        public ObjectDbTextEdit()
        {
        }

        private TextBox lastTextBox;
        private TextBlock lastTextBlock;
        private MenuItem lastEditMenuItem;
        private MenuItem lastCopyCodeElementNameMenuItem;
        private MenuItem lastCopyDatabaseObjectNameMenuItem;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.IsInEditMode)
            {
                if (this.lastCopyDatabaseObjectNameMenuItem != null)
                {
                    this.lastCopyDatabaseObjectNameMenuItem.Click -= this.CopyDatabaseObjectNameMenuItem_Click;
                    this.lastCopyDatabaseObjectNameMenuItem = null;
                }

                if (this.lastCopyCodeElementNameMenuItem != null)
                {
                    this.lastCopyCodeElementNameMenuItem.Click -= this.CopyCodeElementNameMenuItem_Click;
                    this.lastCopyCodeElementNameMenuItem = null;
                }

                if (this.lastEditMenuItem != null)
                {
                    this.lastEditMenuItem.Click -= this.EditMenuItem_Click;
                    this.lastEditMenuItem = null;
                }

                if (this.lastTextBlock != null)
                {
                    this.lastTextBlock.MouseDown -= this.TextBlock_MouseDown;
                    this.lastTextBlock = null;
                }

                this.lastTextBox = (TextBox)this.Template.FindName("PART_Editor", this);
                this.lastTextBox.LostFocus += this.TextBox_LostFocus;
                this.lastTextBox.KeyDown += this.TextBox_KeyDown;
                this.lastTextBox.Select(this.lastTextBox.Text.Length, 0);
                this.lastTextBox.Focus();
            }
            else
            {
                if (this.lastTextBox != null)
                {
                    this.lastTextBox.LostFocus -= this.TextBox_LostFocus;
                    this.lastTextBox.KeyDown -= this.TextBox_KeyDown;
                    this.lastTextBox = null;
                }

                this.lastTextBlock = (TextBlock)this.Template.FindName("PART_Display", this);
                this.lastTextBlock.MouseDown += this.TextBlock_MouseDown;
                
                this.lastEditMenuItem = (MenuItem)this.Template.FindName("PART_Edit", this);
                this.lastEditMenuItem.Click += this.EditMenuItem_Click;

                this.lastCopyCodeElementNameMenuItem = (MenuItem)this.Template.FindName("PART_CopyCodeElementName", this);
                this.lastCopyCodeElementNameMenuItem.Click += this.CopyCodeElementNameMenuItem_Click;

                this.lastCopyDatabaseObjectNameMenuItem = (MenuItem)this.Template.FindName("PART_CopyDbElementName", this);
                this.lastCopyDatabaseObjectNameMenuItem.Click += this.CopyDatabaseObjectNameMenuItem_Click;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.F2 && !this.IsInEditMode)
            {
                this.IsInEditMode = true;
                e.Handled = true;
            }
        }

        private void CopyDatabaseObjectNameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.DbElementName);
        }

        private void CopyCodeElementNameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.CodeElementName);
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.IsInEditMode = true;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
            {
                this.IsInEditMode = true;
                e.Handled = true;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter && e.Key != Key.Escape)
            {
                return;
            }

            this.IsInEditMode = false;
            e.Handled = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsInEditMode = false;
        }
    }
}