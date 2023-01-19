namespace PDMEnginesApp
{
    partial class PDMForm
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
            this.PdmTree = new System.Windows.Forms.TreeView();
            this.AddEngine = new System.Windows.Forms.Button();
            this.NameField = new System.Windows.Forms.TextBox();
            this.AmountField = new System.Windows.Forms.TextBox();
            this.AddComponent = new System.Windows.Forms.Button();
            this.Rename = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PdmTree
            // 
            this.PdmTree.Location = new System.Drawing.Point(12, 12);
            this.PdmTree.Name = "PdmTree";
            this.PdmTree.Size = new System.Drawing.Size(284, 305);
            this.PdmTree.TabIndex = 0;
            // 
            // AddEngine
            // 
            this.AddEngine.Location = new System.Drawing.Point(424, 126);
            this.AddEngine.Name = "AddEngine";
            this.AddEngine.Size = new System.Drawing.Size(134, 25);
            this.AddEngine.TabIndex = 1;
            this.AddEngine.Text = "Добавить двигатель";
            this.AddEngine.UseVisualStyleBackColor = true;
            this.AddEngine.Click += new System.EventHandler(this.btnAddEngine_Click);
            // 
            // NameField
            // 
            this.NameField.Location = new System.Drawing.Point(369, 45);
            this.NameField.Name = "NameField";
            this.NameField.PlaceholderText = "Наименование";
            this.NameField.Size = new System.Drawing.Size(189, 23);
            this.NameField.TabIndex = 5;
            // 
            // AmountField
            // 
            this.AmountField.Location = new System.Drawing.Point(369, 83);
            this.AmountField.Name = "AmountField";
            this.AmountField.PlaceholderText = "Количество";
            this.AmountField.Size = new System.Drawing.Size(189, 23);
            this.AmountField.TabIndex = 6;
            // 
            // AddComponent
            // 
            this.AddComponent.Location = new System.Drawing.Point(425, 157);
            this.AddComponent.Name = "AddComponent";
            this.AddComponent.Size = new System.Drawing.Size(134, 25);
            this.AddComponent.TabIndex = 7;
            this.AddComponent.Text = "Добавить компонент";
            this.AddComponent.UseVisualStyleBackColor = true;
            this.AddComponent.Click += new System.EventHandler(this.btnAddComponent_Click);
            // 
            // Rename
            // 
            this.Rename.Location = new System.Drawing.Point(425, 188);
            this.Rename.Name = "Rename";
            this.Rename.Size = new System.Drawing.Size(134, 25);
            this.Rename.TabIndex = 8;
            this.Rename.Text = "Переименовать";
            this.Rename.UseVisualStyleBackColor = true;
            this.Rename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(425, 219);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(134, 25);
            this.Delete.TabIndex = 9;
            this.Delete.Text = "Удалить";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // PDMApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 333);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.Rename);
            this.Controls.Add(this.AddComponent);
            this.Controls.Add(this.AmountField);
            this.Controls.Add(this.NameField);
            this.Controls.Add(this.AddEngine);
            this.Controls.Add(this.PdmTree);
            this.Name = "PDMApp";
            this.Text = "PDMApp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TreeView PdmTree;
        private Button AddEngine;
        private TextBox NameField;
        private TextBox AmountField;
        private Button AddComponent;
        private Button Rename;
        private Button Delete;
    }
}