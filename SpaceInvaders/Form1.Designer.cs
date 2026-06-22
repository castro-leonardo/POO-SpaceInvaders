namespace SpaceInvaders
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.PointsTxtBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // PointsTxtBox
            // 
            this.PointsTxtBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.PointsTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PointsTxtBox.Enabled = false;
            this.PointsTxtBox.Font = new System.Drawing.Font("SimSun-ExtG", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PointsTxtBox.ForeColor = System.Drawing.SystemColors.Control;
            this.PointsTxtBox.HideSelection = false;
            this.PointsTxtBox.Location = new System.Drawing.Point(16, 15);
            this.PointsTxtBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PointsTxtBox.Name = "PointsTxtBox";
            this.PointsTxtBox.Size = new System.Drawing.Size(241, 27);
            this.PointsTxtBox.TabIndex = 0;
            this.PointsTxtBox.TextChanged += new System.EventHandler(this.PointsTxtBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SpaceInvaders.Properties.Resources.spacebgck;
            this.ClientSize = new System.Drawing.Size(969, 558);
            this.Controls.Add(this.PointsTxtBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PointsTxtBox;
    }
}

