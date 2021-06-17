using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GomokGameProject
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void singlePlayButton_Click(object sender, EventArgs e)
        {
            Hide(); // 현재창 숨기기 
            SinglePlayForm singlePlayForm = new SinglePlayForm(); // 싱글창 객체 생성 
            singlePlayForm.FormClosed += new FormClosedEventHandler(childForm_Closed); // 싱글폼이 닫혔을 떄는 child폼 show 
            singlePlayForm.Show(); // 싱글창 보이기 
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit(); // 프로그램 전체 종료 
        }

        void childForm_Closed(object sender, FormClosedEventArgs e)
        {
            Show(); 
        }
    }
}
