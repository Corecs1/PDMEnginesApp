namespace PDMEnginesApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.Add_Engine = new System.Windows.Forms.Button();
            this.nameField = new System.Windows.Forms.TextBox();
            this.amountField = new System.Windows.Forms.TextBox();
            this.Add_Component = new System.Windows.Forms.Button();
            this.Rename = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 35);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(218, 214);
            this.treeView1.TabIndex = 0;
            // 
            // Add_Engine
            // 
            this.Add_Engine.Location = new System.Drawing.Point(262, 109);
            this.Add_Engine.Name = "Add_Engine";
            this.Add_Engine.Size = new System.Drawing.Size(133, 23);
            this.Add_Engine.TabIndex = 1;
            this.Add_Engine.Text = "Добавить двигатель";
            this.Add_Engine.UseVisualStyleBackColor = true;
            this.Add_Engine.Click += new System.EventHandler(this.addEngine);
            // 
            // nameField
            // 
            this.nameField.Location = new System.Drawing.Point(262, 35);
            this.nameField.Name = "nameField";
            this.nameField.PlaceholderText = "Наименование";
            this.nameField.Size = new System.Drawing.Size(133, 23);
            this.nameField.TabIndex = 2;
            // 
            // amountField
            // 
            this.amountField.Location = new System.Drawing.Point(262, 64);
            this.amountField.Name = "amountField";
            this.amountField.PlaceholderText = "Количество";
            this.amountField.Size = new System.Drawing.Size(133, 23);
            this.amountField.TabIndex = 3;
            // 
            // Add_Component
            // 
            this.Add_Component.Location = new System.Drawing.Point(262, 138);
            this.Add_Component.Name = "Add_Component";
            this.Add_Component.Size = new System.Drawing.Size(133, 23);
            this.Add_Component.TabIndex = 4;
            this.Add_Component.Text = "Добавить компонент";
            this.Add_Component.UseVisualStyleBackColor = true;
            this.Add_Component.Click += new System.EventHandler(this.addComponent);
            // 
            // Rename
            // 
            this.Rename.Location = new System.Drawing.Point(262, 167);
            this.Rename.Name = "Rename";
            this.Rename.Size = new System.Drawing.Size(133, 23);
            this.Rename.TabIndex = 5;
            this.Rename.Text = "Переименовать";
            this.Rename.UseVisualStyleBackColor = true;
            this.Rename.Click += new System.EventHandler(this.rename);
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(262, 196);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(133, 23);
            this.Remove.TabIndex = 6;
            this.Remove.Text = "Удалить";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.remove);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(262, 226);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(133, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Вывести отчёт";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(407, 341);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.Rename);
            this.Controls.Add(this.Add_Component);
            this.Controls.Add(this.amountField);
            this.Controls.Add(this.nameField);
            this.Controls.Add(this.Add_Engine);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private TreeView treeView1;
        private Button Add_Engine;
        private TextBox nameField;
        private TextBox amountField;
        private Button Add_Component;
        private Button Rename;
        private Button Remove;
        private Button button4;
    }
}