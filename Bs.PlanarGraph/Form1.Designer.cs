﻿namespace Bs.PlanarGraph
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.graphView = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonInitGraph = new System.Windows.Forms.Button();
            this.textBoxInitGraph = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonApplyPlanarityStep = new System.Windows.Forms.Button();
            this.buttonSimplifyStep = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.graphView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphView
            // 
            this.graphView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphView.Location = new System.Drawing.Point(3, 3);
            this.graphView.Name = "graphView";
            this.tableLayoutPanel1.SetRowSpan(this.graphView, 3);
            this.graphView.Size = new System.Drawing.Size(396, 423);
            this.graphView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.graphView.TabIndex = 0;
            this.graphView.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonInitGraph);
            this.groupBox1.Controls.Add(this.textBoxInitGraph);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(405, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 137);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // buttonInitGraph
            // 
            this.buttonInitGraph.Location = new System.Drawing.Point(10, 107);
            this.buttonInitGraph.Name = "buttonInitGraph";
            this.buttonInitGraph.Size = new System.Drawing.Size(316, 23);
            this.buttonInitGraph.TabIndex = 2;
            this.buttonInitGraph.Text = "Initialize Graph";
            this.buttonInitGraph.UseVisualStyleBackColor = true;
            this.buttonInitGraph.Click += new System.EventHandler(this.buttonInitGraph_Click);
            // 
            // textBoxInitGraph
            // 
            this.textBoxInitGraph.Location = new System.Drawing.Point(10, 37);
            this.textBoxInitGraph.Multiline = true;
            this.textBoxInitGraph.Name = "textBoxInitGraph";
            this.textBoxInitGraph.Size = new System.Drawing.Size(316, 64);
            this.textBoxInitGraph.TabIndex = 1;
            this.textBoxInitGraph.Text = "Graph {1--2;2--3;3--4;4--5;5--1;1--3;1--4;2--4;2--5;3--5;5--6;1--6;2--7;1--7;6--7" +
    ";a--b;b--c;c--d;b--d;b--e;b--f;a--f;f--e;e--d;b--g;d--i;b--i;g--i;g--h;h--i;7--a" +
    "}";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Initial graph:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxStatus);
            this.groupBox2.Location = new System.Drawing.Point(405, 277);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(332, 128);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(7, 20);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.Size = new System.Drawing.Size(319, 104);
            this.textBoxStatus.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonApplyPlanarityStep);
            this.groupBox3.Controls.Add(this.buttonSimplifyStep);
            this.groupBox3.Location = new System.Drawing.Point(405, 155);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(332, 106);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Controls";
            // 
            // buttonApplyPlanarityStep
            // 
            this.buttonApplyPlanarityStep.Location = new System.Drawing.Point(10, 49);
            this.buttonApplyPlanarityStep.Name = "buttonApplyPlanarityStep";
            this.buttonApplyPlanarityStep.Size = new System.Drawing.Size(316, 23);
            this.buttonApplyPlanarityStep.TabIndex = 1;
            this.buttonApplyPlanarityStep.Text = "Apply Planarity Test Step";
            this.buttonApplyPlanarityStep.UseVisualStyleBackColor = true;
            this.buttonApplyPlanarityStep.Click += new System.EventHandler(this.buttonApplyPlanarityStep_Click);
            // 
            // buttonSimplifyStep
            // 
            this.buttonSimplifyStep.Location = new System.Drawing.Point(10, 20);
            this.buttonSimplifyStep.Name = "buttonSimplifyStep";
            this.buttonSimplifyStep.Size = new System.Drawing.Size(316, 23);
            this.buttonSimplifyStep.TabIndex = 0;
            this.buttonSimplifyStep.Text = "Apply Simplify Step";
            this.buttonSimplifyStep.UseVisualStyleBackColor = true;
            this.buttonSimplifyStep.Click += new System.EventHandler(this.buttonSimplifyStep_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.graphView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.47945F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.52055F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(805, 429);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 429);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Planarity Tester";
            ((System.ComponentModel.ISupportInitialize)(this.graphView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox graphView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxInitGraph;
        private System.Windows.Forms.Button buttonInitGraph;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSimplifyStep;
        private System.Windows.Forms.Button buttonApplyPlanarityStep;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

