using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;

namespace ExtractHtml
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
        HtmlNode root;
      
        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                htmlPath.Text = ofd.FileName;
                if (System.IO.File.Exists(htmlPath.Text.Trim()))
                {
                    htmlDoc.Load(htmlPath.Text.Trim(),true);
                    root = htmlDoc.DocumentNode.SelectSingleNode("html");
                    if (htmlDoc.DocumentNode != null)
                    {
                        textXPath.Text = root.XPath;
                        if (htmlDoc.DocumentNode.ChildNodes.Count > 0)
                        {                           
                            for (int i = 0; i < root.ChildNodes.Count; i++)
                            {
                                combChildren.Items.Add(i);
                                //string content = root.ChildNodes[i].InnerHtml.Trim();
                                //content.Replace("\r\n", "");
                                //if (!string.IsNullOrWhiteSpace(content))
                                //{
                                    
                                //}
                            }
                        }
                    }
                }          
            }
        }

     
        private void gotoNode_Click(object sender, EventArgs e)
        {
            if (combChildren.SelectedIndex!=-1)
            {
                HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode(textXPath.Text).ChildNodes[combChildren.SelectedIndex];
                textXPath.Clear();
                textXPath.Text = currentNode.XPath;
                combChildren.Items.Clear();                
                for (int i = 0; i < currentNode.ChildNodes.Count; i++)
                {
                    //string content = currentNode.ChildNodes[i].InnerHtml.Trim();
                    //content.Replace("\r\n", "");
                    //if (!string.IsNullOrWhiteSpace(content))
                    //{
                    //    combChildren.Items.Add(i);
                    //}
                    combChildren.Items.Add(i);
                }

                if (combChildren.Items.Count>0)
                {
                    combChildren.SelectedIndex = 0;                    
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (combChildren.SelectedIndex != -1)
            {
                HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode(textXPath.Text);
                textBox4.Text = currentNode.InnerHtml;
            }
        }

       

        private void combChildren_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combChildren.SelectedIndex != -1)
            {
                HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode(textXPath.Text);
                HtmlNode tempnode = currentNode.ChildNodes[combChildren.SelectedIndex];
                textBox4.Clear();
                textBox4.Text = tempnode.InnerHtml;
            }        
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode(textXPath.Text);
            textXPath.Clear();
            textXPath.Text = currentNode.XPath;
            combChildren.Items.Clear();
            for (int i = 0; i < currentNode.ChildNodes.Count; i++)
            {
                //string content = currentNode.ChildNodes[i].InnerHtml.Trim();
                //content.Replace("\r\n", "");
                //if (!string.IsNullOrWhiteSpace(content))
                //{
                //    combChildren.Items.Add(i);
                //}
                combChildren.Items.Add(i);
            }
        }

        private void textXPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==(char)13)
            {
                HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode(textXPath.Text);
                textXPath.Clear();
                textXPath.Text = currentNode.XPath;
                combChildren.Items.Clear();
                for (int i = 0; i < currentNode.ChildNodes.Count; i++)
                {
                    combChildren.Items.Add(i);
                }
            }
        }
    }
}
