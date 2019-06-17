using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GetProcessorInformation
{
    public partial class FormProcessorInfo : Form
    {
        public FormProcessorInfo()
        {
            InitializeComponent();

            // three lines below must be  before call to SetUpWindowLocation
            m_bSetWindowPos = false;
            m_rectNewWindowPos = new Rectangle(Location, Size);
            m_bPlot = false;
            m_iUpdateRate = 500;            // Must be initialized before set window loacation
                                            // which reads preferences (NHS 2017-11-20 includes recall of update rate)
            SetUpWindowLocation(false);

            m_iNoProcessors = 0;
            m_iNoCores = 0;
            m_iLogicalProcessors = 0;
            m_iLastError = 0;
            m_iLastState = -1;

            m_DiskPerformanceCount = null;
            m_perform = new List<PerformanceCounter>();
            m_percentPerform = new List<float>();
            m_stFormat = new StringFormat();
            m_stFormat.Alignment = StringAlignment.Far;
            m_stFormat.LineAlignment = StringAlignment.Center;
            CreateListOfBrushes();
            GetProcessorInfo();
            toolStripStatusNoProcessors.Text = m_iNoProcessors.ToString();
            toolStripStatusNoCores.Text = m_iNoCores.ToString();
            toolStripStatusLogicalProcessors.Text = m_iLogicalProcessors.ToString();

            timer1.Interval = m_iUpdateRate;
            timer1.Enabled = true;
        }

        // P/Invoke declarations
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        protected const int WM_SYSCOMMAND = 0x112;
        protected const int MF_STRING = 0x0;
        protected const int MF_SEPARATOR = 0x800;
        protected const int MF_BYPOSITION = 0x400;
        protected const int SYSMENU_ABOUT_ID = 0x1;

        protected int m_iNoProcessors;
        protected int m_iNoCores;
        protected int m_iLogicalProcessors;
        protected int m_iUpdateRate;
        protected int m_iLastError;
        protected int m_iLastState;
        protected bool m_bPlot;
        protected bool m_bSetWindowPos;
        protected float m_fTotalMemoryAvailable;
        protected float m_fTotalMemoryInUse;
        protected float m_fTotalDiskTime;
        protected string m_stAppPath;
        protected Rectangle m_rectNewWindowPos;
        protected StringFormat m_stFormat;
        protected List<PerformanceCounter> m_perform;
        protected List<float> m_percentPerform;
        protected List<SolidBrush> m_brushes;
        protected PerformanceCounter m_DiskPerformanceCount;
        protected PerformanceCounter m_DiskPerformanceReadPerSec;
        protected PerformanceCounter m_DiskPerformanceWritePerSec;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Get a handle to a copy of this form's system (window) menu
            IntPtr hSysMenu = GetSystemMenu(this.Handle, false);

            // Add a separator
            InsertMenu(hSysMenu, 5, MF_SEPARATOR | MF_BYPOSITION, SYSMENU_ABOUT_ID, string.Empty);

            // Add the About menu item
            InsertMenu(hSysMenu, 6, MF_STRING | MF_BYPOSITION, SYSMENU_ABOUT_ID, "&About…");
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Test if the About item was selected from the system menu
            if ((m.Msg == WM_SYSCOMMAND) && ((int)m.WParam == SYSMENU_ABOUT_ID))
            {
                AboutBox1 dlg = new AboutBox1();
                dlg.ShowDialog();
            }
        }

        protected void CreateListOfBrushes()
        {
            SolidBrush brush;

            m_brushes = new List<SolidBrush>();
            brush = new SolidBrush(Color.Aqua);
            m_brushes.Add(brush);
            brush = new SolidBrush(Color.Blue);
            m_brushes.Add(brush);
            brush = new SolidBrush(Color.Green);
            m_brushes.Add(brush);
            brush = new SolidBrush(Color.Red);
            m_brushes.Add(brush);
            brush = new SolidBrush(Color.Yellow);
            m_brushes.Add(brush);
            brush = new SolidBrush(Color.Orange);
            m_brushes.Add(brush);
            brush = new SolidBrush(Color.Magenta);
            m_brushes.Add(brush);
            brush = new SolidBrush(Color.Pink);
            m_brushes.Add(brush);
            brush = new SolidBrush(Color.Chocolate);
            m_brushes.Add(brush);
        }

        protected int GetProcessorInfo()
        {
            int iRet;
            PerformanceCounter performanceCount;

            iRet = 0;
            m_iNoProcessors = 0;
            m_iNoCores = 0;
            try
            {
                foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
                {
                    m_iNoProcessors += Convert.ToInt32(item["NumberOfProcessors"]);
                }
                foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                {
                    m_iNoCores += Convert.ToInt32(item["NumberOfCores"]);
                }
                m_iLogicalProcessors = Environment.ProcessorCount;
                for (int i = 0; i < m_iLogicalProcessors; i++)
                {
                    performanceCount = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                    m_perform.Add(performanceCount);
                    m_percentPerform.Add(-1.0f);
                }

            }
            catch (Exception ex)
            {
                iRet = ex.HResult;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return iRet;
        }

        protected int GetDiskTime()
        {
            int iNewState;
            int iRet;
            float fReadValue, fWriteValue;

            iRet = 0;
            try
            {
                if (m_DiskPerformanceCount == null)
                {
                    string[] arrInst;
                    PerformanceCounterCategory pcc;
                    pcc = new PerformanceCounterCategory("PhysicalDisk");
                    arrInst = pcc.GetInstanceNames();

                    m_DiskPerformanceCount = new PerformanceCounter("PhysicalDisk", "% Idle Time", "_Total");
                    //m_DiskPerformanceCount = new PerformanceCounter("PhysicalDisk", "% Idle Time", "2 D:");
                    m_DiskPerformanceReadPerSec = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
                    m_DiskPerformanceWritePerSec = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
                }
                m_fTotalDiskTime = 100.0f - m_DiskPerformanceCount.NextValue();
                if (m_fTotalDiskTime > 100.0f)
                    m_fTotalDiskTime = 100.0f;
                else if (m_fTotalDiskTime < 0.0f)
                    m_fTotalDiskTime = 0.0f;

                iNewState = 0;
                fReadValue = m_DiskPerformanceReadPerSec.NextValue();
                fWriteValue = m_DiskPerformanceWritePerSec.NextValue();
                if (fReadValue > fWriteValue)
                    iNewState = 1;
                else if (fReadValue < fWriteValue)
                {
                    iNewState = 2;
                }
                if (iNewState != m_iLastState)
                {
                    toolStripStatusDiskActivity.Text = " ";
                    toolStripStatusDiskActivity.ForeColor = Color.Green;
                    switch (iNewState)
                    {
                        case 0:
                            toolStripStatusDiskActivity.ForeColor = Color.Green;
                            toolStripStatusDiskActivity.Text = " ";
                            break;
                        case 1:
                            toolStripStatusDiskActivity.ForeColor = Color.Green;
                            toolStripStatusDiskActivity.Text = "Read";
                            break;
                        case 2:
                            toolStripStatusDiskActivity.ForeColor = Color.Red;
                            toolStripStatusDiskActivity.Text = "Write";
                            break;
                    }
                    m_iLastState = iNewState;
                    statusStrip1.Update();
                }
            }
            catch (Exception ex)
            {
                iRet = ex.HResult;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return iRet;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string stData;
            string stMemStat;
            PerformanceCounter performanceCount;

            txtCPUUsage.Text = "";
            txtCPUUsage.Update();
            for (int i = 0; i < m_iLogicalProcessors; i++)
            {
                m_percentPerform[i] = m_perform[i].NextValue();
                stData = string.Format("Processor {0}:  {1:00.00}", i, m_percentPerform[i]);
                txtCPUUsage.Text += stData;
                txtCPUUsage.Text += "\r\n";
            }

            GetDiskTime();

            if (m_bPlot)
            {
                PlotUsageBars();
            }
            try
            {
                performanceCount = new PerformanceCounter("Memory", "Available MBytes");

                m_fTotalMemoryAvailable = (int)performanceCount.NextValue();

                m_fTotalMemoryInUse = (float)(PerformanceInfo.GetTotalMemoryInMiB() - PerformanceInfo.GetPhysicalAvailableMemoryInMiB());

                stMemStat = string.Format("Mem in use: {0:0.0} Availible: {1:0.0}", m_fTotalMemoryInUse, m_fTotalMemoryAvailable);
                toolStripStatusLabelMemory.Text = stMemStat;

                //
                //performanceCount = new PerformanceCounter("Physical Disk", "% Disk Time");
            }
                
            catch (Exception ex)
            {
                m_iLastError = ex.HResult;
            }

        }

        private void plotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_bPlot = !m_bPlot;
            AdjustForPlot();
        }

        private void AdjustForPlot ()
        {
            if (m_bPlot)
            {
                txtCPUUsage.Text = "";
                txtCPUUsage.Visible = false;
            }
            else
            {
                txtCPUUsage.Text = "";
                txtCPUUsage.Visible = true;
            }
        }

        private void PlotUsageBars()
        {
            int iBrushIndex;
            float fDx;
            string stVal;
            RectangleF rect;
            LinearGradientBrush brDisk;
            Graphics gr = CreateGraphics();

            rect = ClientRectangle;
            gr.FillRectangle(Brushes.Black, rect);
            fDx = rect.Width / 100.0f;
            rect.Height = (float)(ClientRectangle.Height - statusStrip1.Height) / (m_iLogicalProcessors + 1);  // 1 row for disk usage
            rect.X = 0.0f;
            rect.Y = 0.0f;
            for (int i = 0; i < m_iLogicalProcessors; i++)
            {
                iBrushIndex = i % m_brushes.Count;
                rect.Width = fDx * m_percentPerform[i];
                stVal = string.Format("{0:0.0}", m_percentPerform[i]);
                gr.FillRectangle(m_brushes[iBrushIndex], rect);
                rect.Width = ClientRectangle.Width * 0.95f;
                gr.DrawString(stVal, Font, Brushes.White, rect, m_stFormat);
                rect.Y += rect.Height;
            }
            rect.Width = fDx * m_fTotalDiskTime;
            if (rect.Width > 0.0f)
            {
                brDisk = new LinearGradientBrush(rect, Color.Aqua, Color.Blue, LinearGradientMode.Horizontal);
                gr.FillRectangle(brDisk, rect);
            }
            rect.Width = ClientRectangle.Width * 0.95f;
            stVal = string.Format("Disk Usage {0:0.0}", m_fTotalDiskTime);
            if (m_fTotalDiskTime > 80.0)
                gr.DrawString(stVal, Font, Brushes.Black, rect, m_stFormat);
            else
                gr.DrawString(stVal, Font, Brushes.White, rect, m_stFormat);
        }

        private void updateRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult;

            FormUpdateRate frm = new FormUpdateRate();
            frm.iUpdateRate = m_iUpdateRate;
            dlgResult = frm.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                m_iUpdateRate = frm.iUpdateRate;
                timer1.Enabled = false;
                timer1.Interval = m_iUpdateRate;
                timer1.Enabled = true;
            }
        }

        private void SetUpWindowLocation(bool bSave)
        {
            string stFile;
            Rectangle rect = new Rectangle(Location.X, Location.Y, Width, Height);
            BinaryReader br;
            BinaryWriter bw;

            m_stAppPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            m_stAppPath += "\\" + Application.CompanyName + "\\" + Application.ProductName;

            stFile = m_stAppPath + "\\preferences.dat";
            try
            {
                if (bSave)
                {
                    if (!Directory.Exists(m_stAppPath))
                        Directory.CreateDirectory(m_stAppPath);
                    bw = new BinaryWriter(File.Open(stFile, FileMode.Create));
                    bw.Write(rect.X);
                    bw.Write(rect.Y);
                    bw.Write(rect.Width);
                    bw.Write(rect.Height);
                    bw.Write(m_bPlot);
                    bw.Write(m_iUpdateRate);
                    bw.Close();
                }
                else
                {
                    if (!Directory.Exists(m_stAppPath))
                    {
                        Directory.CreateDirectory(m_stAppPath);
                    }
                    else
                    {
                        if (File.Exists(stFile))
                        {
                            br = new BinaryReader(File.OpenRead(stFile));
                            rect.X = br.ReadInt32();
                            rect.Y = br.ReadInt32();
                            rect.Width = br.ReadInt32();
                            rect.Height = br.ReadInt32();
                            m_bPlot = br.ReadBoolean();
                            m_iUpdateRate = br.ReadInt32();
                            br.Close();
                            m_bSetWindowPos = true;
                            m_rectNewWindowPos = rect;
                        }
                    }
                }
            } catch (Exception ex)
            {
                m_iLastError = ex.HResult;
            }
        }

        private void FormProcessorInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetUpWindowLocation(true);
        }

        private void FormProcessorInfo_Shown(object sender, EventArgs e)
        {
            // Adjust Window position
            if (m_bSetWindowPos)
            {
                Location = m_rectNewWindowPos.Location;
                Width = m_rectNewWindowPos.Width;
                Height = m_rectNewWindowPos.Height;
                AdjustForPlot();
                Update();
                m_bSetWindowPos = false;
            }
        }
    }
}

/// ToDo:  Add these parameters to display
/// PhysicalDisk
/// % Diks Time _Total
/// Disk Bytes/sec _Total
/// 