using System;
using System.Windows;
using DasApp.Models;
using DasApp.Socket;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace DasApp
{
    public class ContextMenuBehavior
    {
        public static readonly DependencyProperty ContextmenuPropery =
            DependencyProperty.RegisterAttached("ContextMenu", typeof(FrameworkElement), typeof(ContextMenuBehavior),
                new PropertyMetadata(OnIsEnabledPropertyChanged));

        private readonly FrameworkElement contextMenu;
        private readonly RadGridView gridView;

        public ContextMenuBehavior(RadGridView grid, FrameworkElement contextMenu)
        {
            gridView = grid;
            this.contextMenu = contextMenu;

            (contextMenu as RadContextMenu).Opened += RadContextMenu_Opened;
            (contextMenu as RadContextMenu).ItemClick += RadContextMenu_ItemClick;
        }

        public static void SetContextMenu(DependencyObject dependencyObject, FrameworkElement contextmenu)
        {
            dependencyObject.SetValue(ContextmenuPropery, contextmenu);
        }

        public static FrameworkElement GetContextMenu(DependencyObject dependencyObject)
        {
            return (FrameworkElement) dependencyObject.GetValue(ContextmenuPropery);
        }

        public static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            var grid = dependencyObject as RadGridView;
            var contextMenu = e.NewValue as FrameworkElement;

            if (grid != null && contextMenu != null)
            {
                var behavior = new ContextMenuBehavior(grid, contextMenu);
            }
        }

        private void RadContextMenu_ItemClick(object sender, RadRoutedEventArgs e)
        {
            var menu = (RadContextMenu) sender;
            var clickedItem = e.OriginalSource as RadMenuItem;
            var row = menu.GetClickedElement<GridViewRow>();

            if (clickedItem != null && row != null)
            {
                var header = Convert.ToString(clickedItem.Header);

                switch (header)
                {
                    case "解密":
                        var jkd = row.Item as YXJK_JKD;
                        if (string.IsNullOrEmpty(jkd?.JKD_VALUE))
                            return;
                        if (jkd.JKD_VALUE.TrimEnd('#').Length == 0)
                            return;
                        var dcw = new DeCodeWin();
                        var deStr = DataPacketCodec.Decode(jkd.JKD_VALUE.TrimEnd('#'), Settings.CryptKey);
                        dcw.Tb1.Text = deStr;
                        dcw.ShowDialog();
                        break;
//                    case "Edit":
//                        gridView.BeginEdit();
//                        break;
//                    case "Delete":
//                        gridView.Items.Remove(row.DataContext);
//                        break;
//                    default:
//                        break;
                }
            }
        }

        private void RadContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var menu = (RadContextMenu) sender;
            var row = menu.GetClickedElement<GridViewRow>();

            if (row != null)
            {
                row.IsSelected = row.IsCurrent = true;
                var cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                    cell.IsCurrent = true;
            }
            else
            {
                menu.IsOpen = false;
            }
        }
    }
}