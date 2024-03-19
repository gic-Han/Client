using System.Net.Sockets;

namespace Client;

public partial class Form2 : Form
{
    public Form2()
    {
        InitializeComponent();
        turn = false;
        connect.Click += connect_Click;
        this.MouseUp += Form2_MouseUp;
    }

    private void connect_Click(object? sender, EventArgs e)
    {
        connect.Click -= connect_Click;
        quit.Click += quit_Click;
        this.Paint += Form2_Paint;
        state.Text = "Connecting...";
        Connect();
        Init();
        Invalidate();
    }

    private void quit_Click(object? sender, EventArgs e)
    {
        quit.Click -= quit_Click;
        connect.Click += connect_Click;
        this.Paint -= Form2_Paint;
        state.Text = "Quit";
        Invalidate();
        stream!.WriteByte(255); // Disconnect
    }

    private void Form2_Paint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Pen pen = new Pen(Color.Black, 2f);
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                g.DrawRectangle(pen, j*80+30, i*80+30, 80, 80);

                if(cell_check[i,j] == 1)
                {
                    Rectangle r = new Rectangle(j*80+40, i*80+40, 60, 60);
                    Pen p = new Pen(Color.Green, 10);
                    g.DrawEllipse(p,r);
                }
                if(cell_check[i,j] == -1)
                {
                    Rectangle r = new Rectangle(j*80+40, i*80+40, 60, 60);
                    Pen p = new Pen(Color.Red, 10);
                    g.DrawEllipse(p,r);
                }
            }
        }
        pen.Dispose();
    }
    
    private void Form2_MouseUp(object? sender, MouseEventArgs e)
    {
        if(!turn || stream == null)
            return;
        int i = (e.Y-30)/80;
        int j = (e.X-30)/80;
        if(i < 0 && i >= 3 && j < 0 && j >= 3)
            return;
        if(cell_check[i,j] == 0)
        {
            turn = false;
            cell_check[i,j] = 1;
            byte l = (byte)(i << 2 | j);
            stream!.WriteByte(l);
            Invalidate(new Rectangle(j*80+30, i*80+30, 80, 80));
            WinCheck(i,j);
        }
    }

    private const string ServerAddress = "127.0.0.1";
    private const int Port = 50001;
    private TcpClient? client;
    private NetworkStream? stream;

    private bool turn;
    private int[,] cell_check = new int[3,3];

    private void Connect()
    {
        client = new TcpClient(ServerAddress, Port);
        stream = client.GetStream();
        state.Text = "Connected to server";

        Thread receiveThread = new Thread(ReceiveMessages)
        {
            IsBackground = true
        };
        receiveThread.Start();
    }

    private void ReceiveMessages()
    {
        int recv;
        while (true)
        {
            recv = stream!.ReadByte();
            if(recv == 255)
            {
                state.Text = "Opponant has left";
                break;
            }
            if(recv == 254)
                break;
            if(recv == 250)
            {
                state.Text = "Start!";
                turn = true;
                continue;
            }
            if(recv == 200)
            {
                state.Text = "Loss... T.T";
                break;
            }
            int i = recv >> 2;
            int j = recv & 3;
            if(i >= 0 && i < 3 && j >= 0 && j < 3)
            {
                turn = true;
                cell_check[i,j] = -1;
                Invalidate(new Rectangle(j*80+30, i*80+30, 80, 80));
            }
        }
    }

    private void WinCheck(int i, int j)
    {
        int tmp = 0;
        for(int h = 0; h < 3; h++)
            tmp += cell_check[h,j];
        if(tmp == 3)
        {
            WinSend();
            return;
        }
        tmp = 0;

        for(int h = 0; h < 3; h++)
            tmp += cell_check[i,h];
        if(tmp == 3)
        {
            WinSend();
            return;
        }
        tmp = 0;
        
        if(i != 1 && j != 1)
        {
            for(int h = 0; h < 3; h++)
                tmp += cell_check[h,h];
            if(tmp == 3)
            {
                WinSend();
                return;
            }
            tmp = 0;
            
            for(int h = 0; h < 3; h++)
                tmp += cell_check[2-h,h];
            if(tmp == 3)
            {
                WinSend();
            }
        }
    }

    private void WinSend()
    {
        state.Text = "Winning!";
        stream!.WriteByte(200);
    }

    private void Init()
    {
        for(int i = 0; i < 3; i++)
            for(int j = 0; j < 3; j++)
                cell_check[i,j] = 0;
    }
}
