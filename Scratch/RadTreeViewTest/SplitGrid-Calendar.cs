﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RadTreeViewTest.TestObject;

namespace RadTreeViewTest
{
    public partial class SplitGrid_Calendar : Form
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplitGrid_Calendar));

        Dictionary<string, Person> personHeap = new Dictionary<string,Person>();

        public SplitGrid_Calendar()
        {
            InitializeComponent();
            //radButton1.Image = (Image)resources.GetObject("Image1");
        }

        private void SplitGrid_Calendar_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                
            }
        }
    }
}
