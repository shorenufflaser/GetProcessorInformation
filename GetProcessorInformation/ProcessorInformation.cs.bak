using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
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
            SetUpWindowLocation(false);

            m_iNoProcessors = 0;
            m_iNoCores = 0;
            m_iLogicalProcessors = 0;
            m_iLastError = 0;
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

            m_iUpdateRate = 500;
            timer1.Enabled = true;
        }


        protected int m_iNoProcessors;
        protected int m_iNoCores;
        protected int m_iLogicalProcessors;
        protected int m_iUpdateRate;
        protected int m_iLastError;
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
            int iRet;

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
                }
                m_fTotalDiskTime = 100.0f - m_DiskPerformanceCount.NextValue();
                if (m_fTotalDiskTime > 100.0f)
                    m_fTotalDiskTime = 100.0f;
                else if (m_fTotalDiskTime < 0.0f)
                    m_fTotalDiskTime = 0.0f;
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