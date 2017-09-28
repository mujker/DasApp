using System;
using System.Windows;
using DasApp.Models;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace DasApp
{
    public class ContextMenuBehavior
    {
        private readonly RadGridView gridView = null;
        private readonly FrameworkElement contextMenu = null;

        public static readonly DependencyProperty ContextmenuPropery =
        DependencyProperty.RegisterAttached("ContextMenu", typeof(FrameworkElement), typeof(ContextMenuBehavior),
            new PropertyMetadata(new PropertyChangedCallback(OnIsEnabledPropertyChanged)));

        public static void SetContextMenu(DependencyObject dependencyObject, FrameworkElement contextmenu)
        {
            dependencyObject.SetValue(ContextmenuPropery, contextmenu);
        }

        public static FrameworkElement GetContextMenu(DependencyObject dependencyObject)
        {
            return (FrameworkElement)dependencyObject.GetValue(ContextmenuPropery);
        }

        public static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            RadGridView grid = dependencyObject as RadGridView;
            FrameworkElement contextMenu = e.NewValue as FrameworkElement;

            if (grid != null && contextMenu != null)
            {
                ContextMenuBehavior behavior = new ContextMenuBehavior(grid, contextMenu);
            }
        }

        public ContextMenuBehavior(RadGridView grid, FrameworkElement contextMenu)
        {
            this.gridView = grid;
            this.contextMenu = contextMenu;

            (contextMenu as RadContextMenu).Opened += RadContextMenu_Opened;
            (contextMenu as RadContextMenu).ItemClick += RadContextMenu_ItemClick;
        }

        private void RadContextMenu_ItemClick(object sender, RadRoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            RadMenuItem clickedItem = e.OriginalSource as RadMenuItem;
            GridViewRow row = menu.GetClickedElement<GridViewRow>();

            if (clickedItem != null && row != null)
            {
                string header = Convert.ToString(clickedItem.Header);

                switch (header)
                {
                    case "解密":
                        YXJK_JKD jkd = row.Item as YXJK_JKD;
                        if (string.IsNullOrEmpty(jkd?.JKD_VALUE) || jkd.JKD_VALUE.Equals("#"))
                        {
                            break;
                        }
                        new DeCodeWin(jkd.JKD_VALUE).Show();
                        //                        gridView.BeginInsert();
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
            RadContextMenu menu = (RadContextMenu)sender;
            GridViewRow row = menu.GetClickedElement<GridViewRow>();

            if (row != null)
            {
                row.IsSelected = row.IsCurrent = true;
                GridViewCell cell = menu.GetClickedElement<GridViewCell>();
                if (cell != null)
                {
                    cell.IsCurrent = true;
                }
            }
            else
            {
                menu.IsOpen = false;
            }
        }
    }
}