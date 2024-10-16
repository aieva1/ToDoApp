﻿using DailyApp.WPF.MsgEvents;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DailyApp.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginUC.xaml
    /// </summary>
    public partial class LoginUC : UserControl
    {
        private readonly IEventAggregator Aggregator;
        public LoginUC(IEventAggregator _Aggregator)
        {
            InitializeComponent();
            Aggregator = _Aggregator;
            Aggregator.GetEvent<MsgEvent>().Subscribe(Sub);

        }

        private void Sub(string obj)
        {
           RegLoginBar.MessageQueue.Enqueue(obj);
        }
    }
}
