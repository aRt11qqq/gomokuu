using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GomokGameProject
{
    public partial class SinglePlayForm : Form
    {
        private const int rectSize = 33; // 오목판의 셀의 크기(네모 크기)  
        private const int edgeCount = 15; // 오목판의 선 개수 

        private enum Horse { none = 0, BLACK, WHITE };
        private Horse[,] board = new Horse[edgeCount, edgeCount];
        private Horse nowPlayer = Horse.BLACK;

        private bool playing = false; 

        public SinglePlayForm()
        {
            InitializeComponent();
        }

        private bool judge() // 승리 판정 함수
        {
            for (int i = 0; i < edgeCount - 4; i++) // 가로로 오목이 됐을 경우 
                for (int j = 0; j < edgeCount; j++)
                    if (board[i, j] == nowPlayer && board[i + 1, j] == nowPlayer && board[i + 2, j] == nowPlayer && board[i + 3, j] == nowPlayer && board[i + 4, j] == nowPlayer)
                        return true;

            for (int i = 0; i < edgeCount; i++) // 세로로 오목이 됐을 경우 
                for (int j = 4; j < edgeCount; j++)
                    if (board[i, j] == nowPlayer && board[i, j - 1] == nowPlayer && board[i, j - 2] == nowPlayer && board[i, j - 3] == nowPlayer && board[i, j - 4] == nowPlayer)
                        return true;

            for (int i = 0; i < edgeCount - 4; i++) // Y = X 직선 ( 대각선 ) 
                for (int j = 0; j < edgeCount - 4; j++)
                    if (board[i, j] == nowPlayer && board[i + 1, j+ 1] == nowPlayer && board[i + 2, j + 2] == nowPlayer && board[i + 3, j + 3] == nowPlayer && board[i + 4, j + 4] == nowPlayer)
                        return true;

            for (int i = 4; i < edgeCount; i++) // Y = - X 직선 ( 대각선 반대 ) 
                for (int j = 0; j < edgeCount - 4; j++)
                    if (board[i, j] == nowPlayer && board[i - 1, j + 1] == nowPlayer && board[i - 2, j + 2] == nowPlayer && board[i - 3, j + 3] == nowPlayer && board[i - 4, j + 4] == nowPlayer)
                        return true;

            return false; // 필수 
        }

        private void refresh()
        {
            this.boardPicture.Refresh();
            for (int i = 0; i < edgeCount; i++)
                for (int j = 0; j < edgeCount; j++)
                    board[i, j] = Horse.none; 
        }

        private void boardPicture_MouseDown(object sender, MouseEventArgs e) // 오목판을 클릭했을 떄 
        {
            if (!playing)
            {
                MessageBox.Show("게임을 실행해주세요");
                return; 
            }
            Graphics g = this.boardPicture.CreateGraphics(); // 그림 그리기 위한 그래픽스 개체 
            int x = e.X / rectSize; // 사용자가 클릭한 위치가 몇번째 셀인지 확인 
            int y = e.Y / rectSize;
            if (x < 0 || y < 0 || x >= edgeCount || y >= edgeCount) // 0 ~ 14 미만이거나 초과일 때 x 
            {
                MessageBox.Show("테두리를 벗어날 수 없습니다.");
                return;
            }
            MessageBox.Show(x + "," + y); // 현재 셀의 위치
                                          // 

            if (board[x, y] != Horse.none) return;
            board[x, y] = nowPlayer; 
            
            if (nowPlayer == Horse.BLACK)
            {
                SolidBrush brush = new SolidBrush(Color.Black); // 현재 플레이어가 나면 검은색 돌 
                g.FillEllipse(brush, x * rectSize, y * rectSize, rectSize, rectSize); // 원형 그리기 , 좌표, 지름 
            }
            else
            {
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, x * rectSize, y * rectSize, rectSize, rectSize);
            }

            if (judge())
            {
                status.Text = nowPlayer.ToString() + "플레이어가 승리했습니다. ";
                playing = false;
                playButton.Text = "게임시작"; 
            }
            else
            {
                nowPlayer = ((nowPlayer == Horse.BLACK) ? Horse.WHITE : Horse.BLACK);
                status.Text = nowPlayer.ToString() + "플레이어의 차례입니다. "; 
            }
        }

        private void boardPicture_Paint(object sender, PaintEventArgs e) // 오목판 그리기 
        {
            Graphics gp = e.Graphics;
            Color lineColor = Color.Black; // 오목판의 선 색깔
            Pen p = new Pen(lineColor, 2);
            // 전체 오목판 테두리 
            gp.DrawLine(p, rectSize / 2, rectSize / 2, rectSize / 2, rectSize * edgeCount - rectSize / 2); // 좌측
            gp.DrawLine(p, rectSize / 2, rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize / 2); // 상측
            gp.DrawLine(p, rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize * edgeCount - rectSize / 2); // 하측
            gp.DrawLine(p, rectSize * edgeCount - rectSize / 2, rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize * edgeCount - rectSize / 2); // 우측
            p = new Pen(lineColor, 1);
            // 오목판 내부 , 대각선 방향으로 이동하면서 십자가 모양의 선 그리기
            // + 
            //   +
            //     + ... 
            for (int i = rectSize + rectSize / 2; i < rectSize * edgeCount - rectSize / 2; i += rectSize)
            {
                gp.DrawLine(p, rectSize / 2, i, rectSize * edgeCount - rectSize / 2, i);
                gp.DrawLine(p, i, rectSize / 2, i, rectSize * edgeCount - rectSize / 2);
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (!playing)
            {
                refresh();
                playing = true;
                playButton.Text = "재시작";
                status.Text = nowPlayer.ToString() + " 플레이어의 차례입니다. "; 
            }
            else
            {
                refresh();
                status.Text = "게임이 재시작 되었습니다. "; 
            }
        }
    }
}
