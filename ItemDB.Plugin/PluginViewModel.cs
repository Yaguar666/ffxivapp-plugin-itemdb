﻿// ItemDB.Plugin
// PluginViewModel.cs
// 
// Copyright @2015 Violet Cheng - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Core.Memory.Enums;
using ItemDB.Plugin.Models;


namespace ItemDB.Plugin
{
    public class ItemShow
    {
        public String Key {get; set;}
        public String Character { get; set; }
        public String Location { get; set; }
        public uint Amount { get; set; }
       
    }
    internal sealed class PluginViewModel : INotifyPropertyChanged
    {
        //used for global static properties

        public event EventHandler<PopupResultEvent> PopupResultChanged = delegate { };
        public String ResultAmount
        {
            get { return _label; }
            set
            {
                _label = value;
                RaisePropertyChanged();
            }
        }
        private string _label;
        private ObservableCollection<ItemShow> _results;
        private ItemFilter _item;

        
        public ItemFilter Filter
        {
            get { return _item;} 
            set
            {
                if (_item == null)
                {
                    _item = new ItemFilter();
                }
                _item = value;
                _results.Clear();
                List<ItemShow> tList = ItemWarehouse.SearchItemInItemWarehouse(_item);
                ObservableCollection<ItemShow> tObserval = new ObservableCollection<ItemShow>(tList);
                int lResult = tObserval.Count;
                ResultItems = tObserval;
                ResultAmount = String.Format("搜索“{0}”: 找到{1}个结果", _item.Key, lResult.ToString());
            }
        }
        public ObservableCollection<ItemShow> ResultItems
        {
            get { return _results ?? (_results = new ObservableCollection<ItemShow>()); }
            set
            {
                if (_results == null)
                {
                    _results = new ObservableCollection<ItemShow>();
                }
                _results = value;
               
                RaisePropertyChanged();
            }
        }
        public void OnPopupResultChanged(PopupResultEvent e)
        {
            PopupResultChanged(this, e);
        }

        #region Property Bindings

        private static PluginViewModel _instance;
        private bool _enableHelpLabels;
        private Dictionary<string, string> _locale;

        public static PluginViewModel Instance
        {
            get { return _instance ?? (_instance = new PluginViewModel()); }
        }

        public Dictionary<string, string> Locale
        {
            get { return _locale ?? (_locale = new Dictionary<string, string>()); }
            set
            {
                _locale = value;
                RaisePropertyChanged();
            }
        }

        public static Dictionary<string, string> PluginInfo
        {
            get
            {
                var pluginInfo = new Dictionary<string, string>();
                pluginInfo.Add("Icon", "Logo.png");
                pluginInfo.Add("Name", AssemblyHelper.Name);
                pluginInfo.Add("Description", AssemblyHelper.Description);
                pluginInfo.Add("Copyright", AssemblyHelper.Copyright);
                pluginInfo.Add("Version", AssemblyHelper.Version.ToString());
                return pluginInfo;
            }
        }

        public bool EnableHelpLabels
        {
            get { return _enableHelpLabels; }
            set
            {
                _enableHelpLabels = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
