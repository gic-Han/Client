namespace Client;

partial class Form2
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
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.FormBorderStyle = FormBorderStyle.Fixed3D;
        this.ClientSize = new System.Drawing.Size(300, 400);
        this.Text = "Client";

        connect.Size = new Size(120, 40);
        connect.Location = new Point(20, 340);
        connect.Text = "Connect To Server";

        quit.Size = new Size(120, 40);
        quit.Location = new Point(160, 340);
        quit.Text = "Quit Server";

        state.Size = new Size(200, 40);
        state.Location = new Point(20, 280);
        state.TextAlign = ContentAlignment.MiddleLeft;
        state.Text = "Click to connect";

        this.Controls.Add(connect);
        this.Controls.Add(quit);
        this.Controls.Add(state);
    }

    private Label state = new Label();
    private Button connect = new Button();
    private Button quit = new Button();

    #endregion
}
