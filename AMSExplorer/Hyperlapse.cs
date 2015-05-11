﻿//----------------------------------------------------------------------- 
// <copyright file="Hyperlapse.cs" company="Microsoft">Copyright (c) Microsoft Corporation. All rights reserved.</copyright> 
// <license>
// Azure Media Services Explorer Ver. 3.2
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//  
// http://www.apache.org/licenses/LICENSE-2.0 
//  
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License. 
// </license> 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.WindowsAzure.MediaServices.Client;
using System.Xml;
using System.Xml.Linq;


namespace AMSExplorer
{
    public partial class Hyperlapse : Form
    {
        private CloudMediaContext _context;

        public string HyperlapseInputAssetName
        {
            get
            {
                return labelAssetName.Text;
            }
            set
            {
                labelAssetName.Text = value;
            }
        }
        public string HyperlapseOutputAssetName
        {
            get
            {
                return textboxoutputassetname.Text;
            }
            set
            {
                textboxoutputassetname.Text = value;
            }
        }



        public string StorageSelected
        {
            get
            {
                return ((Item)comboBoxStorage.SelectedItem).Value;
            }
        }

        public string HyperlapseJobName
        {
            get
            {
                return textBoxJobName.Text;
            }
            set
            {
                textBoxJobName.Text = value;
            }
        }

        public int HyperlapseJobPriority
        {
            get
            {
                return (int)numericUpDownPriority.Value;
            }
            set
            {
                numericUpDownPriority.Value = value;
            }
        }

        public int HyperlapseStartFrame
        {
            get
            {
                return (int) numericUpDownStartFrame.Value;
            }
            set
            {
                numericUpDownStartFrame.Value = value;
            }
        }

        public int HyperlapseNumFrames
        {
            get
            {
                return (int)  numericUpDownNumFrames.Value;
            }
            set
            {
                numericUpDownNumFrames.Value = value;
            }
        }

        public int HyperlapseSpeed
        {
            get
            {
                return (int) numericUpDownSpeed.Value;
            }
            set
            {
                numericUpDownSpeed.Value = value;
            }
        }


        public string HyperlapseProcessorName
        {
            get
            {
                return processorlabel.Text;
            }
            set
            {
                processorlabel.Text = value;
            }
        }

        public Hyperlapse(CloudMediaContext context)
        {
            InitializeComponent();
            this.Icon = Bitmaps.Azure_Explorer_ico;
            _context = context;
        }


        private void Hyperlapse_Load(object sender, EventArgs e)
        {
            moreinfoprofilelink.Links.Add(new LinkLabel.Link(0, moreinfoprofilelink.Text.Length, Constants.LinkMoreInfoHyperlapse));
            linkLabelHowItWorks.Links.Add(new LinkLabel.Link(0, moreinfoprofilelink.Text.Length, Constants.LinkHowItWorksHyperlapse));
         
            foreach (var storage in _context.StorageAccounts)
            {
                comboBoxStorage.Items.Add(new Item(string.Format("{0} {1}", storage.Name, storage.IsDefault ? "(default)" : ""), storage.Name));
                if (storage.Name == _context.DefaultStorageAccount.Name) comboBoxStorage.SelectedIndex = comboBoxStorage.Items.Count - 1;
            }
        }

        public static string LoadAndUpdateHyperlapseConfiguration(string xmlFileName, int startFrame, int numFrames, int speed)
        {
            // Prepare the Hyperlapse xml
            XDocument doc = XDocument.Load(xmlFileName);

            XNamespace ns = "http://www.windowsazure.com/media/encoding/Preset/2014/03";

            var presetxml = doc.Element(ns + "Preset");

            var sourcexml = presetxml.Element(ns + "Sources").Element(ns + "Source");
            sourcexml.Attribute("StartFrame").SetValue(startFrame);
            sourcexml.Attribute("NumFrames").SetValue(numFrames);

            var speedxml = presetxml.Element(ns + "Options").Element(ns + "Speed");
            speedxml.SetValue(speed);

            return doc.Declaration.ToString() + doc.ToString();
        }

        private void moreinfoprofilelink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Send the URL to the operating system.
            Process.Start(e.Link.LinkData as string);
        }

        private void linkLabelHowItWorks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Send the URL to the operating system.
            Process.Start(e.Link.LinkData as string);
        }
    }
}
