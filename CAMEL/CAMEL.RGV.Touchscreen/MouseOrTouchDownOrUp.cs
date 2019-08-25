﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CAMEL.RGV.Touchscreen
{
    public static class MouseOrTouchDownOrUp
    {
        public static void MouseOrTouchDown(object sender)
        {
            if (!Current.RGV.IsConnected)
            {
                MessageBox.Show("尚未连接RGV PLC！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var msg = string.Empty;
            var button = sender as Label;
            var btnContent = button.Content.ToString();
            var addr = Parameter.GetAddr(btnContent);

            var val = 0;

            if (Parameter.GetType(btnContent) == "点动")
            {
                addr = Parameter.GetAddr(btnContent);
                Current.RGV.Write(addr, (short)1, out msg);
                button.Background = Brushes.LightGray;
            }
            else if (Parameter.GetType(btnContent) == "单次写入")
            {
                if (Current.RGV.Write(addr, (short)1, out msg))
                {
                    button.Background = Brushes.LightGray;
                }
            }
            else if (Parameter.GetType(btnContent) == "使能")
            {
                Current.RGV.Read(addr, out short oldVal, out msg);
                val = oldVal == 1 ? 2 : 1;
                if (Current.RGV.Write(addr, (short)val, out msg))
                {
                    if (btnContent == "货叉原点")
                    {
                        Current.RGV.货叉原点 = val == 1;
                    }
                    else if (btnContent == "行走测试")
                    {
                        Current.RGV.行走测试 = val == 1;
                    }
                    else if (btnContent == "升降1测试")
                    {
                        Current.RGV.升降1测试 = val == 1;
                    }
                    else if (btnContent == "升降2测试")
                    {
                        Current.RGV.升降2测试 = val == 1;
                    }
                    else if (btnContent == "货叉测试")
                    {
                        Current.RGV.货叉测试 = val == 1;
                    }
                    else if (btnContent == "参数写入")
                    {
                        Current.RGV.参数写入 = val == 1;
                    }
                    else if (btnContent == "蜂鸣停止")
                    {
                        Current.RGV.蜂鸣停止 = val == 1;
                    }
                }
            }
            else if (Parameter.GetType(btnContent) == "手动自动状态")
            {
                if (btnContent == "调度无效")
                {
                    val = Current.RGV.调度无效 ? 1 : 2;
                    if (Current.RGV.Write(addr, (short)val, out msg))
                    {
                        Current.RGV.调度无效 = val == 2;
                    }
                }
                else
                {
                    val = Current.RGV.手动状态 ? 2 : 1;
                    if (Current.RGV.Write(addr, (short)val, out msg))
                    {
                        Current.RGV.手动状态 = val == 1;
                    }
                }

            }
            else
            {
                button.Background = Brushes.LightGray;
            }

        }

        public static void MouseOrTouchUp(object sender)
        {
            if (!Current.RGV.IsConnected)
            {
                return;
            }

            var button = sender as Label;
            var btnContent = button.Content.ToString();

            if (Parameter.GetType(btnContent) == "点动")
            {
                var addr = Parameter.GetAddr(btnContent);
                Current.RGV.Write(addr, (short)2, out string msg);
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF367BB5"));
            }
            else if (Parameter.GetType(btnContent) == "单次写入")
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF367BB5"));
            }
        }

    }
}
