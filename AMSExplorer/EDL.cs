﻿//----------------------------------------------------------------------------------------------
//    Copyright 2015 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//---------------------------------------------------------------------------------------------

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
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.IO;
using System.Threading;

namespace AMSExplorer
{
    public partial class EDL : Form
    {
        private BindingList<ExplorerEDLEntryInOut> TimeCodeList = new BindingList<ExplorerEDLEntryInOut>();

        public EDL()
        {
            InitializeComponent();
            this.Icon = Bitmaps.Azure_Explorer_ico;
        }

        private void EDL_Load(object sender, EventArgs e)
        {
            dataGridViewEDL.DataSource = TimeCodeList;

        }
              

        public void AddEDLEntry(ExplorerEDLEntryInOut entry)
        {
            TimeCodeList.Add(entry);
        }

        public List<ExplorerEDLEntryInOut> GetEDLEntries()
        {
            return TimeCodeList.ToList();
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (dataGridViewEDL.SelectedRows.Count == 1 && dataGridViewEDL.SelectedRows[0].Index > 0)
            {
                int index = dataGridViewEDL.SelectedRows[0].Index;
                var backup = TimeCodeList[index - 1];
                TimeCodeList[index - 1] = TimeCodeList[index];
                TimeCodeList[index] = backup;
                dataGridViewEDL.ClearSelection();
                dataGridViewEDL.Rows[index - 1].Selected = true;
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (dataGridViewEDL.SelectedRows.Count == 1 && dataGridViewEDL.SelectedRows[0].Index < TimeCodeList.Count - 1)
            {
                int index = dataGridViewEDL.SelectedRows[0].Index;
                var backup = TimeCodeList[index + 1];
                TimeCodeList[index + 1] = TimeCodeList[index];
                TimeCodeList[index] = backup;
                dataGridViewEDL.ClearSelection();
                dataGridViewEDL.Rows[index + 1].Selected = true;
            }
        }

        private void buttonDelIP_Click(object sender, EventArgs e)
        {
            if (dataGridViewEDL.SelectedRows.Count == 1)
            {
                TimeCodeList.RemoveAt(dataGridViewEDL.SelectedRows[0].Index);
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridViewEDL_SelectionChanged(object sender, EventArgs e)
        {
            buttonDelEntry.Enabled = dataGridViewEDL.SelectedRows.Count > 0;
            buttonUp.Enabled = dataGridViewEDL.SelectedRows.Count > 0 && dataGridViewEDL.SelectedRows[0].Index > 1;
            buttonDown.Enabled = dataGridViewEDL.SelectedRows.Count > 0 && dataGridViewEDL.SelectedRows[0].Index < dataGridViewEDL.Rows.Count-1;

        }

        private void EDL_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            Hide();
        }
    }


    class ButtonEDL : Button
    {
        EDL myEDL;

        public ButtonEDL()
        {
            this.Click += ButtonEDL_Click;
        }

        public void Initialize()
        {
            myEDL = new EDL();
        }

        public void ButtonEDL_Click(object sender, EventArgs e)
        {
            myEDL.Show();
        }

        public void AddEDLEntry(ExplorerEDLEntryInOut entry)
        {
            myEDL.AddEDLEntry(entry);
        }

        public List<ExplorerEDLEntryInOut> GetEDLEntries()
        {
            return myEDL.GetEDLEntries();
        }

        public string GetXML()
        {
            return null;// myEDL.TextData;
        }
    }

    public class ExplorerEDLEntryInOut
    {
        public TimeSpan In { get; set; }
        public TimeSpan Out { get; set; }
    }

}
